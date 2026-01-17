using CartonCaps.Api.Controllers;
using CartonCaps.Api.Service.Interface;
using CartonCaps.Common.Exceptions;
using CartonCaps.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CartonCaps.Test.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockService;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UsersController(_mockService.Object);
        }

        [Fact]
        public void GetByEmail_ReturnsOkObjectResultWithUser()
        {
            var email = "maria@email.com";
            var user = new User { Id = "user_001", Name = "Maria", Email = email };
            _mockService.Setup(s => s.GetByEmail(email)).Returns(user);

            var actionResult = _controller.GetByEmail(email);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var resultUser = Assert.IsType<User>(okResult.Value);

            Assert.Equal(email, resultUser.Email);
            Assert.Equal("Maria", resultUser.Name);
        }

        [Fact]
        public void GetByEmail_UserNotFound_ThrowsNotFoundException()
        {
            var email = "notfound@email.com";
            _mockService.Setup(s => s.GetByEmail(email))
                        .Throws(new NotFoundException("User not found"));

            Assert.Throws<NotFoundException>(() => _controller.GetByEmail(email));
        }
    }
}