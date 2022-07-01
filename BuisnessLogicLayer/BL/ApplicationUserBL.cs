using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Identity;
using Utils.CacheHelper;

namespace BuisnessLogicLayer.BL
{
    public class ApplicationUserBL : IApplicationUserBL
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly ICacheManager _cacheManager;

        public ApplicationUserBL(IApplicationUserRepository applicationUserRepository, ICacheManager cacheManager)
        {
            _applicationUserRepository = applicationUserRepository;
            _cacheManager = cacheManager;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser newUser, string userPassword)
        {
            return await _applicationUserRepository.CreateAsync(newUser, userPassword);
        }

        public async Task<bool> CheckEmail(string userEmail)
        {
            return await _applicationUserRepository.CheckEmail(userEmail);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _applicationUserRepository.FindByEmailAsync(email);
        }

        public Task<bool> CheckPasswordAsync(ApplicationUser existingUser, string password)
        {
            return _applicationUserRepository.CheckPasswordAsync(existingUser, password);
        }

        public async Task<string> AddTradeBot(TradeBot tradeBot, ApplicationUser applicationUser)
        {
            return await _applicationUserRepository.AddTradeBot(tradeBot, applicationUser);
        }

        public List<Investment> GetAutoTraderWages(string name, ApplicationUser user)
        {
            var AutoTrader = user.TradeBots.FirstOrDefault(t => t.Name == name);
            if (AutoTrader == null)
            {
                throw new Exception("TradeBot does not exist");

            }
            return AutoTrader.BoughtCrypto;
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return _applicationUserRepository.GetAllUsers();
        }

        public List<TradeBot> GetTradeBots(ApplicationUser applicationUser)
        {
            return applicationUser.TradeBots;
        }
    }
}
