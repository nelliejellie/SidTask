using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SIDWeb.Data;
using Microsoft.AspNetCore.Identity;
using SIDWeb.Contracts;
using SIDWeb.Repositories;

namespace SIDWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Fix for CS0103: The name 'Configuration' does not exist in the current context
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Fix for CS1061: 'DbContextOptionsBuilder' does not contain a definition for 'UseSqlServer'
            builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString));

            // Register the repository
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ICustomerRawsqlRepository, CustomerRawSqlRepository>();

            // Fix for CS1061: 'IServiceCollection' does not contain a definition for 'AddDefaultIdentity'
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });



            var app = builder.Build();

            // Apply Migrations & Seed Identity Roles on Startup
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<AppDbContext>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                try
                {
                    // Apply any pending migrations
                    dbContext.Database.Migrate();

                    // Seed Roles
                    await SeedRolesAndAdminUser(roleManager, userManager);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during migration: {ex.Message}");
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }

        public static async Task SeedRolesAndAdminUser(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            string[] roleNames = { "Admin", "User" };

            // Create roles if they don't exist
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create an Admin User (if one doesn't exist)
            string adminEmail = "admin@example.com";
            string adminPassword = "Admin@123"; // Change this in production!

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
