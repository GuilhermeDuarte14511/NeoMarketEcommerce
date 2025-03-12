using NeoMarket.Application.Interfaces;
using NeoMarket.Domain.Entities;
using NeoMarket.Domain.Interfaces;
using System.Collections.Generic;

namespace NeoMarket.Application.Services
{
    public class CustomerStoreService : ICustomerStoreService
    {
        private readonly ICustomerStoreRepository _customerStoreRepository;

        public CustomerStoreService(ICustomerStoreRepository customerStoreRepository)
        {
            _customerStoreRepository = customerStoreRepository;
        }

        public void AddCustomerToStore(int userId, int storeId)
        {
            _customerStoreRepository.AddCustomerToStore(userId, storeId);
        }

        public List<Store> GetStoresByCustomer(int userId)
        {
            return _customerStoreRepository.GetStoresByCustomer(userId);
        }

        public List<User> GetCustomersByStore(int storeId)
        {
            return _customerStoreRepository.GetCustomersByStore(storeId);
        }

        public bool CustomerExistsInStore(int userId, int storeId)
        {
            return _customerStoreRepository.CustomerExistsInStore(userId, storeId);
        }
    }
}
