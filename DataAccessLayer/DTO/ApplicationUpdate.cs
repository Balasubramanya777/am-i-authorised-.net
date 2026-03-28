namespace AmIAuthorised.DataAccessLayer.DTO
{
    public class ApplicationUpdate
    {
        public long ApplicationId { get; set; }
        public required string ApplicationName { get; set; }
    }
}
