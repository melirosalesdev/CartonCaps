using CartonCaps.Api.Repository.Interface;
using CartonCaps.Api.Service.Interface;
using CartonCaps.Common.Exceptions;
using CartonCaps.Model;

namespace CartonCaps.Service
{
    /// <summary>
    /// Application service for user-related operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IReferralRepository _referralRepository;

        public UserService(
            IUserRepository userRepository,
            IReferralRepository referralRepository)
        {
            _userRepository = userRepository;
            _referralRepository = referralRepository;
        }

        public User GetByEmail(string email)
        {
            var user = _userRepository.GetByEmail(email);

            if (user == null)
                throw new NotFoundException($"User not found: {email}");

            AttachReferralIfNeeded(user);
            return user;
        }
        private void AttachReferralIfNeeded(User user)
        {
            if (!user.Referred)
                return;

            user.Referral = _referralRepository.GetByReferred(user.Id);
        }
    }
} 
