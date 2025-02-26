using System.Linq.Expressions;

namespace E_Commerce.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(); //retrive amd display all Categories
        IEnumerable<T> GetAllByfilter(Expression<Func<T, bool>> filter);
        T Get(Expression<Func<T, bool>> filter); //get indvidual category using first or default and lambda expression
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        T GetProductsByCategory(int id, Expression<Func<T, bool>> filter);

    }
}
