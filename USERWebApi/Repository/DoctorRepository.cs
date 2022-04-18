using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using USERWebApi.Models;

namespace USERWebApi.Repository
{
    public class DoctorRepository<T> : IRepository<T> where T : class
    {
        private readonly UserDbContext context;
        public DoctorRepository(UserDbContext context)
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
            Doctor doctor = await context.Doctors.FirstAsync(p => p.RegistrationModelId == id);
            return doctor.DoctorId;
        }
        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
