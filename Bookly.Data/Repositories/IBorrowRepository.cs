using Bookly.Data.Models;
using Bookly.ViewModels;
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
        Borrow GetActiveBorrow(int bookId); 
        void Delete(Borrow borrow);
        Task SaveAsync();

    }
}
