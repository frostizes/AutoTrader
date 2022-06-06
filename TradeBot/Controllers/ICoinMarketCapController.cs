using System.Collections.Generic;
using ContractEntities.Entities;

namespace ISSHost.Controllers
{
    public interface ICoinMarketCapController
    {
        List<Crypto> GetCryptosList();
        Crypto GetCryptoDetail(string id);
    }
}
