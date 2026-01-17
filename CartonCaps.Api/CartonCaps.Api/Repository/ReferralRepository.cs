using CartonCaps.Api.Repository.Interface;
using CartonCaps.Api.Repository.Mock;
using CartonCaps.Common.Exceptions;
using CartonCaps.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;


namespace CartonCaps.Repository
{
    /// <summary>
    /// JSON-based repository for referrals (mock implementation).
    /// </summary>
    public class ReferralRepository : IReferralRepository
    {
        private readonly JsonMock _database;
        public ReferralRepository(JsonMock database)
        {
            _database = database;
        }

        public Referral? GetByCode(string code)
        {
            return _database.Referrals
                .FirstOrDefault(r =>
                    r.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<Referral> GetByUser(string userId)
        {
            return _database.Referrals
                .Where(r => r.UserId == userId)
                .ToList();
        }
        public Referral GetByReferred(string userId)
        {
            return _database.Referrals
                .Where(r => r.ReferredUserId == userId)
                .FirstOrDefault();
        }
        public Referral Create(string userId)
        {
            var referral = new Referral
            {
                Code = GenerateCode(),
                UserId = userId,
                Status = "pending",
                Expiration = DateTime.UtcNow.AddDays(30),
                RedeemDate = null,
                ReferredUserId = null
            };

            _database.Referrals.Add(referral);

            return referral;
        }

        public void Redeem(string code, string referredUserId)
        {
            var referral = GetByCode(code)
                ?? throw new NotFoundException("Referral not found");

            referral.Status = "redeemed";
            referral.RedeemDate = DateTime.UtcNow;
            referral.ReferredUserId = referredUserId;
        }

        private static string GenerateCode()
        {
            return Guid.NewGuid()
                .ToString("N")
                .Substring(0, 5)
                .ToUpperInvariant();
        }
    }
}
