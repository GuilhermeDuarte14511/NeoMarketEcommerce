using System.Collections.Generic;

namespace NeoMarket.Application.DTOs
{
    public class StoreDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UrlSlug { get; set; }
        public List<string> Categories { get; set; }
        public List<ProductDto> Products { get; set; }
        public List<CarouselImageDto> CarouselImages { get; set; }
    }

}
