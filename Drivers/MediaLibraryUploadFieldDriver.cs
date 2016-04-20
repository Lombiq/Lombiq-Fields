using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Lombiq.Fields.Fields;
using Lombiq.Fields.Settings;
using Lombiq.Fields.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.MediaLibrary.Models;
using Orchard.MediaLibrary.Services;
using Orchard.Tokens;
using Orchard.UI.Notify;
using Orchard.Utility.Extensions;
using Piedone.HelpfulLibraries.Utilities;
using Orchard.FileSystems.Media;

namespace Lombiq.Fields.Drivers
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField")]
    public class MediaLibraryUploadFieldDriver : ContentFieldDriver<MediaLibraryUploadField>
    {
        private readonly IContentManager _contentManager;
        private readonly IMediaLibraryService _mediaLibraryService;
        private readonly INotifier _notifier;
        private readonly ITokenizer _tokenizer;
        private readonly IWorkContextAccessor _wca;
        private readonly IStorageProvider _storageProvider;


        public Localizer T { get; set; }


        public MediaLibraryUploadFieldDriver(IContentManager contentManager, IMediaLibraryService mediaLibraryService, INotifier notifier, ITokenizer tokenizer, IWorkContextAccessor wca, IStorageProvider storageProvider)
        {
            _contentManager = contentManager;
            _mediaLibraryService = mediaLibraryService;
            _notifier = notifier;
            _tokenizer = tokenizer;
            _wca = wca;
            _storageProvider = storageProvider;


            T = NullLocalizer.Instance;
        }


        private static string GetPrefix(MediaLibraryUploadField field, ContentPart part)
        {
            return part.PartDefinition.Name + "." + field.Name;
        }

        private static string GetDifferentiator(MediaLibraryUploadField field, ContentPart part)
        {
            return field.Name;
        }


        protected override DriverResult Display(ContentPart part, MediaLibraryUploadField field, string displayType, dynamic shapeHelper)
        {
            return Combined(
                ContentShape("Fields_MediaLibraryUpload", GetDifferentiator(field, part), () => shapeHelper.Fields_MediaLibraryUpload()),
                ContentShape("Fields_MediaLibraryUpload_Summary", GetDifferentiator(field, part), () => shapeHelper.Fields_MediaLibraryUpload_Summary()),
                ContentShape("Fields_MediaLibraryUpload_SummaryAdmin", GetDifferentiator(field, part), () => shapeHelper.Fields_MediaLibraryUpload_SummaryAdmin())
            );
        }

        protected override DriverResult Editor(ContentPart part, MediaLibraryUploadField field, dynamic shapeHelper)
        {
            var model = new MediaLibraryUploadFieldViewModel
            {
                Field = field,
                Part = part,
                ContentItems = _contentManager.GetMany<ContentItem>(field.Ids, VersionOptions.Published, QueryHints.Empty).ToList(),
                SelectedIds = string.Join(",", field.Ids)
            };

            return ContentShape("Fields_MediaLibraryUpload_Edit", GetDifferentiator(field, part), () =>
                shapeHelper.EditorTemplate(TemplateName: "Fields/MediaLibraryUpload.Edit", Model: model, Prefix: GetPrefix(field, part)));
        }

        protected override DriverResult Editor(ContentPart part, MediaLibraryUploadField field, IUpdateModel updater, dynamic shapeHelper)
        {
            var model = new MediaLibraryUploadFieldViewModel { SelectedIds = string.Join(",", field.Ids) };
            var settings = field.PartFieldDefinition.Settings.GetModel<MediaLibraryUploadFieldSettings>();

            if (updater.TryUpdateModel(model, GetPrefix(field, part), null, null))
            {
                field.Ids = String.IsNullOrEmpty(model.SelectedIds) ? new int[0] : model.SelectedIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                var files = ((Controller)updater).Request.Files;
                var mediaPartsCreated = new List<MediaPart>();
                double actualSizeMBOfAllContents = 0;
                
                for (int i = 0; i < files.Count; i++)
                {
                    // To make sure that we only process those files that are uploaded using this field's UI control.
                    if (files.AllKeys[i].Equals(string.Format("MediaLibraryUploadField-{0}-{1}[]", part.PartDefinition.Name, field.Name)))
                    {
                        var file = files[i];

                        if (file.ContentLength == 0) continue;

                        var allowedExtensions = (settings.AllowedExtensions ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => string.Format(".{0}", s));
                        if (allowedExtensions.Any() && !allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
                        {
                            _notifier.Warning(T("The file \"{0}\" was not uploaded, because its extension is not among the accepted ones. The accepted file extensions are: {1}.",
                                file.FileName, string.Join(", ", allowedExtensions)));
                            continue;
                        }

                        if (settings.MaximumSizeKB > 0 && file.ContentLength > settings.MaximumSizeKB * 1024) {
                            _notifier.Warning(T("The file \"{0}\" was not uploaded, because its size exceeds the {1} KB limitation.", file.FileName, settings.MaximumSizeKB));
                            continue;
                        }

                        // Checking against image-specific settings.
                        if (MimeAssistant.GetMimeType(file.FileName.ToLowerInvariant()).StartsWith("image"))
                        {
                            using (var image = Image.FromStream(file.InputStream))
                            {
                                if ((settings.ImageMaximumWidth > 0 && image.Width > settings.ImageMaximumWidth) || (settings.ImageMaximumHeight > 0 && image.Height > settings.ImageMaximumHeight))
                                {
                                    _notifier.Warning(T("The image \"{0}\" was not uploaded, because its dimensions exceed the limitations. The maximum allowed file dimensions are {1}x{2} pixels.",
                                        file.FileName, settings.ImageMaximumWidth, settings.ImageMaximumHeight));
                                    continue;
                                }
                            }

                            file.InputStream.Position = 0;
                        }

                        //Checking maximum size of all contents uploaded by actual user
                        var user = _wca.GetContext().CurrentUser;
                        var folderPath = _tokenizer.Replace(settings.FolderPath, new Dictionary<string, object>
                        {
                            { "Content", part.ContentItem },
                            { "User", user }
                        });

                        folderPath = string.IsNullOrEmpty(folderPath) ? "UserUploads/" + user.Id : folderPath;

                        if (!string.IsNullOrEmpty(folderPath))
                        {
                            double SizeMBOfAlreadyStoredContents = 0;
                            IEnumerable<IStorageFile> storageFiles = _storageProvider.ListFiles(folderPath);

                            foreach (var f in storageFiles)
                            {
                                SizeMBOfAlreadyStoredContents += f.GetSize() / 1024.0 / 1024.0;
                            }

                            actualSizeMBOfAllContents += file.ContentLength / 1024.0 / 1024.0;

                            if (settings.MaximumSizeMBOfContents != 0 && SizeMBOfAlreadyStoredContents + actualSizeMBOfAllContents > settings.MaximumSizeMBOfContents)
                            {
                                _notifier.Warning(T("The file \"{0}\" was not uploaded, because the size of all uploaded files exceed the {1} MB limitation.", file.FileName, settings.MaximumSizeMBOfContents));
                                continue;
                            }
                        }

                        // At this point we can be sure that the files comply with the settings and limitations, so we can import them.
                        folderPath = string.IsNullOrEmpty(folderPath) ? "UserUploads/" + user.Id : folderPath;
                        
                        var mediaPart = _mediaLibraryService.ImportMedia(file.InputStream, folderPath, file.FileName);
                        _contentManager.Create(mediaPart);
                        mediaPartsCreated.Add(mediaPart);
                    }
                }

                if (mediaPartsCreated.Any())
                {
                    field.Ids = field.Ids.Union(mediaPartsCreated.Select(m => m.ContentItem.Id)).ToArray();
                    _notifier.Information(T("The following items were successfully uploaded: {0}.", string.Join(", ", mediaPartsCreated.Select(mp => mp.FileName))));
                }
            }

            if (settings.Required && field.Ids.Length == 0) 
                updater.AddModelError("Id", T("You need to have or upload at least one file for the field {0}.", field.Name.CamelFriendly()));

            return Editor(part, field, shapeHelper);
        }

        protected override void Importing(ContentPart part, MediaLibraryUploadField field, ImportContentContext context)
        {
            var contentItemIds = context.Attribute(field.FieldDefinition.Name + "." + field.Name, "ContentItems");
            field.Ids = contentItemIds == null ? new int[0] : contentItemIds.Split(',').Select(context.GetItemFromSession).Select(contentItem => contentItem.Id).ToArray();
        }

        protected override void Exporting(ContentPart part, MediaLibraryUploadField field, ExportContentContext context)
        {
            if (field.Ids.Any())
            {
                var contentItemIds = field.Ids.Select(i => _contentManager.Get(i)).Select(i => _contentManager.GetItemMetadata(i).Identity.ToString()).ToArray();
                context.Element(field.FieldDefinition.Name + "." + field.Name).SetAttributeValue("ContentItems", string.Join(",", contentItemIds));
            }
        }

        protected override void Describe(DescribeMembersContext context)
        {
            context.Member(null, typeof(string), T("Ids"), T("A formatted list of the ids, e.g., {1},{42}"));
        }
    }
}