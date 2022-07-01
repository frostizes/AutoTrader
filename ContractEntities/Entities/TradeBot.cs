using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractEntities.Entities
{
    public class TradeBot
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public double Wallet { get; set; }
        public double MaxInvest { get; set; }
        public virtual List<Investment> BoughtCrypto { get; set; }
        public bool CanTrade { get; set; }



    }
}
