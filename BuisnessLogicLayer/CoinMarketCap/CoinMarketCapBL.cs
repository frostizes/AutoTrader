using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogicLayer.Boot;
using ContractEntities.Entities;
using ServiceAccessLayer.CoinMarketCapService;
using Utils.CacheHelper;

namespace BuisnessLogicLayer.CoinMarketCap
{
    public class CoinMarketCapBL : ICoinMarketCapBL
    {
        private readonly ICoinMarketCapAgent _coinMarketCapAgent;
        private readonly ICacheManager _cacheManager;
        private readonly IBoot _boot;

        public CoinMarketCapBL(ICoinMarketCapAgent coinMarketCapAgent, ICacheManager cacheManager, IBoot boot)
        {
            _coinMarketCapAgent = coinMarketCapAgent;
            _cacheManager = cacheManager;
            _boot = boot;
        }


        public async Task<List<Crypto>> GetAllCryptos()
        {
            return await _cacheManager.GetRecord<List<Crypto>>(_boot.GenerateCryptoIdListKey());
        }

        public async Task<Crypto> GetCryptoDetail(string id)
        {
            return await _cacheManager.GetRecord<Crypto>(_boot.GenerateCryptoDetailKey(id));
        }

        public Task<List<Crypto>> GetCryptosAboveValueThreshHold(int threshHold)
        {
            throw new NotImplementedException();
        }

        public Task<List<Crypto>> GetCryptosUnderValueThreshHold(int threshHold)
        {
            throw new NotImplementedException();
        }
    }
}
