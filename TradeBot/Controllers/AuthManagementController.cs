using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogicLayer.BL;
using BuisnessLogicLayer.JwtTokenGenerator;
using ContractEntities.DTOs;
using ContractEntities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Util.Configuration;
using Util.ExceptionsHandler;
using ILogger = Castle.Core.Logging.ILogger;

namespace WindowsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly IApplicationUserBL _userManager;
        private readonly ILogger<AuthManagementController> _logger;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthManagementController(IApplicationUserBL userManager, ILogger<AuthManagementController> logger, ITokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _logger = logger;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([Required] UserLoginRequestDTO userLoginRequest)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(userLoginRequest.Email);
                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, userLoginRequest.Password);
                if (!isCorrect)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "passwords must match");
                }
                var jwtToken = _tokenGenerator.GenerateJwtToken(existingUser);
                return Ok(jwtToken);
            }
            catch (GenericException e)
            {
                return StatusCode((int)e.StatusCode, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([Required] UserRegistrationDTO user)
        {
            try
            {
                if (!user.Password.Equals(user.PasswordCheck))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "passwords must match");
                }
                var userExists = await _userManager.CheckEmail(user.Email);
                if (userExists)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "user already exists");
                }
                var newUser = new ApplicationUser()
                {
                    Email = user.Email,
                    UserName = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    var jwtToken = _tokenGenerator.GenerateJwtToken(newUser);
                    return Ok(jwtToken);
                }
                var message = string.Join(", ",
                    isCreated.Errors.Select(x => "Code " + x.Code + " Description" + x.Description));
                return BadRequest($"unable to create user one field is wrong : {message}");
            }
            catch (GenericException e)
            {
                return StatusCode((int) e.StatusCode, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
