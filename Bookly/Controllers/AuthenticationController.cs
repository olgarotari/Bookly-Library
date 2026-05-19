using Bookly.Data;
using Bookly.Data.Models;
using Bookly.Services;
using Bookly.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bookly.Controllers
{
    [ApiController]
    [Route("api/[authentication]")]
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            var user = _userService.GetUserByEmailAndPass(model.Email, model.Password);
            
            if (user == null)
            {
                return Unauthorized("Invalid login attempt.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //HttpContext.SignIn(
            //    CookieAuthenticationDefaults.AuthenticationScheme, 
            //    new ClaimsPrincipal(identity));

            return Ok(new { message = "Success" });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var exists = await _userService.GetUserByEmailAsync(model.Email);
            if (exists != null)
                return BadRequest("User exists");

            await _userService.RegisterUserAsync(model.FullName, model.Email, model.Password);

            return Ok(new { message = "RegistrationSuccessfully!" });
        }


     
    }
}
