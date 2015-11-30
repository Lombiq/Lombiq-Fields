using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Lombiq.Fields.Helpers
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public static class CurrencyHelpers
    {
        /// <summary>
        /// Returns a list of SelectListItems built from the list of available Currencies
        /// for the DropDownList in MoneyFieldSettings.cshtml.
        /// </summary>
        public static IEnumerable<SelectListItem> GetCurrencySelectListItems
        {
            get
            {
                object currencyObj = new Currency();

                return typeof(Currency).GetFields()
                    .Where(currency => currency.Name != "None" && currency.Name != "Xxx")
                    .Select(currency =>
                                new SelectListItem()
                                {
                                    Text = ((Currency)currency.GetValue(currencyObj)).ToString(),
                                    Value = ((Currency)currency.GetValue(currencyObj)).Iso3LetterCode
                                }).OrderBy(listitem => listitem.Text);
            }
        }
    }
}