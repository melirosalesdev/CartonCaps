using CartonCaps.Model;

namespace CartonCaps.Api.Service.Interface
{
    public interface IUserService
    {
        User GetByEmail(string email);

    }
}
