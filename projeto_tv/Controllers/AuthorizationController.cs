using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projeto_tv.Dto;
using projeto_tv.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using vansyncenterprise.web.Repositorys.Data;

namespace projeto_tv.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly Context _context;

        public AuthorizationController(Context context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<ActionResult<Guid>> Login([FromBody] Login loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await ValidateEmail(loginDto.Email);

            var password = HashPassword(loginDto.Senha);
            if (password != user.Senha)
                return Unauthorized("Login ou Senha estão incorretos.");

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, loginDto.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Ok();
        }

        [HttpPost("register")]
        public async Task<ActionResult<bool>> Register([FromBody] RegistroModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Empresa.Add(new RegistroModel
            {
                Email = user.Email,
                Name = user.Name,
                Senha = user.Senha,
            });

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<RegistroModel?> ValidateEmail(string email)
            => await _context.Empresa.FirstOrDefaultAsync(u => u.Email == email);

        public string HashPassword(string password)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha1.ComputeHash(passwordBytes);

                StringBuilder hash = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hash.Append(b.ToString("X2"));
                }

                return hash.ToString();
            }
        }
    }
}
