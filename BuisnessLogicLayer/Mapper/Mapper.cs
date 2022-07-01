using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;

namespace BuisnessLogicLayer.Mapper
{
    public class Mapper : IMapper
    {
        public List<Crypto> ToCryptoIdList(List<Crypto> cryptos)
        {
            return cryptos.Select(c => new Crypto()
            {
                CmcKey = c.CmcKey
            }).ToList();
        }
    }
}
