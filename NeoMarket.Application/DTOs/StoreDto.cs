using System.Collections.Generic;

namespace NeoMarket.Application.DTOs
{
    public class StoreDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UrlSlug { get; set; }
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
        public List<CarouselImageDto> CarouselImages { get; set; } = new List<CarouselImageDto>();
    }
}
