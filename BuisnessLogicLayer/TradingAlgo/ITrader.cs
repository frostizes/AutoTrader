using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;
using Utils.Random;

namespace BuisnessLogicLayer.TradingAlgo
{
    public interface ITrader
    {
        double GetCryptoRSIValue(Crypto crypto);
        double MeasureRelativeStrength(FixedSizedList<double> previous14Values);
        void Trade(ApplicationUser user, List<Crypto> allCryptos);
        void Sell(ApplicationUser user, Crypto crypto);
        void Buy(ApplicationUser user, Crypto crypto);
    }
}
