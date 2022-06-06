using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAccessLayer.Models
{
    public class CryptoModel
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int CmcRank { get; set; }
        public int CirculatingSupply { get; set; }
        public int TotalSupply { get; set; }
        public List<string> Tags { get; set; }
        public CurrentcyList Quote { get; set; }

    }
}
