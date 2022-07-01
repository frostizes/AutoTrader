using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;

namespace BuisnessLogicLayer.Boot
{
    public interface IBoot
    {
        Task Start();
        Task BootAsync();
        Task<List<Crypto>> BootCryptos();
        Task<List<Crypto>> BootCryptosValue(List<Crypto> allCryptos);
        void Trade(List<Crypto> allCryptos);
        string GenerateCryptoSummaryListKey();
        string GenerateCryptoIdListKey();
        string GenerateCryptoDetailKey(string id);
        string GenerateIsBootedKey();
    }
}
