using DAL.Criteria;
using DAL.Database;
using DAL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetUsersFromCountry(string countryName);

        IEnumerable<User> GetUsers(UserCriteria criteria, int pageNumber = 1, int pageSize = 10);
    }
}
