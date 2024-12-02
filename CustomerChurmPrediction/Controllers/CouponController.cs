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
    public class CouponController : Controller
    {
        ICouponService _couponService;
        IUserService _userService;
        ILogger<CouponController> _logger;

        public CouponController(ICouponService couponService, IUserService userService, ILogger<CouponController> logger)
        {
            _couponService = couponService;
            _userService = userService;
            _logger = logger;
        }

		// Получить список сущностей
		// GET: api/coupon

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
		// Получить сущность по id
		// GET: api/coupon/{couponId}

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

        // Добавить сущность
        // POST: api/coupon

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
					Key = couponAdd.Key,
					ProductIds = couponAdd.ProductIds,
					CompanyId = couponAdd.CompanyId,
					StardDate = couponAdd.StartDate.ToUniversalTime(),
					EndDate = couponAdd.EndDate.ToUniversalTime()
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
        // Изменить сущность
        // Put: api/coupon/{couponId}

        [Authorize(Roles = "Admin, Owner")]
        [HttpPut]
		[Route("{couponId}")]
		public async Task<IActionResult> UpdateAsync(string couponId, [FromBody] CouponUpdate couponUpdate)
		{
			if (couponId is null || couponUpdate is null)
				return BadRequest();
			try
			{
				var coupon = await _couponService.FindByIdAsync(couponId, default);

				coupon.Key = couponUpdate.Key;
				coupon.ProductIds = couponUpdate.ProductIds;
				coupon.CategoriesIds = couponUpdate.CategoriesIds;
				coupon.CompanyId = couponUpdate.CompanyId;

                bool isSuccess = await _couponService.SaveOrUpdateAsync(coupon);
                if (isSuccess)
                    return Ok(new { coupon = coupon });

                return StatusCode(500);
            }
			catch (Exception ex)
			{
                throw new Exception(ex.Message);
            }
		}
        // Удалить сущность
        // Delete: api/coupon/{couponId}

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
