using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniMarket_API.Model.Entities
{
    public class Users
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Username { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        public string Email { get; set; }

        [Column(TypeName = "binary(32)")]
        public byte[] Password { get; set; }


        [Column("Role", TypeName = "nvarchar(25)")]
        public string UserType { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
