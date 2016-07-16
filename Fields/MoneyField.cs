using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using Orchard.Environment.Extensions;
using System;

namespace Lombiq.Fields.Fields
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public class MoneyField : ContentField
    {
        public decimal? Amount
        {
            get { return Storage.Get<decimal?>("Amount"); }
            set { Storage.Set("Amount", value); }
        }

        public string CurrencyIso3LetterCode
        {
            get { return Storage.Get<string>("CurrencyIso3LetterCode"); }
            set { Storage.Set("CurrencyIso3LetterCode", value); }
        }

        private readonly LazyField<Money?> _value = new LazyField<Money?>();
        internal LazyField<Money?> ValueField { get { return _value; } }
        public Money? Value
        {
            get { return _value.Value; }
        }
    }
}
