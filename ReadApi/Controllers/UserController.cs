using Microsoft.AspNetCore.Mvc;
using ReadApi.Services;

namespace ReadApi.Controllers
{
    [ApiController]
    [Route("api/readapi/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService) => _userService = userService;

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            var existingUser = await _userService.GetAsync(id);
            if (existingUser is null)
            {
                return NotFound();
            }

            return Ok(existingUser);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allUsers = await _userService.GetAsync();

            if(allUsers.Any())
            {
                return Ok(allUsers);
            }
            return NotFound();
        }

    }
}
