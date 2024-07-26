using Microsoft.EntityFrameworkCore;
using MiniMarket_API.Model.Entities;

namespace WebApplication1.Models
{
    public class GenericAPIContext : DbContext
    {
        public GenericAPIContext(DbContextOptions<GenericAPIContext> options)
            : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }
    }
}
