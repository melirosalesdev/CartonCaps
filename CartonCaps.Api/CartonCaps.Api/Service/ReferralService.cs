using CartonCaps.Api.Repository.Interface;
using CartonCaps.Api.Service.Interface;
using CartonCaps.Common.Exceptions;
using CartonCaps.Model;

namespace CartonCaps.Service
{
    public class ReferralService : IReferralService
    {
        private readonly IReferralRepository _referrals;
        private readonly IUserRepository _users;

        public ReferralService(
            IReferralRepository referrals,
            IUserRepository users)
        {
            _referrals = referrals;
            _users = users;
        }

        public CartonCaps.Model.Referral GenerateReferral(string userId)
        {
            var user = _users.GetById(userId)
                ?? throw new NotFoundException("User not found");

            return _referrals.Create(userId);
        }

        public Model.Referral RedeemReferral(string code, string referredUserId)
        {
            var referral = _referrals.GetByCode(code)
                ?? throw new NotFoundException("Referral not found");
             
            if (referral.Status != "pending")
                throw new InvalidOperationException("Referral not valid");

            if (referral.UserId == referredUserId)
                throw new InvalidOperationException("User not allowed to use this referral");

            _referrals.Redeem(code, referredUserId);
            return referral;
        }

        public IReadOnlyList<Model.Referral> GetUserReferrals(string userId)
        {
            return _referrals.GetByUser(userId);
        }

        public Referral GetReferralByCode(string code)
        {
            return _referrals.GetByCode(code)
                ?? throw new NotFoundException("Referral not found");
        }
    }
}
