using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HardwareHub.Data.Services.IRepositories
{
    public interface IGenericRepository<T>
    {
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null, bool tracked = true , String? includeProperties = null);
        Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true, String ? includeProperties = null);
        Task Create(T entity);
        Task Delete(T entity);
    }
}
