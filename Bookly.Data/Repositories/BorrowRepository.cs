using Bookly.Data.Models;
using Microsoft.EntityFrameworkCore;
using Bookly.ViewModels;
namespace Bookly.Data.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly BooklyDbContext _context;
        public BorrowRepository(BooklyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Borrow>> GetAllAsync()
        {
            return await _context.Borrows
                .Include(b => b.User)
                .Include(b => b.Book)
                .ToListAsync();
               
        }

        public async Task<Borrow?> GetByIdAsync(int id)
        {
            return await _context.Borrows
              .Include(b => b.User)
              .Include(b => b.Book)
              .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task Create(Borrow borrow)
        {
           
            _context.Borrows.Add(borrow);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Borrow borrow)
        {
            _context.Borrows.Update(borrow);
            await _context.SaveChangesAsync();
        }

           
        public async Task<bool> IsBookBorrowed(int bookId)
        {
            return await  _context.Borrows
                .AnyAsync(b => b.BookId == bookId && b.ReturnDate == null);
        }
        public Borrow GetActiveBorrow(int bookId)
        {
            return _context.Borrows
                .FirstOrDefault(b => b.BookId == bookId && b.ReturnDate == null);
        }
        public async Task AddAsync(Borrow borrow)
        {
            await _context.Borrows.AddAsync(borrow);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public  void Delete(Borrow borrow)
        {
            _context.Borrows.Remove(borrow);
        }
    }
}
