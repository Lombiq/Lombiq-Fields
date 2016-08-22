using Orchard.DynamicForms.Validators.Settings;
using Orchard.Localization;

namespace Orchard.DynamicForms.Elements
{
    public class MediaLibraryUploadField : LabeledFormElement
    {
        public MediaLibraryUploadFieldValidationSettings ValidationSettings
        {
            get { return Data.GetModel<MediaLibraryUploadFieldValidationSettings>(""); }
        }

        public override LocalizedString DisplayText
        {
            get { return T("Media Upload Field"); }
        }
    }
}