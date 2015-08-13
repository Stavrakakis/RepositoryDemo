using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using LinqKit;

namespace DAL
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        private DbContext context;
        private DbSet<TEntity> dbSet;

        public GenericRepository(DbContext context) 
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public void Insert(TEntity entity) 
        {
            dbSet.Add(entity);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter) 
        {
            return this.dbSet.AsExpandable().Where(filter).ToList();
        }
    }
}
