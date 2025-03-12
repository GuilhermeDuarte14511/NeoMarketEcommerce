using NeoMarket.Domain.Entities;
using NeoMarket.Domain.Interfaces;
using NeoMarket.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;

namespace NeoMarket.Infrastructure.Repositories
{
    public class CustomerStoreRepository : ICustomerStoreRepository
    {
        private readonly AppDbContext _context;

        public CustomerStoreRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddCustomerToStore(int userId, int storeId)
        {
            if (!_context.CustomerStore.Any(cs => cs.UserId == userId && cs.StoreId == storeId))
            {
                _context.CustomerStore.Add(new CustomerStore
                {
                    UserId = userId,
                    StoreId = storeId
                });
                _context.SaveChanges();
            }
        }

        public List<Store> GetStoresByCustomer(int userId)
        {
            return _context.CustomerStore
                .Where(cs => cs.UserId == userId)
                .Select(cs => cs.Store)
                .ToList();
        }

        public List<User> GetCustomersByStore(int storeId)
        {
            return _context.CustomerStore
                .Where(cs => cs.StoreId == storeId)
                .Select(cs => cs.User)
                .ToList();
        }

        public bool CustomerExistsInStore(int userId, int storeId)
        {
            return _context.CustomerStore.Any(cs => cs.UserId == userId && cs.StoreId == storeId);
        }
    }
}
