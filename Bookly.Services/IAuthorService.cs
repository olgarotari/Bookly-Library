using Bookly.Data.Models;
using Bookly.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Services
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAllAsync();
        Task<Author?> GetByIdAsync(int id);
        Task<IEnumerable<Author>> SearchAuthorsAsync(string term);
        Task UpdateAsync(int id, string newName);
        Task DeleteAsync(int id);
        Task CreateAsync(string authorName);
    }
}
