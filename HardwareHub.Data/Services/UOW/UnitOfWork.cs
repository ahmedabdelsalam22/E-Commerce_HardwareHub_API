using HardwareHub.Data.Services.IRepositories;
using HardwareHub.Data.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareHub.Data.Services.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context) 
        {
            _context = context;
            productRepository = new ProductRepository(_context);
        }

        public IProductRepository productRepository { get; private set; }
    }
}
