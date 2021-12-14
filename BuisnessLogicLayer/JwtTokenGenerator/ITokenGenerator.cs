using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;

namespace BuisnessLogicLayer.JwtTokenGenerator
{
    public interface ITokenGenerator
    {
        public string GenerateJwtToken(ApplicationUser applicationUser);
    }
}
