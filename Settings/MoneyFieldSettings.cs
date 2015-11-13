using Orchard.Environment.Extensions;
using System;

namespace Lombiq.Fields.Settings
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public class MoneyFieldSettings
    {
        public string DefaultCurrency { get; set; }
        public bool IsCurrencyReadOnly { get; set; }

        public MoneyFieldSettings()
        {
            DefaultCurrency = Currency.Usd.Iso3LetterCode;
            IsCurrencyReadOnly = false;
        }
    }
}
