using CartonCaps.Model;

namespace CartonCaps.Api.Repository.Interface
{
    public interface IReferralRepository
    {
        Referral? GetByCode(string code);
        IReadOnlyList<Referral> GetByUser(string userId);
        Referral GetByReferred(string userId);
        Referral Create(string userId);
        void Redeem(string code, string referredUserId);
    }
}
