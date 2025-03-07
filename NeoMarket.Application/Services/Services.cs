using NeoMarket.Application.Interfaces;
using NeoMarket.Domain.Interfaces;

namespace NeoMarket.Application.Services
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public bool Add(T entity)
        {
            return _repository.Add(entity);
        }

        public bool Update(T entity)
        {
            return _repository.Update(entity);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }
    }
}
