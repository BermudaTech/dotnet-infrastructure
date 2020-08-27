using Bermuda.Core.Repository.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bermuda.Core.Database.Extensions
{
    public static class QueryableExtensions
    {
        public static Expression<Func<TEntity, bool>> ToExpression<TEntity>(this List<ExpressionParameter> expressionParameters) where TEntity : class
        {
            Expression<Func<TEntity, bool>> expressions = null;
            if (expressionParameters == null || expressionParameters.Count == 0)
            {
                return expressions;
            }

            Expression left = null;
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "x");

            foreach (ExpressionParameter expressionParameter in expressionParameters)
            {
                if (left == null)
                {
                    left = GetExpression<TEntity>(parameterExpression = Expression.Parameter(typeof(TEntity), "x"), expressionParameter);
                }
                else
                {
                    left = Expression.AndAlso(left, GetExpression<TEntity>(parameterExpression, expressionParameter));
                }
            }

            return Expression.Lambda<Func<TEntity, bool>>(left, new ParameterExpression[] { parameterExpression });
        }

        public static IQueryable<TEntity> ToOrderBy<TEntity>(this IQueryable<TEntity> source, string orderBy, OrderType orderType)
        {
            string methodName = orderType == OrderType.Desc ? "OrderByDescending" : "OrderBy";

            Type type = typeof(TEntity);

            PropertyInfo propertyInfo = null;

            if (string.IsNullOrEmpty(orderBy))
            {
                propertyInfo = type.GetProperties().FirstOrDefault();
            }
            else
            {
                propertyInfo = type.GetProperty(orderBy);
            }

            if (propertyInfo == null)
            {
                throw new Exception(String.Format("This Property: {0} Not a Member Class Name: {1}", orderBy, type.FullName));
            }

            ParameterExpression expression = Expression.Parameter(type, "x");

            LambdaExpression expression3 = Expression.Lambda(Expression.MakeMemberAccess(expression, propertyInfo), new ParameterExpression[] { expression });

            MethodCallExpression expression4 = Expression.Call(typeof(Queryable), methodName, new Type[] { type, propertyInfo.PropertyType }, new Expression[] { source.Expression, Expression.Quote(expression3) });

            return source.Provider.CreateQuery<TEntity>(expression4);
        }

        private static object ConvertToPropType(PropertyInfo property, object value)
        {
            object obj2 = null;
            if (property == null)
            {
                return obj2;
            }
            Type underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
            bool flag = underlyingType != null;
            if (!flag)
            {
                underlyingType = property.PropertyType;
            }
            bool flag2 = (value != null) || flag;
            if (!flag2)
            {
                throw new Exception("Cant attrib null on non nullable. ");
            }
            if (underlyingType.IsEnum)
            {
                return (((value == null) || Convert.IsDBNull(value)) ? null : Enum.Parse(underlyingType, value.ToString()));
            }
            return (((value == null) || Convert.IsDBNull(value)) ? null : Convert.ChangeType(value, underlyingType));
        }

        private static Expression GetExpression<TEntity>(ParameterExpression parameterExpression, ExpressionParameter expressionParameter)
        {
            PropertyInfo property = typeof(TEntity).GetProperty(expressionParameter.Property);

            object obj2 = ConvertToPropType(property, expressionParameter.Value);

            MemberExpression left = Expression.Property(parameterExpression, property);

            ConstantExpression right = Expression.Constant(obj2, property.PropertyType);

            switch (expressionParameter.Operator)
            {
                case OperatorType.Equals:
                    return Expression.Equal(left, right);

                case OperatorType.NotEquals:
                    return Expression.NotEqual(left, right);

                case OperatorType.GreaterThan:
                    return Expression.GreaterThan(left, right);

                case OperatorType.LessThan:
                    return Expression.LessThan(left, right);

                case OperatorType.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(left, right);

                case OperatorType.LessThanOrEqual:
                    return Expression.LessThanOrEqual(left, right);

                case OperatorType.Contains:

                    var parameter = Expression.Parameter(typeof(TEntity), "entity");

                    //ToString is not supported in Linq-To-Entities, throw an exception if the property is not a string.

                    //string.Contains with string parameter.
                    var stringContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    return Expression.Call(left, stringContainsMethod,
                        Expression.Constant(expressionParameter.Value, typeof(string)));

                //return Expression.Call(left, typeof(string).GetMethod("Contains"), new Expression[] { right });

                case OperatorType.StartsWith:
                    return Expression.Call(left, typeof(string).GetMethod("StartsWith"), new Expression[] { right });

                case OperatorType.EndsWith:
                    return Expression.Call(left, typeof(string).GetMethod("EndsWith"), new Expression[] { right });
            }

            return null;
        }
    }
}
