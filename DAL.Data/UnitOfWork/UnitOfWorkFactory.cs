namespace DAL.Data.UnitOfWork
{
    using Context;
    using Mapping;
    using Domain.Repositories;
    using Effort;
    using System;

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IMapper mapper;

        public UnitOfWorkFactory(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public IUnitOfWork Create()
        { 
            var inMemoryConnection = DbConnectionFactory.CreateTransient();
            
            var test = false;

            var context = test ?  new DataContext(inMemoryConnection) : new DataContext();

            context.Database.Log = Console.Write;
            
            return new UnitOfWork(context, this.mapper);
        }
    }
}
