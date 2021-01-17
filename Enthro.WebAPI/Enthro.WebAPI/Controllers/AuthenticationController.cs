using Enthro.Application.Configs;
using Enthro.Application.Dto;
using Enthro.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Enthro.Controllers
{
    [ApiController]
    [Route("authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserClaimsPrincipalFactory<User> _claimsPrincipalFactory;
        private readonly EndpointConfig _endpointConfig;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
            IUserClaimsPrincipalFactory<User> claimsPrincipalFactory,
            IOptionsSnapshot<EndpointConfig> endpointConfigOptions,
            UserManager<User> userManager,
            ILogger<AuthenticationController> logger
        )
        {
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _endpointConfig = endpointConfigOptions.Value;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost("token")]
        [Produces(typeof(String))]
        public async Task<IActionResult> GetTokenAsync(LoginDto login)
        {
            if (String.IsNullOrEmpty(login.UserName))
            {
                return BadRequest("UserName cannot be empty!");
            }

            if (String.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Password cannot be empty!");
            }

            User user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
            {
                return BadRequest("Invalid username or password");
            }

            ClaimsPrincipal identity = await _claimsPrincipalFactory.CreateAsync(user);
            if (identity == null)
            {
                return BadRequest("Invalid username or password");
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(login.Secret)
            );

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _endpointConfig.Issuer,
                audience: login.Audience,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(14),
                claims: identity.Claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return Ok(
                new JwtSecurityTokenHandler()
                    .WriteToken(jwt)
            );
        }

        [Authorize]
        [HttpGet("userInfo")]
        [Produces(typeof(UserInfoDto))]
        public IActionResult GetClaims()
        {
            UserInfoDto userInfo = new UserInfoDto
            {
                IsAuthenticated = true,
                Claims = User.Claims
                    .Select(c => new ClaimDto
                    {
                        Type = c.Type,
                        Value = c.Value,
                        ValueType = c.ValueType,
                        Issuer = c.Issuer,
                        OriginalIssuer = c.OriginalIssuer
                    })
                    .ToArray()
            };

            return Ok(userInfo);
        }
    }
}
