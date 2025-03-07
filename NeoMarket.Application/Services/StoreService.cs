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
                    .Where(p => p.Category != null)
                    .GroupBy(p => p.Category)
                    .Select(g => new CategoryDto
                    {
                        Id = g.Key.Id,
                        Name = g.Key.Name,
                        Subcategories = g.Where(p => p.Subcategory != null)
                                         .GroupBy(p => p.Subcategory.Id)
                                         .Select(sg => new SubcategoryDto
                                         {
                                             Id = sg.Key,
                                             Name = sg.First().Subcategory.Name,
                                             UrlSlug = sg.First().Subcategory.UrlSlug
                                         })
                                         .ToList()
                    })
                    .ToList(),
                Products = storeData.Products.Select(p => new ProductDto
                {
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryName = p.Category?.Name,
                    SubcategoryName = p.Subcategory?.Name
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
