using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.Entities;
using Voting.Core.Infrastructure;
using Voting.Core.Interfaces;
using Voting.Core.Interfaces.Services;

namespace Voting.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(
            IConfiguration configuration,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<OperationResult> Authenticate(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new OperationResult(false, "User not found", "");

            if (user.IsBlocked)
                return new OperationResult(false, "Account blocked", "");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                return new OperationResult(false, "Invalid login attempt", "");

            var token = await GenerateToken(user);

            return new OperationResult(true, token, "");
        }

        private async Task<string> GenerateToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Id),
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtIssuerOptions:Key"])), 
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["JwtIssuerOptions:Issuer"],
                _configuration["JwtIssuerOptions:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(3000),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}