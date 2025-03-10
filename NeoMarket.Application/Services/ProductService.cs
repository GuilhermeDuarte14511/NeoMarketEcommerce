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

        public IEnumerable<ProductDto> GetFilteredProducts(string subCategorySlug, List<string> brands, decimal? minPrice, decimal? maxPrice, int? minRating, string sortBy)
        {
            var products = _productRepository.GetFilteredProducts(subCategorySlug, brands, minPrice, maxPrice, minRating, sortBy);

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

        public ProductDto GetProductById(int productId)
        {
            var product = _productRepository.GetProductById(productId);

            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                SubcategoryName = product.Subcategory?.Name,
                CategoryName = product.Category?.Name,
                Brand = product.Brand?.Name,
                Rating = product.Reviews.Any() ? product.Reviews.Average(r => r.Rating) : 0,
                Weight = product.Weight,
                Width = product.Width,
                Height = product.Height,
                Length = product.Length,
                Reviews = product.Reviews.Select(r => new ReviewDto
                {
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                }).ToList()
            };
        }



    }
}
