using Bookly.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Data.ModelsConfig
{
    public class BookCopyConfiguration : IEntityTypeConfiguration<BookCopy>
    {
        public void Configure(EntityTypeBuilder<BookCopy> builder)
        {
            builder.HasKey(bc => bc.Id);

            builder.Property(bc => bc.InventoryNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(bc => bc.Condition)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValue("New");

            builder.HasOne(bc => bc.Book)
                .WithMany(b => b.BookCopies)
                .HasForeignKey(bc => bc.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
