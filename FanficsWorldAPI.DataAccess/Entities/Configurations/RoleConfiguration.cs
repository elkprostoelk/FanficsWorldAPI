using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FanficsWorldAPI.DataAccess.Entities.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(30);
            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasData(
                new Role { Id = 1, Name = "Administrator" },
                new Role { Id = 2, Name = "Moderator" },
                new Role { Id = 3, Name = "User" });
        }
    }
}
