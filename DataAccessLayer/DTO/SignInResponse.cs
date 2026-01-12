namespace AmIAuthorised.DataAccessLayer.DTO
{
    public class SignInResponse
    {
        public required string AccessToken { get; set; }
        public required UserDTO User { get; set; }
    }
}
