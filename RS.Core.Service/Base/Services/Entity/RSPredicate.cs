using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RS.Core.Service
{
    public static class RSPredicate
    {
        public static IQueryable<T> Equal<T>(this IQueryable<T> query, string property, object value)
        {
            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);
            var constant = Expression.Constant(value);
            var body = Expression.Equal(prop, constant);

            return query.Where(Expression.Lambda<Func<T, bool>>(body, param));
        }
        public static IQueryable<T> Contains<T>(this IQueryable<T> query, string property, string value)
        {
            var methodInfo = typeof(String).GetMethod("Contains", new Type[] { typeof(String) });

            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);
            var constant = Expression.Constant(value);
            var body = Expression.Call(prop, methodInfo, constant);

            return query.Where(Expression.Lambda<Func<T, bool>>(body, param));
        }
        public static IQueryable<T> GreaterThan<T>(this IQueryable<T> query, string property, object value)
        {
            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);
            var constant = Expression.Constant(value);
            var body = Expression.GreaterThan(prop, constant);

            return query.Where(Expression.Lambda<Func<T, bool>>(body, param));
        }
        public static IQueryable<T> GreaterThanOrEqual<T>(this IQueryable<T> query, string property, object value)
        {
            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);
            var constant = Expression.Constant(value);
            var body = Expression.GreaterThanOrEqual(prop, constant);

            return query.Where(Expression.Lambda<Func<T, bool>>(body, param));
        }
        public static IQueryable<T> LessThan<T>(this IQueryable<T> query, string property, object value)
        {
            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);
            var constant = Expression.Constant(value);
            var body = Expression.LessThan(prop, constant);

            return query.Where(Expression.Lambda<Func<T, bool>>(body, param));
        }
        public static IQueryable<T> LessThanOrEqual<T>(this IQueryable<T> query, string property, object value)
        {
            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);
            var constant = Expression.Constant(value);
            var body = Expression.LessThanOrEqual(prop, constant);

            return query.Where(Expression.Lambda<Func<T, bool>>(body, param));
        }
        public static IQueryable<T> DiffDaysEqual<T>(this IQueryable<T> query, string property, object value)
        {
            var methodInfo = typeof(DbFunctions).GetMethod("DiffDays", new Type[] { typeof(DateTime?), typeof(DateTime?) });

            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);

            var left = Expression.Call(
                methodInfo,
                Expression.Convert(Expression.Constant(value), typeof(DateTime?)), Expression.Convert(prop, typeof(DateTime?)));
            var right = Expression.Convert(Expression.Constant(0), typeof(int?));

            var body = Expression.Equal(left, right);

            return query.Where(Expression.Lambda<Func<T, bool>>(body, param));
        }
        public static IQueryable<T> DiffDaysGreaterThan<T>(this IQueryable<T> query, string property, object value)
        {
            var methodInfo = typeof(DbFunctions).GetMethod("DiffDays", new Type[] { typeof(DateTime?), typeof(DateTime?) });

            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);

            var left = Expression.Call(
                methodInfo,
                Expression.Convert(Expression.Constant(value), typeof(DateTime?)), Expression.Convert(prop, typeof(DateTime?)));
            var right = Expression.Convert(Expression.Constant(0), typeof(int?));

            var body = Expression.GreaterThanOrEqual(left, right);

            return query.Where(Expression.Lambda<Func<T, bool>>(body, param));
        }
        public static IQueryable<T> DiffDaysLessThan<T>(this IQueryable<T> query, string property, object value)
        {
            var methodInfo = typeof(DbFunctions).GetMethod("DiffDays", new Type[] { typeof(DateTime?), typeof(DateTime?) });

            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);

            var left = Expression.Call(
                methodInfo,
                Expression.Convert(Expression.Constant(value), typeof(DateTime?)), Expression.Convert(prop, typeof(DateTime?)));
            var right = Expression.Convert(Expression.Constant(0), typeof(int?));

            var body = Expression.LessThanOrEqual(left, right);

            return query.Where(Expression.Lambda<Func<T, bool>>(body, param));
        }
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string property)
        {
            var param = Expression.Parameter(query.ElementType, "o");
            var prop = Expression.Property(param, property);

            return query.OrderBy(Expression.Lambda<Func<T, object>>(prop, param));
        }
        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string property)
        {
            var param = Expression.Parameter(query.ElementType, "o");
            var prop = Expression.Property(param, property);

            return query.OrderByDescending(Expression.Lambda<Func<T, object>>(prop, param));
        }
    }
}
