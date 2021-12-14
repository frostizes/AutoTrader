using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;
using Microsoft.AspNetCore.Identity;

namespace BuisnessLogicLayer.BL
{
    public interface IApplicationUserBL
    {
        Task<IdentityResult> CreateAsync(ApplicationUser newUser, string userPassword);
        Task<bool> CheckEmail(string userEmail);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser existingUser, string password);
    }
}
