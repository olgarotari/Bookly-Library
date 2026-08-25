using Bookly.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Data.Repositories
{
    public interface IFavoriteRepository
    {
        Task<List<UserFavorite>> GetByUserIdAsync(int userId);
        Task<UserFavorite?> GetAsync(int userId, int bookId);
        Task<bool> IsFavoriteAsync(int userId, int bookId);
        Task AddAsync(UserFavorite favorite);
        Task DeleteAsync(UserFavorite favorite);
    }
}
