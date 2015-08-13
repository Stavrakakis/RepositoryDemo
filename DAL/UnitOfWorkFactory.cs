using Effort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        { 
            var inMemoryConnection = DbConnectionFactory.CreateTransient();
            var test = false;

            var context = test ?  new DataContext(inMemoryConnection) : new DataContext();

            return new UnitOfWork(context);
        }
    }
}
