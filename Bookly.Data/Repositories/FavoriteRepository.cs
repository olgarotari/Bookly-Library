using Bookly.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Data.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly BooklyDbContext _context;

        public FavoriteRepository(BooklyDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserFavorite>> GetByUserIdAsync(int userId)
        {
            return await _context.UserFavorites
                .Include(f => f.Book)
                 .ThenInclude(b => b.Author)
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<UserFavorite?> GetAsync(int userId, int bookId)
        {
            return await _context.UserFavorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.BookId == bookId);
        }

        public async Task<bool> IsFavoriteAsync(int userId, int bookId)
        {
            return await _context.UserFavorites
                .AnyAsync(f => f.UserId == userId && f.BookId == bookId);
        }

        public async Task AddAsync(UserFavorite favorite)
        {
            await _context.UserFavorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserFavorite favorite)
        {
            _context.UserFavorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }
    }
}
