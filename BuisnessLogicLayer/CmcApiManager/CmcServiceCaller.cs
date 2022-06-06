using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using DataAccessLayer.Repository;
using Microsoft.Extensions.Logging;
using ServiceAccessLayer.CmcService;

namespace BuisnessLogicLayer.CmcApiManager
{
    public class CmcServiceCaller : ICmcServiceCaller
    {
        private readonly ICoinMarketCapClient _cmcService;
        private readonly ICryptoRepository _cryptoRepository;
        private readonly ILogger<CmcServiceCaller> _logger;

        public CmcServiceCaller(ICoinMarketCapClient cmcService, ICryptoRepository cryptoRepository, ILogger<CmcServiceCaller> logger)
        {
            _cmcService = cmcService;
            _cryptoRepository = cryptoRepository;
            _logger = logger;
        }

        public void initCmcServiceCallTimer(int sec)
        {
            var aTimer = new System.Timers.Timer(2000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                e.SignalTime);
        }
    }
}
