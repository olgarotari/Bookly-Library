using Bookly.Services;
using Bookly.ViewModels;
using Microsoft.AspNetCore.Mvc;

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



        [HttpPatch("{id}/return")]
        public async Task<IActionResult> Return(int bookId)
        {
            await _borrowService.ReturnBookAsync(bookId);
            return Ok();

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
