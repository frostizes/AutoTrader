using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogicLayer.Caching;
using ContractEntities.Entities;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Identity;

namespace BuisnessLogicLayer.BL
{
    public class ApplicationUserBL : IApplicationUserBL
    {
        private readonly ApplicationUserRepository _applicationUserRepository;
        private readonly ICacheManager _cacheManager;

        public ApplicationUserBL(ApplicationUserRepository applicationUserRepository, ICacheManager cacheManager)
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
    }
}
