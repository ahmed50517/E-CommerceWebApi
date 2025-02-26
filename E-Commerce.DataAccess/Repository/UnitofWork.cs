using E_Commerce.Data;
using E_Commerce.Repository.IRepository;

namespace E_Commerce.Repository
{
    public class UnitofWork : IUnitofWork
    {
        private readonly ApplicationDbContext _db;

        public ICategoryRepository Categories { get; private set; }
        public IProductRepository Products { get; private set; }
        public IOrderRepository Orders { get; private set; }

        public UnitofWork(ApplicationDbContext db)
        {
            _db = db;
            Categories = new CategoryRepository(_db);
            Products = new ProductRepository(_db);
            Orders = new OrderRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
