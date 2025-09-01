using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StajProjesi.Models;

namespace StajProjesi.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration config)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();

        // Roller
        const string adminRole = "Admin";
        if (!await roleManager.Roles.AnyAsync(r => r.Name == adminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        // Admin kullanıcı
        var email = config["Admin:Email"] ?? "admin@staj.local";
        var password = config["Admin:Password"] ?? "Admin123!";

        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new AppUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new Exception("Admin kullanıcısı oluşturulamadı: " + errors);
            }
        }

        // Admin rolünde mi?
        if (!await userManager.IsInRoleAsync(user, adminRole))
        {
            await userManager.AddToRoleAsync(user, adminRole);
        }
    }
}