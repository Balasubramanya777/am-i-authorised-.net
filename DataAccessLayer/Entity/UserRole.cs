using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AmIAuthorised.DataAccessLayer.Entity
{
    [Table("user_role", Schema = "public")]
    [PrimaryKey(nameof(UserId), nameof(RoleId))]
    public class UserRole
    {
        [Column("user_id")]
        public long UserId { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }

        public User? User { get; set; }
        public Role? Role { get; set; }
    }
}
