using LinqKit;
using System;
using System.Linq.Expressions;

namespace DAL
{
    public static class Filters
    {
        public static Expression<Func<T, bool>> Or<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            return PredicateBuilder.False<T>().Or<T>(left.Expand()).Or<T>(right.Expand());
        }

        public static Expression<Func<T, bool>> And<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            return PredicateBuilder.True<T>().And<T>(left.Expand()).And<T>(right.Expand());
        }
    }
}
