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
                return NotFound(new { id = id, error = $"User with id: {id} not found."});
            return Ok(user);
        }

        [HttpGet("{name}")]
        public IActionResult SearchUserByName(string name)
        {
            var users = _userService.GetUsersByName(name);
            return Ok(users);
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

        [HttpPut]
        public IActionResult UpdateUser([FromBody] User input)
        {
            if (input == null)
                return BadRequest("User data is null.");

            if (input.Id == 0)
                return BadRequest("UserId not specified.");

            try
            {
                _userService.UpdateUser(input);
                return CreatedAtAction(nameof(UpdateUser), input);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public IActionResult DeleteUser([FromQuery] int id)
        {
            if (id == 0)                
                return BadRequest("Invalid UsedId.");

            try
            {
                User user = _userService.GetUser(id);
                if (user == null)
                    return NotFound(new { id = id, error = $"User with id: {id} not found." });

                _userService.DeleteUser(user);
                return Ok(new {message = $"User with id: {id} deleted successfully."});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
