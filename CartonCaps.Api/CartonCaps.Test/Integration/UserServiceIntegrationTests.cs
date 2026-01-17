using CartonCaps.Api.Repository.Mock;
using CartonCaps.Common.Exceptions;
using CartonCaps.Model;
using CartonCaps.Repository;
using CartonCaps.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq; 

namespace CartonCaps.Test.Integration
{
    public class UserServiceIntegrationTests
    {
        private readonly UserService _service;
        private readonly JsonMock _jsonMock;

        public UserServiceIntegrationTests()
        { 
            var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.json")
                .Build();

            var dataFilePath = config["MockStorage:DataFilePath"]; 
             
            var options = Options.Create(new MockStorageOptions
            {
                DataFilePath = dataFilePath
            });
             
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.ContentRootPath)
                   .Returns(System.IO.Directory.GetCurrentDirectory());
             
            _jsonMock = new JsonMock(mockEnv.Object, options);
             
            var userRepo = new UserRepository(_jsonMock);
            var referralRepo = new ReferralRepository(_jsonMock);

            _service = new UserService(userRepo, referralRepo);
        }

        [Fact]
        public void GetByEmail_UserExists_ReturnsUser()
        {
            var email = "maria.gomez@email.com";

            var user = _service.GetByEmail(email);

            Assert.NotNull(user);
            Assert.Equal(email, user.Email);
            Assert.Equal("Maria", user.Name);
        }

        [Fact]
        public void GetByEmail_UserReferred_AttachesReferral()
        {
            var email = "juan.perez@email.com";

            var user = _service.GetByEmail(email);

            Assert.True(user.Referred);  
            Assert.NotNull(user.Referral);  
            Assert.Equal(user.Id, user.Referral.ReferredUserId); 
        }

        [Fact]
        public void GetByEmail_UserNotFound_ThrowsNotFoundException()
        {
            var email = "notfound@email.com";

            Assert.Throws<NotFoundException>(() => _service.GetByEmail(email));
        }
    }
}