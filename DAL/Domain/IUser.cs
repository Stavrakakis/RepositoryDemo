using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Domain
{
    public interface IUser
    {
        int Id { get; }
 
        string FirstName { get; }

        string Surname { get; }

        int Age { get; }

        Address Address { get; }
    }
}
