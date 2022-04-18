using System.Collections.Generic;
using System.Threading.Tasks;

namespace USERWebApi.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll(params string[] includes);
        public T Add(T item);
        Task<int> SaveAsync();
        Task<int> GetByUserId(string id);
    }
}
