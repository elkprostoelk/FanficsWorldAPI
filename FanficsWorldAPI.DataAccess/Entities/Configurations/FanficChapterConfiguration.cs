using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FanficsWorldAPI.DataAccess.Entities.Configurations
{
    public class FanficChapterConfiguration : IEntityTypeConfiguration<FanficChapter>
    {
        public void Configure(EntityTypeBuilder<FanficChapter> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasMaxLength(26);

            builder.Property(x => x.Title)
                .HasMaxLength(100);

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.Property(x => x.LastModifiedDate)
                .IsRequired();

            builder.Property(x => x.TextHtml)
                .IsRequired();

            builder.Property(x => x.Number)
                .IsRequired();

            builder.Property(x => x.IsDraft)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.HasOne(x => x.Fanfic)
                .WithMany(f => f.Chapters)
                .HasForeignKey(x => x.FanficId);
        }
    }
}
