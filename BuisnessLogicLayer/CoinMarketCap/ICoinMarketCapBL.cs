using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;

namespace BuisnessLogicLayer.CoinMarketCap
{
    public interface ICoinMarketCapBL
    {
        Task<List<Crypto>> GetAllCryptos();
        Task<Crypto> GetCryptoDetail(string id);
        Task<List<Crypto>> GetCryptosAboveValueThreshHold(int threshHold);
        Task<List<Crypto>> GetCryptosUnderValueThreshHold(int threshHold);
    }
}
