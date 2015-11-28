using Orchard.Environment.Extensions;
using System.Collections.Generic;
using System.Linq;
using Lombiq.Fields.Constants;
using System.Web.Mvc;

namespace Lombiq.Fields.Helpers
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public static class CurrencyHelpers
    {
        public static IEnumerable<SelectListItem> GetCurrencySelectListItems
        {
            get
            {
                return MoneyFieldConstants.Currencies.Select(currency =>
                                        new SelectListItem()
                                        {
                                            Text = currency.ToString(),
                                            Value = currency.Iso3LetterCode
                                        }).OrderBy(listitem => listitem.Text);
            }
        }
    }
}