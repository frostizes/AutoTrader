using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ServiceAccessLayer.CmcService;
using ServiceAccessLayer.CoinMarketCapService.Entities;
using ServiceAccessLayer.CoinMarketCapService.Mapper;

namespace ServiceAccessLayer.CoinMarketCapService
{
    public class CoinMarketCapClient : ICoinMarketCapClient
    {
        private readonly HttpClient _client;
        private readonly IOptions<CoinMarketCapOptions> _coinMarketCapOptions;
        private readonly ILogger<CoinMarketCapClient> _logger;

        public CoinMarketCapClient(HttpClient client, IOptions<CoinMarketCapOptions> coinMarketCapOptions, ILogger<CoinMarketCapClient> logger)
        {
            _client = client;
            _coinMarketCapOptions = coinMarketCapOptions;
            _logger = logger;
        }

        public async Task<AllCryptosEntity> GetAllCryptos()
        {
            try
            {
                var url = _client.BaseAddress + _coinMarketCapOptions.Value.AllCryptosEndpoint;
                var response = await _client.GetAsync(new Uri(url, UriKind.RelativeOrAbsolute));
                if (response.IsSuccessStatusCode)
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<AllCryptosEntity>(stringResponse);
                }
                throw new HttpRequestException(response.ReasonPhrase, null, response.StatusCode);
            }
            catch (Exception e)
            {
                _logger.LogError("error in getAllCryptos", e);
                return null;
            }

        }
    }
}
