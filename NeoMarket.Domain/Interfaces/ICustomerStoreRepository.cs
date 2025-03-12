using NeoMarket.Domain.Entities;
using System.Collections.Generic;

namespace NeoMarket.Domain.Interfaces
{
    public interface ICustomerStoreRepository
    {
        void AddCustomerToStore(int userId, int storeId);
        List<Store> GetStoresByCustomer(int userId);
        List<User> GetCustomersByStore(int storeId);
        bool CustomerExistsInStore(int userId, int storeId);
    }
}
