using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/token")]
    public class TokenController : Controller
    {
        ITokenService _tokenService;
        ILogger<UserController> _logger;

        public TokenController(ITokenService tokenService, ILogger<UserController> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }
        // GET: api/token/refresh-token

        [HttpGet]
        [Route("update")]
        public async Task<IActionResult> UpdateToken()
        {
            try
            {
                var authToken = HttpContext.Request.Headers["Authorization"].ToString();

                var token = authToken.Replace("Bearer ", "");

                var refreshToken = HttpContext.Request.Cookies["RefreshToken"];

                string newJwtToken = "Bearer" + await _tokenService.UpdateJwtTokenAsync(token);
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
    }
}
