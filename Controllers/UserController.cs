using AmIAuthorised.DataAccessLayer.DTO;
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
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(SignInRequest signInReq)
        {
            var response = await _userService.Authenticate(signInReq);
            return response.ToActionResult();
        }

        [Authorize(Policy = "USER_CREATE")]
        [HttpPost("user")]
        public async Task<IActionResult> CreateUser(SignUpRequest signUpRequest)
        {
            var response = await _userService.CreateUser(signUpRequest);
            return response.ToActionResult();
        }

        //[HttpPost("role")]
        //public async Task<IActionResult> CreateRole(RoleUpsertDTO roleDTO)
        //{
        //    var response = await _userService.CreateRole(roleDTO);
        //    return response.ToActionResult();
        //}

        //[HttpPost("permission")]
        //public async Task<IActionResult> CreatePermission(PermissionDTO permissionDTO)
        //{
        //    var response = await _userService.CreatePermission(permissionDTO);
        //    return response.ToActionResult();
        //}

        [Authorize(Policy = "USER_VIEW_ALL")]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _userService.GetUsers();
            return response.ToActionResult();
        }

        [Authorize(Policy = "ROLE_VIEW_ALL")]
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var response = await _userService.GetRoles();
            return response.ToActionResult();
        }

    }
}
