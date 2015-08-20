using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using DAL.Database;
using System.Reflection;

namespace DAL.UnitOfWork
{
    public class GenericRepository<TModel, TEntity> : IRepository<TEntity> where TEntity : class, new() where TModel : class, new()
    {
        private DbContext context;
        private DbSet<TModel> dbSet;
        private readonly IMapper mapper;

        public GenericRepository(DbContext context, IMapper mapper) 
        {
            this.context = context;
            this.dbSet = context.Set<TModel>();
            this.mapper = mapper;
        }

        public void Insert(TEntity entity) 
        {
            var model = this.mapper.Map<TEntity, TModel>(entity);
            dbSet.Add(model);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null) 
        {
            var queryable =  this.dbSet.AsExpandable();

            // todo - add expression mapping here

            var dtoFilter = MappingHelper.ConvertExpression<TEntity, TModel>(filter);

            queryable = filter == null ? queryable : queryable.Where(dtoFilter);

            return queryable.ToList().Select(u => this.mapper.Map<TModel, TEntity>(u));
        }
    }

    /// <summary>
    /// Enables the efficient, dynamic composition of query predicates.
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate that evaluates to true.
        /// </summary>
        public static Expression<Func<T, bool>> True<T>() { return param => true; }

        /// <summary>
        /// Creates a predicate that evaluates to false.
        /// </summary>
        public static Expression<Func<T, bool>> False<T>() { return param => false; }

        /// <summary>
        /// Creates a predicate expression from the specified lambda expression.
        /// </summary>
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) { return predicate; }

        /// <summary>
        /// Combines the first predicate with the second using the logical "and".
        /// </summary>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        /// <summary>
        /// Combines the first predicate with the second using the logical "or".
        /// </summary>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        /// <summary>
        /// Negates the predicate.
        /// </summary>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        /// <summary>
        /// Combines the first expression with the second using the specified merge function.
        /// </summary>
        static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // create a merged lambda expression with parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        class ParameterRebinder : System.Linq.Expressions.ExpressionVisitor
        {
            readonly Dictionary<ParameterExpression, ParameterExpression> map;

            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;

                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }

                return base.VisitParameter(p);
            }
        }
    }

    public static class MappingHelper
    {
        public static Expression<Func<TTo, bool>> ConvertExpression<TFrom, TTo>(this Expression<Func<TFrom, bool>> expr)
        {
            Dictionary<Expression, Expression> substitutes = new Dictionary<Expression, Expression>();
            var oldParam = expr.Parameters[0];
            var newParam = Expression.Parameter(typeof(TTo), oldParam.Name);
            substitutes.Add(oldParam, newParam);
            Expression body = ConvertNode<TFrom, TTo>(expr.Body, substitutes);
            return Expression.Lambda<Func<TTo, bool>>(body, newParam);
        }

        static Expression ConvertNode<TFrom, TTo>(Expression node, IDictionary<Expression, Expression> subst)
        {
            if (node == null) return null;
            if (subst.ContainsKey(node)) return subst[node];

            switch (node.NodeType)
            {
                case ExpressionType.Constant:
                    return node;
                case ExpressionType.Convert:
                    {
                        var me = (UnaryExpression)node;

                        Expression operand = ConvertNode<TFrom, TTo>(me.Operand, subst);

                        if (operand != me.Operand)
                        {
                            return Expression.MakeUnary(me.NodeType, operand, me.Type, me.Method);
                        }
                        return me;
                    }
                case ExpressionType.Parameter:
                    return node;
                case ExpressionType.Invoke:
                    {
                        var me = (InvocationExpression)node;

                        return ConvertNode<TFrom, TTo>(me.Expression, subst);
                    }
                case ExpressionType.Lambda:
                    {
                        var me = (LambdaExpression)node;

                        return ConvertNode<TFrom, TTo>(me.Body, subst);
                    }
                case ExpressionType.Call:
                    {
                        var me = (MethodCallExpression)node;
                        Expression obj = ConvertNode<TFrom, TTo>(me.Object, subst);
                        
                        var args = me.Arguments.Select(a => ConvertNode<TFrom, TTo>(a, subst));
                        if (obj != me.Object || args != me.Arguments)
                        {

                            return Expression.Call(obj, me.Method, args);
                        }
                        return me;                        
                    }
                case ExpressionType.MemberAccess:
                    {
                        var me = (MemberExpression)node;
                        var newNode = ConvertNode<TFrom, TTo>(me.Expression, subst);

                        MemberInfo info = null;
                        var map = AutoMapper.Mapper.FindTypeMapFor(typeof(TFrom), typeof(TTo));
                        var propertyMaps = map.GetPropertyMaps();
                        foreach (var property in propertyMaps)
                        {
                            if (property.SourceMember.Name == me.Member.Name)
                            {
                                info = property.DestinationProperty.MemberInfo;
                                break;
                            }
                        }

                        return Expression.MakeMemberAccess(newNode, info);
                    }
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.NotEqual:
                case ExpressionType.Equal: /* will probably work for a range of common binary-expressions */
                    {
                        var be = (BinaryExpression)node;
                        return Expression.MakeBinary(be.NodeType, ConvertNode<TFrom, TTo>(be.Left, subst), ConvertNode<TFrom, TTo>(be.Right, subst), be.IsLiftedToNull, be.Method);
                    }

                default:
                    throw new NotSupportedException(node.NodeType.ToString());
            }
        }
    }


}
