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
        /// <param name="user"></param>
        /// <returns>jwt-токен</returns>
        /// <exception cref="Exception"></exception>
        public string GenerateJwtToken(User user);

        /// <summary>
        /// Генерация refresh-токена (токен вида: RA2Isc+d/w51Y1vttEk2/rx1DUuOi7CLCvHu41rjbpI=)
        /// </summary>
        /// <returns>refresh-токен</returns>
        public string GenerateRefreshToken();

        /// <summary>
        /// Обновить токен
        /// </summary>
        /// <param name="token">Истёкший jwt-токен</param>
        /// <returns>обновлённый jwt-токен</returns>
        /// <exception cref="Exception"></exception>
        public Task<string> UpdateJwtTokenAsync(string token);
    }

    /// <summary>
    /// Сервис для генерации и обновления jwt-токена 
    /// </summary>
    /// <param name="_userService">Сервис для регистрации, аутентификации и управления данными пользователей</param>
    /// <param name="_config">Конфигурация</param>
    /// <param name="_logger">Логгер</param>
    public class TokenService(IUserService _userService,
        IConfiguration _config,
        ILogger<TokenService> _logger) : ITokenService
    {
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

                if (principal.FindFirst("Id") is null || principal.FindFirst("Email") is null || principal.FindFirst(ClaimTypes.Role) is null)
                {
                    return null;
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

        /// <summary>
        /// Получить данные пользователя из истёкшего jwt-токена
        /// </summary>
        /// <param name="expiredToken"></param>
        /// <returns>Данные пользователя (Объект ClaimsPrincipal)</returns>
        /// <exception cref="Exception"></exception>
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
