using NeoMarket.Application.DTOs;
using NeoMarket.Application.Interfaces;
using NeoMarket.Domain.Interfaces;

namespace NeoMarket.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<ProductDto> GetProductsBySubCategorySlug(string subCategorySlug)
        {
            var products = _productRepository.GetProductsBySubCategorySlug(subCategorySlug);

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                SubcategoryName = p.Subcategory?.Name,
                CategoryName = p.Category?.Name,
                Brand = p.Brand?.Name,
                Rating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0
            }).ToList();
        }

        public IEnumerable<ProductDto> GetFilteredProducts(string subCategorySlug, List<string> brands, decimal? minPrice, decimal? maxPrice, int? minRating)
        {
            var products = _productRepository.GetFilteredProducts(subCategorySlug, brands, minPrice, maxPrice, minRating);

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                SubcategoryName = p.Subcategory?.Name,
                CategoryName = p.Category?.Name,
                Brand = p.Brand?.Name,
                Rating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0
            }).ToList();
        }

    }
}
