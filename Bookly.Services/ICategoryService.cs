using Bookly.Data.Models;
using Bookly.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);

        Task<IEnumerable<Category>> SearchCategoriesAsync(string term);
        Task CreateAsync(string categoryName);
        Task UpdateAsync(int id, string newCategory);
        Task<bool> DeleteAsync(int id);
    }
}
