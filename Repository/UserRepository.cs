using AmIAuthorised.DataAccessLayer.Database;
using AmIAuthorised.DataAccessLayer.DTO;
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

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public void Add(Role role)
        {
            _context.Roles.Add(role);
        }

        public void Add(Permission permission)
        {
            _context.Permissions.Add(permission);
        }

        public void Add(RolePermission rolePermission)
        {
            _context.RolePermissions.Add(rolePermission);
        }

        public async Task<UserDTO?> GetUserByUserName(string userName)
        {
            return await (
              from u in _context.Users
              join rp in _context.RolePermissions on u.RoleId equals rp.RoleId
              join p in _context.Permissions on rp.PermissionId equals p.PermissionId
              where u.UserName == userName
              group p by new
              {
                  u.UserId,
                  u.UserName,
                  u.Password,
                  u.FirstName,
                  u.LastName,
                  u.Email
              } into g
              select new UserDTO
              {
                  UserId = g.Key.UserId,
                  UserName = g.Key.UserName,
                  Password = g.Key.Password,
                  FirstName = g.Key.FirstName,
                  LastName = g.Key.LastName,
                  Email = g.Key.Email,
                  Permissions = g.Select(x => x.Code).ToList()
              }
          ).SingleOrDefaultAsync();
        }

        public async Task<Role?> GetRoleByName(string roleName)
        {
            return await _context.Roles.Where(r => r.RoleName == roleName).FirstOrDefaultAsync();
        }

        public async Task<Role?> GetRoleById(int roleId)
        {
            return await _context.Roles.Where(r => r.RoleId == roleId).FirstOrDefaultAsync();
        }

        public async Task<Permission?> GetPermissionByCode(string code)
        {
            return await _context.Permissions.Where(p => p.Code == code).FirstOrDefaultAsync();
        }

        public async Task<Permission?> GetPermissionById(int permissionId)
        {
            return await _context.Permissions.Where(p => p.PermissionId == permissionId).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<List<Role>> GetRoles()
        {
            return await _context.Roles.AsNoTracking().ToListAsync();
        }

    }
}
