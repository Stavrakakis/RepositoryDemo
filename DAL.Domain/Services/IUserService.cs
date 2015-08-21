namespace DAL.Services
{
    using Domain.Criteria;
    using Domain.Entities;
    using System.Collections.Generic;

    public interface IUserService
    {
        IEnumerable<User> GetUsersFromCountry(string countryName);

        IEnumerable<User> GetUsers(UserCriteria criteria, int pageNumber = 1, int pageSize = 10);
    }
}
