using BackendPlayground.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendPlayground.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {        
        private readonly UserDbContext _dbContext;

        public UsersController(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetUsers")]
        public IEnumerable<User> Get()
        {
            return [new User { 
                Id = 1,
                UserName = "Test",
                Email = "johndoe@example.com"
            }];
        }
    }
}
