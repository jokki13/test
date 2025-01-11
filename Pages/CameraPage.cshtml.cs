using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System.IO;

namespace IdentityProject.Pages
{
    public class CameraPageModel : PageModel
    {

        [BindProperty]
        public string imageData { get; set; }

        public List<CameraDevice> Cameras { get; set; }


        public void OnGet()
        {
            //UserHelper.GetUserImageFolderPath(User.Identity.Name, "Camera Page",User.Identity.Name);
            Cameras = GetAvailableCameras();
        }


        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(imageData))
            {
                //var username = Request.Query["username"];
                string username = User.Identity.Name;

                int broj = 0;

                List<int> brojevi = new List<int>();

                var email = username;

                var userFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", email, "images");

                if (!Directory.Exists(userFolder))
                {
                    Directory.CreateDirectory(userFolder);
                }

                //var imageFolder = userFolder;
                email = email.ToString();
                var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", email, "images");

                var imageFiles = Directory.GetFiles(imageFolder);

                foreach (var img_l in imageFiles)
                {
                    string img = Path.GetFileName(img_l);
                    string naziv = img.Split('.')[0];
                    string broj_s = naziv.Split("-")[1];
                    int.TryParse(broj_s, out int broj1);
                    brojevi.Add(broj1);
                }
                brojevi.Sort((a, b) => b.CompareTo(a));

                if (brojevi.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine("Broj pret: " + brojevi[0]);
                    if (brojevi[0] >= broj)
                    {
                        broj = brojevi[0] + 1;
                    }
                    System.Diagnostics.Debug.WriteLine("Broj: " + broj);
                }



                // Remove the data URL prefix
                var base64Data = imageData.Substring(imageData.IndexOf(',') + 1);

                // Convert base64 string to byte array
                byte[] imageBytes = Convert.FromBase64String(base64Data);

                // Save the image to a file (for example)
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", email, "images", "picture-" + broj + ".png");

                System.IO.File.WriteAllBytes(filePath, imageBytes);

                // Optionally, return a result or redirect
                // return RedirectToPage("Success"); // Redirect to a success page or another page

            }

            // Handle case where image data is not available
            return RedirectToPage();
        }
        private List<CameraDevice> GetAvailableCameras()
        {
            // This list should be populated dynamically if possible
            return new List<CameraDevice>
            {
                new CameraDevice { DeviceId = "deviceId1", Name = "Camera 1" },
                new CameraDevice { DeviceId = "deviceId2", Name = "Camera 2" }
            };
        }
        public class CameraDevice
        {
            public string DeviceId { get; set; }
            public string Name { get; set; }
        }
    }
}
