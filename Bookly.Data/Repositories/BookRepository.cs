using Bookly.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BooklyDbContext _context;

        public BookRepository(BooklyDbContext context)
        {
            _context = context;
        }
        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.BookCopies)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)  
        {
            return await _context.Books

              .Include(b => b.Author)
              .Include(b => b.Category)
              .FirstOrDefaultAsync(b => b.Id == id);

        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void Update(Book book)
        {
            _context.Books.Update(book);
        }

        public void Delete(Book book)
        {
            _context.Books.Remove(book);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Book>> SearchByTermAsync(string term)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(term) ||
                            b.Summary.Contains(term))
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> LiveSearchBookAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();
            }

            var lowerTerm = term.Trim().ToLower();

            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => (b.Title != null && b.Title.ToLower().Contains(lowerTerm)) ||
                    (b.Author != null && b.Author.Name.ToLower().Contains(lowerTerm)) ||
                    (b.Category != null && b.Category.Name.ToLower().Contains(lowerTerm)) ||
                    (b.Summary != null && b.Summary.ToLower().Contains(lowerTerm)))
                .ToListAsync();



        }
    }
}
