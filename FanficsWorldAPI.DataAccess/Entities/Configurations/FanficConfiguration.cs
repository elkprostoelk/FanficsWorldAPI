using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FanficsWorldAPI.DataAccess.Entities.Configurations
{
    public class FanficConfiguration : IEntityTypeConfiguration<Fanfic>
    {
        public void Configure(EntityTypeBuilder<Fanfic> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasMaxLength(26);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(x => x.Title);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.Property(x => x.LastModifiedDate)
                .IsRequired();

            builder.HasOne(x => x.Author)
                .WithMany(u => u.Fanfics)
                .HasForeignKey(x => x.AuthorId);
        }
    }
}
