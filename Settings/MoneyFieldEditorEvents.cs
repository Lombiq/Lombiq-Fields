using Lombiq.Fields.Fields;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lombiq.Fields.Settings
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public class MoneyFieldEditorEvents : ContentDefinitionEditorEventsBase
    {
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
                builder.WithSetting("MoneyFieldSettings.DefaultCurrency", model.DefaultCurrency);
                builder.WithSetting("MoneyFieldSettings.IsCurrencyReadOnly", model.IsCurrencyReadOnly.ToString(CultureInfo.InvariantCulture));
            }

            yield return DefinitionTemplate(model);
        }

    }
}
