using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Services;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/promotion")]
    public class PromotionController : Controller
    {
        IPromotionService _promotionService;
        ILogger<PromotionController> _logger;

        public PromotionController(IPromotionService promotionService, ILogger<PromotionController> logger)
        {
            _promotionService = promotionService;
            _logger = logger;

        }
    }
}
