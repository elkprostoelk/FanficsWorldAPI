using FanficsWorldAPI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FanficsWorldAPI.DataAccess
{
    public class FanficsWorldDbContext(DbContextOptions<FanficsWorldDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Fanfic> Fanfics { get; set; }

        public DbSet<FanficChapter> FanficChapters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
