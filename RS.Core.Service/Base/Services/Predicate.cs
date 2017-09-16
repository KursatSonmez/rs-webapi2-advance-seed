using RS.Core.Domain;
using System;
using System.Linq.Expressions;

namespace RS.Core.Service
{
    public static class Predicate
    {
        public static Expression<Func<T, bool>> GenericId<T,Y>(string property,Y constant)
            where Y:struct
            where T:Entity<Y>
        {
            var arg = Expression.Parameter(typeof(T), "x");
            var predicate = Expression.Lambda<Func<T, bool>>
                (Expression.Equal(
                    Expression.Property(arg, property),
                    Expression.Constant(constant)), arg);

            return predicate;
        }
    }
}
