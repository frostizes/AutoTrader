using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;

namespace BuisnessLogicLayer.Mapper
{
    public interface IMapper
    {
        List<Crypto> ToCryptoIdList(List<Crypto> cryptos);
    }
}
