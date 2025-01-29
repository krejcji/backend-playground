using BackendPlayground.Server.Models;
using BackendPlayground.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendPlayground.Server.Controllers
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

        [HttpGet("{id:int}")]
        public IActionResult GetUser(int id)
        {
            var user = _userService.GetUser(id);
            if (user == null)
                return NotFound(new { id = id, error = $"User with id {id} not found."});
            return Ok(user);
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User input)
        {
            if (input == null)            
                return BadRequest("User data is null.");

            if (input.Id != 0)
                return BadRequest("UserId not zero.");                        

            try
            {               
                _userService.AddUser(input);
                return CreatedAtAction(nameof(AddUser), input);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("addWithQuery")]
        public IActionResult AddUserQueryParams([FromQuery] string userName, [FromQuery] string email)
        {            
            try
            {
                User user = new User { UserName = userName, Email = email };
                _userService.AddUser(user);
                return CreatedAtAction(nameof(AddUserQueryParams), new { userName = userName, email = email }, user);                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented);
            }
        }
    }
}
