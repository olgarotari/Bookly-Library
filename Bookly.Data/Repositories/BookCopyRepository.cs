using Bookly.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Data.Repositories
{
    public class BookCopyRepository : IBookCopyRepository
    {
        private readonly BooklyDbContext _context;
        public BookCopyRepository(BooklyDbContext context)
        {
            _context = context;
        }

        public async Task<BookCopy?> GetByIdAsync(int  id)
        {
            return await _context.BookCopies.FindAsync(id);
        }

        public async Task<BookCopy?> GetAvailableCopyByBookIdAsync(int bookId)
        {
            return await _context.BookCopies
                .FirstOrDefaultAsync(bc  => bc.BookId == bookId && bc.IsAvailable);
        }

        public async Task UpdateAsync(BookCopy bookCopy)
        {
            _context.BookCopies.Update(bookCopy);
            await _context.SaveChangesAsync();

        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
