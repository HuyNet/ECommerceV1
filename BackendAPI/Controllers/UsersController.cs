using Application.Common.System.Users;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.System.Users;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]// AllowAnonymous: Allows access without logging in
        public async Task<IActionResult> Authenticate([FromForm]LoginRequest request)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            var resultToken = await _userService.Authenticated(request);
            if (string.IsNullOrEmpty(resultToken)) 
            {
                return BadRequest("Username of password is incorrect");
            }
            return Ok(new {Token=resultToken});
        }

        [HttpPost("register")]
        [AllowAnonymous]// AllowAnonymous: Allows access without logging in
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Register(request);
            if (!result)
            {
                return BadRequest("Register is unsuccessful");
            }
            return Ok();
        }
    }
}
