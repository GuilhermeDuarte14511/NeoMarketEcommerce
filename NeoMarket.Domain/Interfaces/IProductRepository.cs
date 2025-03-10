using NeoMarket.Domain.Entities;

namespace NeoMarket.Domain.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProductsBySubCategorySlug(string subCategorySlug);
        IEnumerable<Product> GetFilteredProducts(string subCategorySlug, List<string> brands, decimal? minPrice, decimal? maxPrice, int? minRating, string sortBy);

        Product GetProductById(int productId);

    }
}
