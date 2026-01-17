using CartonCaps.Api.Repository.Interface;
using CartonCaps.Common.Exceptions;
using CartonCaps.Model;
using CartonCaps.Service;
using Moq;
namespace CartonCaps.Test.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IReferralRepository> _mockReferralRepo;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockReferralRepo = new Mock<IReferralRepository>();
            _service = new UserService(_mockUserRepo.Object, _mockReferralRepo.Object);
        }

        [Fact]
        public void GetByEmail_UserExists_ReturnsUserWithoutReferral()
        {
            var email = "maria@email.com";
            var user = new User { Id = "user_001", Name = "Maria", Email = email, Referred = false };
            _mockUserRepo.Setup(r => r.GetByEmail(email)).Returns(user);

            var result = _service.GetByEmail(email);

            Assert.Equal(email, result.Email);
            Assert.Null(result.Referral);
        }

        [Fact]
        public void GetByEmail_UserDoesNotExist_ThrowsNotFound()
        {
            _mockUserRepo.Setup(r => r.GetByEmail("notfound@email.com")).Returns((User)null);

            Assert.Throws<NotFoundException>(() => _service.GetByEmail("notfound@email.com"));
        }

        [Fact]
        public void GetByEmail_UserReferred_AttachesReferral()
        {
            var email = "juan@email.com";
            var user = new User { Id = "user_002", Referred = true };
            var referral = new Referral { Code = "REF10002", ReferredUserId = "user_002" };

            _mockUserRepo.Setup(r => r.GetByEmail(email)).Returns(user);
            _mockReferralRepo.Setup(r => r.GetByReferred(user.Id)).Returns(referral);

            var result = _service.GetByEmail(email);

            Assert.NotNull(result.Referral);
            Assert.Equal("REF10002", result.Referral.Code);
        }
    }
}