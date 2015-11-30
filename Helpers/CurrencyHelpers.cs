using Lombiq.Fields.Constants;
using Orchard.Environment.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Lombiq.Fields.Helpers
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public static class CurrencyHelpers
    {
        /// <summary>
        /// returns a List with SelecListItems from the constans Currencies.
        /// Its needed for the dropdown as source in he MoneyFieldSettings.cshtml
        /// </summary>
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