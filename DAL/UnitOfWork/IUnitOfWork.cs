using DAL.Database;
using DAL.Domain;
using System;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }
        IRepository<Address> AddressRepository { get; }

        void Save();
    }
}
