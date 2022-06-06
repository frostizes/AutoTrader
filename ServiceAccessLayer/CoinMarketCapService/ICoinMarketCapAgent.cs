using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;

namespace ServiceAccessLayer.CoinMarketCapService
{
    public interface ICoinMarketCapAgent
    {
        Task<List<Crypto>> GetAllCryptos();
        Task<List<Crypto>> GetCryptosSummary();
        Task<Crypto> GetCryptoDetail(CryptoId cryptoId);
    }
}
