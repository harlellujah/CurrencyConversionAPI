using CurrencyConversion.Business.Entities.Api;
using CurrencyConversion.Common.AppSettings;
using CurrencyConversion.Common.Types;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyConversion.Business.ApiManagement
{
    public class ApiService : IApiService
    {
        private readonly AppSettings _appSettings;

        private HttpWebRequest request;

        private Dictionary<string, string> parameters;

        public ApiService(IOptions<AppSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
        }

        public ConvertResponse TryConvertValue(double baseValue, string baseCurrency, string targetCurrency)
        {
            if (string.IsNullOrEmpty(baseCurrency))
            {
                throw new ArgumentNullException(nameof(baseCurrency), $"parameter '{nameof(baseCurrency)}' cannot be null in ApiService.TryConvertValue");
            }

            if (string.IsNullOrEmpty(targetCurrency))
            {
                throw new ArgumentNullException(nameof(targetCurrency), $"parameter '{nameof(targetCurrency)}' cannot be null in ApiService.TryConvertValue");
            }

            parameters = new Dictionary<string, string>();

            parameters.Add("compact", "ultra");
            parameters.Add("q", $"{baseCurrency}_{targetCurrency}");

            SetupRequest("convert");

            string result = GetResponse();
            JObject obj = JObject.Parse(result);
            double exchangeRate = obj.First.First.ToObject<double>();

            ConvertResponse response = new ConvertResponse()
            {
                BaseCurrency = Enum.Parse<CurrencyType>(baseCurrency),
                TargetCurrency = Enum.Parse<CurrencyType>(targetCurrency),
                BaseValue = baseValue,
                ExchangeRate = exchangeRate,
                ConvertedValue = baseValue * exchangeRate
            };

            return response;
        }

        private void SetupRequest(string command)
        {
            string requestString = $"{_appSettings.CurrencyApiUrl}{command}?apiKey={_appSettings.CurrencyApiKey}";

            foreach (var parameter in parameters)
            {
                requestString += $"&{parameter.Key}={parameter.Value}";
            }

            request = (HttpWebRequest)WebRequest.Create(requestString);

            request.Accept = "application/json";
        }

        // JH - I wrote this generic deserialization method before I found out fixer.io free was no good
        private T GetResponse<T>()
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string responseFromServer = reader.ReadToEnd();
                            return JsonSerializer.Deserialize<T>(responseFromServer);
                        }
                    }
                }

                throw new Exception(response.StatusDescription);
            }
        }

        private string GetResponse()
        {
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }

                throw new Exception(response.StatusDescription);
            }
        }
    }
}
