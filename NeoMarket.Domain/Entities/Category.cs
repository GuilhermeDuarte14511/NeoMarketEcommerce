using System;
using System.Collections.Generic;

namespace NeoMarket.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        public ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
