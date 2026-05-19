using Bookly.Data;
using Bookly.Data.Models;
using Bookly.Services;
using Bookly.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Controllers
{
    [ApiController]
    [Route("api/users")]

    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null) return NotFound("User not found");
            return Ok(user);    
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            var users = await _userService.SearchUsersAsync(term);

            // Transformăm datele în formatul așteptat de Select2: { id, name }
            var result = users.Select(u => new {
                id = u.Id,
                name = u.FullName
            });

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserViewModel model)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.RoleId = model.RoleId;

            await _userService.UpdateUserAsync(user);
            return Ok("User updated successfully");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound("User not found");

            await _userService.DeleteAsync(id);
            return Ok("User deleted");
        }
    }
}
