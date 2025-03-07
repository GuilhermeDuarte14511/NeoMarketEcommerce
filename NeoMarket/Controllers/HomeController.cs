using Microsoft.AspNetCore.Mvc;
using NeoMarket.Application.DTOs;
using NeoMarket.Application.Interfaces;

namespace NeoMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStoreService _storeService;

        public HomeController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        public IActionResult Index(string urlSlug)
        {
            if (string.IsNullOrEmpty(urlSlug))
            {
                return RedirectToAction("Error");
            }

            var storeDto = _storeService.GetStoreDtoBySlug(urlSlug);
            if (storeDto == null)
            {
                return RedirectToAction("Error");
            }

            HttpContext.Session.SetString("StoreName", storeDto.Name);
            HttpContext.Session.SetString("StoreUrlSlug", storeDto.UrlSlug);

            ViewData["StoreDto"] = storeDto;
            ViewData["Title"] = storeDto.Name;

            return View(storeDto);
        }

    }
}
