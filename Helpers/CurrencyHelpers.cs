using Orchard.Environment.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lombiq.Fields.Constants;

namespace Lombiq.Fields.Helpers
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public static class CurrencyHelpers
    {
        public static IEnumerable<SelectListItem> GetCurrencySelectListItems
        {
            get
            {
                return MoneyFieldConstants.Currencies.Select(c =>
                                        new SelectListItem()
                                        {
                                            Text = c.ToString(),
                                            Value = c.Iso3LetterCode
                                        }).OrderBy(c => c.Text);
            }
        }
    }
}