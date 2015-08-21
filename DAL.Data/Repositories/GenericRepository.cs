namespace DAL.Data.Repositories
{
    using Database.Dtos;
    using DAL.Domain.Repositories;
    using Mapping;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    public class GenericRepository<TModel, TEntity> : IRepository<TEntity> where TEntity : class where TModel : class, IMapTo<TEntity>
    {
        private DbContext context;
        private DbSet<TModel> dbSet;
        private readonly IMapper mapper;

        public GenericRepository(DbContext context, IMapper mapper) 
        {
            this.context = context;
            this.dbSet = context.Set<TModel>();
            this.mapper = mapper;
        }

        public void Insert(TEntity entity) 
        {
            var model = this.mapper.Map<TEntity, TModel>(entity);

            dbSet.Add(model);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null) 
        {
            var queryable = this.dbSet.AsQueryable();
            
            var dtoFilter = MappingHelper.ConvertExpression<TEntity, TModel>(filter);

            queryable = filter == null ? queryable : queryable.Where(dtoFilter);

            var dtos = queryable.ToList();
            
            return queryable.ToList().Select(u => u.Map());
        }
    }
}
