using Bookly.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace Bookly.Data
{
    public class BooklyDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<BookCopy> BookCopies { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<Borrow> Borrows { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<UserFavorite> UserFavorites { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-4CVUJKM\\SQLEXPRESS;Database=BooklyDB;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BooklyDbContext).Assembly);

            modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = 1, Name = "Admin" },
                new ApplicationRole { Id = 2, Name = "User" }

            );
        }

    }

}
