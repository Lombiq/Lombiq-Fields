using Orchard.DynamicForms.Elements;
using Orchard.DynamicForms.Services;
using System.Collections.Generic;
using Orchard.DynamicForms.ValidationRules;

namespace Orchard.DynamicForms.Validators
{
    public class MediaLibraryUploadFieldValidator : ElementValidator<MediaLibraryUploadField>
    {
        private readonly IValidationRuleFactory _validationRuleFactory;
        public MediaLibraryUploadFieldValidator(IValidationRuleFactory validationRuleFactory)
        {
            _validationRuleFactory = validationRuleFactory;
        }

        protected override IEnumerable<IValidationRule> GetValidationRules(MediaLibraryUploadField element)
        {
            var settings = element.ValidationSettings;

            if (settings.IsRequired == true)
                yield return _validationRuleFactory.Create<Required>(settings.CustomValidationMessage);
        }
    }
}