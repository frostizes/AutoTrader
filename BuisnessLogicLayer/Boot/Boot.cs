using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using BuisnessLogicLayer.BL;
using BuisnessLogicLayer.Mapper;
using BuisnessLogicLayer.TradingAlgo;
using ContractEntities.Entities;
using DataAccessLayer.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ServiceAccessLayer.CoinMarketCapService;
using Utils.CacheHelper;

namespace BuisnessLogicLayer.Boot
{
    public class Boot : IBoot
    {
        private readonly ICacheManager _cacheManager;
        private readonly IOptions<BootOptions> _bootOptions;
        private readonly ICoinMarketCapAgent _coinMarketCapAgent;
        private readonly ITrader _trader;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public Boot(ICacheManager cacheManager, IOptions<BootOptions> bootOptions, ICoinMarketCapAgent coinMarketCapAgent, ITrader trader, IMapper mapper, IServiceProvider serviceProvider)
        {
            _cacheManager = cacheManager;
            _bootOptions = bootOptions;
            _coinMarketCapAgent = coinMarketCapAgent;
            _trader = trader;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
            //_applicationUserBl = applicationUserBl;
            initTimer();
        }

        public void initTimer()
        {
            _timer = new Timer(_bootOptions.Value.RefreshTime * 1000);
            _timer.Elapsed += OnTimedEvent;
        }

        public async Task Start()
        {
            _timer.Start();
            var isBooted = await _cacheManager.GetRecord<string>(GenerateIsBootedKey());
            if (isBooted == null)
            {
                BootAsync();
            }
        }

        public async Task BootAsync()
        {
            await _cacheManager.AddRecord<string>("IsBooted", GenerateIsBootedKey());
            var allCryptos = await BootCryptos();
            allCryptos = await BootCryptosValue(allCryptos);
            //Trade(allCryptos);

        }

        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            BootAsync();
        }

        //Updates booted cryptos by adding the new data
        public async Task<List<Crypto>> BootCryptos()
        {
            var allCryptos = await _coinMarketCapAgent.GetAllCryptos();
            var cachedCryptoList = await _cacheManager.GetRecord<List<Crypto>>(GenerateCryptoSummaryListKey());
            List<Crypto> allCryptosId;

            //data already cached
            if (cachedCryptoList != null)
            {
                foreach (var crypto in cachedCryptoList)
                {
                    var matchingCrypto = allCryptos.FirstOrDefault(c => c.CmcKey == crypto.CmcKey);
                    if (matchingCrypto != null)
                    {
                        crypto.OldValueList.Add(matchingCrypto.Value);
                        allCryptos.Remove(matchingCrypto);
                    }
                }
                var resultCryptoList = cachedCryptoList.Concat(allCryptos).ToList();
                resultCryptoList.ForEach(async c => await _cacheManager.AddRecord(c, GenerateCryptoDetailKey(c.CmcKey)));
                allCryptosId = _mapper.ToCryptoIdList(resultCryptoList);
                await _cacheManager.AddRecord(allCryptosId, GenerateCryptoIdListKey());
                await _cacheManager.AddRecord(resultCryptoList, GenerateCryptoSummaryListKey());
                return resultCryptoList;
            }

            //first time caching
            allCryptosId = _mapper.ToCryptoIdList(allCryptos);
            await _cacheManager.AddRecord(allCryptosId, GenerateCryptoIdListKey());
            await _cacheManager.AddRecord(allCryptos, GenerateCryptoSummaryListKey());
            allCryptos.ForEach(async c => await _cacheManager.AddRecord(c, GenerateCryptoDetailKey(c.CmcKey)));
            return allCryptos;
        }

        public async Task<List<Crypto>> BootCryptosValue(List<Crypto> cryptos)
        {
            foreach (var crypto in cryptos)
            {
                if (crypto.OldValueList.Count == 14)
                {
                    crypto.Value = _trader.GetCryptoRSIValue(crypto);
                    await _cacheManager.AddRecord(crypto, GenerateCryptoDetailKey(crypto.CmcKey));
                }
            }
            return cryptos;
        }

        public void Trade(List<Crypto> allCryptos)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var dataAccess = scope.ServiceProvider.GetRequiredService<IApplicationUserRepository>();
                var users = dataAccess.GetAllUsers();
                users.ForEach(u => _trader.Trade(u, allCryptos));
            }
        }

        public string GenerateCryptoSummaryListKey()
        {
            return _cacheManager.GenerateCacheKey();
        }

        public string GenerateCryptoIdListKey()
        {
            return _cacheManager.GenerateCacheKey();
        }

        public string GenerateCryptoDetailKey(string id)
        {
            return _cacheManager.GenerateCacheKey(id.ToString());
        }

        public string GenerateIsBootedKey()
        {
            return _cacheManager.GenerateCacheKey();
        }
    }
}
