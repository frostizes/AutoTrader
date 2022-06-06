using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;
using ServiceAccessLayer.CoinMarketCapService;

namespace BuisnessLogicLayer.CoinMarketCap
{
    public class CoinMarketCapBL : ICoinMarketCapBL
    {
        private readonly ICoinMarketCapAgent _coinMarketCapAgent;

        public CoinMarketCapBL(ICoinMarketCapAgent coinMarketCapAgent)
        {
            _coinMarketCapAgent = coinMarketCapAgent;
        }


        public List<Crypto> GetAllCryptos()
        {
            return _coinMarketCapAgent.GetAllCryptos().Result;
        }

        public Crypto GetCryptoDetail(CryptoId id)
        {
            return _coinMarketCapAgent.GetCryptoDetail(id).Result;
        }
    }
}
