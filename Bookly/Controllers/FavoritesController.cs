using Bookly.Services;
using Bookly.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bookly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserFavorites()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }

            var favorites = await _favoriteService.GetUserFavoritesAsync(userId);
            return Ok(favorites);
        }

        //[HttpGet("user/{userId}")]
        //public async Task<IActionResult> GetUserFavorites(int userId)
        //{
        //    var favorites = await _favoriteService.GetUserFavoritesAsync(userId);
        //    return Ok(favorites);
        //}

        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleFavorite([FromBody] FavoriteDto dto)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(); // Întoarce 401 dacă nu e logat

            dto.UserId = int.Parse(userIdClaim);

            var isFav = await _favoriteService.ToggleFavoriteAsync(dto);

            return Ok(new { isFavorite = isFav });
        }

        [HttpGet("is-favorite/{userId}/{bookId}")]
        public async Task<IActionResult> IsFavorite(int userId, int bookId)
        {
            var isFavorite = await _favoriteService.IsBookFavoriteAsync(userId, bookId);
            return Ok(new { isFavorite = isFavorite });
        }
    }
}
