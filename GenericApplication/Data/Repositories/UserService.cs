using Microsoft.AspNetCore.Identity;
using MiniMarket_API.Model.Entities;
using WebApplication1.Data.Interfaces;
using WebApplication1.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data.Repositories
{
    public class UserService : IUserRepository
    {
        private readonly GenericAPIContext _context;
        private readonly IPasswordHasher<Users> _passwordHasher;

        public UserService(GenericAPIContext context, IPasswordHasher<Users> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        //Authenticate
        public Users Authenticate(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == username);

            if (user == null)
                return null;

            // Convert the hashed password from byte[] to string
            var hashedPasswordString = Encoding.UTF8.GetString(user.Password);

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, hashedPasswordString, password);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
                return null;

            // Clear the password field before returning the user
            user.Password = null;
            return user;
        }

        
        // end authenticate


        //create user
        public async Task<Users> CreateUserAsync(Users user) 
        {
            user.Id = Guid.NewGuid();
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        // 


        //traer user por id
        public Task<Users?> GetUserByIdAsync(Guid id)
        {
            return _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        //
    }
}
