using Microsoft.AspNetCore.Mvc;
using NeoMarket.Application.DTOs;
using NeoMarket.Application.Interfaces;
using System.Threading.Tasks;

namespace NeoMarket.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMelhorEnvioService _melhorEnvioService;

        public ProductController(IProductService productService, IMelhorEnvioService melhorEnvioService)
        {
            _productService = productService;
            _melhorEnvioService = melhorEnvioService;
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
                request.MinRating,
                request.SortBy
            );

            return Json(filteredProducts);
        }

        public IActionResult Details(int productId)
        {
            var product = _productService.GetProductById(productId);

            if (product == null)
            {
                return NotFound("Produto não encontrado.");
            }

            ViewData["Title"] = product.Name;
            return View("Details", product);
        }

        [HttpGet]
        public async Task<IActionResult> CalculateShipping(int productId, string cep)
        {
            var product = _productService.GetProductById(productId);

            if (product == null)
            {
                return BadRequest("Produto não encontrado.");
            }

            var shippingOptions = await _melhorEnvioService.CalculateShipping(
                cep,
                product.Weight,
                product.Width,
                product.Height,
                product.Length,
                product.Price
            );

            return Json(shippingOptions);
        }

        [HttpGet]
        public IActionResult Search(string term)
        {
            var products = _productService.SearchProducts(term);
            return Json(products);
        }

    }
}
