using ASPNETWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNETWebAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().HasData(
                new Currency
                {
                    Id = 1,
                    Country = "United States",
                    IsoCode = "USD"
                },
                new Currency
                {
                    Id = 2,
                    Country = "Russia",
                    IsoCode = "RUB"
                },
                new Currency
                {
                    Id = 3,
                    Country = "China",
                    IsoCode = "CNY"
                },
                new Currency
                {
                    Id = 4,
                    Country = "India",
                    IsoCode = "INR"
                },
                new Currency
                {
                    Id = 5,
                    Country = "Japan",
                    IsoCode = "JPY"
                },
                new Currency
                {
                    Id = 6,
                    Country = "France",
                    IsoCode = "EUR"
                }
             );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Anthony",
                    LastName = "Stark",
                    CurrencyId= 1,
                },
                new User
                {
                    Id = 2,
                    FirstName = "Natasha",
                    LastName = "Romanova",
                    CurrencyId = 2,
                }
             );

            modelBuilder.Entity<SpendingType>().HasData(
                new SpendingType
                {
                    Id = 1,
                    Name = "Restaurant",
                },
                new SpendingType
                {
                    Id = 2,
                    Name = "Hotel",
                },
                new SpendingType
                {
                    Id = 3,
                    Name = "Misc",
                }
             );

            modelBuilder.Entity<Spending>().HasData(
                new Spending
                {
                    Id = 1,
                    Date = DateTime.Now,
                    Amount = 250.00f,
                    Comment = "I bought a new screen",
                    UserId = 1,
                    SpendingTypeId = 3
                },
                new Spending
                {
                    Id = 2,
                    Date = DateTime.Now.AddDays(-1),
                    Amount = 50.50f,
                    Comment = "I had a nice meal at the restaurant",
                    UserId = 2,
                    SpendingTypeId = 1
                },
                new Spending
                {
                    Id = 3,
                    Date = DateTime.Now.AddDays(-5),
                    Amount = 110.90f,
                    Comment = "I went to the best hotel in town",
                    UserId = 1,
                    SpendingTypeId = 2
                }
             );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Spending> Spendings { get; set; }
        public DbSet<SpendingType> SpendingTypes { get; set; }
    }
}
