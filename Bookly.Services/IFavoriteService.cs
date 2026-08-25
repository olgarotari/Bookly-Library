using Bookly.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Services
{
    public interface IFavoriteService
    {
        Task<List<FavoriteViewModel>> GetUserFavoritesAsync(int userId);
        Task<bool> ToggleFavoriteAsync(FavoriteDto dto);
        Task<bool> IsBookFavoriteAsync(int userId, int bookId);
    }
}
