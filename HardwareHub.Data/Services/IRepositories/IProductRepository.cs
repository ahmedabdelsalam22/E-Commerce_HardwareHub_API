using HardwareHub.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareHub.Data.Services.IRepositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task Update(Product product);
    }
}
