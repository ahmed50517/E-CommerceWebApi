using E_Commerce.Models;

namespace E_Commerce.Repository.IRepository
{
    public interface IOrderRepository :IRepository<Order>
    {
        void Update(Order obj);
    }
}
