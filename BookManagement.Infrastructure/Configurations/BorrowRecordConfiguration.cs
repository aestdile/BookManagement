using BookManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookManagement.Infrastructure.Configurations;

public class BorrowRecordConfiguration : IEntityTypeConfiguration<BorrowRecord>
{
    public void Configure(EntityTypeBuilder<BorrowRecord> builder)
    {
        builder.HasKey(br => br.Id);

        builder.Property(br => br.BorrowedDate)
            .IsRequired();

        builder.Property(br => br.DueDate)
            .IsRequired();

        builder.Property(br => br.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(br => br.RenewalCount)
            .IsRequired();

        builder.HasIndex(br => new { br.BookId, br.UserId, br.Status });
        builder.HasIndex(br => br.Status);
        builder.HasIndex(br => br.DueDate);
    }
}