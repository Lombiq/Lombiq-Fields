using Lombiq.Fields.Fields;
using Lombiq.Fields.Settings;
using Orchard.Environment.Extensions;
using System;

namespace Lombiq.Fields.ViewModels
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public class MoneyFieldViewModel
    {
        public MoneyField Field { get; set; }

        public MoneyFieldSettings Settings { get; set; }

        public string CurrencyIso3LetterCode { get; set; }

        public decimal Amount { get; set; }

        public bool SynchroniseWithDefaultCurrency { get; set; }
    }
}
