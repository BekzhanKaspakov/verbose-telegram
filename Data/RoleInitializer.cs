using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WebApplication5.Models;

namespace WebApplication5.Data
{
    public class RoleInitializer
    {
        public static async System.Threading.Tasks.Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            string adminEmail = "asd@mail.com";
            string password = "Rabbit12'";
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new ApplicationRole("Admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new ApplicationRole("user"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var user = new ApplicationUser { UserName = adminEmail, Email = adminEmail, FirstName = "Admin", LastName = "Adminovich" };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}