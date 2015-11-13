using Lombiq.Fields.Fields;
using Lombiq.Fields.Settings;
using Lombiq.Fields.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lombiq.Fields.Drivers
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public class MoneyFieldDriver : ContentFieldDriver<MoneyField>
    {
        public IOrchardServices Services { get; set; }
        private const string TemplateName = "Fields/MoneyField.Edit";
        private readonly Lazy<CultureInfo> _cultureInfo;

        public MoneyFieldDriver(IOrchardServices services)
        {
            Services = services;
            T = NullLocalizer.Instance;

            _cultureInfo = new Lazy<CultureInfo>(() => CultureInfo.GetCultureInfo(Services.WorkContext.CurrentCulture));
        }

        public Localizer T { get; set; }

        private static string GetPrefix(ContentField field, ContentPart part)
        {
            return part.PartDefinition.Name + "." + field.Name;
        }

        private static string GetDifferentiator(MoneyField field, ContentPart part)
        {
            return field.Name;
        }

        protected override DriverResult Display(ContentPart part, MoneyField field, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Fields_Money", GetDifferentiator(field, part), () =>
            {
                return shapeHelper.Fields_Money()
                    .Settings(field.PartFieldDefinition.Settings.GetModel<MoneyFieldSettings>())
                    .Value(Convert.ToString(field.Value, _cultureInfo.Value))
                    .DefaultCurrency(field.DefaultCurrency.ToString());
            });
        }

        protected override DriverResult Editor(ContentPart part, MoneyField field, dynamic shapeHelper)
        {

            return ContentShape("Fields_Money_Edit", GetDifferentiator(field, part),
                () =>
                {
                    var model = new MoneyFieldViewModel
                    {
                        Field = field,
                        Settings = field.PartFieldDefinition.Settings.GetModel<MoneyFieldSettings>(),
                        Value = Convert.ToString(field.Value, _cultureInfo.Value),
                        CurrencyIsoCode = field.DefaultCurrency.ToString()
                    };

                    return shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: model, Prefix: GetPrefix(field, part));
                });
        }

        protected override DriverResult Editor(ContentPart part, MoneyField field, IUpdateModel updater, dynamic shapeHelper)
        {
            var viewModel = new MoneyFieldViewModel();
            var settings = field.PartFieldDefinition.Settings.GetModel<MoneyFieldSettings>();

            if (updater.TryUpdateModel(viewModel, GetPrefix(field, part), null, null))
            {
                if (!string.IsNullOrEmpty(viewModel.CurrencyIsoCode))
                {
                    Currency parsedCurrency;
                    if (!Currency.TryParse(viewModel.CurrencyIsoCode, out parsedCurrency))
                    {
                        updater.AddModelError("InvalidCurrencyIsoCode",
                            T("Invalid currency iso code was given."));
                    }
                }

                Decimal value;
                if (Decimal.TryParse(viewModel.Value, NumberStyles.Any, _cultureInfo.Value, out value))
                {
                    field.Value = value;
                }
                else
                {
                    updater.AddModelError(GetPrefix(field, part), T("{0} is an invalid number", field.DisplayName));
                    field.Value = null;
                }
            }

            return Editor(part, field, shapeHelper);
        }

        protected override void Importing(ContentPart part, MoneyField field, ImportContentContext context)
        {
            context.ImportAttribute(field.FieldDefinition.Name + "." + field.Name, "Value", v => field.Value = decimal.Parse(v, CultureInfo.InvariantCulture), () => field.Value = (decimal?)null);
        }

        protected override void Exporting(ContentPart part, MoneyField field, ExportContentContext context)
        {
            if (field.Value.HasValue)
                context.Element(field.FieldDefinition.Name + "." + field.Name).SetAttributeValue("Value", field.Value.Value.ToString(CultureInfo.InvariantCulture));
        }

        protected override void Describe(DescribeMembersContext context)
        {
            context
                .Member(null, typeof(decimal), T("Value"), T("The value of the field."))
                .Enumerate<MoneyField>(() => field => new[] { field.Value });
        }
    }
}
