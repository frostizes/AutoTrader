using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;

namespace ServiceAccessLayer.Boot
{
    public interface ICoinMarketCapServiceBoot
    {
        Task Start();
        Task<List<Crypto>> CallCryptoListService();
        void BootCryptos(List<Crypto> allCryptos);
        string GenerateCryptoSummaryListKey();
        string GenerateCryptoIdListKey();
        string GenerateCryptoDetailKey(CryptoId id);
        Task InitBootProcess();
    }
}
