using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;
using DataAccessLayer.DBConfig;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Util.ExceptionsHandler;

namespace DataAccessLayer.Repository
{
    public class ApplicationUserRepository : UserManager<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ILogger<UserManager<ApplicationUser>> _logger;
        private readonly AppDbContext _appDbContext;


        public ApplicationUserRepository(
            IUserStore<ApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger,
            AppDbContext appDbContext) :
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public override async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var user = await base.FindByEmailAsync(email);
            if (user is null)
            {
                throw new DBRequestException("user does not exist", HttpStatusCode.NotFound);
            }
            return user;
        }

        public override async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            var result = await base.CheckPasswordAsync(user, password);
            if (result)
            {
                return true;
            }
            return false;
        }

        public override Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            user.TradeBots = new List<TradeBot>();
            return base.CreateAsync(user);
        }

        public async Task<bool> CheckEmail(string email)
        {
            try
            {
                var user = await FindByEmailAsync(email);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<string> AddTradeBot(TradeBot tradeBot, ApplicationUser user)
        {
            var alreadyCreatedAutoTrader = user.TradeBots.FirstOrDefault(t => t.Name == tradeBot.Name);
            if (alreadyCreatedAutoTrader == null)
            {
                user.TradeBots.Add(tradeBot);
                var result = await base.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.ToString());
                }
                return "succesfully added a tradebot to your account";
            }
            else
            {
                throw new Exception("TradeBot with that name already created for this user");
            }
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return _appDbContext.Users.ToList();
        }
    }
}
