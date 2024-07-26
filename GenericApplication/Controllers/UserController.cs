using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Data.Interfaces;

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


        //create user
        [HttpPost]
        public async Task<IActionResult> CreateNewAdminAsync([FromBody] CreateUserDto createAdmin)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (userRole == typeof(SuperAdmin).Name)
            {
                var adminToCreate = mapper.Map<SuperAdmin>(createAdmin);

                var createdAdmin = await _userService.CreateUser(adminToCreate, createAdmin.Password);
                if (createdAdmin == null)
                {
                    return Conflict("Admin Creation Failed: Email Currently in Use!");
                }

                return Ok(createdAdmin);
            }

            return Forbid();

        }
    }
}
