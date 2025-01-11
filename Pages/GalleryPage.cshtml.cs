using IdentityProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityProject.Pages
{
    public class GalleryPageModel : PageModel
    {
        private readonly UserManager<IdentityProjectUser> _userManager;

        public GalleryPageModel(UserManager<IdentityProjectUser> userManager)
        {
            _userManager = userManager;
        }

        public List<IdentityProjectUser> Users { get; set; }
        public List<string> ImagePaths { get; private set; }

        public async void OnGet()
        {
               //test
               //test2
            //string password =  UserHelper.GetUserImageFolderPath(User.Identity.Name, "Gallery Page", await UserHelperHash.GetPasswordHashAsync(User.Identity.Name,_userManager));
            var email = User.Identity.Name;

            var userFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", email,"images");

            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", email, "images");
            var imageFiles = Directory.GetFiles(imageFolder);


            ImagePaths = new List<string>();

            foreach (var imageFile in imageFiles)
            {
                var relativePath = Path.Combine("users", email, "images", Path.GetFileName(imageFile));
                ImagePaths.Add(relativePath);
            }
        }
    }
}
