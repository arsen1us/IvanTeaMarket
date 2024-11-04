using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using CustomerChurmPrediction.Entities;

namespace CustomerChurmPrediction.Services
{
    public interface ITokenService
    {
        public string GenerateJwtToken(User user);
        public string GenerateRefreshToken();
        public Task<string> UpdateJwtTokenAsync(string token);
    }
    public class TokenService : ITokenService
    {
        IUserService _userService;
        IConfiguration _config;
        ILogger<TokenService> _logger;

        public TokenService(IUserService userService, IConfiguration config, ILogger<TokenService> logger)
        {
            _userService = userService;
            _config = config;
            _logger = logger;
        }
        // Генерация jwt-токена

        public string GenerateJwtToken(User user)
        {
            try
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim("Id", user.Id),
                    new Claim("Email", user.Email)
                };

                JwtSecurityToken token = new JwtSecurityToken
                (
                issuer: _config["TokenSettings:Issuer"],
                audience: _config["TokenSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(1)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenSettings:Key"])), SecurityAlgorithms.HmacSha256Signature)
                );

                var tokenHandler = new JwtSecurityTokenHandler();

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error. Details: {ex.Message}");
            }
        }

        // Генерация Refresh токна
        // Получается токен вида: RA2Isc+d/w51Y1vttEk2/rx1DUuOi7CLCvHu41rjbpI=
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }
        // Обновить jwt-токен

        public async Task<string> UpdateJwtTokenAsync(string token)
        {
            try
            {
                var principal = GetPrincipalExpiredToken(token);
                if (principal != null)
                {
                    // log ошибка счмтывания jwt-токен или он поддельный 
                    return null;
                }

                string _id = principal.FindFirst("Id").Value;
                string email = principal.FindFirst("Email").Value;

                if (_id is null || email is null)
                {
                    // log ошибка счмтывания jwt-токен или он поддельный 
                    return null;
                }

                var user = await _userService.FindByIdAsync(_id, default);

                return GenerateJwtToken(user);

            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error. Details: {ex.Message}");
            }
        }

        // Проверить истёкший jwt-token
        private ClaimsPrincipal GetPrincipalExpiredToken(string expiredToken)
        {
            try
            {
                var tokenValidationParameter = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenSettings:Key"]))
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(expiredToken, tokenValidationParameter, out SecurityToken securityToken);

                var jwtSecureToken = securityToken as JwtSecurityToken;

                if (jwtSecureToken != null || jwtSecureToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
                    return principal;

                var claims = new ClaimsIdentity();

                return new ClaimsPrincipal(claims);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error. Details: {ex.Message}");
            }

        }
    }
}
