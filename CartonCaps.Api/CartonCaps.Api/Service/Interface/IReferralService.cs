using CartonCaps.Model;

namespace CartonCaps.Api.Service.Interface
{
    public interface IReferralService
    {
        Referral GenerateReferral(string userId);
        Referral RedeemReferral(string code, string referredUserId);
        IReadOnlyList<Referral> GetUserReferrals(string userId);
        Referral GetReferralByCode(string code);
    }
}
