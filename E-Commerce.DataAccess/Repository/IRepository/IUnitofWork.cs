

namespace E_Commerce.Repository.IRepository
{
    public interface IUnitofWork
    {
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }

        void Save();
    }
}
