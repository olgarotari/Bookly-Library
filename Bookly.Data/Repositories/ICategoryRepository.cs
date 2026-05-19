using Bookly.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Data.Repositories
{
    public interface ICategoryRepository
    {

        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);

        Task<IEnumerable<Category>> SearchCategoriesAsync(string term);

        Task AddAsync(Category category);
        void Update(Category category);
        Task SaveAsync();
        void Delete(Category category);
    }
}
