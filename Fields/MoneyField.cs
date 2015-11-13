using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lombiq.Fields.Fields
{
    [OrchardFeature("Lombiq.Fields.MoneyField")]
    public class MoneyField : ContentField
    {
        public Decimal? Value
        {
            get { return Storage.Get<Decimal?>("amount"); }
            set { Storage.Set("amount", value); }
        }

        public string DefaultCurrencyIsoCode
        {
            get { return Storage.Get<string>("DefaultCurrencyIsoCode"); }
            set { Storage.Set("DefaultCurrencyIsoCode", value); }
        }

        private readonly LazyField<Money> _moneyPart = new LazyField<Money>();
        internal LazyField<Money> MoneyPartField { get { return _moneyPart; } }
        public Money MoneyPart
        {
            get { return _moneyPart.Value; }
        }

        private readonly LazyField<Currency> _defaultCurrency = new LazyField<Currency>();
        internal LazyField<Currency> DefaultCurrencyField { get { return _defaultCurrency; } }
        public Currency DefaultCurrency
        {
            get { return _defaultCurrency.Value; }
        }
    }
}
