
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services
{
    public class UserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // Search users by a specific column and value
        public async Task<List<IdentityUser>> SearchUsersAsync(Expression<Func<IdentityUser, bool>> predicate)
        {
            return await _userManager.Users
                .Where(predicate) // Apply the search expression
                .ToListAsync();
        }
    }
}
