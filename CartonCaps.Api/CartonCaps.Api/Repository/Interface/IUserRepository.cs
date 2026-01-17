using CartonCaps.Model;

namespace CartonCaps.Api.Repository.Interface
{
    public interface IUserRepository
    {
        User? GetByEmail(string email);
        User? GetById(string id);
    }
}
