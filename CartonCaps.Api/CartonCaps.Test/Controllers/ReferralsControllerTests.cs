using CartonCaps.Api.Controllers;
using CartonCaps.Api.Service.Interface;
using CartonCaps.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CartonCaps.Test.Controllers
{
    public class ReferralsControllerTests
    {
        private readonly Mock<IReferralService> _mockService;
        private readonly ReferralsController _controller;

        public ReferralsControllerTests()
        {
            _mockService = new Mock<IReferralService>();
            _controller = new ReferralsController(_mockService.Object);
        }

        [Fact]
        public void GenerateReferral_ReturnsCreatedAtActionResult()
        {
            var userId = "user_001";
            var referral = new Referral { Code = "REF10001", UserId = userId };
            _mockService.Setup(s => s.GenerateReferral(userId)).Returns(referral);

            var actionResult = _controller.GenerateReferral(userId);

            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var resultReferral = Assert.IsType<Referral>(createdResult.Value);

            Assert.Equal("REF10001", resultReferral.Code);
            Assert.Equal(userId, resultReferral.UserId);
            Assert.Equal(nameof(ReferralsController.GetReferralByCode), createdResult.ActionName);
        }

        [Fact]
        public void RedeemReferral_ReturnsOkObjectResult()
        {
            var code = "REF10001";
            var referral = new Referral { Code = code };
            _mockService.Setup(s => s.RedeemReferral(code, "user_002")).Returns(referral);

            var actionResult = _controller.RedeemReferral(code, "user_002");

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var resultReferral = Assert.IsType<Referral>(okResult.Value);

            Assert.Equal(code, resultReferral.Code);
        }

        [Fact]
        public void GetReferralsByUser_ReturnsOkObjectResultWithList()
        {
            var userId = "user_001";
            var referrals = new List<Referral> { new Referral { Code = "REF10001", UserId = userId } };
            _mockService.Setup(s => s.GetUserReferrals(userId)).Returns(referrals);

            var actionResult = _controller.GetReferralsByUser(userId);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var resultList = Assert.IsType<List<Referral>>(okResult.Value);

            Assert.Single(resultList);
            Assert.Equal("REF10001", resultList[0].Code);
        }

        [Fact]
        public void GetReferralByCode_ReturnsOkObjectResult()
        {
            var referral = new Referral { Code = "REF10001" };
            _mockService.Setup(s => s.GetReferralByCode("REF10001")).Returns(referral);

            var actionResult = _controller.GetReferralByCode("REF10001");

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var resultReferral = Assert.IsType<Referral>(okResult.Value);

            Assert.Equal("REF10001", resultReferral.Code);
        }
    }
}
