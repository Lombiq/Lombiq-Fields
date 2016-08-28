using System.Collections.Generic;
using Orchard.DynamicForms.Elements;
using Orchard.Layouts.Framework.Display;
using Orchard.Layouts.Framework.Drivers;
using Orchard.Layouts.Helpers;
using Orchard.Layouts.Services;
using Orchard.Tokens;
using DescribeContext = Orchard.Forms.Services.DescribeContext;

namespace Orchard.DynamicForms.Drivers
{
    public class MediaLibraryUploadFieldElementDriver : FormsElementDriver<MediaLibraryUploadField>
    {
        private readonly ITokenizer _tokenizer;

        public MediaLibraryUploadFieldElementDriver(IFormsBasedElementServices formsServices, ITokenizer tokenizer) : base(formsServices) {
            _tokenizer = tokenizer;
        }

        protected override IEnumerable<string> FormNames
        {
            get
            {
                yield return "AutoLabel";
            }
        }

        protected override EditorResult OnBuildEditor(MediaLibraryUploadField element, ElementEditorContext context)
        {
            var formShape = BuildForms(context);
            var mediaFieldEditor = BuildForm(context, "MediaLibraryUploadField");
            var mediaButtonValidation = BuildForm(context, "MediaLibraryUploadFieldValidation", "Validation:10");

            return Editor(context, formShape, mediaFieldEditor, mediaButtonValidation);
        }

        protected override void DescribeForm(DescribeContext context)
        {
            context.Form("MediaLibraryUploadFieldValidation", factory => {
                var shape = (dynamic)factory;
                var form = shape.Fieldset(
                    Id: "MediaLibraryUploadFieldValidation",
                    _IsRequired: shape.Checkbox(
                        Id: "IsRequired",
                        Name: "IsRequired",
                        Title: "Required",
                        Value: "true",
                        Description: T("Check to make this text field a required field.")),
                    _CustomValidationMessage: shape.Textbox(
                        Id: "CustomValidationMessage",
                        Name: "CustomValidationMessage",
                        Title: "Custom Validation Message",
                        Classes: new[] { "text", "large", "tokenized" },
                        Description: T("Optionally provide a custom validation message.")),
                    _ShowValidationMessage: shape.Checkbox(
                        Id: "ShowValidationMessage",
                        Name: "ShowValidationMessage",
                        Title: "Show Validation Message",
                        Value: "true",
                        Description: T("Autogenerate a validation message when a validation error occurs for the current field. Alternatively, to control the placement of the validation message you can use the ValidationMessage element instead.")));

                return form;
            });
        }

        protected override void OnDisplaying(MediaLibraryUploadField element, ElementDisplayingContext context)
        {
            context.ElementShape.ProcessedType = "MediaLibraryUploadField";
            context.ElementShape.ProcessedName = _tokenizer.Replace(element.Name, context.GetTokenData());
            context.ElementShape.ProcessedLabel = _tokenizer.Replace(element.Label, context.GetTokenData(), new ReplaceOptions { Encoding = ReplaceOptions.NoEncode });
        }
    }
}