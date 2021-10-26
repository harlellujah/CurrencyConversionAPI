using CurrencyConversion.Business.Entities.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConversion.Business.ApiManagement
{
    public interface IApiService
    {
        ConvertResponse TryConvertValue(double baseValue, string baseCurrency, string targetCurrency);
    }
}
