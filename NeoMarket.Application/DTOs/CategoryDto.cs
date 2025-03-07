using System.Collections.Generic;

namespace NeoMarket.Application.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubcategoryDto> Subcategories { get; set; } = new List<SubcategoryDto>();
    }
}
