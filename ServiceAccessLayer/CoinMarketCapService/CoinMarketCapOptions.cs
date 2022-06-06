using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAccessLayer.CoinMarketCapService
{
    public class CoinMarketCapOptions
    {
        public string BaseUrl { get; set; }
        public string CoinMarketCapAPIValue { get; set; }
        public string CoinMarketCapAPIKey { get; set; }
        public string AllCryptosEndpoint { get; set; }

    }
}
