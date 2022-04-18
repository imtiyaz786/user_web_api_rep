using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using USERWebApi.Models;
namespace USERWebApi.Repository
{
    public class PatientRepository<T> : IRepository<T> where T : class
    {
        private readonly UserDbContext context;
        public PatientRepository(UserDbContext context)
        {
            this.context = context;
        }
        public T Add(T item)
        {
            return context.Add(item).Entity;
        }
        public async Task<List<T>> GetAll(params string[] includes)
        {
            var data = context.Set<T>().AsQueryable();
            if (includes != null)
            {
                foreach (var item in includes)
                {
                    data = data.Include(item);
                }
            }
            return await data.ToListAsync();
        }
        public async Task<int> GetByUserId(string id)
        {
            Patient patient = await context.Patients.FirstAsync(p => p.RegistrationModelId == id);
            return patient.PatientId;
        }
        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
