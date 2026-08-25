using Bookly.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Data.Repositories
{
    public interface IBorrowRepository
    {

        Task<List<Borrow>> GetAllAsync();
        Task<Borrow?> GetByIdAsync(int id);
        Task<bool> IsBookBorrowed(int bookId);
        Task AddAsync(Borrow borrow);
        Task Update(Borrow borrow);
        Task Create(Borrow borrow);
        Task<BookCopy?> GetFirstAvailableCopyAsync(int bookId);
        Borrow GetActiveBorrow(int bookId);
        Task<Borrow?> GetActiveBorrowByIdAsync(int borrowId);
        Task<List<Borrow>> GetActiveBorrowsByUserIdAsync(int userId);
        Task<List<Borrow>> GetBorrowHistoryByUserIdAsync(int userId);
        void Delete(Borrow borrow);
        Task SaveAsync();

    }
}
