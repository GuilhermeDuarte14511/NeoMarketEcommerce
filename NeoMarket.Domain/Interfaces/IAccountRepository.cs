using NeoMarket.Domain.Entities;

namespace NeoMarket.Domain.Interfaces
{
    public interface IAccountRepository
    {
        User GetUserByEmailAndPassword(string email, string password);
        User GetUserById(int userId);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(int userId);
    }
}
