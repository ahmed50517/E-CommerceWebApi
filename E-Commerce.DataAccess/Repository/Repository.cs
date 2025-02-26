using System.Linq.Expressions;
using E_Commerce.Data;
using E_Commerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet; // because we dont know which table in database we are dealing with _db.Categories or _db.Product
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
            //_db.Categories==dbSet
            //instead of _db.Categories.Add() it will be dbSet.Add()
            _db.Products.Include(u => u.Category).Include(u => u.CategoryId);

        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            
            return query.FirstOrDefault();
            //instead of  Category? categoryfromdb = _db.Categories.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public IEnumerable<T> GetAllByfilter(Expression<Func<T, bool>> filter)
        {

            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.ToList();
        }

        public T GetProductsByCategory(int id, Expression<Func<T, bool>> filter)
        {
            return dbSet.Where(filter).ToList().FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
