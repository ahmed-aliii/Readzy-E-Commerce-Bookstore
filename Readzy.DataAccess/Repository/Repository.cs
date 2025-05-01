using Microsoft.EntityFrameworkCore;
using Readzy.DataAccess.Data;
using Readzy.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Readzy.DataAccess.Repository
{
    //Generic Repo
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Dependency Injection

        private readonly ReadzyContext context;
        internal DbSet<T> dbSet;
        public Repository(ReadzyContext _context)
        {
            this.context = _context;
            this.dbSet = context.Set<T>();
        } 

        #endregion

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> selector)
        {
            IQueryable<T> queryableList = dbSet;
            queryableList = queryableList.Where(selector);
            return queryableList.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> queryableList = dbSet;
            return queryableList.ToList();
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
