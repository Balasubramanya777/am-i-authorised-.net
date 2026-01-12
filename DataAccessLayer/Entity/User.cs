using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmIAuthorised.DataAccessLayer.Entity
{
    [Table("user", Schema = "public")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public long UserId { get; set; }

        [Required]
        [MaxLength(25)]
        [Column("user_name")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(25)]
        [Column("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(25)]
        [Column("last_name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("password")]
        public string Password { get; set; } = string.Empty;
    }
}
