using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FanficsWorldAPI.DataAccess.Entities.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasMaxLength(26);

            builder.Property(x => x.Id)
                .HasMaxLength(50);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255);
            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.Bio)
                .HasMaxLength(1000);

            builder.Property(x => x.BirthDate)
                .IsRequired();

            builder.Property(x => x.PasswordSalt)
                .IsRequired();

            builder.Property(x => x.PasswordHash)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasOne(x => x.Role)
                .WithMany(y => y.Users)
                .HasForeignKey(x => x.RoleId);
        }
    }
}
