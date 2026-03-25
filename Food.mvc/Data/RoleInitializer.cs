using Microsoft.AspNetCore.Identity;

namespace Food.mvc.Data
{
    public static class RoleInitializer
    {
        // ✅ CRIAR ROLES
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Inspector", "Viewer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        // ✅ CRIAR ADMIN
        public static async Task SeedAdmin(UserManager<IdentityUser> userManager)
        {
            var adminEmail = "admin@food.com";
            var password = "Admin123!";

            var user = await userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

        // ✅ CRIAR USERS DE TESTE
        public static async Task SeedTestUsers(UserManager<IdentityUser> userManager)
        {
            await CreateUserWithRole(userManager, "inspector@food.com", "Inspector123!", "Inspector");
            await CreateUserWithRole(userManager, "viewer@food.com", "Viewer123!", "Viewer");
        }

        private static async Task CreateUserWithRole(
            UserManager<IdentityUser> userManager,
            string email,
            string password,
            string role)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}