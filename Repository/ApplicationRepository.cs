using AmIAuthorised.DataAccessLayer.Database;
using AmIAuthorised.DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace AmIAuthorised.Repository
{
    public class ApplicationRepository : AbstractRepository
    {
        private readonly AmIAuthorisedContext _context;
        public ApplicationRepository(AmIAuthorisedContext context)
        {
            _context = context;
        }

        public async Task CreateApplication(Application application)
        {
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateApplication(Application application)
        {
            _context.Applications.Update(application);
            await _context.SaveChangesAsync();
        }

        public async Task<Application?> GetApplicationById(long applicationId)
        {
            return await _context.Applications.FindAsync(applicationId);
        }

        public async Task<List<Application>> GetApplications()
        {
            return await _context.Applications.AsNoTracking().ToListAsync();
        }
    }
}
