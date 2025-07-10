using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DynamicFormsApp.Shared.Models;
using DynamicFormsApp.Shared.Services;

namespace DynamicFormsApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<bool> ValidateUser([FromBody] UserModel user)
        {
            return await _userService.ValidateUser(user);
        }


        [HttpGet("{userName}")]
        public async Task<ActionResult<UserModel>> GetUserData(string userName)
        {
            var userData = await _userService.GetUserData(userName);
            if (userData == null)
            {
                return NotFound();
            }

            return Ok(userData);
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAll()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserModel>>> Search([FromQuery] string term)
        {
            var users = await _userService.SearchUsers(term ?? string.Empty);
            return Ok(users);
        }
    }
}
