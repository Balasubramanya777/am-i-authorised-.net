using AmIAuthorised.DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace AmIAuthorised.DataAccessLayer.Database
{
    public class AmIAuthorisedContext : DbContext
    {
        public AmIAuthorisedContext(DbContextOptions options) : base(options) { }

        DbSet<User> Users { get; set; }

    }
}
