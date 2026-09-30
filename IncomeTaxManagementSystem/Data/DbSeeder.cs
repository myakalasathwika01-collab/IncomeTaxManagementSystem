using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IncomeTaxManagementSystem.Models;

namespace IncomeTaxManagementSystem.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");

            const string adminEmail = "admin@incometax.local";
            const string adminPAN = "ADMNT1234A";

            // Check if an admin account already exists
            bool adminExists = await context.Users.AnyAsync(u => u.Email == adminEmail || u.Role == "Admin");

            if (!adminExists)
            {
                logger.LogInformation("Seeding development admin account: {Email}", adminEmail);

                var adminUser = new User
                {
                    FullName = "System Administrator",
                    PAN = adminPAN,
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Mobile = "9876543210",
                    Email = adminEmail,
                    Address = "Central Processing Center, Income Tax Department",
                    Role = "Admin",
                    Status = "Active",
                    CreatedDate = DateTime.Now
                };

                // Temporary development password - MUST be changed in production
                const string devPassword = "Admin@Dev2026!";
                adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, devPassword);

                context.Users.Add(adminUser);
                await context.SaveChangesAsync();

                logger.LogInformation("Development admin account seeded successfully.");
            }
        }
    }
}
