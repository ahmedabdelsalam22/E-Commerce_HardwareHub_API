using HardwareHub.Data.Services.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareHub.Data.Services.UOW
{
    public interface IUnitOfWork
    {
        public IProductRepository productRepository { get; }
        public ICategoryRepository categoryRepository { get; }

    }
}
