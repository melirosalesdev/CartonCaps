using CartonCaps.Api.Service.Interface;
using CartonCaps.Model;
using Microsoft.AspNetCore.Mvc;

namespace CartonCaps.Api.Controllers
{
    [ApiController]
    [Route("api/referrals")]
    public class ReferralsController : ControllerBase
    {
        private readonly IReferralService _referralService;

        public ReferralsController(IReferralService referralService)
        {
            _referralService = referralService;
        }

        /// <summary>
        /// Generates a new referral code for a user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /api/referrals?userId=user_123
        ///
        /// Sample response:
        /// {
        ///   "code": "ABC12345",
        ///   "userId": "user_123",
        ///   "status": "pending",
        ///   "expiration": "2026-03-01T00:00:00Z",
        ///   "redeemDate": null,
        ///   "referredUserId": null
        /// }
        /// </remarks>
        [HttpPost]
        public ActionResult<Referral> GenerateReferral([FromQuery] string userId)
        {
            var referral = _referralService.GenerateReferral(userId);

            return CreatedAtAction(nameof(GetReferralByCode),new { code = referral.Code },referral);
        }

        /// <summary>
        /// Redeems a referral code.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /api/referrals/redeem?code=ABC12345&amp;referredUserId=user_456
        /// </remarks>
        [HttpPost("redeem")]
        public ActionResult<Referral> RedeemReferral([FromQuery] string code,[FromQuery] string referredUserId)
        {
            var referral = _referralService.RedeemReferral(code,referredUserId);

            return Ok(referral);
        }

        /// <summary>
        /// Gets all referrals created by a user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/referrals?userId=user_123
        /// </remarks>
        [HttpGet]
        public ActionResult<IReadOnlyList<Referral>> GetReferralsByUser([FromQuery] string userId)
        {
            var referrals = _referralService.GetUserReferrals(userId);

            return Ok(referrals);
        }

        /// <summary>
        /// Gets a referral by its code.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/referrals/ABC12345
        /// </remarks>
        [HttpGet("{code}")]
        public ActionResult<Referral> GetReferralByCode(string code)
        {
            var referral = _referralService.GetReferralByCode(code);

            return Ok(referral);
        }
    }
}
