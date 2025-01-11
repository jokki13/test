using IdentityProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IdentityProject.Pages
{
    //[Authorize(Policy = "RequireSuperAdmin")]
    public class EditUserRolesModel : PageModel
    {
        private readonly UserManager<IdentityProjectUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditUserRolesModel(UserManager<IdentityProjectUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IdentityProjectUser User { get; set; }
        public IList<IdentityRole> Roles { get; set; }
        public IList<string> UserRoles { get; set; }

        [BindProperty]
        public List<string> SelectedRoles { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            User = await _userManager.FindByIdAsync(id);
            if (User == null)
            {
                return NotFound();
            }

            Roles = await _roleManager.Roles.ToListAsync();
            UserRoles = await _userManager.GetRolesAsync(User);
            SelectedRoles = UserRoles.ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            User = await _userManager.FindByIdAsync(id);
            if (User == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(User);
            var rolesToAdd = SelectedRoles.Except(currentRoles);
            var rolesToRemove = currentRoles.Except(SelectedRoles);

            await _userManager.AddToRolesAsync(User, rolesToAdd);
            await _userManager.RemoveFromRolesAsync(User, rolesToRemove);

            return RedirectToPage("UserList");
        }
    }
}
