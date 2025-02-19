using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Services;
using CustomerChurmPrediction.Entities.ProductEntity;
using MongoDB.Driver;
using CustomerChurmPrediction.Entities.CouponEntity;
using ZstdSharp.Unsafe;
using Renci.SshNet.Security;
using Microsoft.AspNetCore.Authorization;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/coupon")]
    public class CouponController(
        ICouponService _couponService,
        IUserService _userService,
        ILogger<CouponController> _logger) : Controller
    {
		/// <summary>
		/// Проверить, валиден ли купон
		/// </summary>
		/// <returns></returns>
		[HttpGet("validate/{code}")]
		public async Task<IActionResult> ValidateCoupon(string code)
		{
			try
			{
				var couponFilter = Builders<Coupon>.Filter.Eq(coupon => coupon.Code, code);
				var existingCoupon = await _couponService.FindAllAsync(couponFilter, default);

				if (existingCoupon is null)
					return NotFound();

				return Ok();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

        /// <summary>
        /// Получить список купонов
        /// </summary>
		/// <returns></returns>
        // GET: https://localhost:7299/api/coupon

        [HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			try
			{
				var filter = Builders<Coupon>.Filter.Empty;
				List<Coupon> couponList = await _couponService.FindAllAsync(filter, default);

				return Ok(new { couponList = couponList });
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
        /// <summary>
        /// Получить купон по id
        /// </summary>
		/// <returns></returns>
        // GET: https://localhost:7299/api/coupon/{couponId}

        [HttpGet]
		[Route("{couponId}")]
		public async Task<IActionResult> GetByIdAsync(string couponId)
		{
			if (string.IsNullOrEmpty(couponId))
				return BadRequest();

			try
			{
				Coupon coupon = await _couponService.FindByIdAsync(couponId);
				if (coupon is null)
					return NotFound();

				return Ok(new { coupon = coupon });

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
        /// <summary>
        /// Получить список купонов по id компании
        /// </summary>
		/// <returns></returns>
        // GET: https://localhost:7299/api/coupon/company/{companyId}

        [HttpGet]
		[Route("company/{companyId}")]
        public async Task<IActionResult> GetByCompanyIdAsync(string companyId)
		{
            if (string.IsNullOrEmpty(companyId))
                return BadRequest();

            try
            {
				var filter = Builders<Coupon>.Filter.Eq(c => c.CompanyId, companyId);

				var couponList = await _couponService.FindAllAsync(filter, default);

				if(couponList is not null)
					return Ok(new {couponList = couponList});

                return NotFound();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Добавить купон
        /// </summary>
		/// <returns></returns>
        // POST: https://localhost:7299/api/coupon

        [Authorize(Roles = "Admin, Owner")]
        [HttpPost]
		public async Task<IActionResult> AddAsync([FromBody] CouponAdd couponAdd)
		{
			if (couponAdd is null)
				return BadRequest();
			try
			{
				Coupon coupon = new Coupon
				{
					Code = couponAdd.Code,
					CompanyId = couponAdd.CompanyId,
					ExpirationDate = couponAdd.ExpirationDate switch
					{
						"day" => DateTime.UtcNow.AddDays(1),
						"three days" => DateTime.UtcNow.AddDays(1),
						"week" => DateTime.UtcNow.AddDays(1),
						"mounth" => DateTime.UtcNow.AddDays(1),
						_ => throw new NotImplementedException()
					},
					DiscountPercentage = couponAdd.DiscountPercentage
                };

				bool isSuccess = await _couponService.SaveOrUpdateAsync(coupon);
				if(isSuccess)
					return Ok(new {coupon = coupon});

				return StatusCode(500);
			}
			catch (Exception ex)
			{
                throw new Exception(ex.Message);
            }
		}
        /// <summary>
        /// Изменить купон
        /// </summary>
		/// <returns></returns>
        // PUT: https://localhost:7299/api/coupon/{couponId}

        [Authorize(Roles = "Admin, Owner")]
        [HttpPut]
		[Route("{couponId}")]
		public async Task<IActionResult> UpdateAsync(string couponId, [FromBody] CouponUpdate couponUpdate)
		{
			throw new Exception();
			//if (couponId is null || couponUpdate is null)
			//	return BadRequest();
			//try
			//{
			//	var coupon = await _couponService.FindByIdAsync(couponId, default);
			//
			//	coupon.Code = couponUpdate.Key;
			//	coupon.ApplicableProductIds = couponUpdate.ProductIds;
			//	coupon.CompanyId = couponUpdate.CompanyId;
			//
            //    bool isSuccess = await _couponService.SaveOrUpdateAsync(coupon);
            //    if (isSuccess)
            //        return Ok(new { coupon = coupon });
			//
            //    return StatusCode(500);
            //}
			//catch (Exception ex)
			//{
            //    throw new Exception(ex.Message);
            //}
		}
        /// <summary>
        /// Удалить купон
        /// </summary>
		/// <returns></returns>
        // DELETE: https://localhost:7299/api/coupon/{couponId}

        [Authorize(Roles = "Admin, Owner")]
        [HttpDelete]
		[Route("{couponId}")]
		public async Task<IActionResult> DeleteAsync(string couponId)
		{
			if (string.IsNullOrEmpty(couponId))
				return BadRequest();
			try
			{
				var deletedCount = await _couponService.DeleteAsync(couponId, default);
				if (deletedCount > 0)
					return Ok(new { deletedCount = deletedCount });

				return NotFound();
			}
			catch (Exception ex)
			{
                throw new Exception(ex.Message);
            }
		}
	}
}
