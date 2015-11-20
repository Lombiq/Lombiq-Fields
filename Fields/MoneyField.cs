using System;
using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;
using Orchard.Environment.Extensions;

namespace Lombiq.Fields.Fields
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public class MoneyField : ContentField
    {
        public Decimal Amount
        {
            get { return Storage.Get<Decimal>("Amount"); }
            set { Storage.Set("Amount", value); }
        }

        public string CurrencyIso3LetterCode
        {
            get { return Storage.Get<string>("CurrencyIso3LetterCode"); }
            set { Storage.Set("CurrencyIso3LetterCode", value); }
        }

        private readonly LazyField<Money> _moneyPart = new LazyField<Money>();
        internal LazyField<Money> MoneyPartField { get { return _moneyPart; } }
        public Money MoneyPart
        {
            get { return _moneyPart.Value; }
        }
    }
}
