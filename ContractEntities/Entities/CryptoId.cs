using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractEntities.Entities
{
    public  class CryptoId
    {
        public string Id { get; set; }

        public override string ToString()
        {
            return Id;
        }
    }
}
