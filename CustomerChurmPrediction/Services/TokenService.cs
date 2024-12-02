using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using CustomerChurmPrediction.Entities.UserEntity;
using CustomerChurmPrediction.Utils;

namespace CustomerChurmPrediction.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// Генерация jwt-токена
        /// </summary>
        public string GenerateJwtToken(User user);

        /// <summary>
        /// Генерация refresh-токена
        /// </summary>
        public string GenerateRefreshToken();

        /// <summary>
        /// Обновиль jwt-токен
        /// </summary>
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
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", user.Email),
                    // Если у пользователя нет роли - присвоить роль - "Пользователь"
                    new Claim(ClaimTypes.Role, user.Role ?? UserRoles.User),
                    // Id компании, с которой может работать пользователь
                    new Claim("CompanyId", user.CompanyId ?? "")
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

        public async Task<string> UpdateJwtTokenAsync(string token)
        {
            try
            {
                var principal = GetPrincipalExpiredToken(token);
                if (principal == null)
                {
                    // log ошибка счмтывания jwt-токен или он поддельный 
                    throw new Exception("Токен поддельный");
                }

                string _id = principal.FindFirst("Id").Value;
                string email = principal.FindFirst("Email").Value;
                string role = principal.FindFirst(ClaimTypes.Role).Value;

                // по умолчанию у пользователя должен быть id, email, role
                if (string.IsNullOrEmpty(_id)
                    || string.IsNullOrEmpty(email)
                    || string.IsNullOrEmpty(role))
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
                    ValidIssuer = _config["TokenSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["TokenSettings:Audience"],
                    ValidateLifetime = false,
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
