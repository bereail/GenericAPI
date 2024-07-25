using MiniMarket_API.Model.Entities;

namespace WebApplication1.Data.Interfaces
{
    public interface IUserService
    {
        Users Authenticate(string username, string password);
    }
}
