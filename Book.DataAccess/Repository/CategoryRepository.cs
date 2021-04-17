using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Book.DataAccess.Data;
using Book.DataAccess.Repository.IRepository;
using Book.Models;

namespace Book.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Category category)
        {
            var objFromDb = _dbContext.Categories
                .FirstOrDefault(s => s.Id == category.Id);
            if (objFromDb == null) return;
            objFromDb.Name = category.Name;
        }
    }
}
