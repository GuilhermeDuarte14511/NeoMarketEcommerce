using NeoMarket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoMarket.Domain.Interfaces
{
    public interface IStoreRepository : IRepository<Store>
    {
        Store GetBySlug(string urlSlug);
    }
}
