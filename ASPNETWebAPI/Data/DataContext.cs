using Microsoft.EntityFrameworkCore;

namespace ASPNETWebAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Currency> Currencies { get; set; }
    }
}
