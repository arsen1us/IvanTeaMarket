using CustomerChurmPrediction.Entities.CompanyEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Utils;
using CustomerChurmPrediction.Entities.UserEntity;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/company")]
    public class CompanyController(
        ICompanyService _companyService,
        IUserService _userService,
        ITokenService _tokenService) : Controller
    {
        /// <summary>
        /// Получить список компаний
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        // GET: https://localhost:7299/api/company

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var companyList = await _companyService.FindAllAsync(default, default);
                if (companyList is not null)
                    return Ok(new { companyList = companyList });

                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// // Получить компанию по id
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        // GET: https://localhost:7299/api/company/{companyId}

        [Authorize(Roles = "Admin, Owner")]
        [HttpGet]
        [Route("{companyId}")]
        public async Task<IActionResult> GetByIdAsync(string companyId)
        {
            try
            {
                var company = await _companyService.FindByIdAsync(companyId, default);
                if (company is not null)
                    return Ok(new { company = company });

                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Получить компанию по id пользователя (проверить, является ли пользователь владельцем компании)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        [Authorize(Roles = "User, Admin, Owner")]
        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception();
            try
            {
                var company = await _companyService.FindByUserIdAsync(userId, default);
                if (company is not null)
                    return Ok(new { company = company });

                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Получить компанию по id продукта
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        // GET: https://localhost:7299/api/company/product/{productId}

        [HttpGet]
        [Route("product/{productId}")]
        public async Task<IActionResult> GetByProductIdAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                return BadRequest();
            try
            {
                var company = await _companyService.GetByProductIdAsync(productId, default);
                if (company is null)
                    return BadRequest();

                return Ok(new { company = company });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        /// <summary>
        /// Создать компанию
        /// </summary>
        // POST: https://localhost:7299/api/company

        [Authorize(Roles = "User, Admin, Owner")]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromForm] CompanyAdd companyAdd)
        {
            if (companyAdd is null
                || string.IsNullOrEmpty(companyAdd.Name)
                || string.IsNullOrEmpty(companyAdd.Description)
                || string.IsNullOrEmpty(companyAdd.UserId))
            {
                return BadRequest();
            }
            try
            {
                Company company = new Company
                {
                    Name = companyAdd.Name,
                    Description = companyAdd.Description,
                    CreatorId = companyAdd.UserId,
                    UserIdLastUpdate = companyAdd.UserId,
                    OwnerId = companyAdd.UserId
                };

                var companyImageSrc = await _companyService.UploadImagesAsync(companyAdd.Images, default);

                if (companyImageSrc is null)
                    return StatusCode(500);

                company.ImageSrcs = companyImageSrc;

                bool isSuccess = await _companyService.SaveOrUpdateAsync(company, default);

                if (isSuccess)
                {
                    var user = await _userService.FindByIdAsync(companyAdd.UserId);
                    if(user is not null)
                    {
                        // Присваиваю компанию пользователю
                        user.CompanyId = company.Id;
                        // Делаю его владжельцем компании
                        user.Role = UserRoles.Owner;

                        isSuccess = await _userService.SaveOrUpdateAsync(user);
                        if (isSuccess)
                        {
                            // Создать новый jwt-токен на основе новой роли пользователя
                            string token = _tokenService.GenerateJwtToken(user);

                            // Создать новый refresh-токен
                            string refreshToken = _tokenService.GenerateRefreshToken();

                            Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                SameSite = SameSiteMode.Strict,
                                Expires = DateTimeOffset.UtcNow.AddHours(1)
                            });

                            return Ok(new 
                            { 
                                token = token,
                                company = company 
                            });
                        }
                        return StatusCode(500, "При создании компании произошла ошибка. Не удалось присвоить пользователю новую роль!");
                    }
                    return NotFound("Не удалось найти пользователя!");

                }
                return StatusCode(500, "Не удалось создать компанию!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Обновить информацию о компании
        /// </summary>
        // PUT: https://localhost:7299/api/company/{companyId}

        [Authorize(Roles = "Admin, Owner")]
        [HttpPut]
        [Route("{companyId}")]
        public async Task<IActionResult> UpdateAsync(string companyId, [FromBody] CompanyUpdate companyUpdate)
        {
            try
            {
                var company = await _companyService.FindByIdAsync(companyId, default);

                company.Name = companyUpdate.Name;
                company.Description = companyUpdate.Description;
                company.OwnerId = companyUpdate.OwnerId;

                bool isSuccess = await _companyService.SaveOrUpdateAsync(company);
                if (isSuccess)
                    return Ok(new { company = company });
                return StatusCode(500);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Удалить компанию
        /// </summary>
        // DELETE: https://localhost:7299/api/company/{companyId}

        [Authorize(Roles = "Admin, Owner")]
        [HttpDelete]
        [Route("{companyId}")]
        public async Task<IActionResult> DeleteAsync(string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
                return BadRequest();
            try
            {
                long deletedCount = await _companyService.DeleteAsync(companyId);
                if (deletedCount > 0)
                    return Ok(new { deletedCound = deletedCount });
                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Управление работниками компании =======================================
        ///<summary>
        /// Добавить роль пользователю
        /// </summary>
        // GET: /api/company/add-role

        // Роль пользователя будет обновлена после того, как пользователь снова войдёт на сайт, или когда потребуется обновить jwt-токен
        //[Authorize]
        //[HttpPost]
        //[Route("add-role")]
        //public async Task<IActionResult> AddRoleToUserAsync([FromBody] UserRoleAdd userAddRole)
        //{
            //if (userAddRole is null
                //|| string.IsNullOrEmpty(userAddRole.UserId)
                //|| string.IsNullOrEmpty(userAddRole.Role)
                //|| string.IsNullOrEmpty(userAddRole.CompanyId))
                //return BadRequest();
            //try
            //{
                //var user = await _userService.FindByIdAsync(userAddRole.UserId, default);

                //if (user is null)
                    //return NotFound();

                //// Добавить роль
                //user.Role = userAddRole.Role;
                //// Добавить id компании
                //user.CompanyId = userAddRole.CompanyId;

                //bool isSuccess = await _userService.SaveOrUpdateAsync(user, default);

                //if (isSuccess) return Ok(new { user = user });

                //return StatusCode(500, "Произошла ошибка во время добавления роли пользователю");
            //}
            //catch (Exception ex)
            //{
                //throw new Exception(ex.Message);
            //}
        //}

        ///// <summary>
        ///// Добавить пользователя в работники компании
        ///// </summary>
        //public async Task<IActionResult> AddUserToCompanyEmployeesAsync(string userId, string companyId)
        //{
            //if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(companyId))
                //return BadRequest();
            //try
            //{
                //var user = await _userService.FindByIdAsync(userId, default);

                //// Если id компании совпадает с переданным id - удаляю его
                //if (user.CompanyId == companyId)
                    //user.CompanyId = null;

                //bool isSuccess = await _userService.SaveOrUpdateAsync(user, default);

                //if (isSuccess)
                    //return Ok(new { user = user });

                //return StatusCode(500, "Не удалось удалить пользователя из списка работников компании!");

            //}
            //catch (Exception ex)
            //{
                //throw new Exception(ex.Message);
            //}
        //}

        ///// <summary>
        ///// Обновить поль работника компании
        ///// </summary>
        //[Authorize]
        //[HttpPut]
        //[Route("update-role/{userId}")]
        //public async Task<IActionResult> DeleteRoleFromUserAsync(string userId, [FromBody] UserRoleUpdate userRoleUpdate)
        //{
            //if (userRoleUpdate is null
                //|| string.IsNullOrEmpty(userId)
                //|| string.IsNullOrEmpty(userRoleUpdate.UserId)
                //|| string.IsNullOrEmpty(userRoleUpdate.Role)
                //|| string.IsNullOrEmpty(userRoleUpdate.CompanyId))
                //return BadRequest();
            //try
            //{
                //var user = await _userService.FindByIdAsync(userId, default);

                //if (user is null)
                    //return NotFound();

                //// Добавить роль
                //user.Role = userRoleUpdate.Role;

                //// Обновить компанию
                //// Если роль пользователя - "Пользователь"
                //if (userRoleUpdate.Role == UserRoles.User)
                //{
                    //user.CompanyId = null;
                //}

                //bool isSuccess = await _userService.SaveOrUpdateAsync(user, default);

                //if (isSuccess) return Ok(new { user = user });

                //return StatusCode(500, "Произошла ошибка во время добавления роли пользователю");
            //}
            //catch (Exception ex)
            //{
                //throw new Exception(ex.Message);
            //}
        //}

        ///// <summary>
        ///// Удалить пользователя из работников компании
        ///// </summary>
        //public async Task<IActionResult> DeleteUserFromCompanyEmployeesAsync(string userId, string companyId)
        //{
            //if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(companyId))
                //return BadRequest();
            //try
            //{
                //var user = await _userService.FindByIdAsync(userId, default);

                //// Если id компании совпадает с переданным id - удаляю его
                //if (user.CompanyId == companyId)
                    //user.CompanyId = null;

                //bool isSuccess = await _userService.SaveOrUpdateAsync(user, default);

                //if (isSuccess)
                    //return Ok(new { user = user });

                //return StatusCode(500, "Не удалось удалить пользователя из списка работников компании!");

            //}
            //catch (Exception ex)
            //{
                //throw new Exception(ex.Message);
            //}
        //}
    }
}
