using Lombiq.Fields.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.FileSystems.Media;
using Orchard.Localization;
using Orchard.MediaLibrary.Models;
using Orchard.MediaLibrary.Services;
using Orchard.UI.Notify;
using Orchard.Utility.Extensions;
using Piedone.HelpfulLibraries.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Lombiq.Fields.Handlers
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField")]
    public class MediaLibraryUploadHandler : IMediaLibraryUploadHandler
    {
        private readonly IWorkContextAccessor _wca;
        private readonly IStorageProvider _storageProvider;
        private readonly IMediaLibraryService _mediaLibraryService;
        private readonly IContentManager _contentManager;
        private readonly INotifier _notifier;


        public Localizer T { get; set; }


        public MediaLibraryUploadHandler(
             IWorkContextAccessor wca,
            IStorageProvider storageProvider,
            IMediaLibraryService mediaLibraryService,
            IContentManager contentManager,
            INotifier notifier)
        {
            _wca = wca;
            _storageProvider = storageProvider;
            _mediaLibraryService = mediaLibraryService;
            _contentManager = contentManager;
            _notifier = notifier;

            T = NullLocalizer.Instance;
        }


        public void ValidateAndStore(MediaLibraryUploadingContext context)
        {
            // Collecting files that this field should handle.
            var workContext = _wca.GetContext();
            var files = workContext.HttpContext.Request.Files;
            var validFiles = new List<HttpPostedFileBase>();
            long uploadingFilesSizeInBytes = 0;

            #region Validation against different file properties and collecting valid files.

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];

                // Making sure that only those files are processed that are uploaded through the current field's editor.
                if (file.ContentLength > 0 && files.Keys[i] == context.FileFieldName)
                {
                    var allowedExtensions = (context.MediaLibraryUploadSettings.AllowedExtensions ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => string.Format(".{0}", s));

                    // Validating against allowed file extension.
                    if (allowedExtensions.Any() && !allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
                    {
                        context.Updater.AddModelError("FileExtensionNotAllowed", T("The file \"{0}\" was not uploaded, because its extension is not among the accepted ones. The accepted file extensions are: {1}.",
                            file.FileName, string.Join(", ", allowedExtensions)));

                        continue;
                    }
                    // Validating against individual files size limitation.
                    else if (context.MediaLibraryUploadSettings.MaximumSizeKB > 0 && file.ContentLength > context.MediaLibraryUploadSettings.MaximumSizeKB * 1024)
                    {
                        context.Updater.AddModelError("FileSizeLimitExceeded", T("The file \"{0}\" was not uploaded, because its size exceeds the {1} KB limitation.", file.FileName, context.MediaLibraryUploadSettings.MaximumSizeKB));

                        continue;
                    }
                    // Checking against image-specific settings.
                    else if (MimeAssistant.GetMimeType(file.FileName.ToLowerInvariant()).StartsWith("image"))
                    {
                        using (var image = Image.FromStream(file.InputStream))
                        {
                            if ((context.MediaLibraryUploadSettings.ImageMaximumWidth > 0 && image.Width > context.MediaLibraryUploadSettings.ImageMaximumWidth) || (context.MediaLibraryUploadSettings.ImageMaximumHeight > 0 && image.Height > context.MediaLibraryUploadSettings.ImageMaximumHeight))
                            {
                                context.Updater.AddModelError("ImageError", T("The image \"{0}\" was not uploaded, because its dimensions exceed the limitations. The maximum allowed file dimensions are {1}x{2} pixels.",
                                    file.FileName, context.MediaLibraryUploadSettings.ImageMaximumWidth, context.MediaLibraryUploadSettings.ImageMaximumHeight));

                                continue;
                            }
                        }

                        file.InputStream.Position = 0;
                    }

                    // The individual file is valid, further validation will happen later.
                    validFiles.Add(files[i]);
                    uploadingFilesSizeInBytes += files[i].ContentLength;
                }
            }

            #endregion

            if (validFiles.Any())
            {
                var canUpload = true;

                #region Validating against the setting to allow multiple files to uploaded/selected.

                var alreadyUploadedFiles = context.AlreadyUploadedFiles;
                if (!context.MediaLibraryUploadSettings.Multiple && validFiles.Count + alreadyUploadedFiles.Count > 1)
                {
                    context.Updater.AddModelError("MultipleItemsNotAllowed", T("This field only allows one file to be selected. Please remove content before adding another."));
                    canUpload = false;
                }

                #endregion

                #region Validating against field storage user quota.

                // Determining the path to the folder where this field stores files and should save new ones.
                var folderPath = context.FolderPath;
                folderPath = string.IsNullOrEmpty(folderPath) ? "UserUploads/" + workContext.CurrentUser.Id : folderPath;
                long uploadedFilesSizeInBytes = 0;

                // Calculating the size of the files that are already uploaded for this field.
                if (alreadyUploadedFiles.Any() && _storageProvider.FolderExists(folderPath))
                {
                    var storedFiles = _storageProvider.ListFiles(folderPath);

                    _storageProvider.ListFiles(folderPath)
                        .Where(stored => alreadyUploadedFiles.Any(uploaded => uploaded.FileName == stored.GetName()))
                        .Select(file =>
                        {
                            uploadedFilesSizeInBytes += file.GetSize();

                            return file;
                        });
                }

                long fieldStorageUserQuotaInBytes = context.MediaLibraryUploadSettings.FieldStorageUserQuotaMB * 1024 * 1024;

                // Checking if the size of all uploaded files and stored files exceed the limit for this field.
                if (fieldStorageUserQuotaInBytes > 0 && uploadedFilesSizeInBytes + uploadingFilesSizeInBytes > fieldStorageUserQuotaInBytes)
                {
                    context.Updater.AddModelError("FieldStorageUserQuotaExceeded", T("The files were not uploaded, because their size and the alrady stored files size exceed the {0} MB limitation by {1} bytes.",
                        context.MediaLibraryUploadSettings.FieldStorageUserQuotaMB, (int)((uploadedFilesSizeInBytes + uploadingFilesSizeInBytes) - fieldStorageUserQuotaInBytes)));
                    canUpload = false;
                }

                #endregion

                // The files comply with the settings and limitations, so we can proceed with importing them.
                if (canUpload)
                {
                    var mediaPartsCreated = new List<MediaPart>();

                    foreach (var file in validFiles)
                    {
                        var mediaPart = _mediaLibraryService.ImportMedia(file.InputStream, folderPath, file.FileName);
                        _contentManager.Create(mediaPart);
                        mediaPartsCreated.Add(mediaPart);
                    }

                    context.StoreIds(mediaPartsCreated.Select(m => m.ContentItem.Id));

                    _notifier.Information(T("The following items were successfully uploaded: {0}.", string.Join(", ", mediaPartsCreated.Select(mp => mp.FileName))));
                }
            }
        }
    }
}