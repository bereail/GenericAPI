using MiniMarket_API.Model.Entities;

namespace WebApplication1.Data.Interfaces
{
    public interface IUserRepository
    {
        Users Authenticate(string username, string password);

        //funcion para crear user
        Task<Users> CreateUserAsync(Users user);
    }
}
