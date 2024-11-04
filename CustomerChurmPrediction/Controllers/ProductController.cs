using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Services;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/product")]
    public class ProductController : ControllerBase
    {
        IUserService _userService;
        IProductService _productService;
        ILogger<ProductController> _logger;

        public ProductController(IUserService userService, IProductService productService, ILogger<ProductController> logger)
        {
            _userService = userService;
            _productService = productService;
            _logger = logger;
        }
    }
}
