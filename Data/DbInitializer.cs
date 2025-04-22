using GAP.Models;
using Microsoft.EntityFrameworkCore;

namespace GAP.Data
{
    public class DbInitializer
    {
        private readonly DbContext dbContext;

        public DbInitializer(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Seed()
        {
            // Check if the User table is empty before seeding
            if (!dbContext.Set<User>().Any())
            {
                // Create the pre-registered admin user
                string newPassword = "admin1234";  // Replace with your new password
                var adminUser = new User
                {
                    Email = "admin@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword(newPassword),
                    FirstName = "admin",
                    LastName = "admin",
                    IsAdmin =true 
                    
                };

                // Add the admin user to the User table
                dbContext.Set<User>().Add(adminUser);

                // Save the changes to the database
                dbContext.SaveChanges();
            }
        }
    }
}
