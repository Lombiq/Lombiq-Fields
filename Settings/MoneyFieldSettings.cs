using Orchard.Environment.Extensions;
using System;

namespace Lombiq.Fields.Settings
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public class MoneyFieldSettings
    {
        public string Hint { get; set; }
        public bool Required { get; set; }
        public string DefaultCurrency { get; set; }
        public bool IsCurrencyReadOnly { get; set; }

        public MoneyFieldSettings()
        {
            DefaultCurrency = Currency.FromCurrentCulture().Iso3LetterCode;
            IsCurrencyReadOnly = false;
        }
    }
}
