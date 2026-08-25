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
    [Route("api/[controller]")]
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
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return Ok(new { message = "Success" });
        }





        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = await _userService.GetUserByEmailAsync(model.Email);
            if (exists != null)
                return BadRequest("User already exists");

            // 1. Salvează utilizatorul
            await _userService.RegisterUserAsync(model.FullName, model.Email, model.Password);

            // 2. LOGARE AUTOMATĂ: Generezi Claim-urile/Sesiunea pentru noul user
            var user = await _userService.GetUserByEmailAsync(model.Email);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.FullName),
        new Claim(ClaimTypes.Email, user.Email)
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Ok(new { message = "Registration successful!" });
        }








        //[HttpPost("register")]
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    //return BadRequest();

        //    var exists = await _userService.GetUserByEmailAsync(model.Email);
        //    if (exists != null)
        //    {
        //        ModelState.AddModelError("Email", "Utilizatorul cu acest email există deja.");
        //        return View(model);
        //    }
        //    //return BadRequest("User exists");

        //    await _userService.RegisterUserAsync(model.FullName, model.Email, model.Password);

        //    return Ok(new { message = "RegistrationSuccessfully!" });
        //}

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");

        }





    }
}
