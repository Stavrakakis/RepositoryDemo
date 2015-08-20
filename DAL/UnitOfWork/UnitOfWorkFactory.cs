namespace DAL.UnitOfWork
{
    using DAL.Database;
    using Effort;
    using System;

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        { 
            var inMemoryConnection = DbConnectionFactory.CreateTransient();
            
            var test = false;

            var context = test ?  new DataContext(inMemoryConnection) : new DataContext();

            context.Database.Log = Console.Write;

            var mapper = new Mapper();

            return new UnitOfWork(context, mapper);
        }
    }
}
