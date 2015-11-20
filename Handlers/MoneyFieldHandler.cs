using System.Linq;
using Lombiq.Fields.Fields;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.MetaData;
using Orchard.Environment.Extensions;
using System;
using Orchard.Data;

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
                field.MoneyPartField.Loader(() =>
                {
                    Currency parsedCurrency;
                    return new Money(field.Amount, !String.IsNullOrEmpty(field.CurrencyIso3LetterCode) ?
                        Currency.TryParse(field.CurrencyIso3LetterCode, out parsedCurrency) ?
                        parsedCurrency : Currency.FromCurrentCulture() : Currency.FromCurrentCulture());
                });
            }
        }
    }
}
