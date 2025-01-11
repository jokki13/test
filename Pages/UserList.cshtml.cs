using IdentityProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IdentityProject.Pages
{
    [Authorize(Policy = "RequireSuperAdmin")]
    public class UserListModel : PageModel
    {
        private readonly UserManager<IdentityProjectUser> _userManager;

        public UserListModel(UserManager<IdentityProjectUser> userManager)
        {
            _userManager = userManager;
        }

        public List<IdentityProjectUser> Users { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            string storedPasswordHash = user.PasswordHash;
            //System.Diagnostics.Debug.WriteLine("HASH:", storedPasswordHash);


            Users = await _userManager.Users.ToListAsync();
            string password = UserHelper.GetUserImageFolderPath(User.Identity.Name, "Admin Page", await UserHelperHash.GetPasswordHashAsync(User.Identity.Name, _userManager));
        }
    }
}
