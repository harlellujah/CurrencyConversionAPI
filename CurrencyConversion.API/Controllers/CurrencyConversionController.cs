using CurrencyConversion.Business.ApiManagement;
using CurrencyConversion.Business.Entities.Api;
using CurrencyConversion.Common.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConversion.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class CurrencyConversionController : ControllerBase
    {
        private readonly ILogger<CurrencyConversionController> _logger;

        private readonly IApiService _apiService;

        private readonly IMemoryCache _cache;

        public CurrencyConversionController(ILogger<CurrencyConversionController> logger, IApiService apiService, IMemoryCache cache)
        {
            _logger = logger;
            this._apiService = apiService;
            // JH - I was going to cache the rates rather than call the 3rd party API every time, but this use case seems like it should be live data
            this._cache = cache;
        }

        [HttpGet]
        [Route("Convert/{baseCurrency}/{targetCurrency}/{baseValue}")]
        public ConvertResponse Convert(CurrencyType baseCurrency, CurrencyType targetCurrency, double baseValue)
        {
            return _apiService.TryConvertValue(baseValue, baseCurrency.ToString(), targetCurrency.ToString());
        }
    }
}
