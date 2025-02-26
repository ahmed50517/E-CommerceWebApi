using System.Linq.Expressions;
using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repository.IRepository;

namespace E_Commerce.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Order obj)
        {
            _db.Orders.Update(obj);
        }
       
    }
}
