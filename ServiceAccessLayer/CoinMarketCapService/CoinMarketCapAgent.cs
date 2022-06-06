using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;
using ServiceAccessLayer.Boot;
using ServiceAccessLayer.CmcService;
using ServiceAccessLayer.CoinMarketCapService.Mapper;
using Utils.CacheHelper;

namespace ServiceAccessLayer.CoinMarketCapService
{
    public class CoinMarketCapAgent : ICoinMarketCapAgent
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICoinMarketCapServiceBoot _coinMarketCapServiceBoot;

        public CoinMarketCapAgent(ICacheManager cacheManager, ICoinMarketCapServiceBoot coinMarketCapServiceBoot)
        {
            _cacheManager = cacheManager;
            _coinMarketCapServiceBoot = coinMarketCapServiceBoot;
        }

        public async Task<List<Crypto>> GetAllCryptos()
        {
            var allCryptos = await _cacheManager.GetRecord<List<Crypto>>(_coinMarketCapServiceBoot.GenerateCryptoIdListKey());
            return allCryptos;
        }

        public async Task<List<Crypto>> GetCryptosSummary()
        {
            var cryptosSummary = await _cacheManager.GetRecord<List<Crypto>>(_coinMarketCapServiceBoot.GenerateCryptoSummaryListKey());
            return cryptosSummary;
        }
        public async Task<Crypto> GetCryptoDetail(CryptoId cryptoId)
        {
            var cryptoDetail = await _cacheManager.GetRecord<Crypto>(_coinMarketCapServiceBoot.GenerateCryptoDetailKey(cryptoId));
            return cryptoDetail;
        }
    }
}
