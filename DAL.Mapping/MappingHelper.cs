namespace DAL.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

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
                        var map = AutoMapper.Mapper.FindTypeMapFor(me.Expression.Type, newNode.Type);
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
