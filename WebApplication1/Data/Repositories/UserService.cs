using Microsoft.AspNetCore.Identity;
using MiniMarket_API.Model.Entities;
using WebApplication1.Data.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Data.Repositories
{
    public class UserService : IUserService
    {
        private readonly GenericAPIContext _context;
        
        public UserService(GenericAPIContext context) 
        {
            _context = context;
        }
        public Users Authenticate(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
                return null;

            user.Password = null;
            return user;
        }
    }
}
