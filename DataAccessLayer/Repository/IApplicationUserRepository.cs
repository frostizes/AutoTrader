using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Repository
{
    public interface IApplicationUserRepository
    {
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task<bool> CheckEmail(string email);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<string> AddTradeBot(TradeBot tradeBot, ApplicationUser user);
        List<ApplicationUser> GetAllUsers();
    }
}
