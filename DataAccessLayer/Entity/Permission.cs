using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmIAuthorised.DataAccessLayer.Entity
{
    [Table("permission", Schema = "public")]
    public class Permission
    {
        [Key]
        [Column("permission_id")]
        public int PermissionId { get; set; }

        [Column("code")]
        public string Code { get; set; } = string.Empty;

        [Column("description")]
        public string Description { get; set; } = string.Empty;
    }
}
