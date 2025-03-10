using NeoMarket.Domain.Entities;
using NeoMarket.Domain.Interfaces;
using NeoMarket.Infrastructure.Data;
using System.Linq;

namespace NeoMarket.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            return _context.Users
                .Where(u => u.Email == email && u.Password == password && u.UserType == UserType.Customer && u.IsActive)
                .Select(u => new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    RG = u.RG,
                    CPF = u.CPF,
                    BirthDate = u.BirthDate,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    UserType = u.UserType
                })
                .FirstOrDefault();
        }

        public User GetUserById(int userId)
        {
            return _context.Users
                .Where(u => u.Id == userId && u.IsActive)
                .Select(u => new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    RG = u.RG,
                    CPF = u.CPF,
                    BirthDate = u.BirthDate,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    UserType = u.UserType
                })
                .FirstOrDefault();
        }

        public bool CreateUser(User user)
        {
            _context.Users.Add(user);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateUser(User user)
        {
            var existingUser = _context.Users.Find(user.Id);
            if (existingUser == null) return false;

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.RG = user.RG;
            existingUser.CPF = user.CPF;
            existingUser.BirthDate = user.BirthDate;
            existingUser.IsActive = user.IsActive;
            existingUser.UserType = user.UserType;

            _context.Users.Update(existingUser);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            return _context.SaveChanges() > 0;
        }
    }
}
