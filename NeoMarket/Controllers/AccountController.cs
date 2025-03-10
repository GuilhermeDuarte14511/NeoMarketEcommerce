using Microsoft.AspNetCore.Mvc;
using NeoMarket.Application.Interfaces;
using NeoMarket.Application.DTOs.Login;

namespace NeoMarket.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _accountService.Authenticate(request);
            if (user == null)
            {
                return Json(new { success = false, message = "Email ou senha inválidos." });
            }

            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserId", user.Id.ToString());

            return Json(new { success = true, message = "Login realizado com sucesso!" });
        }


    }
}
