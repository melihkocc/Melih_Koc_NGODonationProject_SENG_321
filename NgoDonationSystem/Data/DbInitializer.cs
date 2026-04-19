using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Models;
using System;
using System.Linq;

namespace NgoDonationSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {





            var roles = new[]
            {
                new Role { RoleName = "Admin", Description = "Full access to all system features." },
                new Role { RoleName = "Donor", Description = "Access to personal donations and history." },
                new Role { RoleName = "FieldWorker", Description = "Access to aid requests and distributions." },
                new Role { RoleName = "Accountant", Description = "Access to expense management and reports." },
                new Role { RoleName = "WarehouseStaff", Description = "Access to inventory and stock operations." }
            };

            foreach (var role in roles)
            {
                if (!context.Roles.Any(r => r.RoleName == role.RoleName))
                {
                    context.Roles.Add(role);
                }
            }
            context.SaveChanges();


            var passwordHasher = new PasswordHasher<User>();
            var testUsers = new[]
            {
                new { Email = "admin@ngo.com", RoleName = "Admin", FirstName = "Admin", LastName = "User" },
                new { Email = "donor@ngo.com", RoleName = "Donor", FirstName = "Donor", LastName = "User" },
                new { Email = "worker@ngo.com", RoleName = "FieldWorker", FirstName = "Field", LastName = "Worker" },
                new { Email = "accountant@ngo.com", RoleName = "Accountant", FirstName = "Account", LastName = "User" },
                new { Email = "staff@ngo.com", RoleName = "WarehouseStaff", FirstName = "Warehouse", LastName = "Staff" }
            };

            foreach (var userData in testUsers)
            {
                if (!context.Users.Any(u => u.Email == userData.Email))
                {
                    var role = context.Roles.First(r => r.RoleName == userData.RoleName);
                    var user = new User
                    {
                        FirstName = userData.FirstName,
                        LastName = userData.LastName,
                        Email = userData.Email,
                        RoleId = role.Id,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    user.PasswordHash = passwordHasher.HashPassword(user, "Melih123");
                    context.Users.Add(user);
                    context.SaveChanges();


                    if (userData.RoleName == "Donor")
                    {
                        context.Donors.Add(new Donor
                        {
                            UserId = user.Id,
                            DonorType = "Individual",
                            TaxNumber = "11111111111"
                        });
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
