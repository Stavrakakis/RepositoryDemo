using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Filter
    {
        public static Expression<Func<T, bool>> Or<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            return PredicateBuilder.False<T>().Or<T>(left).Or<T>(right);
        }

        public static Expression<Func<T, bool>> And<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            return PredicateBuilder.True<T>().Or<T>(left).And<T>(right);
        }
    }
}
