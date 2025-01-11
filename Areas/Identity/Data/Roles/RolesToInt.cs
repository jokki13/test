namespace IdentityProject.Areas.Identity.Data.Roles
{
    public static class RolesToInt
    {
        private static readonly string[] Roles = { "Super Admin", "Admin", "Operater", "Super User", "User", "Guest" };

        public static int GetAccessLevel(string role)
        {
            for (int i = 0; i < Roles.Length; i++)
            {
                if (Roles[i] == role)
                {
                    return i; 
                }
            }
            return -1; 
        }
    }
}
