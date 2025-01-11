using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityProject.Data;
using IdentityProject.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = "server=localhost;database=test;uid=root;pwd=;";
builder.Services.AddDbContext<IdentityProjectContext>(options => options.UseMySql(connectionString,new MySqlServerVersion(new Version(10,4,32))));

builder.Services.AddDefaultIdentity<IdentityProjectUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityProjectContext>();



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireSuperAdmin", policy => policy.RequireRole("Super Admin"));
});


// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

app.Urls.Add("http://192.168.1.107:7017");
app.Urls.Add("https://192.168.1.107:7018");

//app.Urls.Add("http://192.168.153.254:7017");
//app.Urls.Add("https://192.168.153.254:7018");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager=scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Super Admin","Admin","Operater","Super User","User","Guest"};

    foreach (var role in roles)
    {
        if(!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityProjectUser>>();

    string email = "admin@leparec.org";
    string password = "Admin.123";

    if(await userManager.FindByEmailAsync(email) == null)
    {
        var user =  new IdentityProjectUser();
        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;
        user.Gender = "male";
        await userManager.CreateAsync(user,password);

        await userManager.AddToRoleAsync(user,"Super Admin");
    }

}




app.Run();
