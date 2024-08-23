using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NetAng.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "3038aa51-856d-4380-a41d-5ac4582290a3";
            var writerRoleId = "f71157cf-d249-4c47-a1b7-b8614954b7d4";

            // Create Reader and Writer Role
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId,
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId,
                }
            };

            // Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            // Create Admin User
            var adminUserId = "5845860d-3aa9-4214-b4b9-2a1717ee7357";

            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var userName = MyConfig.GetValue<string>("AdminInfo:AdminUsername");
            var userEmail = MyConfig.GetValue<string>("AdminInfo:AdminEmail");
            var userPassword = MyConfig.GetValue<string>("AdminInfo:AdminPassword");

            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = userName,
                Email = userEmail,
                NormalizedUserName = userName!.ToUpper(),
                NormalizedEmail = userEmail!.ToUpper(),
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, userPassword!);

            builder.Entity<IdentityUser>().HasData(admin);

            // Assign Roles to Admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new ()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId,
                },
                new ()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId,
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
