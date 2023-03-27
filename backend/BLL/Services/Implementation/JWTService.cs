using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
using backend.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace backend.BLL.Services.Implementation;

public class JWTService : IJWTService
{
    private readonly IConfiguration _configuration;
    private readonly IRepository<RefreshToken> _repositoryTokens;
    private readonly UserManager<User> _userManager;

    public JWTService(
        IConfiguration configuration,
        UserManager<User> userManager,
        IRepository<RefreshToken> reposioryTokens)
    {
        _repositoryTokens = reposioryTokens;
        _configuration = configuration;
        _userManager = userManager;
    }

    public string CreateRefreshToken(User user)
    {
        var _refreshToken = _repositoryTokens.GetById(user.Id);
        if (_refreshToken == null)
        {
            var t = new RefreshToken
            {
                Id = user.Id,
                Token = Guid.NewGuid().ToString(),
                ToLife = DateTime.Now.AddMinutes(_configuration.GetSection("JWT").GetValue<int>("REFRESH_LIFETIME"))
            };
            _repositoryTokens.Add(t);
            _refreshToken = t;
        }
        else
        {
            _refreshToken.Token = Guid.NewGuid().ToString();
            _refreshToken.ToLife =
                DateTime.Now.AddMinutes(_configuration.GetSection("JWT").GetValue<int>("REFRESH_LIFETIME"));
            _repositoryTokens.Edit(_refreshToken);
        }

        return _refreshToken.Token;
    }

    public string CreateToken(User user)
    {
        var identity = GetIdentity(user);
        var now = DateTime.UtcNow;

        var KEY = _configuration.GetSection("JWT").GetValue<string>("KEY");

        var SSK = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));

        var jwt = new JwtSecurityToken(
            _configuration.GetSection("JWT").GetValue<string>("ISSUER"),
            _configuration.GetSection("JWT").GetValue<string>("AUDIENCE"),
            notBefore: now,
            claims: identity.Claims,
            expires: now.AddMinutes(_configuration.GetSection("JWT").GetValue<int>("LIFETIME")),
            signingCredentials: new SigningCredentials(SSK, SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }

    public ClaimsIdentity GetIdentity(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, user.Id)
        };
        var roles = _userManager.GetRolesAsync(user).Result.ToList();
        foreach (var el in roles) claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, el));
        claims.Add(new Claim("email", user.Email));
        var claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }
}