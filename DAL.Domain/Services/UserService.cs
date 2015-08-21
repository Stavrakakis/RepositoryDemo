namespace DAL.Services
{
    using Domain.Repositories;
    using Domain.Criteria;
    using Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Expressions;

    public class UserService : IUserService
    {
        private readonly IUnitOfWorkFactory sessionFactory;

        public UserService(IUnitOfWorkFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public IEnumerable<User> GetUsersFromCountry(string countryName)
        {
            using (var session = this.sessionFactory.Create())
            {
                var userDtos = session.UserRepository.GetAll();

                return Enumerable.Empty<User>();
            }            
        }

        public IEnumerable<User> GetUsers(UserCriteria criteria, int pageNumber = 1, int pageSize = 10)
        {
            using (var session = this.sessionFactory.Create())
            {                   
                Expression<Func<User, bool>> userIdFilter = u => u.Id != 2;

                var expr = this.OlderThan(28);                
                
                var idInList = this.ContainsValue<User, int>(criteria.Ids.ToList(), u => u.Id);

                var filter = PredicateBuilder.Or(idInList, expr);

                Expression<Func<User, bool>> nameStartsWithN = u => u.FirstName.Contains("N");                
                
                var users = session.UserRepository.GetAll(expr).ToList();
                
                return users;
            } 
        }
        
        private Expression<Func<User, bool>> OlderThan(int age)
        {
            ParameterExpression argParam = Expression.Parameter(typeof(User), "u");

            Expression nameProperty = Expression.Property(argParam, "Age");

            var val1 = Expression.Constant(age);

            Expression e1 = Expression.GreaterThan(nameProperty, val1);

            var lambda = Expression.Lambda<Func<User, bool>>(e1, argParam);

            return lambda;
        }

        private Expression<Func<TEntity, bool>> ContainsValue<TEntity, TProperty>(List<TProperty> things, Expression<Func<TEntity, TProperty>> lookup)
        {
            var name = "Id";

            ParameterExpression entityParameter = Expression.Parameter(typeof(TEntity), "r");
            ConstantExpression foreignKeysParameter = Expression.Constant(things, typeof(List<TProperty>));
            MemberExpression memberExpression = Expression.Property(entityParameter, name);
            Expression convertExpression = Expression.Convert(memberExpression, typeof(int));
            MethodCallExpression containsExpression = Expression.Call(foreignKeysParameter
                , "Contains", new Type[] { }, convertExpression);

            return Expression.Lambda<Func<TEntity, bool>>(containsExpression, entityParameter);
        }
    }
}
