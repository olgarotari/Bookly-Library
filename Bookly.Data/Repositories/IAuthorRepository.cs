using Bookly.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Data.Repositories
{
    public interface IAuthorRepository
    {

        Task<List<Author>> GetAllAsync();
        Task<Author> GetByIdAsync(int id);
        Task<IEnumerable<Author>> SearchAuthorsAsync(string term);
        Task AddAsync(Author author);
        Task Update(Author author);
        Task SaveAsync();
        Task Delete(Author author);
    }
}
