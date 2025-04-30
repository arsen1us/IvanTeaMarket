using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Services;
using CustomerChurmPrediction.Entities.UserEntity;
using Microsoft.AspNetCore.Authorization;
using CustomerChurmPrediction.Utils;


namespace CustomerChurmPrediction.Controllers
{
    /// <summary>
    /// Контроллер для работы с пользователями
    /// </summary>
    /// <param name="_userService"></param>
    /// <param name="_tokenService"></param>
    /// <param name="_logger"></param>
    [ApiController]
    [Route("/api/user")]
    public class UserController(
        IUserService _userService,
        ITokenService _tokenService,
        ILogger<UserController> _logger) : ControllerBase
    {

        /// <summary>
        /// Регистрирует пользователя
        /// </summary>
        /// <param name="userReg"></param>
        /// <returns></returns>
        // POST: /api/user/reg

        [HttpPost]
        [Route("reg")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserReg userReg)
        {
            if (userReg is null
                || string.IsNullOrEmpty(userReg.FirstName)
                || string.IsNullOrEmpty(userReg.LastName)
                || string.IsNullOrEmpty(userReg.Email)
                || string.IsNullOrEmpty(userReg.Password))
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(RegisterAsync)}] Переданный параметр [{nameof(userReg)}] равен null");
                return BadRequest();
            }

            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            CancellationToken stoppingToken = cancellationTokenSource.Token;

            try
            {

                User user = new User(userReg);

                // Выдаю роль - "Пользователь"
                user.Role = UserRoles.User;

                bool isSuccess = await _userService.SaveOrUpdateAsync(user, stoppingToken);

                if (isSuccess)
                {
                    string token = _tokenService.GenerateJwtToken(user);

                    string refreshToken = _tokenService.GenerateRefreshToken();
                    Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddHours(1)
                    });

                    _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(RegisterAsync)}] Пользователь с id [{user.Id}] успешно зарегистрирован");
                    return Ok(new { token = token });
                }
                
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(RegisterAsync)}] Не удалось зарегистрировать пользователя с id [{user.Id}]");
                return StatusCode(500);
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(RegisterAsync)}] Во время регистрации пользователя возникла ошибка. Детали ошибки: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Аутентифицирует пользователя
        /// </summary>
        /// <param name="userAuth"></param>
        /// <returns></returns>
        // POST: /api/user/auth

        [HttpPost]
        [Route("auth")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] UserAuth userAuth)
        {
            if (userAuth is null
                || string.IsNullOrEmpty(userAuth.Email)
                || string.IsNullOrEmpty(userAuth.Password))
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(AuthenticateAsync)}] Переданный параметр [{nameof(userAuth)}] равен null");
                return BadRequest();
            }

            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            CancellationToken stoppingToken = cancellationTokenSource.Token;

            try
            {
                var user = await _userService.FindByEmailAndPassword(userAuth.Email, userAuth.Password, stoppingToken);

                if (user != null)
                {
                    string token = _tokenService.GenerateJwtToken(user);

                    string refreshToken = _tokenService.GenerateRefreshToken();
                    Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddHours(1)
                    });
                    _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(AuthenticateAsync)}] Пользователь с id [{user.Id}] успешно прошёл аутентификацию");
                    return Ok(new { token = token });
                }

                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(AuthenticateAsync)}] Не удалось аутентифицировать пользователя с id [{user.Id}]");
                return StatusCode(500);
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(AuthenticateAsync)}] Во время аутентификации пользователя возникла ошибка. Детали ошибки: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получает пользователя по id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        // GET: /api/user/{userId}

        // [Authorize(Roles = "User, Admin")]
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> FindUserByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(FindUserByIdAsync)}] Переданный параметр [{nameof(userId)}] равен null");
                return BadRequest();
            }

            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            CancellationToken stoppingToken = cancellationTokenSource.Token;

            try
            {

                User user = await _userService.FindByIdAsync(userId, stoppingToken);

                if (user is null)
                {
                    _logger.LogError($"[{DateTime.Now}] Метод [{nameof(FindUserByIdAsync)}] Пользователь с id [{userId}] не найден");
                    return NotFound();
                }

                _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(FindUserByIdAsync)}] Пользователь с id [{userId}] был успешно получен");
                return Ok(new { user = user });
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(FindUserByIdAsync)}] Во время получения пользователя с id [{userId}] произошла ошибка. Детали ошибки: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Изменяет пользователя 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userUpdate"></param>
        /// <returns></returns>
        // PUT: /api/user/{userId}

        [Authorize(Roles = "User,Admin")]
        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> UpdateUserAsync(string userId, [FromForm] UserUpdate userUpdate)
        {
            if (string.IsNullOrEmpty(userId) || userUpdate is null)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(UpdateUserAsync)}] Переданный параметр [{nameof(userId)}] или [{nameof(userUpdate)}] равен null");
                return BadRequest();
            }

            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            CancellationToken stoppingToken = cancellationTokenSource.Token;

            try
            {
                User user = await _userService.FindByIdAsync(userId, stoppingToken);

                if (user is null)
                {
                    _logger.LogError($"[{DateTime.Now}] Метод [{nameof(UpdateUserAsync)}] Пользователь с id [{userId}] не найден");
                    return NotFound();
                }

                user.FirstName = user.FirstName == userUpdate.FirstName ? user.FirstName : userUpdate.FirstName;
                user.LastName = user.LastName == userUpdate.LastName ? user.LastName : userUpdate.LastName;
                user.Email = user.Email == userUpdate.Email ? user.Email : userUpdate.Email;
                user.Password = user.Password == userUpdate.Password ? user.Password : userUpdate.Password;

                if (userUpdate.Images is not null)
                {
                    var userImageSrc = await _userService.UploadImagesAsync(userUpdate.Images, stoppingToken);

                    if (userImageSrc is null)
                    {
                        _logger.LogError($"[{DateTime.Now}] Метод [{nameof(UpdateUserAsync)}] Не удалось обновить аватар пользователя");
                        return StatusCode(500);
                    }

                    user.ImageSrcs = userImageSrc;
                }

                bool isSuccess = await _userService.SaveOrUpdateAsync(user, stoppingToken);
                if (isSuccess)
                {
                    _logger.LogInformation($"[{DateTime.Now}] Метод [{nameof(UpdateUserAsync)}] Информация о пользователе с id [{userId}] успешно обновлена");
                    return Ok(new { user = user });
                }

                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(UpdateUserAsync)}] Не удалось обновить информацию у пользователя с id [{userId}]");
                return StatusCode(500);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{DateTime.Now}] Метод [{nameof(UpdateUserAsync)}] Во время обновления информации у пользователя с id [{userId}] произошла ошибка. Детали ошибки: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}



