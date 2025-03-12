using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NeoMarket.Application.Interfaces;
using NeoMarket.Application.DTOs.Login;

namespace NeoMarket.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerStoreService _customerStoreService;

        public AccountController(IAccountService accountService, ICustomerStoreService customerStoreService)
        {
            _accountService = accountService;
            _customerStoreService = customerStoreService;
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

            // Salvar dados do usuário na sessão
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetInt32("UserId", user.Id);

            // Buscar a loja associada ao cliente (se houver)
            string storeUrlSlug = user.UrlSlug ?? "";
            bool isCustomerLinkedToStore = !string.IsNullOrEmpty(storeUrlSlug);

            HttpContext.Session.SetString("UserStoreUrlSlug", storeUrlSlug);

            return Json(new
            {
                success = true,
                message = "Login realizado com sucesso!",
                urlSlug = storeUrlSlug,
                redirectUrl = isCustomerLinkedToStore ? $"/{storeUrlSlug}" : "/"
            });
        }

    }
}
