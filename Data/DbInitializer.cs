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
                var adminUser = new User
                {
                    Email = "admin11@gmail.com",
                    Password = "$2a$10$9OJ3LDj0MJ70gcyS3bZmjutk4Y5TFu8PSCCA5GtjLOBftMPlbZirK",
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
