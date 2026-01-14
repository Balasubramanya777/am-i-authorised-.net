using AmIAuthorised.DataAccessLayer.DTO;
using AmIAuthorised.DataAccessLayer.Entity;
using AmIAuthorised.Service;
using AmIAuthorised.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmIAuthorised.Controllers
{
    [ApiController]
    [Route("api/")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("user")]
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

        [HttpPost("role")]
        public async Task<IActionResult> CreateRole(Role role)
        {
            var response = await _userService.CreateRole(role);
            return response.ToActionResult();
        }
    }
}
