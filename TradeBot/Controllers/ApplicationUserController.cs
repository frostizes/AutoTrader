using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuisnessLogicLayer.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ILogger = Castle.Core.Logging.ILogger;

namespace TradeBot.Controllers
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
    }
}
