using NeoMarket.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int StoreId { get; set; }
    public Store Store { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
