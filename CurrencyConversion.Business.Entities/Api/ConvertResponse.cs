using CurrencyConversion.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyConversion.Business.Entities.Api
{
    public class ConvertResponse
    {
        public CurrencyType BaseCurrency { get; set; }

        public CurrencyType TargetCurrency { get; set; }

        public double ExchangeRate { get; set; }

        public double BaseValue { get; set; }

        public double ConvertedValue { get; set; }
    }
}
