using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;
using Utils.Random;

namespace BuisnessLogicLayer.TradingAlgo
{
    public class Trader : ITrader
    {
        private int _period = 14;

        public Trader()
        {
            
        }

        public double GetCryptoRSIValue(Crypto crypto)
        {
            var cryptoRelativeStrength = MeasureRelativeStrength(crypto.OldValueList);
            var rsi = 100 - (100/(1+cryptoRelativeStrength));
            return rsi;
        }

        public double MeasureRelativeStrength(FixedSizedList<double> previous14Values)
        {
            double previousValue = 0;
            double gains = 0;
            double loss = 0;
            foreach (var value in previous14Values)
            {
                if (previousValue == 0)
                {
                    previousValue = value;
                }
                else
                {
                    if (previousValue - value > 0)
                    {
                        loss += value;
                    }
                    else
                    {
                        gains += value;
                    }

                    previousValue = value;
                }
            }
            return gains/loss;
        }

        public void Trade(ApplicationUser user, List<Crypto> allCryptos)
        {
            foreach (var tradeBot in user.TradeBots)
            {
                if (tradeBot.CanTrade)
                {
                    var newInvesments = new List<Investment>();
                    //selling part
                    foreach (var investment in tradeBot.BoughtCrypto)
                    {
                        var updatedCrypto = allCryptos.FirstOrDefault(c => c.CmcKey == investment.Crypto.CmcKey);
                        if (updatedCrypto != null)
                        {
                            if (updatedCrypto.Value < 30)
                            {
                                tradeBot.Wallet += investment.InvestedMoney;
                            }
                            else
                            {
                                newInvesments.Add(investment);
                            }
                        }
                    }
                    //buying part
                    foreach (var crypto in allCryptos)
                    {
                        if (crypto.Value > 70 && !CointainsCrypto(newInvesments, crypto))
                        {
                            newInvesments.Add(new Investment()
                            {
                                Crypto = crypto,
                                InvestedMoney = tradeBot.MaxInvest
                            });
                        }
                    }
                }
            }
        }

        private bool CointainsCrypto(List<Investment> investments, Crypto crypto)
        {
            foreach (var investment in investments)
            {
                if (investment.Crypto.CmcKey == crypto.CmcKey)
                    return true;
            }
            return false;
        }

        public void Sell(ApplicationUser user, Crypto crypto)
        {
            throw new NotImplementedException();
        }

        public void Buy(ApplicationUser user, Crypto crypto)
        {
            throw new NotImplementedException();
        }
    }
}
