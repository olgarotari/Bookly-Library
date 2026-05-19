using Bookly.Services;
using Bookly.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Bookly.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound($"Category with ID {id} not found.");

            return Ok(category);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            var categories = await _categoryService.SearchCategoriesAsync(term);

            // Transformăm datele în formatul așteptat de Select2: { id, name }
            var result = categories.Select(c => new {
                id = c.Id,
                name = c.Name
            });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return BadRequest("Name is required");
            }
            await _categoryService.CreateAsync(categoryName);
            return Ok("Author created successfully!");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                return BadRequest("Date Invalide");
            

            try
            {
                await _categoryService.UpdateAsync(id, categoryName);
                return Ok($"Category with ID {id} UpdateDataOperation successufully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.DeleteAsync(id);

            if (!category)
                return NotFound($"Cannot delete: Category with ID {id} not found.");

            return Ok($"Category with ID {id} deleted successfully.");
        }
    }
}
