using Lombiq.Fields.Elements;
using Lombiq.Fields.Helpers;
using Lombiq.Fields.Models;
using Lombiq.Fields.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.DynamicForms.Services;
using Orchard.DynamicForms.Services.Models;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.MediaLibrary.Models;
using Orchard.Tokens;
using Orchard.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lombiq.Fields.Handlers
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField.DynamicForms")]
    public class MediaLibraryUploadFieldElementDynamicFormEventHandler : IDynamicFormEventHandler
    {
        private readonly IMediaLibraryUploadService _mediaLibraryUploadService;
        private readonly IContentManager _contentManager;
        private readonly ITokenizer _tokenizer;
        private readonly IWorkContextAccessor _wca;


        public Localizer T { get; set; }


        public MediaLibraryUploadFieldElementDynamicFormEventHandler(
            IMediaLibraryUploadService mediaLibraryUploadService,
            IContentManager contentManager,
            ITokenizer tokenizer,
            IWorkContextAccessor wca)
        {
            _mediaLibraryUploadService = mediaLibraryUploadService;
            _contentManager = contentManager;
            _tokenizer = tokenizer;
            _wca = wca;

            T = NullLocalizer.Instance;
        }


        public void Submitted(FormSubmittedEventContext context)
        {
            var element = context
                .Form
                .Elements
                .FirstOrDefault(formElement => formElement.Type == "Lombiq.Fields.Elements.MediaLibraryUploadField");
            if (element == null) return;

            var mediaLibraryUploadElement = element as MediaLibraryUploadField;

            var workContext = _wca.GetContext();
            var selectedIdsHiddenInputValue =
                workContext.HttpContext.Request.Form[MediaLibraryUploadField.NameForSelectedIdsHiddenInput];
            mediaLibraryUploadElement.Ids =
                string.IsNullOrEmpty(selectedIdsHiddenInputValue)
                ? new int[0]
                : selectedIdsHiddenInputValue
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

            _mediaLibraryUploadService.Handle(new MediaLibraryUploadFieldPostHandlerContext
            {
                FileFieldName = $"MediaLibraryUploadField-{mediaLibraryUploadElement.Name}[]",
                AllowedExtensions = mediaLibraryUploadElement.AllowedExtensions,
                Updater = context.Updater,
                MaximumSizeKB = mediaLibraryUploadElement.MaximumSizeKB,
                ImageMaximumWidth = mediaLibraryUploadElement.ImageMaximumWidth,
                ImageMaximumHeight = mediaLibraryUploadElement.ImageMaximumHeight,
                AlreadyUploadedFiles =
                    _contentManager
                    .GetMany<MediaPart>(mediaLibraryUploadElement.Ids, VersionOptions.Published, QueryHints.Empty)
                    .ToList(),
                Multiple = mediaLibraryUploadElement.Multiple,
                FolderPath = _tokenizer.Replace(mediaLibraryUploadElement.FolderPath, new Dictionary<string, object>
                    {
                        { "User", workContext.CurrentUser }
                    }),
                FieldStorageUserQuotaMB = mediaLibraryUploadElement.FieldStorageUserQuotaMB,
                Required = mediaLibraryUploadElement.Required,
                StoreIds = (ids) => mediaLibraryUploadElement.Ids = mediaLibraryUploadElement.Ids.Union(ids).ToArray(),
            });

            if (mediaLibraryUploadElement.Required && mediaLibraryUploadElement.Ids.Length == 0)
            {
                context.Updater.AddModelError("Id", T("You need to have or upload at least one file for the field {0}.", mediaLibraryUploadElement.Name.CamelFriendly()));
            }

            // Set the element's value so the submission event handler can find the value for this element. 
            context.Values[mediaLibraryUploadElement.Name] =
                MediaLibraryUploadHelper.EncodeIds(mediaLibraryUploadElement.Ids);
        }

        public void Validated(FormValidatedEventContext context)
        {
        }

        public void Validating(FormValidatingEventContext context)
        {
        }
    }
}