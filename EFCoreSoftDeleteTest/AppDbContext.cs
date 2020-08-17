using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreSoftDeleteTest
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Demo");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasQueryFilter(e => e.IsDeleted == false);
            modelBuilder.Entity<Address>().HasQueryFilter(e => e.IsDeleted == false);
        }

        public async Task<int> SaveChangesWithSoftDeleteAsync()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                // Skip non-soft-deletables
                if (typeof(ISoftDeletable).IsAssignableFrom(entry.Entity.GetType()))
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues["IsDeleted"] = false;
                            break;

                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            entry.CurrentValues["IsDeleted"] = true;
                            break;
                    }
                }
            }

            return await SaveChangesAsync();
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
