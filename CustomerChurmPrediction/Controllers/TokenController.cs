﻿using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.Controllers
{
    /// <summary>
    /// Контроллер для работы с jwt-токеном
    /// </summary>
    /// <param name="_tokenService"></param>
    /// <param name="_userService"></param>
    /// <param name="_logger"></param>
    [ApiController]
    [Route("/api/token")]
    public class TokenController(
        ITokenService _tokenService,
        IUserService _userService,
        ILogger<UserController> _logger) : Controller
    {
        /// <summary>
        /// Обновляет jwt-токен и refresh-токен
        /// </summary>
        // GET: api/token/refresh-token

        [HttpGet]
        [Route("update")]
        public async Task<IActionResult> UpdateToken()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            CancellationToken cancellationToken = cts.Token;

            try
            {
                var authToken = HttpContext.Request.Headers["Authorization"].ToString();

                var token = authToken.Replace("Bearer ", "");

                if (!HttpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
                {
                    // return Unauthorized("Refresh token is missing");
                }

                string newJwtToken = "Bearer" + await _tokenService.UpdateJwtTokenAsync(token, cancellationToken);

                // Если не удалосб создать токен возвращаю 401
                if (string.IsNullOrEmpty(newJwtToken))
                    return Unauthorized();

                string newRefreshToken = _tokenService.GenerateRefreshToken();

                Response.Cookies.Append("RefreshToken", newRefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });

                return Ok(new { token = newJwtToken });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Метод для проверки авторизации пользователя
        /// </summary>

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [Route("check")]
        public async Task<IActionResult> CheckAuthorization()
        {
            return Ok();
        }
    }
}
