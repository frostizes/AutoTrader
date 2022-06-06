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
        List<Crypto> GetAllCryptos();
        Crypto GetCryptoDetail(CryptoId id);
    }
}
