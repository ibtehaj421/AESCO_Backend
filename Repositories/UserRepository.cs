using ASCO.Models;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using ASCO.DbContext;


namespace ASCO.Repositories {
    public class UserRepository
    {
        private readonly ASCODbContext _context;

        public UserRepository(ASCODbContext context)
        {
            _context = context;
        }


        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User?> GetUserWithPermissionsAsync(string email)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<int> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync(); //if the value is positive, the user was added successfully.
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
        public async Task<long> GetUserID(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user != null ? user.Id : 0;
        }
    }
}