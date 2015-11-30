using Lombiq.Fields.Fields;
using Lombiq.Fields.Settings;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.MetaData;
using Orchard.Environment.Extensions;
using System;
using System.Linq;

namespace Lombiq.Fields.Handlers
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public class MoneyFieldHandler : ContentHandler
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;


        public MoneyFieldHandler(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }


        protected override void Loaded(LoadContentContext context)
        {
            base.Loaded(context);

            var fields = context.ContentItem.Parts.SelectMany(x => x.Fields.OfType<MoneyField>());

            if (_contentDefinitionManager.GetTypeDefinition(context.ContentItem.ContentType) == null) return;

            foreach (var field in fields)
            {
                field.MoneyValueField.Loader(() =>
                {
                    Currency parsedCurrency;

                    Currency.TryParse(
                        string.IsNullOrEmpty(field.CurrencyIso3LetterCode)
                            ? field.PartFieldDefinition.Settings.GetModel<MoneyFieldSettings>().DefaultCurrency
                            : field.CurrencyIso3LetterCode,
                        out parsedCurrency);

                    return new Money(field.Amount, parsedCurrency);
                });
            }
        }
    }
}
