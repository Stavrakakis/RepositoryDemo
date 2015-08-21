namespace DAL.Domain.Repositories
{
    using Entities;
    using System;

    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }
        IRepository<Address> AddressRepository { get; }

        void Save();
    }
}
