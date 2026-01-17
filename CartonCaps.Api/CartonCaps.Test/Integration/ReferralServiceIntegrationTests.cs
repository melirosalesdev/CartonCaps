using CartonCaps.Api.Repository.Mock;
using CartonCaps.Model;
using CartonCaps.Repository;
using CartonCaps.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace CartonCaps.Test.Integration
{
    public class ReferralServiceIntegrationTests
    {
        private readonly ReferralService _service;
        private readonly JsonMock _jsonMock;

        public ReferralServiceIntegrationTests()
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
             
            var referralRepo = new ReferralRepository(_jsonMock);
            var userRepo = new UserRepository(_jsonMock);
            _service = new ReferralService(referralRepo, userRepo);
        }

        [Fact]
        public void GenerateReferral_AddsReferralToJsonMock()
        {
            var userId = "user_001";

            var referral = _service.GenerateReferral(userId);

            Assert.NotNull(referral);
            Assert.Equal(userId, referral.UserId);
            Assert.Contains(_jsonMock.Referrals, r => r.Code == referral.Code);
        }

        [Fact]
        public void RedeemReferral_UpdatesReferralStatus()
        {
            var userId = "user_003";
            var referral = _service.GenerateReferral(userId);
            var referredUserId = "user_004";

            var redeemedReferral = _service.RedeemReferral(referral.Code, referredUserId);

            Assert.Equal("redeemed", redeemedReferral.Status);
            Assert.Equal(referredUserId, redeemedReferral.ReferredUserId);
            Assert.NotNull(redeemedReferral.RedeemDate);
        }

        [Fact]
        public void GetUserReferrals_ReturnsCorrectList()
        {
            var userId = "user_001";
            var referrals = _service.GetUserReferrals(userId);

            Assert.All(referrals, r => Assert.Equal(userId, r.UserId));
        }

        [Fact]
        public void GetReferralByCode_ReturnsCorrectReferral()
        {
            var referral = _jsonMock.Referrals.First();
            var fetched = _service.GetReferralByCode(referral.Code);

            Assert.Equal(referral.Code, fetched.Code);
        }
    }
}