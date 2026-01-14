using AmIAuthorised.DataAccessLayer.Database;
using AmIAuthorised.DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace AmIAuthorised.Repository
{
    public class UserRepository : AbstractRepository
    {
        private readonly AmIAuthorisedContext _context;
        public UserRepository(AmIAuthorisedContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUserName(string userName)
        {
            return await _context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Role?> GetRoleByName(string roleName)
        {
            return await _context.Roles.Where(r => r.RoleName == roleName).FirstOrDefaultAsync();
        }

        public async Task CreateRole(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task<Role?> GetRoleById(int roleId)
        {
            return await _context.Roles.Where(r => r.RoleId == roleId).FirstOrDefaultAsync();
        }
    }
}
