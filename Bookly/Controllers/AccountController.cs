using Bookly.Services;
using Bookly.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bookly.Controllers
{
    public class AccountController : Controller
    {
        private readonly IBorrowService _borrowService;

            public AccountController(IBorrowService borrowService)
            {
                _borrowService = borrowService;
            }

            [HttpGet]
            public async Task<IActionResult> Profile()
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = User.Identity?.Name ?? "User";
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                     return Redirect("/html/login.html");
                }

                var activeBorrows = await _borrowService.GetActiveBorrowsByUserIdAsync(userId);

                var borrowHistory = await _borrowService.GetBorrowHistoryByUserIdAsync(userId);

            var model = new ProfileViewModel
                {
                    UserName = userName,
                    Email = userEmail,
                    ActiveBorrows = activeBorrows,
                    BorrowHistory = borrowHistory
            };

                return View(model);
            }
        }

    }
