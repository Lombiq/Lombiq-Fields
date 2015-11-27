using Orchard.Environment.Extensions;
using System;

namespace Lombiq.Fields.Settings
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public class MoneyFieldSettings
    {
        public string DefaultCurrency { get; set; }
        public Boolean IsCurrencyReadOnly { get; set; }


        public MoneyFieldSettings()
        {
            DefaultCurrency = Currency.FromCurrentCulture().Iso3LetterCode;
            IsCurrencyReadOnly = false;
        }
    }
}
