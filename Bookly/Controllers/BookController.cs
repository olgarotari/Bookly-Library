using Bookly.Services;
using Bookly.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace Bookly.Controllers

{
    [ApiController]
    [Route("api/book")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

       

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookService.GetAllAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            
            if (book == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }

            return Ok(book);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            var books = await _bookService.SearchBooksAsync(term);
            var result = books.Select(b => new
            {
                id = b.Id,
                text = b.Title
            });
            return Ok(result);
        }



        [HttpPost]
        public IActionResult Create([FromBody] EditBookViewModel model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            _bookService.Create(model);

            return Ok("Book created successfully");
        }




        [HttpPost("upload-book-image")]
        public async Task<IActionResult> UploadBookImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nu a fost selectata nici o imagine");

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/books");
            if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadFolder, fileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok(new { url = "/images/books/" + fileName }); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EditBookViewModel model)
        {
            try
            {
                var updated = await _bookService.UpdateAsync(id, model);
                if (!updated) return NotFound("Book with this ID {id} was not found in database.");

                return Ok("Book updated succesfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookService.DeleteAsync(id);

            if (!result)
            
                return NotFound($"Cannot delete: Book with ID {id} not found.");

            return Ok($"Book with ID {id} deleted successfully.");
        }
    }
}
