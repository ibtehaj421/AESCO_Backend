using ASCO.Models;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using ASCO.DbContext;


namespace ASCP.Repositories {
    public class RoleRepository
    {
        private readonly ASCODbContext _context;

        public RoleRepository(ASCODbContext context)
        {
            _context = context;
        }

        public async Task<int> CheckRoleExistsAsync(int roleId)
        {
            return await _context.Roles.CountAsync(r => r.Id == roleId);
        }

        //add user role values 
        public async Task<int> AssignRolesToUserAsync(List<UserRole> userRoles)
        {
            await _context.UserRoles.AddRangeAsync(userRoles);
            return await _context.SaveChangesAsync(); //if the value is positive, the roles were assigned successfully.
        }

        public async Task<int> AddPermissionsAsync(List<Permission> permissions)
        {
            await _context.Permissions.AddRangeAsync(permissions);
            return await _context.SaveChangesAsync(); //if the value is positive, the permissions were added successfully.
        }
        public async Task<bool> AnyPermissionAsync()
        {
            return await _context.Permissions.AnyAsync();
        }
        public async Task<bool> AnyRolesAsync()
        {
            return await _context.Roles.AnyAsync();
        }
        public async Task<int> AddRolesAsync(List<Role> roles)
        {
            await _context.Roles.AddRangeAsync(roles);
            return await _context.SaveChangesAsync(); //if the value is positive, the roles were added successfully.
        }
        public async Task<List<Role>> RolesToListAsync()
        {
            return await _context.Roles.ToListAsync();
        }
        public async Task<List<Permission>> PermissionsToListAsync()
        {
            return await _context.Permissions.ToListAsync();
        }

        public async Task<int> AddRolePermissionsAsync(List<RolePermission> rolePermissions)
        {
            await _context.RolePermissions.AddRangeAsync(rolePermissions);
            return await _context.SaveChangesAsync(); //if the value is positive, the role-permissions were added successfully.
        }
    }
}