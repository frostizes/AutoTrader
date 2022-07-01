﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;
using ServiceAccessLayer.CoinMarketCapService.Entities;
using ServiceAccessLayer.CoinMarketCapService.Mapper;
using ServiceAccessLayer.Models;
using Utils.Random;

namespace ServiceAccessLayer.Mapper
{
    public class CoinMarketCapMapper : ICoinMarketCapMapper
    {
        public List<Crypto> MapCryptoModelsToCryptos(AllCryptosEntity models)
        {
            List<Crypto> cryptos = new List<Crypto>();
            foreach (var model in models.data)
            {
                var crypto = new Crypto()
                {
                    Value = model.quote.USD.price,
                    CirculationSupply = model.circulating_supply,
                    CmcRank = model.cmc_rank,
                    CryptoAge = model.date_added,
                    CmcKey = model.id.ToString(),
                    MaxSupply = model.max_supply,
                    Name = model.name,
                    Symbol = model.symbol,
                    OldValueList = new FixedSizedList<double>()
                };
                crypto.OldValueList.Add(model.quote.USD.price);
                cryptos.Add(crypto);
            }

            return cryptos;
        }

        public List<Crypto> ToCryptoIdList(List<Crypto> cryptos)
        {
            return cryptos.Select(c => new Crypto()
            {
                CmcKey = c.CmcKey
            }).ToList();
        }

        public List<Crypto> ToCryptoSummaryList(List<Crypto> cryptos)
        {
            return cryptos.Select(c => new Crypto()
            {
                CmcKey = c.CmcKey,
                Value = c.Value,
                Name = c.Name,
                Symbol = c.Symbol
            }).ToList();
        }
    }
}
