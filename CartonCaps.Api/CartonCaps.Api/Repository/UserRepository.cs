using CartonCaps.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;
using CartonCaps.Common.Exceptions;
using CartonCaps.Api.Repository.Interface;
using CartonCaps.Api.Repository.Mock;

namespace CartonCaps.Repository
{
    /// <summary>
    /// JSON-based repository for users (mock implementation).
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly JsonMock _database;

        public UserRepository(JsonMock database)
        {
            _database = database;
        }

        public User? GetByEmail(string email)
        {
            User _user = _database.Users
                .FirstOrDefault(u =>
                    u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            return _user;
        }

        public User? GetById(string id)
        {
            return _database.Users
                .FirstOrDefault(u => u.Id == id );
        }

        public IReadOnlyList<User> GetAll()
        {
            return _database.Users.ToList();
        }
    }
}