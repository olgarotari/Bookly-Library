using Bookly.Data.Models;
using Bookly.ViewModels;

namespace Bookly.Services
{
    public interface IBookService
    {
        Task<List<BookViewModel>> GetAllAsync();
        Task<BookViewModel?> GetByIdAsync(int id, int userId);
        void Create(EditBookViewModel model);
        Task<bool> UpdateAsync(int id, EditBookViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<List<Book>> SearchBooksAsync(string term);
        Task<IEnumerable<Book>> LiveSearchBookAsync(string term);
    }
}
