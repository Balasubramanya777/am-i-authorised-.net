using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AmIAuthorised.DataAccessLayer.Entity
{

    [Table("role_permission", Schema = "public")]
    [PrimaryKey(nameof(RoleId), nameof(PermissionId))]
    public class RolePermission
    {
        [Column("role_id")]
        public int RoleId { get; set; }

        [Column("permission_id")]
        public int PermissionId { get; set; }


        //[ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }

        //[ForeignKey(nameof(PermissionId))]
        public Permission? Permission { get; set; }
    }
}
