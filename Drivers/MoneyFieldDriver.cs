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
        private readonly IOrchardServices _orchardServices;


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
                    shapeHelper.Fields_MoneyField(Settings: field.PartFieldDefinition.Settings.GetModel<MoneyFieldSettings>()));
        }

        protected override DriverResult Editor(ContentPart part, MoneyField field, dynamic shapeHelper)
        {
            return ContentShape("Fields_MoneyField_Edit", GetDifferentiator(field, part),
                () =>
                {
                    var settings = field.PartFieldDefinition.Settings.GetModel<MoneyFieldSettings>();
                    var model = new MoneyFieldViewModel
                    {
                        Field = field,
                        Settings = settings,
                        Amount = field.Amount,
                        CurrencyIso3LetterCode = string.IsNullOrEmpty(field.CurrencyIso3LetterCode) ? settings.DefaultCurrency : field.CurrencyIso3LetterCode
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
                    if (!string.IsNullOrEmpty(viewModel.CurrencyIso3LetterCode))
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
                    if (string.IsNullOrEmpty(field.CurrencyIso3LetterCode) || viewModel.SynchroniseWithDefaultCurrency)
                    {
                        field.CurrencyIso3LetterCode = settings.DefaultCurrency;
                    }
                }

                field.Amount = viewModel.Amount;
            }
            return Editor(part, field, shapeHelper);
        }


        protected override void Importing(ContentPart part, MoneyField field, ImportContentContext context)
        {
            context.ImportAttribute(field.FieldDefinition.Name + "." + field.Name, "Amount", amount => field.Amount = decimal.Parse(amount, CultureInfo.InvariantCulture), () => field.Amount = 0);
            context.ImportAttribute(field.FieldDefinition.Name + "." + field.Name, "CurrencyIso3LetterCode", currency => field.CurrencyIso3LetterCode = !string.IsNullOrEmpty(currency) ? currency : Currency.FromCurrentCulture().Iso3LetterCode);

        }

        protected override void Exporting(ContentPart part, MoneyField field, ExportContentContext context)
        {
            context.Element(field.FieldDefinition.Name + "." + field.Name).SetAttributeValue("Amount", field.Amount.ToString(CultureInfo.InvariantCulture));
            context.Element(field.FieldDefinition.Name + "." + field.Name).SetAttributeValue("CurrencyIso3LetterCode", !string.IsNullOrEmpty(field.CurrencyIso3LetterCode) ? field.CurrencyIso3LetterCode : Currency.FromCurrentCulture().Iso3LetterCode);
        }

        protected override void Describe(DescribeMembersContext context)
        {
            context.Member("Amount", typeof(decimal), T("Amount"), T("The amount of the money."));
            context.Member("Currency", typeof(string), T("Currency"), T("The currency of the money."));
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
