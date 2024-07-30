using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniMarket_API.Model.Entities;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Data.Interfaces;
using WebApplication1.Models.Dtos;

namespace WebApplication1.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userService;
        
        public UserController(IUserRepository userService) 
        {
            _userService = userService;
        }


        // Crear usuario sin requerir autenticación
        [HttpPost("createUser")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto createUserDto)
        {
            if (createUserDto == null)
            {
                return BadRequest("Invalid user data.");
            }

            try
            {
                // Convertir la contraseña de string a byte[]
                var passwordBytes = HashPassword(createUserDto.Password);

                var user = new Users
                {
                    Name = createUserDto.Name,
                    Username = createUserDto.Username,
                    Email = createUserDto.Email,
                    Password = passwordBytes,
                    UserType = createUserDto.UserType // Esto puede ser nulo
                };

                var createdUser = await _userService.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetUserByIdAsync), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                // Manejo del error, puedes registrar el error si es necesario
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }



        // Definición de la función HashPassword
        private byte[] HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
