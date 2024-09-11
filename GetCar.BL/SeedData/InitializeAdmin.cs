using GetCar.DB.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.SeedData
{
    public class InitializeAdmin
    {

            public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                var roleName = "Admin";
                var email = "admin@GetCar.com";
                var password = "Admin123?#";

                // Create roles if they don't exist
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }

                // Create admin user if it doesn't exist
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new Admin
                    {
                        UserName = email,  
                        Email = email,
                        Address="Cairo",
                        FristName="GetCar",
                        LastName="Admin",
                        CreatedAt=DateTime.Now,
                        Gender="Male",
                        PhoneNumber="01012456789",
                        IsActived=true,
                        EmailConfirmed=true,
                    };

                    var result = await userManager.CreateAsync(user, password);

                    if (!result.Succeeded)
                    {
                        // Log or handle error
                        throw new Exception($"Failed to create the admin user: {string.Join(", ", result.Errors)}");
                    }
                }

                // Re-fetch the user from the database to ensure we have the latest data
                user = await userManager.FindByEmailAsync(email);

                // Add user to Admin role
                if (!await userManager.IsInRoleAsync(user, roleName))
                {
                    var roleResult = await userManager.AddToRoleAsync(user, roleName);

                    if (!roleResult.Succeeded)
                    {
                        // Log or handle error
                        throw new Exception($"Failed to add the user to the Admin role: {string.Join(", ", roleResult.Errors)}");
                    }
                }
            }
        }
    
}
