using Microsoft.EntityFrameworkCore;
using NeoMarket.Domain.Entities;
using NeoMarket.Domain.Interfaces;
using NeoMarket.Infrastructure.Data;

namespace NeoMarket.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetProductsBySubCategorySlug(string subCategorySlug)
        {
            return _context.Products
                .Include(p => p.Subcategory)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Reviews)
                .Where(p => p.Subcategory.UrlSlug == subCategorySlug && p.IsActive)
                .ToList();
        }

        public IEnumerable<Product> GetFilteredProducts(string subCategorySlug, List<string> brands, decimal? minPrice, decimal? maxPrice, int? minRating, string sortBy)
        {
            var query = _context.Products
                .Include(p => p.Subcategory)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Reviews)
                .Where(p => p.Subcategory != null && p.Subcategory.UrlSlug == subCategorySlug && p.IsActive);

            if (brands != null && brands.Any())
            {
                query = query.Where(p => p.Brand != null && brands.Contains(p.Brand.Name));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue && maxPrice.Value > 0)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (minRating.HasValue)
            {
                query = query.Where(p => p.Reviews.Any() && p.Reviews.Average(r => r.Rating) >= minRating.Value);
            }

            switch (sortBy)
            {
                case "priceAsc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                case "bestRated":
                    query = query.OrderByDescending(p => p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0);
                    break;
                case "newest":
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
                default:
                    query = query.OrderBy(p => p.Name);
                    break;
            }

            return query.ToList();
        }

        public Product GetProductById(int productId)
        {
            return _context.Products
                .Include(p => p.Subcategory)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Reviews)
                .FirstOrDefault(p => p.Id == productId && p.IsActive);
        }

        public IEnumerable<Product> SearchProducts(string term)
        {
            return _context.Products
                .Where(p => p.Name.Contains(term) || p.Description.Contains(term))
                .ToList();
        }

    }
}
