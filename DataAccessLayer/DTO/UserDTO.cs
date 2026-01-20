namespace AmIAuthorised.DataAccessLayer.DTO
{
    public class UserDTO
    {
        public long UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = [];
    }
}
