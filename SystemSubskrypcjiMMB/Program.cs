using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SystemSubskrypcjiMMB.Data;
using SystemSubskrypcjiMMB.Models;
using SystemSubskrypcjiMMB.Repositories;
using SystemSubskrypcjiMMB.Services;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<DbContextMMB>(options =>
    options.UseInMemoryDatabase("SubskrypcjeMMB"));

// Identity Z PEŁNĄ KONFIGURACJĄ
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<DbContextMMB>()
.AddDefaultUI()  // Login/Register UI
.AddDefaultTokenProviders();

// Controllers + Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Nasze serwisy
builder.Services.AddScoped<ISubskrypcjaRepositoryMMB, SubskrypcjaRepositoryMMB>();
builder.Services.AddScoped<ISubskrypcjaServiceMMB, SubskrypcjaServiceMMB>();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// SEED ROLES
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roleNames = { "Admin", "User" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
            await roleManager.CreateAsync(new IdentityRole(roleName));
    }

    // Admin user
    var adminEmail = "admin@test.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
        await userManager.CreateAsync(adminUser, "Test123!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

app.Run();
