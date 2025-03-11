using NeoMarket.Application.DTOs;

namespace NeoMarket.Application.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductDto> GetProductsBySubCategorySlug(string subCategorySlug);
        IEnumerable<ProductDto> GetFilteredProducts(string subCategorySlug, List<string> brands, decimal? minPrice, decimal? maxPrice, int? minRating, string sortBy);
        ProductDto GetProductById(int productId);
        IEnumerable<ProductDto> SearchProducts(string term);

    }
}
