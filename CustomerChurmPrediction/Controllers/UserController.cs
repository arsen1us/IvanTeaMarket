using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;


namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/user")]
    public class UserController : ControllerBase
    {
        IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            
            return NotFound("123");
        }

    }
}
