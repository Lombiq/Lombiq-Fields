using System.Linq;
using Lombiq.Fields.Fields;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.MetaData;
using Orchard.Environment.Extensions;
using System;

namespace Lombiq.Fields.Handlers
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public class MoneyFieldHandler : ContentHandler
    {
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public MoneyFieldHandler(IContentManager contentManager, IContentDefinitionManager contentDefinitionManager)
        {
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
        }


        protected override void Loaded(LoadContentContext context)
        {
            base.Loaded(context);

            var fields = context.ContentItem.Parts.SelectMany(x => x.Fields.OfType<MoneyField>());

            if (_contentDefinitionManager.GetTypeDefinition(context.ContentItem.ContentType) == null) return;

            foreach (var field in fields)
            {
                field.DefaultCurrencyField.Loader(() =>
                {
                    Currency parsedCurrency;

                    return Currency.TryParse(field.DefaultCurrencyIsoCode, out parsedCurrency) ? parsedCurrency : Currency.FromCurrentCulture();
                });

                field.MoneyPartField.Loader(() =>
                {
                    return new Money(field.Value.Value, field.DefaultCurrency);
                });
            }
        }
    }
}
