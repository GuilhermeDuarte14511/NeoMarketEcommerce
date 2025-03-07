using Microsoft.EntityFrameworkCore;
using NeoMarket.Domain.Interfaces;
using NeoMarket.Infrastructure.Data;
using System.Collections.Generic;

namespace NeoMarket.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public bool Add(T entity)
        {
            _dbSet.Add(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Update(T entity)
        {
            _dbSet.Update(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
