using ExampleImageInAzureStorage.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExampleImageInAzureStorage.Core.Persistence
{
    public class StorageAzureContext : DbContext
    {
        public StorageAzureContext(DbContextOptions options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<ExampleStorage> ExampleStorage => Set<ExampleStorage>();
    }
}
