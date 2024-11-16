using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Services;
using CustomerChurmPrediction.Entities.ProductEntity;
using MongoDB.Driver;
using CustomerChurmPrediction.Entities.CouponEntity;
using ZstdSharp.Unsafe;

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
				List<Coupon> coupons = await _couponService.FindAllAsync(filter, default);

				return Ok(new { coupons = coupons });
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
		// Добавить сущность
		// POST: api/coupon

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
					CategoriesIds = couponAdd.CategoriesIds
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

		[HttpPut]
		[Route("{couponId}")]
		public async Task<IActionResult> UpdateAsync(string couponId, [FromBody] CouponUpdate couponUpdate)
		{
			if (couponId is null || couponUpdate is null)
				return BadRequest();
			try
			{
                Coupon coupon = new Coupon
                {
                    Key = couponUpdate.Key,
                    ProductIds = couponUpdate.ProductIds,
                    CategoriesIds = couponUpdate.CategoriesIds
                };

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
