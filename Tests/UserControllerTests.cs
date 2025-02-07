using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BackendPlayground.Server.Controllers;
using BackendPlayground.Server.Models;
using BackendPlayground.Server.Services;

public class UserControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UsersController _controller;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UsersController(_mockUserService.Object);
    }

    [Fact]
    public void GetUser_UserExists_ReturnsOkWithUser()
    {        
        var userId = 1;
        var email = "test@test.com";
        var mockUser = new User { Id = userId, UserName = "Test", Email = email};
        _mockUserService.Setup(s => s.GetUser(userId)).Returns(mockUser);
                
        var result = _controller.GetUser(userId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<User>(okResult.Value);
        Assert.Equal(email, returnedUser.Email);
        Assert.Equal(userId, returnedUser.Id);
        Assert.Equal("Test", returnedUser.UserName);
    }

    [Fact]
    public void GetUser_UserDoesNotExist_ReturnsNotFound()
    {        
        var userId = 2;
        _mockUserService.Setup(s => s.GetUser(userId)).Returns((User)null);

        var result = _controller.GetUser(userId);

        Assert.IsType<NotFoundObjectResult>(result);
    }
}
