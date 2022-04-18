using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace USERWebApi.Models
{
    public class AppSeedAdmin
    {
        private readonly UserManager<RegistrationModel> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AppSeedAdmin(UserManager<RegistrationModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task SeedUsersAsync()
        {
            await roleManager.CreateAsync(new IdentityRole { Name = "SystemAdmin" });
            if (!userManager.Users.Any())
            {
                var SystemAdmin = new RegistrationModel
                {

                    UserName = "Rohitkumar@gmail.com",
                    PhoneNumber = "9867457677",
                    Email = "rohitkumar@gmail.com",
                    PasswordHash = "Rohit@123",
                    Category = "SystemAdmin"

                };
                await userManager.CreateAsync(SystemAdmin, "Rohit@123");

                await userManager.AddToRoleAsync(SystemAdmin, "SystemAdmin");
            }
        }
    }
}
