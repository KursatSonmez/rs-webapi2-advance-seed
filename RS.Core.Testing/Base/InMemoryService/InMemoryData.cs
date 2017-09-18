using Moq;
using RS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace RS.Core.Testing
{
    public interface IInMemoryData<T,Y>
        where Y:struct
        where T:Entity<Y>
    {
        Mock<DbSet<T>> GetList(List<T> list);
        Mock<DbSet<T>> GetListAsNoTracking(List<T> list);
        Mock<DbSet<T>> GetListAsync(List<T> list);
        Mock<DbSet<T>> GetListAsNoTrackingAsync(List<T> list);
    }

    /// <summary>
    /// Adds in-memory data for Unit Test.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InMemoryData<T,Y>:IInMemoryData<T,Y>
        where Y:struct
        where T:Entity<Y> 
    {
        private static Tuple<IQueryable<T>, Mock<DbSet<T>>> PreparationToAddInMemoryData(List<T> list)
        {
            var dataList = list.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();

            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(dataList.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(dataList.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(dataList.GetEnumerator());

            return Tuple.Create(dataList, mockDbSet);
        }
        public Mock<DbSet<T>> GetList(List<T>  list)
        {
            var preparationResult = PreparationToAddInMemoryData(list);
            var dataList = preparationResult.Item1;
            var mockDbSet = preparationResult.Item2;

            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(dataList.Provider);

            return mockDbSet;
        }
        public Mock<DbSet<T>> GetListAsNoTracking(List<T> list)
        {
            var preparationResult = PreparationToAddInMemoryData(list);
            var dataList = preparationResult.Item1;
            var mockDbSet = preparationResult.Item2;

            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(dataList.Provider);
            mockDbSet.Setup(m => m.AsNoTracking()).Returns(mockDbSet.Object);

            return mockDbSet;
        }
        public Mock<DbSet<T>> GetListAsync(List<T> list)
        {
            var preparationResult = PreparationToAddInMemoryData(list);
            var dataList = preparationResult.Item1;
            var mockDbSet = preparationResult.Item2;

            mockDbSet.As<IDbAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<T>(dataList.GetEnumerator()));
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<T>(dataList.Provider));

            return mockDbSet;
        }
        public Mock<DbSet<T>> GetListAsNoTrackingAsync(List<T> list)
        {
            var preparationResult = PreparationToAddInMemoryData(list);
            var dataList = preparationResult.Item1;
            var mockDbSet = preparationResult.Item2;

            mockDbSet.As<IDbAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<T>(dataList.GetEnumerator()));
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<T>(dataList.Provider));
            mockDbSet.Setup(m => m.AsNoTracking()).Returns(mockDbSet.Object);

            return mockDbSet;
        }
    }
}
