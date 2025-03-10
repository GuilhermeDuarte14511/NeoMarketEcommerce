using NeoMarket.Domain.Entities;
using NeoMarket.Application.Interfaces;
using NeoMarket.Domain.Interfaces;
using NeoMarket.Application.DTOs.Login;

namespace NeoMarket.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public UserDTO Authenticate(LoginRequest request)   
        {
            var user = _accountRepository.GetUserByEmailAndPassword(request.Email, request.Password);
            if (user == null || user.UserType != UserType.Customer) return null;

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RG = user.RG,
                CPF = user.CPF,
                BirthDate = user.BirthDate,
                IsActive = user.IsActive
            };
        }
    }
}
