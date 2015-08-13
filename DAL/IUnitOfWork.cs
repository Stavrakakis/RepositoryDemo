using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IUnitOfWork : IDisposable
    {
        GenericRepository<User> UserRepository { get; }

        void Save();
    }
}
