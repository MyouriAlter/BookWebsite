using System;
using System.Collections.Generic;
using System.Text;
using Book.Models;

namespace Book.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
    }
}
