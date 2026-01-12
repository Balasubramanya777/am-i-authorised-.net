using AmIAuthorised.DataAccessLayer.DTO;
using AmIAuthorised.Service;
using AmIAuthorised.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmIAuthorised.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser(SignUpRequest userDto)
        {
            var response = await _userService.CreateUser(userDto);
            return response.ToActionResult();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(SignInRequest signInReq)
        {
            var response = await _userService.Authenticate(signInReq);
            return response.ToActionResult();
        }
    }
}
