using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BuisnessLogicLayer.BL;
using ContractEntities.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Util.ExceptionsHandler;
using ILogger = Castle.Core.Logging.ILogger;

namespace IISHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IApplicationUserBL _applicationUserBl;
        private readonly ILogger<ApplicationUserController> _logger;

        public ApplicationUserController(IApplicationUserBL applicationUserBl, ILogger<ApplicationUserController> logger)
        {
            _applicationUserBl = applicationUserBl;
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("addtradebot/{name};{wallet};{maxinvest};{cantrade}")]
        public async Task<string> AddAutoTrader([FromRoute] string name, [FromRoute] double wallet, [FromRoute] double maxinvest, [FromRoute] bool cantrade)
        {
            var user = await GetCurrentUser();
            var tradeBot = new TradeBot()
            {
                CanTrade = cantrade,
                MaxInvest = maxinvest,
                Name = name,
                Wallet = wallet
            };
            return await _applicationUserBl.AddTradeBot(tradeBot, user);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("getalltradebots")]
        public async Task<List<TradeBot>> GetAllTradeBots()
        {
            var user = await GetCurrentUser();
            return _applicationUserBl.GetTradeBots(user);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("gettradebotwages/{name}")]
        public async Task<List<Investment>> GetAutoTraderCurrentWages([FromRoute] string name)
        {
            var user = await GetCurrentUser();
            return _applicationUserBl.GetAutoTraderWages(name, user);
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                var email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
                var applicationUser = await _applicationUserBl.FindByEmailAsync(email);
                return applicationUser;
            }
            throw new GenericException("no token provided", HttpStatusCode.BadRequest);
        }
    }
}
