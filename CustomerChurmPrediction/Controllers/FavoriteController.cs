using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/favorite")]
    public class FavoriteController : ControllerBase
    {
        IFavoriteService _favoriteService;
        IUserService _userService;
        ILogger<FavoriteController> _logger;

        public FavoriteController(IFavoriteService favoriteService, IUserService userService, ILogger<FavoriteController> logger)
        {
            _favoriteService = favoriteService;
            _userService = userService;
            _logger = logger;
        }
    }
}
