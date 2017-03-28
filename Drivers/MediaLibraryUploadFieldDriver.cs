using Lombiq.Fields.Fields;
using Lombiq.Fields.Models;
using Lombiq.Fields.Handlers;
using Lombiq.Fields.Settings;
using Lombiq.Fields.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.MediaLibrary.Models;
using Orchard.Tokens;
using Orchard.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lombiq.Fields.Drivers
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField")]
    public class MediaLibraryUploadFieldDriver : ContentFieldDriver<MediaLibraryUploadField>
    {
        private readonly IContentManager _contentManager;
        private readonly ITokenizer _tokenizer;
        private readonly IWorkContextAccessor _wca;
        private readonly IMediaLibraryUploadHandler _mediaLibraryUploadHandler;


        public Localizer T { get; set; }


        public MediaLibraryUploadFieldDriver(IContentManager contentManager, ITokenizer tokenizer, IWorkContextAccessor wca, IMediaLibraryUploadHandler mediaLibraryUploadHandler)
        {
            _contentManager = contentManager;
            _tokenizer = tokenizer;
            _wca = wca;
            _mediaLibraryUploadHandler = mediaLibraryUploadHandler;

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
                field.Ids = string.IsNullOrEmpty(model.SelectedIds) ? new int[0] : model.SelectedIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                var workContext = _wca.GetContext();

                _mediaLibraryUploadHandler.ValidateAndStore(new MediaLibraryUploadFieldPostHandlerContext
                {
                    MediaLibraryUploadSettings = settings,
                    FileFieldName = $"MediaLibraryUploadField-{part.PartDefinition.Name}-{field.Name}[]",
                    Updater = updater,
                    AlreadyUploadedFiles = field.MediaParts != null ? field.MediaParts.ToList() : new List<MediaPart>(),
                    FolderPath = _tokenizer.Replace(settings.FolderPath, new Dictionary<string, object>
                    {
                        { "Content", part.ContentItem },
                        { "User", workContext.CurrentUser }
                    }),
                    StoreIds = (ids) => field.Ids = field.Ids.Union(ids).ToArray(),
                });
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