using Bookly.Data.Models;
using Bookly.Services;
using Bookly.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorController : ControllerBase
    {
       private readonly IAuthorService _authorService;
       public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var author = await _authorService.GetAllAsync();
            return Ok(author.Select(a => new { id = a.Id, name = a.Name }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await _authorService.GetByIdAsync(id);

            if (author == null)
                return NotFound();

            return Ok(author);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            var authors = await _authorService.SearchAuthorsAsync(term);

            var result = authors.Select(a => new {
                id = a.Id,
                name = a.Name
            });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string authorName)
        {
            if(string.IsNullOrWhiteSpace(authorName))
            {
                return BadRequest("Name is required");
            }
            await _authorService.CreateAsync(authorName);
            return Ok("Author created successfully!");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] string authorName)
        {
           
            var existingAuthor = await _authorService.GetByIdAsync(id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(authorName))
            {
                return BadRequest("Namw cannot be empty");
            }

            await _authorService.UpdateAsync(id, authorName);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            await _authorService.DeleteAsync(id);
            return Ok();
        }
    }
}
