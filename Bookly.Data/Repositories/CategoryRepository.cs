using Bookly.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BooklyDbContext _context;
        public CategoryRepository(BooklyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return await _context.Categories.Take(10).ToListAsync();
            }

            return await _context.Categories
                .Where(c => c.Name.Contains(term))
                .ToListAsync();
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category); 
        }

        public void Update(Category category)
        {
             _context.Categories.Update(category);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Delete(Category  category)
        {
            _context.Categories.Remove(category);
        }
    }
}
