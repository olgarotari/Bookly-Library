
using Bookly.Data.Models;
using Bookly.ViewModels;


namespace Bookly.Services
{
    public interface IBorrowService
    {
        Task<BorrowViewModel?> GetByIdAsync(int id);
        Task<List<Borrow>> GetActiveBorrowsByUserIdAsync(int userId);
        Task<bool> CreateAsync(EditBorrowViewModel model);
        Task BorrowBookAsync(int userId, int bookCopyId);
        //Task ReturnBookAsync(int bookId);
        Task<string> ReturnBookAsync(int borrowId);
        Task<List<Borrow>> GetBorrowHistoryByUserIdAsync(int userId);
        //Task<Borrow?> GetMostUrgentBorrowByUserIdAsync(int userId);
        Task UpdateAsync(int id, EditBorrowViewModel model);
        Task<List<BorrowViewModel>> GetAllAsync();
        Task<bool> DeleteBorrowAsync(int id);
        Task<bool> ReturnBorrowAsync(int id);
    }
}
