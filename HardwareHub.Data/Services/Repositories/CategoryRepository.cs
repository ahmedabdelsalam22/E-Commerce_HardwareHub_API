using HardwareHub.Data.Services.IRepositories;
using HardwareHub.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareHub.Data.Services.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Update(Category category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
