using System.Collections.Generic;
using ContractEntities.Entities;
using ServiceAccessLayer.CoinMarketCapService.Entities;
using ServiceAccessLayer.Models;

namespace ServiceAccessLayer.CoinMarketCapService.Mapper
{
    public interface ICoinMarketCapMapper
    {
        List<Crypto> MapCryptoModelsToCryptos(AllCryptosEntity models);
        List<Crypto> ToCryptoIdList(List<Crypto> cryptos);
        List<Crypto> ToCryptoSummaryList(List<Crypto> cryptos);

    }
}
