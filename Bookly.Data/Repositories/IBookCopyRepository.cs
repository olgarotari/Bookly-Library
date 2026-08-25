using Bookly.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Data.Repositories
{
    public interface IBookCopyRepository
    {
        Task<BookCopy?> GetByIdAsync(int id);
        Task<BookCopy?> GetAvailableCopyByBookIdAsync(int bookId);
        Task UpdateAsync(BookCopy bookCopy);
        Task SaveAsync();
    }
}
