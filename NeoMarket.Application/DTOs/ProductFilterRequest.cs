namespace NeoMarket.Application.DTOs
{
    public class ProductFilterRequest
    {
        public string SubCategorySlug { get; set; }
        public List<string> Brands { get; set; } = new List<string>();
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinRating { get; set; }
    }
}
