using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Services;
using CustomerChurmPrediction.Entities.UserEntity;
using Microsoft.AspNetCore.Authorization;
using CustomerChurmPrediction.Utils;


namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/user")]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        ITokenService _tokenService;
        ILogger<UserController> _logger;

        public UserController(IUserService userService, ITokenService tokenService, ILogger<UserController> logger)
        {
            _userService = userService;
            _tokenService = tokenService;
            _logger = logger;
        }
        /// <summary>
        /// Регистрация
        /// </summary>
        // POST: /api/user/reg

        [HttpPost]
        [Route("reg")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserReg userReg)
        {
            try
            {
                if(userReg is null 
                    || string.IsNullOrEmpty(userReg.FirstName)
                    || string.IsNullOrEmpty(userReg.LastName)
                    || string.IsNullOrEmpty(userReg.Email)
                    || string.IsNullOrEmpty(userReg.Password))
                {
                    return BadRequest();
                }
                User user = new User(userReg);

                // Выдаю роль - "Пользователь"
                user.Role = UserRoles.Admin;

                var successSave = await _userService.SaveOrUpdateAsync(user, default);

                if(successSave)
                {
                    string token = _tokenService.GenerateJwtToken(user);

                    // HttpContext.Response.Headers.Add("Authorization", token);
                    // HttpContext.Response.Headers["Authorization"] = token;

                    string refreshToken = _tokenService.GenerateRefreshToken();
                    Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddHours(1)
                    });

                    return Ok(new { token = token });
                }
                else
                {
                    // Возвращаю Enternal Error (ошибка сервера)
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Аутентификация
        /// </summary>
        // POST: /api/user/auth

        [HttpPost]
        [Route("auth")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] UserAuth userAuth)
        {
            try
            {
                if (userAuth is null
                    || string.IsNullOrEmpty(userAuth.Email)
                    || string.IsNullOrEmpty(userAuth.Password))
                {
                    return BadRequest();
                }
                var user = await _userService.FindByEmailAndPassword(userAuth.Email, userAuth.Password, default);

                if (user != null)
                {
                    string token = _tokenService.GenerateJwtToken(user);

                    // HttpContext.Response.Headers.Add("Authorization", token);
                    // HttpContext.Response.Headers["Authorization"] = token;

                    string refreshToken = _tokenService.GenerateRefreshToken();
                    Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddHours(1)
                    });

                    return Ok(new { token = token });
                }
                else
                {
                    // Возвращаю Enternal Error (ошибка сервера)
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Получить пользователя по id
        /// </summary>
        // GET: /api/user/{userId}

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> FindByIdAsync(string userId)
        {
            try
            {
                if(string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentNullException();
                }

                User user = await _userService.FindByIdAsync(userId, default);

                if(user is null)
                {

                }

                return Ok(new
                {
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    email = user.Email,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> FindAllAsync()
        {
            try
            {
                List<User> users = await _userService.FindAllAsync(default, default);

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
