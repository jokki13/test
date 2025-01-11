using IdentityProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace IdentityProject.Pages
{
    public class UserHelperHash
    {
        private readonly UserManager<IdentityProjectUser> _userManager;

        public UserHelperHash(UserManager<IdentityProjectUser> userManager)
        {
            _userManager = userManager;
        }

        public static async Task<string> GetPasswordHashAsync(string userName, UserManager<IdentityProjectUser> _userManager) 
        {
            var user = await _userManager.FindByNameAsync(userName);
            string storedPasswordHash = user.PasswordHash;

            return storedPasswordHash;
        }
    }
}
