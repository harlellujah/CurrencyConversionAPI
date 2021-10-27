using CurrencyConversion.Business.ApiManagement;
using CurrencyConversion.Business.Entities.Api;
using CurrencyConversion.Common.AppSettings;
using CurrencyConversion.Common.Types;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConversion.Tests.ApiTests
{
    public class ApiTests
    {
        private readonly IApiService _apiService;

        public ApiTests()
        {
            var services = new ServiceCollection();

            AppSettings appSettings = new AppSettings()
            {
                CurrencyApiUrl = "https://free.currconv.com/api/v7/",
                CurrencyApiKey = "c4bed8d87e4743c7e1f9"
            };

            services.AddTransient<IApiService, ApiService>(s => new ApiService(appSettings));

            var serviceProvider = services.BuildServiceProvider();

            _apiService = serviceProvider.GetService<IApiService>();
        }

        [Test]
        public void TestInputsArePersisted()
        {
            double baseValue = 100;
            CurrencyType baseCurrency = CurrencyType.GBP;
            CurrencyType targetCurrency = CurrencyType.EUR;

            ConvertResponse response =  _apiService.TryConvertValue(baseValue, baseCurrency.ToString(), targetCurrency.ToString());

            Assert.AreEqual(baseValue, response.BaseValue);
            Assert.AreEqual(baseCurrency, response.BaseCurrency);
            Assert.AreEqual(targetCurrency, response.TargetCurrency);
        }

        [Test]
        public void TestInvalidCurrencyType()
        {
            Assert.Throws<NullReferenceException>(InvalidTargetCurrency);
        }

        void InvalidTargetCurrency()
        {
            _apiService.TryConvertValue(100, CurrencyType.GBP.ToString(), "ERU");
        }

        [Test]
        public void TestValidCurrencyType()
        {
            Assert.DoesNotThrow(ValidTargetCurrency);
        }

        void ValidTargetCurrency()
        {
            _apiService.TryConvertValue(100, CurrencyType.GBP.ToString(), CurrencyType.EUR.ToString());
        }
    }
}
