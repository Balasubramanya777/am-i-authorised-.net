namespace AmIAuthorised.DataAccessLayer.DTO
{
    public class RoleUpsertDTO
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public List<int>? PermissionIds { get; set; }
    }
}
