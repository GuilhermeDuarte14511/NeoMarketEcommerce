using System;
using System.Collections.Generic;
using NeoMarket.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public decimal Weight { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public decimal Length { get; set; }
    public int StoreId { get; set; }
    public int CategoryId { get; set; }
    public int? SubcategoryId { get; set; }
    public int? BrandId { get; set; }
    public string ImageUrl { get; set; }

    public Store Store { get; set; }
    public Category Category { get; set; }
    public Subcategory Subcategory { get; set; }
    public Brand Brand { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
