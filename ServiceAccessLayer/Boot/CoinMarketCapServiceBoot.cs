using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ContractEntities.Entities;
using Microsoft.Extensions.Options;
using ServiceAccessLayer.CmcService;
using ServiceAccessLayer.CoinMarketCapService.Mapper;
using Utils.CacheHelper;

namespace ServiceAccessLayer.Boot
{
    public class CoinMarketCapServiceBoot : ICoinMarketCapServiceBoot
    {
        private readonly ICacheManager _cacheManager;
        private readonly IOptions<BootOptions> _bootOptions;
        private readonly ICoinMarketCapClient _coinMarketCapClient;
        private readonly ICoinMarketCapMapper _coinMarketCapMapper;
        private Timer _timer;

        public CoinMarketCapServiceBoot(ICacheManager cacheManager, IOptions<BootOptions> bootOptions, ICoinMarketCapClient coinMarketCapClient, ICoinMarketCapMapper coinMarketCapMapper)
        {
            _cacheManager = cacheManager;
            _bootOptions = bootOptions;
            _coinMarketCapClient = coinMarketCapClient;
            _coinMarketCapMapper = coinMarketCapMapper;
            initTimer();
        }

        public void initTimer()
        {
            _timer = new Timer(_bootOptions.Value.RefreshTime*1000);
            _timer.Elapsed += OnTimedEvent;
        }

        public async Task Start()
        {
            _timer.Start();
            await InitBootProcess();


        }

        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            InitBootProcess();
        }

        public async Task InitBootProcess()
        {
            var cryptoList = await CallCryptoListService();
            BootCryptos(cryptoList);
        }

        public async Task<List<Crypto>> CallCryptoListService()
        {
            var cryptosResponse = await _coinMarketCapClient.GetAllCryptos();
            if (cryptosResponse.status.error_code != 0)
            {
                throw new HttpRequestException(cryptosResponse.status.error_message.ToString());
            }
            return _coinMarketCapMapper.MapCryptoModelsToCryptos(cryptosResponse);
        }

        public void BootCryptos(List<Crypto> allCryptos)
        {
            var allCryptosId = _coinMarketCapMapper.ToCryptoIdList(allCryptos);
            var allCryptosSummary = _coinMarketCapMapper.ToCryptoSummaryList(allCryptos);
            allCryptos.ForEach(c => _cacheManager.AddRecord(c,GenerateCryptoDetailKey(c.Id)));
            _cacheManager.AddRecord(allCryptosId, GenerateCryptoIdListKey());
            _cacheManager.AddRecord(allCryptosSummary, GenerateCryptoSummaryListKey());

        }

        public string GenerateCryptoSummaryListKey()
        {
            return _cacheManager.GenerateCacheKey();
        }

        public string GenerateCryptoIdListKey()
        {
            return _cacheManager.GenerateCacheKey();
        }

        public string GenerateCryptoDetailKey(CryptoId id)
        {
            return _cacheManager.GenerateCacheKey(id.ToString());
        }
    }
}
