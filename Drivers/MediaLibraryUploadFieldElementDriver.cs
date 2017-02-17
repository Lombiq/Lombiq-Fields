using Lombiq.Fields.Elements;
using Lombiq.Fields.Helpers;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Layouts.Framework.Display;
using Orchard.Layouts.Framework.Drivers;
using Orchard.Layouts.Helpers;
using Orchard.Layouts.Services;
using Orchard.Tokens;
using DescribeContext = Orchard.Forms.Services.DescribeContext;

namespace Lombiq.Fields.Drivers
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField.DynamicForms")]
    public class MediaLibraryUploadFieldElementDriver : FormsElementDriver<MediaLibraryUploadField>
    {
        private readonly ITokenizer _tokenizer;
        private readonly IContentManager _contentManager;


        public MediaLibraryUploadFieldElementDriver(IFormsBasedElementServices formsServices, ITokenizer tokenizer, IContentManager contentManager) : base(formsServices)
        {
            _tokenizer = tokenizer;
            _contentManager = contentManager;
        }


        protected override EditorResult OnBuildEditor(MediaLibraryUploadField element, ElementEditorContext context) =>
            Editor(context, BuildForm(context, "MediaLibraryUploadField"));

        protected override void DescribeForm(DescribeContext context)
        {
            context.Form("MediaLibraryUploadField", factory =>
            {
                var shape = (dynamic)factory;
                var form = shape.Fieldset(
                    Id: "MediaLibraryUploadField",
                    _Required: shape.Checkbox(
                        Id: "Required",
                        Name: "Required",
                        Title: "At least one media item is required",
                        Value: "true",
                        Description: T("Check to ensure that the user is providing at least one media item."),
                        Classes: new[] { "forcheckbox" }),
                    _Multiple: shape.Checkbox(
                        Id: "Multiple",
                        Name: "Multiple",
                        Title: "Allow multiple items to be uploaded",
                        Value: "true",
                        Description: T("Check to allow the user to upload multiple items."),
                        Classes: new[] { "forcheckbox" }),
                    _DisplayName: shape.Textbox(
                        Id: "DisplayName",
                        Name: "DisplayName",
                        Title: "Display name",
                        Description: T("The display name of the field."),
                        Classes: new[] { "text", "medium" }),
                    _Hint: shape.Textbox(
                        Id: "Hint",
                        Name: "Hint",
                        Title: "Help text",
                        Classes: new[] { "text", "medium" },
                        Description: T("The help text is written under the field when an author is uploading a file.")),
                    _AllowedExtensions: shape.Textbox(
                        Id: "AllowedExtensions",
                        Name: "AllowedExtensions",
                        Title: "Allowed file extensions",
                        Classes: new[] { "text", "medium" },
                        Description: T("A comma-separated list of the file extensions that are allowed to be uploaded. Leaving it empty will make the field to accept all file types.")),
                    _FolderPath: shape.Textbox(
                        Id: "FolderPath",
                        Name: "FolderPath",
                        Title: "Folder Path",
                        Classes: new[] { "text", "medium", "tokenized" },
                        Description: T("Provide a path to a folder where the files will be uploaded. You can also use tokens. The default path is \"UserUploads/[user ID]\".")),
                    _MaximumSizeKB: shape.Textbox(
                        Id: "MaximumSizeKB",
                        Name: "MaximumSizeKB",
                        Title: "",
                        Description: T("The maximum file size per uploaded file in kilobytes. 0 means no limit.")),
                    _FieldStorageUserQuotaMB: shape.Textbox(
                        Id: "FieldStorageUserQuotaMB",
                        Name: "FieldStorageUserQuotaMB",
                        Title: "Maximum size of all uploaded files for this field",
                        Description: T("The maximum size of all uploaded files for this field in megabytes. 0 means no limit.")),
                    _ImageMaximumWidth: shape.Textbox(
                        Id: "ImageMaximumWidth",
                        Name: "ImageMaximumWidth",
                        Title: "Maximum width",
                        Description: T("The maximum width for uploaded images in pixels. 0 means no limit.")),
                    _ImageMaximumHeight: shape.Textbox(
                        Id: "ImageMaximumHeight",
                        Name: "ImageMaximumHeight",
                        Title: "Maximum height",
                        Description: T("The maximum height for uploaded images in pixels. 0 means no limit.")),
                    _MediaProfile: shape.Textbox(
                        Id: "MediaProfile",
                        Name: "MediaProfile",
                        Title: "Media Profile",
                        Description: T("You can optionally specify the name of a Media Profile that will be applied to the images of this field when displaying them.")));

                return form;
            });
        }

        protected override void OnDisplaying(MediaLibraryUploadField element, ElementDisplayingContext context)
        {
            var contentItemIds = string.IsNullOrEmpty(element.RuntimeValue) ? new int[0] : MediaLibraryUploadHelper.DecodeIds(element.RuntimeValue);
            context.ElementShape.ContentItems = _contentManager.GetMany<ContentItem>(contentItemIds, VersionOptions.Published, QueryHints.Empty);
            context.ElementShape.ProcessedName = _tokenizer.Replace(element.Name, context.GetTokenData());
            context.ElementShape.ProcessedValue = element.RuntimeValue;
        }
    }
}