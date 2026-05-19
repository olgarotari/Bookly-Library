using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookly.Data.Models;
using Bookly.ViewModels;


namespace Bookly.Services
{
    public interface IBorrowService
    {
        Task<BorrowViewModel?> GetByIdAsync(int id);
        Task CreateAsync(EditBorrowViewModel model);
        Task ReturnBookAsync(int bookId);

        Task UpdateAsync(int id, EditBorrowViewModel model);
        Task<List<BorrowViewModel>> GetAllAsync();
        Task<bool> DeleteBorrowAsync(int id);
       
    }
}
