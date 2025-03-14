﻿using System.Collections.Generic;

namespace NeoMarket.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string SubcategoryName { get; set; }
        public string CategoryName { get; set; }
        public string Brand { get; set; }
        public double Rating { get; set; }

        public decimal Weight { get; set; }
        public decimal Width { get; set; } 
        public decimal Height { get; set; }
        public decimal Length { get; set; }

        public List<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
    }
}
