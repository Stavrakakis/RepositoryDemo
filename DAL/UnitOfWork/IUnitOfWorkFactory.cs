using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.UnitOfWork;

namespace DAL
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}
