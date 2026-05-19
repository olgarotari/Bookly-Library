using Bookly.Data.Models;
using Bookly.Data.Repositories;
using Bookly.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string term)
        {
            return await _categoryRepository.SearchCategoriesAsync(term);
        }

        public async Task CreateAsync(string categoryName)
        {
            var category = new Category
            {
                Name = categoryName,
            };
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveAsync();
        }
    
        public async Task UpdateAsync(int id, string newCategory)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category != null)
            {
                category.Name = newCategory;
                _categoryRepository.Update(category);
            }

            await _categoryRepository.SaveAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null) return false;

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveAsync();
            return true;
        }
    }
}
