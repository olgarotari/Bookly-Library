using Bookly.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Bookly.Data.ModelsConfig
{
    public class BorrowConfiguration : IEntityTypeConfiguration<Borrow>
    {
        public void Configure(EntityTypeBuilder<Borrow> entity)
        {
            entity.HasKey(b => b.Id);

            entity.HasOne(b => b.User)
                .WithMany(u => u.Borrows)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Book)
                .WithMany(book => book.Borrows)
                .HasForeignKey(b =>b.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(b => b.BorrowDate)
                .IsRequired();
        }
    }
}
