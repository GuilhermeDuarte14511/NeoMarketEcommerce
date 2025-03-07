using NeoMarket.Application.DTOs;
using NeoMarket.Application.Interfaces;
using NeoMarket.Domain.Interfaces;
using System.Linq;

namespace NeoMarket.Application.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;

        public StoreService(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public StoreDto GetStoreDtoBySlug(string urlSlug)
        {
            var storeData = _storeRepository.GetBySlug(urlSlug);

            if (storeData == null) return null;

            return new StoreDto
            {
                Name = storeData.Name,
                Description = storeData.Description,
                UrlSlug = storeData.UrlSlug,
                Categories = storeData.Products
                            .Where(p => p.Category != null && p.CategoryId > 0)
                            .Select(p => p.Category.Name)
                            .Where(c => !string.IsNullOrEmpty(c))
                            .Distinct()
                            .ToList(),
                Products = storeData.Products.Select(p => new ProductDto
                {
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryName = p.Category?.Name
                }).ToList(),
                CarouselImages = storeData.CarouselImages.Select(c => new CarouselImageDto
                {
                    ImageUrl = c.ImageUrl,
                    AltText = c.AltText
                }).ToList()
            };
        }

    }
}
