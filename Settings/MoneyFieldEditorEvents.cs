using Lombiq.Fields.Fields;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Lombiq.Fields.Settings
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public class MoneyFieldEditorEvents : ContentDefinitionEditorEventsBase
    {
        public Localizer T { get; set; }


        public override IEnumerable<TemplateViewModel> PartFieldEditor(ContentPartFieldDefinition definition)
        {
            if (definition.FieldDefinition.Name.Equals(typeof(MoneyField).Name))
            {
                yield return DefinitionTemplate(definition.Settings.GetModel<MoneyFieldSettings>());
            }
        }

        public override IEnumerable<TemplateViewModel> PartFieldEditorUpdate(ContentPartFieldDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (!builder.FieldType.Equals(typeof(MoneyField).Name)) yield break;

            var model = new MoneyFieldSettings();

            if (updateModel.TryUpdateModel(model, typeof(MoneyFieldSettings).Name, null, null))
            {
                if (string.IsNullOrEmpty(model.DefaultCurrency))
                {
                    builder.WithSetting("MoneyFieldSettings.DefaultCurrency", Currency.FromCurrentCulture().Iso3LetterCode);
                }
                else
                {
                    Currency parsedCurrency;
                    if (Currency.TryParse(model.DefaultCurrency, out parsedCurrency))
                    {
                        builder.WithSetting("MoneyFieldSettings.DefaultCurrency", model.DefaultCurrency);
                    }
                    else
                    {
                        updateModel.AddModelError("InvalidCurrencyIsoCode", T("MoneyField - Invalid currency iso code was given."));
                    }
                }

                builder.WithSetting("MoneyFieldSettings.IsCurrencyReadOnly", model.IsCurrencyReadOnly.ToString(CultureInfo.InvariantCulture));
                builder.WithSetting("MoneyFieldSettings.Hint", model.Hint);
                builder.WithSetting("MoneyFieldSettings.Required", model.Required.ToString(CultureInfo.InvariantCulture));
            }
            yield return DefinitionTemplate(model);
        }

    }
}