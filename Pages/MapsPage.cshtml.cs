using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityProject.Pages
{
    public class MapsPageModel : PageModel
    {
        public void onPost()
        {
            //UserHelper.GetUserImageFolderPath(User.Identity.Name, "Maps Page");
        }
        public void OnGet()
        {
            //UserHelper.GetUserImageFolderPath(User.Identity.Name, "Maps Page", User.Identity.GetHashCode().ToString());
        }
    }
}
