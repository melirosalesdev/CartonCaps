using CartonCaps.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CartonCaps.Api.Repository.Mock
{
    public class JsonMock
    {
        private readonly MockDatabase _database;

        public JsonMock(
            IWebHostEnvironment env,
            IOptions<MockStorageOptions> options)
        {
            var filePath = Path.Combine(
                env.ContentRootPath,
                options.Value.DataFilePath
            );

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(
                    $"Mock data file not found at path: {filePath}"
                );
            }

            var json = File.ReadAllText(filePath);

            _database = JsonSerializer.Deserialize<MockDatabase>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new MockDatabase();
        }

        public IReadOnlyList<User> Users => _database.Users;
        public List<Referral> Referrals => _database.Referrals;
    }
}
