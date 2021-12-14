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

        public Task<bool> CheckEmail(string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckPasswordAsync(ApplicationUser existingUser, string password)
        {
            throw new NotImplementedException();
        }
    }
}
