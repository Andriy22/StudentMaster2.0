using backend.BLL.Common.DTOs.Auth;
using backend.BLL.Common.Exceptions;
using backend.BLL.Common.VMs.Auth;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
using backend.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJWTService _jWTService;
        private readonly IRepository<RefreshToken> _refreshTokens;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<User> userManager, IJWTService jWTService, IRepository<RefreshToken> refreshTokens, IConfiguration configuration)
        {
            _userManager = userManager;
            _jWTService = jWTService;
            _refreshTokens = refreshTokens;
            _configuration = configuration;
        }

        public async Task<AuthorizationVM> AuthorizationAsync(AuthorizationDTO model)
        {
            var user = await this._userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new CustomHttpException("User not found");
            }


            var isLoggined = await this._userManager.CheckPasswordAsync(user, model.Password);
            if (!isLoggined)
            {
                throw new CustomHttpException("Password is wrong");
            }
            
            if (!user.EmailConfirmed)
            {
                throw new CustomHttpException("Account not confirmed", System.Net.HttpStatusCode.Forbidden);
            }

            var access_token = this._jWTService.CreateToken(user);
            var refresh_token = this._jWTService.CreateRefreshToken(user);


            return new AuthorizationVM()
            {
                AccessToken = access_token,
                RefreshToken = refresh_token,
                Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                UserId = user.Id,
                UserName = user.UserName,
            };
        }

        public async Task<AuthorizationVM> RefreshAsync(RefreshTokenDTO model)
        {
            var token = await _refreshTokens.GetQueryable(x => x.Token == model.RefreshToken).Include(x => x.User).FirstOrDefaultAsync();
            var refresh_time = _configuration.GetSection("JWT").GetValue<int>("REFRESH_LIFETIME");

            if (token == null)
                throw new CustomHttpException("We can't find your token...");

            if (token.ToLife.AddMinutes(refresh_time) <= DateTime.Now)
                throw new CustomHttpException("Refresh token is expired...");

            var handler = new JwtSecurityTokenHandler();
            var decrypt_token = handler.ReadJwtToken(model.AccessToken);

            if (decrypt_token.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value != token.User.Id)
                throw new CustomHttpException("Unknown error...");


            return new AuthorizationVM()
            {
                AccessToken = _jWTService.CreateToken(token.User),
                RefreshToken = _jWTService.CreateRefreshToken(token.User),
                Roles = (await _userManager.GetRolesAsync(token.User)).ToList(),
                UserId = token.User.Id,
                UserName = token.User.UserName,
            };
        }
    }
}
