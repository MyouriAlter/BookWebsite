using Book.DataAccess.Data;
using Book.DataAccess.Repository.IRepository;

namespace Book.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Category = new CategoryRepository(_dbContext);
            CoverType = new CoverTypeRepository(_dbContext);
            Product = new ProductRepository(_dbContext);
            SpCall = new SpCall(_dbContext);
        }

        public IProductRepository Product { get; }
        public ICoverTypeRepository CoverType { get; }
        public ICategoryRepository Category { get; }
        public ISpCall SpCall { get; }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}