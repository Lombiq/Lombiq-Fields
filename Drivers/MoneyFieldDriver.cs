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
using System.Globalization;

namespace Lombiq.Fields.Drivers
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public class MoneyFieldDriver : ContentFieldDriver<MoneyField>
    {
        private readonly Lazy<CultureInfo> _cultureInfo;
        private IOrchardServices _orchardServices { get; set; }

        public Localizer T { get; set; }


        public MoneyFieldDriver(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;

            _cultureInfo = new Lazy<CultureInfo>(() => CultureInfo.GetCultureInfo(_orchardServices.WorkContext.CurrentCulture));
        }



        protected override DriverResult Display(ContentPart part, MoneyField field, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Fields_MoneyField", GetDifferentiator(field, part), () =>
                    shapeHelper.Fields_MoneyField()
                    .Settings(field.PartFieldDefinition.Settings.GetModel<MoneyFieldSettings>())
                    .Value(Convert.ToString(field.Amount, _cultureInfo.Value))
                    .DefaultCurrency(field.CurrencyIso3LetterCode));
        }

        protected override DriverResult Editor(ContentPart part, MoneyField field, dynamic shapeHelper)
        {

            return ContentShape("Fields_MoneyField_Edit", GetDifferentiator(field, part),
                () =>
                {
                    var model = new MoneyFieldViewModel
                    {
                        Field = field,
                        Settings = field.PartFieldDefinition.Settings.GetModel<MoneyFieldSettings>(),
                        Amount = Convert.ToString(field.Amount, _cultureInfo.Value),
                        CurrencyIso3LetterCode = field.CurrencyIso3LetterCode
                    };

                    return shapeHelper.EditorTemplate(TemplateName: "Fields/MoneyField.Edit", Model: model, Prefix: GetPrefix(field, part));
                });
        }

        protected override DriverResult Editor(ContentPart part, MoneyField field, IUpdateModel updater, dynamic shapeHelper)
        {
            var viewModel = new MoneyFieldViewModel();
            var settings = field.PartFieldDefinition.Settings.GetModel<MoneyFieldSettings>();

            if (updater.TryUpdateModel(viewModel, GetPrefix(field, part), null, null))
            {
                if (!settings.IsCurrencyReadOnly)
                {
                    if (!String.IsNullOrEmpty(viewModel.CurrencyIso3LetterCode))
                    {
                        Currency parsedCurrency;
                        if (Currency.TryParse(viewModel.CurrencyIso3LetterCode, out parsedCurrency))
                        {
                            field.CurrencyIso3LetterCode = viewModel.CurrencyIso3LetterCode;
                        }
                        else
                        {
                            updater.AddModelError("InvalidCurrencyIsoCode", T("Invalid currency iso code was given."));
                        }
                    }
                    else
                    {
                        updater.AddModelError("CurrencyIsoCodeIsEmpty", T("Currency iso code was not given."));
                    }
                }
                else
                {
                    field.CurrencyIso3LetterCode = settings.DefaultCurrency;
                }

                Decimal amount;

                if (Decimal.TryParse(viewModel.Amount, NumberStyles.Any, _cultureInfo.Value, out amount))
                {
                    field.Amount = amount;
                }
                else
                {
                    updater.AddModelError(GetPrefix(field, part), T("{0} is an invalid number", viewModel.Amount));
                }
            }
            return Editor(part, field, shapeHelper);
        }


        protected override void Importing(ContentPart part, MoneyField field, ImportContentContext context)
        {
            context.ImportAttribute(field.FieldDefinition.Name + "." + field.Name, "Amount", v => field.Amount = decimal.Parse(v, CultureInfo.InvariantCulture), () => field.Amount = 0);
            context.ImportAttribute(field.FieldDefinition.Name + "." + field.Name, "CurrencyIso3LetterCode", v => field.CurrencyIso3LetterCode = !String.IsNullOrEmpty(v) ? v : Currency.FromCurrentCulture().Iso3LetterCode);

        }

        protected override void Exporting(ContentPart part, MoneyField field, ExportContentContext context)
        {
            context.Element(field.FieldDefinition.Name + "." + field.Name).SetAttributeValue("Amount", field.Amount.ToString(CultureInfo.InvariantCulture));
            context.Element(field.FieldDefinition.Name + "." + field.Name).SetAttributeValue("CurrencyIso3LetterCode", !String.IsNullOrEmpty(field.CurrencyIso3LetterCode) ? field.CurrencyIso3LetterCode : Currency.FromCurrentCulture().Iso3LetterCode);
        }

        protected override void Describe(DescribeMembersContext context)
        {
            context
                .Member(null, typeof(MoneyField), T("Value"), T("The value of the field."))
                .Enumerate<MoneyField>(() => field => new[] { field.Amount });
        }


        private static string GetPrefix(ContentField field, ContentPart part)
        {
            return part.PartDefinition.Name + "." + field.Name;
        }

        private static string GetDifferentiator(MoneyField field, ContentPart part)
        {
            return field.Name;
        }
    }
}
