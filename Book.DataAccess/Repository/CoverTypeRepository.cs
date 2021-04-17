using System.Linq;
using Book.DataAccess.Data;
using Book.DataAccess.Repository.IRepository;
using Book.Models;

namespace Book.DataAccess.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CoverTypeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(CoverType coverType)
        {
            var objFromDb = _dbContext.CoverTypes
                .FirstOrDefault(s => s.Id == coverType.Id);
            if (objFromDb == null) return;
            objFromDb.Name = coverType.Name;
        }
    }
}