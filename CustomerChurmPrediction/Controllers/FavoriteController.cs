using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.Controllers
{
    public class FavoriteController : ControllerBase
    {
        IFavoriteService _favoriteService;
        IUserService _userService;

        public FavoriteController(IFavoriteService favoriteService, IUserService userService)
        {
            _favoriteService = favoriteService;
            _userService = userService;
        }
    }
}
