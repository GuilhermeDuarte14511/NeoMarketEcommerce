using Microsoft.EntityFrameworkCore;
using NeoMarket.Domain.Entities;
using NeoMarket.Domain.Interfaces;
using NeoMarket.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoMarket.Infrastructure.Repositories
{
    public class StoreRepository : Repository<Store>, IStoreRepository
    {
        public StoreRepository(AppDbContext context) : base(context) { }

        public Store GetBySlug(string urlSlug)
        {
            return _context.Stores
                .Include(s => s.Products)
                    .ThenInclude(p => p.Category)
                .Include(s => s.CarouselImages)
                .FirstOrDefault(s => s.UrlSlug == urlSlug);
        }

    }
}
