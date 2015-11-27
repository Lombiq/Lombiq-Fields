using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;
using Orchard.Environment.Extensions;
using System;

namespace Lombiq.Fields.Fields
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public class MoneyField : ContentField
    {
        public decimal Amount
        {
            get { return Storage.Get<decimal>("Amount"); }
            set { Storage.Set("Amount", value); }
        }

        public string CurrencyIso3LetterCode
        {
            get { return Storage.Get<string>("CurrencyIso3LetterCode"); }
            set { Storage.Set("CurrencyIso3LetterCode", value); }
        }

        private readonly LazyField<Money> _moneyValue = new LazyField<Money>();
        internal LazyField<Money> MoneyValueField { get { return _moneyValue; } }
        public Money MoneyValue
        {
            get { return _moneyValue.Value; }
        }
    }
}
