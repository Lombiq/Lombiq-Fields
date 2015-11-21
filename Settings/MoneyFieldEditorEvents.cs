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
                var model = definition.Settings.GetModel<MoneyFieldSettings>();
                yield return DefinitionTemplate(model);
            }
        }

        public override IEnumerable<TemplateViewModel> PartFieldEditorUpdate(ContentPartFieldDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (!builder.FieldType.Equals(typeof(MoneyField).Name)) yield break;

            var model = new MoneyFieldSettings();

            if (updateModel.TryUpdateModel(model, typeof(MoneyFieldSettings).Name, null, null))
            {
                if (!String.IsNullOrEmpty(model.DefaultCurrency))
                {
                    Currency parsedCurrency;
                    if (Currency.TryParse(model.DefaultCurrency, out parsedCurrency))
                    {
                        builder.WithSetting("MoneyFieldSettings.DefaultCurrency", model.DefaultCurrency);
                        builder.WithSetting("MoneyFieldSettings.IsCurrencyReadOnly", model.IsCurrencyReadOnly.ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        updateModel.AddModelError("InvalidCurrencyIsoCode", T("MoneyField - Invalid currency iso code was given."));
                    }
                }
                else
                {
                    updateModel.AddModelError("DefaultIsoCodeIsEmpty", T("MoneyField - Currency iso code was not given."));
                }
            }
            yield return DefinitionTemplate(model);
        }

    }
}