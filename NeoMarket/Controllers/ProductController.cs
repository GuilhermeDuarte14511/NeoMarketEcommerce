using Microsoft.AspNetCore.Mvc;
using NeoMarket.Application.DTOs;
using NeoMarket.Application.Interfaces;

namespace NeoMarket.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index(string subCategorySlug)
        {
            var products = _productService.GetProductsBySubCategorySlug(subCategorySlug);

            if (products == null || !products.Any())
            {
                ViewBag.Message = "Nenhum produto disponível nesta subcategoria.";
                return View("NoProducts");
            }

            ViewBag.SubCategoryName = products.FirstOrDefault()?.SubcategoryName ?? "Produtos";
            ViewData["Title"] = $"Produtos - {ViewBag.SubCategoryName}";

            return View("ListProduct", products);
        }

        [HttpPost]
        public IActionResult GetFilteredProducts([FromBody] ProductFilterRequest request)
        {
            if (request == null)
                return BadRequest("Filtros inválidos.");

            var filteredProducts = _productService.GetFilteredProducts(
                request.SubCategorySlug,
                request.Brands,
                request.MinPrice,
                request.MaxPrice,
                request.MinRating
            );

            return Json(filteredProducts);
        }

    }
}
