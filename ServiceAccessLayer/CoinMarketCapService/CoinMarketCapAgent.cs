using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;
using ServiceAccessLayer.CmcService;
using ServiceAccessLayer.CoinMarketCapService.Mapper;
using Utils.CacheHelper;

namespace ServiceAccessLayer.CoinMarketCapService
{
    public class CoinMarketCapAgent : ICoinMarketCapAgent
    {
        private readonly ICoinMarketCapClient _coinMarketCapClient;
        private readonly ICoinMarketCapMapper _coinMarketCapMapper;

        public CoinMarketCapAgent(ICoinMarketCapClient coinMarketCapClient, ICoinMarketCapMapper coinMarketCapMapper)
        {
            _coinMarketCapClient = coinMarketCapClient;
            _coinMarketCapMapper = coinMarketCapMapper;
        }

        public async Task<List<Crypto>> GetAllCryptos()
        {
            var allCryptos = await _coinMarketCapClient.GetAllCryptos();
            return _coinMarketCapMapper.MapCryptoModelsToCryptos(allCryptos);
        }
    }
}
