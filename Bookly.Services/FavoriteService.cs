using Bookly.Data.Models;
using Bookly.Data.Repositories;
using Bookly.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<List<FavoriteViewModel>> GetUserFavoritesAsync(int userId)
        {
            var favorites = await _favoriteRepository.GetByUserIdAsync(userId);

            return favorites.Select(f => new FavoriteViewModel
            {
                BookId = f.BookId,
                Title = f.Book?.Title ?? "Unknown",
                Author = f.Book?.Author?.Name ?? "Unknown Author",
                CoverImageUrl = f.Book?.ImageUrl
            }).ToList();
        }

        public async Task<bool> ToggleFavoriteAsync(FavoriteDto dto)
        {
            var existing = await _favoriteRepository.GetAsync(dto.UserId, dto.BookId);

            if (existing != null)
            {
                await _favoriteRepository.DeleteAsync(existing);
                return false; // Returnează false dacă a fost șters din favorite
            }

            var newFavorite = new UserFavorite
            {
                UserId = dto.UserId,
                BookId = dto.BookId,
                CreatedAt = DateTime.Now
            };

            await _favoriteRepository.AddAsync(newFavorite);
            return true; // Returnează true dacă a fost adăugat la favorite
        }

        public async Task<bool> IsBookFavoriteAsync(int userId, int bookId)
        {
            var favorite = await _favoriteRepository.GetAsync(userId, bookId);
            return favorite != null;
        }
    }
}
