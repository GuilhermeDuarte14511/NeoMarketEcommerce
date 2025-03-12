using System;
using System.Collections.Generic;

namespace NeoMarket.Domain.Entities
{

    public class Store
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UrlSlug { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }  // Proprietário da loja (Administrador)
        public User User { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>(); // Relacionamento com os usuários da loja
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<StoreCarouselImage> CarouselImages { get; set; } = new List<StoreCarouselImage>();
    }


}
