using Bookly.Services;
using Bookly.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bookly.Controllers
{
    [ApiController]
    [Route("api/borrows")]
    public class BorrowController : ControllerBase
    {
      private readonly IBorrowService _borrowService;
      public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _borrowService.GetAllAsync());
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var borrow = await _borrowService.GetByIdAsync(id);

            if (borrow == null)
                return NotFound($"Borrow with ID {id} not found.");

            return Ok(borrow);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EditBorrowViewModel model)
        {
            try
            {
                await _borrowService.CreateAsync(model);
                return Ok("Book borrowed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EditBorrowViewModel model)
        {
            await _borrowService.UpdateAsync(id, model);
            return Ok();
        }


        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                // Întoarce obiect JSON, nu doar Unauthorized() simplu
                return Unauthorized(new { message = "Trebuie să fii logat!" });
            }

            try
            {
                await _borrowService.BorrowBookAsync(userId, dto.BookCopyId);
                return Ok(new { message = "Succes" });
            }
            catch (Exception ex)
            {
                // Întoarce obiect JSON cu mesajul de eroare
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("return/{id}")]
        public async Task<IActionResult> ReturnBorrow(int id)
        {
            var success = await _borrowService.ReturnBorrowAsync(id);
            if (!success)
            {
                return NotFound("Împrumutul nu a fost găsit sau a fost deja returnat.");
            }

            return Ok(new { message = "Cartea a fost returnată cu succes." });
        }



        [HttpPatch("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            try
            {
                string message = await _borrowService.ReturnBookAsync(id);

                return Ok(new { message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A apărut o eroare la procesarea returnării." });
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _borrowService.DeleteBorrowAsync(id);

            if (!result)
                return NotFound("Borrow record not found");

            return Ok("Borrow record deleted successfully.");
        }
    }
 }
