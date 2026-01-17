using Xunit;
using Moq;
using CartonCaps.Service;
using CartonCaps.Api.Repository.Interface;
using CartonCaps.Model;
using CartonCaps.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CartonCaps.Test.Services
{
    public class ReferralServiceTests
    {
        private readonly Mock<IReferralRepository> _mockReferralRepo;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly ReferralService _service;

        public ReferralServiceTests()
        {
            _mockReferralRepo = new Mock<IReferralRepository>();
            _mockUserRepo = new Mock<IUserRepository>();
            _service = new ReferralService(_mockReferralRepo.Object, _mockUserRepo.Object);
        }

        [Fact]
        public void GenerateReferral_UserExists_ReturnsReferral()
        {
            var userId = "user_001";
            _mockUserRepo.Setup(r => r.GetById(userId)).Returns(new User { Id = userId });
            _mockReferralRepo.Setup(r => r.Create(userId))
                .Returns(new Referral { Code = "REF10001", UserId = userId, Status = "pending" });

            var referral = _service.GenerateReferral(userId);

            Assert.NotNull(referral);
            Assert.Equal("REF10001", referral.Code);
            Assert.Equal(userId, referral.UserId);
        }

        [Fact]
        public void GenerateReferral_UserDoesNotExist_ThrowsNotFound()
        {
            var userId = "user_999";
            _mockUserRepo.Setup(r => r.GetById(userId)).Returns((User)null);

            Assert.Throws<NotFoundException>(() => _service.GenerateReferral(userId));
        }

        [Fact]
        public void RedeemReferral_ValidReferral_UpdatesStatus()
        {
            var referral = new Referral { Code = "REF10002", UserId = "user_001", Status = "pending" };
            _mockReferralRepo.Setup(r => r.GetByCode("REF10002")).Returns(referral);
            _mockReferralRepo.Setup(r => r.Redeem("REF10002", "user_002"));

            var result = _service.RedeemReferral("REF10002", "user_002");

            Assert.Equal(referral, result);
            _mockReferralRepo.Verify(r => r.Redeem("REF10002", "user_002"), Times.Once);
        }

        [Fact]
        public void RedeemReferral_ReferralNotFound_ThrowsNotFound()
        {
            _mockReferralRepo.Setup(r => r.GetByCode("INVALID")).Returns((Referral)null);

            Assert.Throws<NotFoundException>(() => _service.RedeemReferral("INVALID", "user_002"));
        }

        [Fact]
        public void RedeemReferral_AlreadyRedeemed_ThrowsInvalidOperation()
        {
            var referral = new Referral { Code = "REF10002", Status = "redeemed", UserId = "user_001" };
            _mockReferralRepo.Setup(r => r.GetByCode("REF10002")).Returns(referral);

            Assert.Throws<InvalidOperationException>(() => _service.RedeemReferral("REF10002", "user_002"));
        }

        [Fact]
        public void RedeemReferral_UserTriesOwnReferral_ThrowsInvalidOperation()
        {
            var referral = new Referral { Code = "REF10002", Status = "pending", UserId = "user_001" };
            _mockReferralRepo.Setup(r => r.GetByCode("REF10002")).Returns(referral);

            Assert.Throws<InvalidOperationException>(() => _service.RedeemReferral("REF10002", "user_001"));
        }

        [Fact]
        public void GetUserReferrals_ReturnsList()
        {
            var userId = "user_001";
            var referrals = new List<Referral>
            {
                new Referral { Code = "REF10001", UserId = userId },
                new Referral { Code = "REF10002", UserId = userId }
            };
            _mockReferralRepo.Setup(r => r.GetByUser(userId)).Returns(referrals);

            var result = _service.GetUserReferrals(userId);

            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(userId, r.UserId));
        }

        [Fact]
        public void GetReferralByCode_ReturnsReferral()
        {
            var referral = new Referral { Code = "REF10001", UserId = "user_001" };
            _mockReferralRepo.Setup(r => r.GetByCode("REF10001")).Returns(referral);

            var result = _service.GetReferralByCode("REF10001");

            Assert.Equal("REF10001", result.Code);
        }

        [Fact]
        public void GetReferralByCode_NotFound_ThrowsNotFound()
        {
            _mockReferralRepo.Setup(r => r.GetByCode("INVALID")).Returns((Referral)null);

            Assert.Throws<NotFoundException>(() => _service.GetReferralByCode("INVALID"));
        }
    }
}