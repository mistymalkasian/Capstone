namespace PaulyMacs.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PaulyMacs.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PaulyMacs.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.

            context.Customers.AddOrUpdate(
              p => p.FirstName,
              new Customer { FirstName = "Misty", LastName = "Malkasian", PhoneNumber = "2629946699", EmailAddress = "andrewandmisty@gmail.com", BalanceOwed = 12.25 },
              new Customer { FirstName = "Uma", LastName = "Bob", PhoneNumber = "2222222222", EmailAddress = "fakeaddress01@gmail.com", BalanceOwed = 25.86 },
              new Customer { FirstName = "Alfonso", LastName = "Squinkle", PhoneNumber = "5555555555", EmailAddress = "fakeaddress02@gmail.com", BalanceOwed = 16.14 }        
              
              );

            context.Employees.AddOrUpdate(

                new Employee { FirstName = "John", LastName = "Smith", PhoneNumber ="1111111111", EmailAddress="fakeaddress03@gmail.com"},
                new Employee { FirstName = "Jane", LastName = "Doe", PhoneNumber = "3333333333", EmailAddress = "fakeaddress04@gmail.com" }
                );

        }
    }
}
