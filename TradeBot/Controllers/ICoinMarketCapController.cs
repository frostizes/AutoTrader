using System.Collections.Generic;
using System.Threading.Tasks;
using ContractEntities.Entities;

namespace IISHost.Controllers
{
    public interface ICoinMarketCapController
    {
        Task<List<Crypto>> GetCryptosList();
        Task<Crypto> GetCryptoDetail(string id);
    }
}
