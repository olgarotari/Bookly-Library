using Bookly.Data.Models;
using Microsoft.EntityFrameworkCore;
//using Bookly.ViewModels;
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
                .Include(b => b.BookCopy)
                    .ThenInclude(bc => bc.Book)
                .ToListAsync();
               
        }

        public async Task<Borrow?> GetByIdAsync(int id)
        {
            return await _context.Borrows
              .Include(b => b.User)
              .Include(b => b.BookCopy)
                 .ThenInclude(bc => bc.Book)
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

        public async Task<BookCopy?> GetFirstAvailableCopyAsync(int bookId)
        {
            return await _context.BookCopies
                .FirstOrDefaultAsync(c => c.BookId == bookId && c.IsAvailable);
        }



        public async Task<bool> IsBookBorrowed(int bookId)
        {
            return await  _context.Borrows
                .AnyAsync(b => b.BookCopyId == bookId && b.ReturnDate == null);
        }
        public Borrow GetActiveBorrow(int bookId)
        {
            return _context.Borrows
                .FirstOrDefault(b => b.BookCopyId == bookId && b.ReturnDate == null);
        }

        public async Task<List<Borrow>> GetActiveBorrowsByUserIdAsync(int userId)
        {
            return await _context.Borrows
           .Include(b => b.BookCopy)
             .ThenInclude(bc => bc.Book)
             .ThenInclude(b => b.Author)
           .Where(b => b.UserId == userId && b.ReturnDate == null)
        .ToListAsync();
        }

        public async Task<Borrow?> GetActiveBorrowByIdAsync(int borrowId)
        {
            return await _context.Borrows
                .Include(b => b.BookCopy)
                .FirstOrDefaultAsync(b => b.Id == borrowId && b.ReturnDate == null);
        }

        public async Task<List<Borrow>> GetBorrowHistoryByUserIdAsync(int userId)
        {
            return await _context.Borrows
                .Include(b => b.BookCopy)
                    .ThenInclude(bc => bc.Book)
                        .ThenInclude(b => b.Author)
                .Where(b => b.UserId == userId && b.ReturnDate != null)
                .OrderByDescending(b => b.ReturnDate)
                .ToListAsync();
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
