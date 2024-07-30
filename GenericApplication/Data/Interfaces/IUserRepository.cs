using MiniMarket_API.Model.Entities;

namespace WebApplication1.Data.Interfaces
{
    public interface IUserRepository
    {
        //autenticacion
        Users Authenticate(string username, string password);

        //traer user por id
        Task<Users?> GetUserByIdAsync(Guid id);

        //funcion para crear user
        Task<Users> CreateUserAsync(Users user);
    }
}
