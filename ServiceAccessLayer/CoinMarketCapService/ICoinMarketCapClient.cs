using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;
using ServiceAccessLayer.CoinMarketCapService.Entities;

namespace ServiceAccessLayer.CmcService
{
    public interface ICoinMarketCapClient
    {
        Task<AllCryptosEntity> GetAllCryptos();
    }
}
