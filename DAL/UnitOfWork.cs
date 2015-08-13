using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        private DbContext context;

        private GenericRepository<User> userRepository;

        public UnitOfWork(DbContext context)
        {
            this.context = context; 
        }

        public GenericRepository<User> UserRepository 
        {
            get
            {
                return this.userRepository ?? (this.userRepository = new GenericRepository<User>(this.context));
            }
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
