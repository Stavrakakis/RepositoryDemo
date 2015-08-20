using DAL.Database;
using DAL.Domain;
using System;
using System.Data.Entity;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        private DbContext context;

        private IRepository<User> userRepository;
        private IRepository<Address> addressRepository;
        private readonly IMapper mapper;

        public UnitOfWork(DbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IRepository<Address> AddressRepository
        {
            get
            {
                return this.addressRepository ?? (this.addressRepository = new GenericRepository<AddressDto, Address>(this.context, this.mapper));
            }
        }

        public IRepository<User> UserRepository 
        {
            get
            {
                return this.userRepository ?? (this.userRepository = new GenericRepository<UserDto, User>(this.context, this.mapper));
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
