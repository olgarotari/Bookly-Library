using Bookly.Data.Models;

namespace Bookly.Data.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        void AddBook(Book book);
        void Update(Book book);
        void Delete(Book book);
        Task SaveAsync();
        Task<List<Book>> SearchByTermAsync(string term);
    }
}
