using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using AlleyCatBarbers.Data;
using AlleyCatBarbers.Models;
using Microsoft.EntityFrameworkCore;
using AlleyCatBarbers.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using SendGrid.Helpers.Mail;

namespace AlleyCatBarbers.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string spreadsheetPath)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.EnsureCreated();

            // Create roles
            if (!context.Roles.Any())
            {
                string[] roleNames = { "Admin", "Customer", "Staff" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));

                }
            }

            //Create users
            if (!userManager.Users.Any())
            { 
                
                await CreateUser(userManager, "admin@admin.com", "password", "Admin", "John", "Doe", "0400 000 000", new DateOnly(2000, 1, 1));
                await CreateUser(userManager, "staff@staff.com", "password", "Staff", "Jane", "Doe", "0400 000 000", new DateOnly(2000, 1, 1));
                await CreateUser(userManager, "customer@customer.com", "password", "Customer", "Jack", "Doe", "0400 000 000", new DateOnly(2000, 1, 1));

                //Create multiple customers
                //var userRecords = SpreadsheetReader.ReadUserRecords(spreadsheetPath);

                //foreach (var userRecord in userRecords)
                //{
                //    await CreateUser(userManager, userRecord.Email, userRecord.Password, userRecord.Role, userRecord.FirstName, userRecord.LastName, userRecord.PhoneNumber, userRecord.DateOfBirth);
                //}
            }

            //Create reviews
            if (!context.Reviews.Any())
            {
                await CreateReviews(context, userManager);
            }

            //Create services
            if (!context.Services.Any())
            {
                await CreateServices(context, userManager);
            }

            //Create bookings
            if (!context.Bookings.Any())
            {
                await CreateBookings(context, userManager);
            }

        }

        public static async Task CreateUser(UserManager<ApplicationUser> userManager, string email, 
            string password, string role, string firstName, string lastName, string phoneNumber, DateOnly dateOfBirth)
        {
            

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    DateOfBirth = dateOfBirth
                    
                };

                var createUser = await userManager.CreateAsync(user, password);
                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    throw new Exception("Failed to create new user.");
                }
            }
            else
            {
                // Ensure the user is assigned to the role
                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }


        public static async Task CreateReviews(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {

            var customer = await userManager.FindByNameAsync("customer@customer.com");

            var reviews = new List<Review>
            {
                new Review { Comments = "Great service!", Rating = 5, DateCreated = DateTime.Now, UserId = customer.Id },
                new Review { Comments = "Good barbers..", Rating = 4, DateCreated = DateTime.Now, UserId = customer.Id },
            };

            context.Reviews.AddRange(reviews);
            await context.SaveChangesAsync();
        }


        public static async Task CreateServices(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            var services = new List<Service>
            {
                new Service { Type = "Haircut", Price = 55, Description = "Standard haircut" },
                new Service { Type = "Beard Trim", Price = 35, Description = "Beard trim" }
            
            };

            context.Services.AddRange(services);
            await context.SaveChangesAsync();
        }

        public static async Task CreateBookings(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            var customer = await userManager.FindByNameAsync("customer@customer.com");
            var haircut = await context.Services.FirstOrDefaultAsync(s => s.Type == "Haircut");

            var bookings = new List<Booking>
            {
                new Booking { Date = new DateTime(2024,08,20), TimeSlot = new TimeOnly(10,00), ServiceId = haircut.Id, UserId = customer.Id },
                new Booking { Date = new DateTime(2024,08,20), TimeSlot = new TimeOnly(15,00), ServiceId = haircut.Id, UserId = customer.Id }

            };

            context.Bookings.AddRange(bookings);
            await context.SaveChangesAsync();
        }

        



    }
}
