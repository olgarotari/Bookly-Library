using Bookly.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace Bookly.Data.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BooklyDbContext _context;
        public AuthorRepository(BooklyDbContext context)
        {
            _context = context;
        }
        public async Task<List<Author>> GetAllAsync()
        {
            return await _context.Authors.ToListAsync();
        }
        public async Task<Author> GetByIdAsync(int id)
        {
            return await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Author>> SearchAuthorsAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return await _context.Authors.Take(10).ToListAsync();
            }

            return await _context.Authors
                .Where(a => a.Name.Contains(term)) // Aici se întâmplă filtrarea 
                .ToListAsync();
        }

        public async Task AddAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
        }

        public async Task Update(Author author)
        {
             _context.Authors.Update(author);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Author author)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
    }

}
