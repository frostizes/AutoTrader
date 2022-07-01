using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Utils.Random;

namespace ContractEntities.Entities
{
    public class Crypto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string CmcKey { get; set; }
        public string Symbol { get; set; }
        public double Value { get; set; }
        public int InvestValue { get; set; }
        public DateTime? CryptoAge { get; set; }
        public long? MaxSupply { get; set; }
        public double? CirculationSupply { get; set; }
        public int CmcRank { get; set; }
        [NotMapped]
        public FixedSizedList<double> OldValueList { get; set; }


    }
}
