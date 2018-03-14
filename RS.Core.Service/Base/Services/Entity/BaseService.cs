using AutoMapper;
using AutoMapper.QueryableExtensions;
using RS.Core.Const;
using RS.Core.Domain;
using RS.Core.Lib;
using RS.Core.Service.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static RS.Core.Const.Enum;

namespace RS.Core.Service
{
    public interface IBaseService<A, U, G, C, P, D, Y> : ICRUDService<A, U, G, C, P, Y>
        where Y : struct
        where D : Entity<Y>
        where U : EntityUpdateDto<Y>
        where G : EntityGetDto<Y>
        where C : EntityGetDto<Y>
    {
        D AddMapping(A entity);
        IQueryable<D> PrepareGetQuery(P parameters);
    }
    public class BaseService<A, U, G, C, P, D, Y> : IBaseService<A, U, G, C, P, D, Y>
        where Y : struct
        where D : Entity<Y>
        where U : EntityUpdateDto<Y>
        where G : EntityGetDto<Y>
        where C : EntityGetDto<Y>
    {
        protected EntityUnitofWork<Y> uow;
        public BaseService(EntityUnitofWork<Y> _uow)
        {
            uow = _uow;
        }
        public virtual D AddMapping(A model)
        {
            D entity = Mapper.Map<D>(model);
            return entity;
        }
        public virtual async Task<APIResult> Add(A model, Y userID, bool isCommit = true)
        {
            D entity = AddMapping(model);

            var pkType = default(Y);

            if (pkType is Guid)
            {
                if ((entity.ID as Guid?).IsNullOrEmpty())
                    entity.ID = (Y)TypeDescriptor.GetConverter(typeof(Y)).ConvertFromInvariantString(Guid.NewGuid().ToString());
            }

            if (entity is ITableEntity<Y>)
            {
                (entity as ITableEntity<Y>).CreateBy = userID;
                (entity as ITableEntity<Y>).CreateDT = DateTime.Now;
            }

            uow.Repository<D>().Add(entity);

            if (isCommit)
                await uow.SaveChangesAsync();

            return new APIResult { Data = entity.ID, Message = Messages.Ok };
        }
        public virtual async Task<APIResult> Update(U model, Y? userID = default(Y?), bool isCommit = true, bool checkAuthorize = false)
        {
            D entity = await uow.Repository<D>().GetByID(model.ID);

            if (entity == null)
                return new APIResult() { Data = model.ID, Message = Messages.GNE0001 };

            if (entity is ITableEntity<Y>)
            {
                ///Access Control
                if (userID != null && checkAuthorize)
                {
                    if (!(entity as ITableEntity<Y>).CreateBy.Equals(userID.Value))
                        return new APIResult() { Data = model.ID, Message = Messages.GNW0001 };
                }

                (entity as ITableEntity<Y>).UpdateDT = DateTime.Now;
                (entity as ITableEntity<Y>).UpdateBy = userID.Value;
            }

            Mapper.Map(model, entity);

            if (isCommit)
                await uow.SaveChangesAsync();

            return new APIResult() { Data = entity.ID, Message = Messages.Ok };
        }
        public virtual async Task<APIResult> Delete(Y id, Y? userID = default(Y?), bool isCommit = true, bool checkAuthorize = false)
        {
            D entity = await uow.Repository<D>().GetByID(id);

            if (entity == null)
                return new APIResult() { Data = id, Message = Messages.GNE0001 };

            if (entity is ITableEntity<Y>)
            {
                //Access Control
                if (userID != null && checkAuthorize)
                {
                    if (!(entity as ITableEntity<Y>).CreateBy.Equals(userID.Value))
                        return new APIResult() { Message = Messages.GNW0001 };
                }

                 (entity as ITableEntity<Y>).UpdateDT = DateTime.Now;
                (entity as ITableEntity<Y>).UpdateBy = userID.Value;
            }

            entity.IsDeleted = true;

            if (isCommit)
                await uow.SaveChangesAsync();

            return new APIResult() { Data = id, Message = Messages.Ok };
        }
        public virtual async Task<C> GetByID(Y id, Y? userID = default(Y?), bool isDeleted = false)
        {
            var query = uow.Repository<D>().Query(isDeleted).Equal("ID", id);

            //İlgili kaydın, ilgili kullanıcıya ait olma durumunu kontrol etmektedir.
            if (userID != null)
                query = query.Equal("CreateBy", userID.Value);

            return await query.ProjectTo<C>().FirstOrDefaultAsync();
        }
        public virtual IQueryable<D> PrepareGetQuery(P parameters)
        {
            var query = uow.Repository<D>().Query();

            var properties = typeof(P).GetProperties()
                .Select(s => new
                {
                    name = s.Name,
                    type = s.PropertyType.IsGenericType && s.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                            ? Type.GetTypeCode(s.PropertyType.GetGenericArguments()[0])
                            : Type.GetTypeCode(s.PropertyType),
                    value = s.GetValue(parameters),
                    atts = s.CustomAttributes
                        .Where(x => x.AttributeType == typeof(RSFilterAttribute))
                        .Select(st => st.ConstructorArguments
                            .Select(sc => sc.Value)
                            .ToList())
                        .FirstOrDefault()
                })
                .ToList();

            foreach (var prop in properties)
            {
                string dbName = prop.name;
                SearchType searchType = 0;

                /// if use RS-Filter attribute.
                if (prop.atts != null)
                {
                    if ((bool)prop.atts[2])
                        continue;

                    dbName = (prop.atts[1] != null && !String.IsNullOrWhiteSpace(prop.atts[1].ToString())) ? prop.atts[1].ToString() : dbName;
                    searchType = (SearchType)prop.atts[0];
                }

                if (prop.type == TypeCode.String)
                {
                    if (prop.value != null && !String.IsNullOrWhiteSpace(prop.value.ToString()))
                        if (searchType == SearchType.Contains)
                            query = query.Contains(dbName, prop.value.ToString());
                        else if (searchType == SearchType.Equal)
                            query = query.Equal(dbName, prop.value.ToString());
                        else
                            throw new Exception(String.Format(
                                format: "{0} is a string type property. You can not query a property of type {1} {2}.",
                                arg0: dbName, arg1: prop.type, arg2: searchType));
                }

                else if (prop.type == TypeCode.Byte || prop.type == TypeCode.Double || prop.type == TypeCode.Decimal ||
                        prop.type == TypeCode.Int16 || prop.type == TypeCode.Int32 || prop.type == TypeCode.Int64 ||
                        prop.type == TypeCode.UInt16 || prop.type == TypeCode.UInt32 || prop.type == TypeCode.UInt64)
                {
                    if (prop.value != null)
                        if (searchType == SearchType.Equal)
                            query = query.Equal(dbName, prop.value);
                        else if (searchType == SearchType.GreaterThan)
                            query = query.GreaterThan(dbName, prop.value);
                        else if (searchType == SearchType.GreaterThanOrEqual)
                            query = query.GreaterThanOrEqual(dbName, prop.value);
                        else if (searchType == SearchType.LessThan)
                            query = query.LessThan(dbName, prop.value);
                        else if (searchType == SearchType.LessThanOrEqual)
                            query = query.LessThanOrEqual(dbName, prop.value);
                        else
                            throw new Exception(String.Format(
                                format: "{0} is a numeric type property. You can not query a property of type {1} {2}.",
                                arg0: dbName, arg1: prop.type, arg2: searchType));
                }

                else if (prop.type == TypeCode.Boolean)
                {
                    if (prop.value != null)
                        if (searchType == SearchType.Equal)
                            query = query.Equal(dbName, prop.value);
                        else
                            throw new Exception(String.Format(
                                format: "{0} is a boolean type property. You can not query a property of type {1} {2}.",
                                arg0: dbName, arg1: prop.type, arg2: searchType));
                }

                else if (prop.type == TypeCode.Object)
                {
                    if (prop.value != null && !Guid.Parse(prop.value.ToString()).IsNullOrEmpty())
                        if (searchType == SearchType.Equal)
                            query = query.Equal(dbName, prop.value);
                        else
                            throw new Exception(String.Format(
                                format: "{0} is a object type property. You can not query a property of type {1} {2}.",
                                arg0: dbName, arg1: prop.type, arg2: searchType));
                }

                else if (prop.type == TypeCode.DateTime)
                {
                    if (prop.value != null)
                        if (searchType == SearchType.Equal)
                            query = query.DiffDaysEqual(dbName, prop.value);
                        else if (searchType == SearchType.GreaterThanOrEqual)
                            query = query.DiffDaysGreaterThan(dbName, prop.value);
                        else if (searchType == SearchType.LessThanOrEqual)
                            query = query.DiffDaysLessThan(dbName, prop.value);
                        else
                            throw new Exception(String.Format(
                                format: "{0} is a datetime type property. You can not query a property of type {1} {2}.",
                                arg0: dbName, arg1: prop.type, arg2: searchType));
                }
            }

            return query;
        }
        public virtual async Task<IList<AutoCompleteListVM<Y>>> AutoCompleteList(P parameters,
            Y? id = default(Y?), string text = default(string))
        {
            IQueryable<AutoCompleteList<Y>> query = null;

            if (parameters != null)
                query = PrepareGetQuery(parameters).ProjectTo<AutoCompleteList<Y>>().AsQueryable();
            else
                query = uow.Repository<D>().Query().ProjectTo<AutoCompleteList<Y>>().AsQueryable();

            var pkType = default(Y);

            if ((pkType is Guid && !(id as Guid?).IsNullOrEmpty()) || id != null)
                query = query.Where(x => (object)x.ID == (object)id);

            if (text != null)
                query = query.Where(x => x.Search.Contains(text));

            return await query.OrderBy(x => x.Text).ProjectTo<AutoCompleteListVM<Y>>().ToListAsync();
        }
        public virtual async Task<IList<G>> Get(P parameters, string sortField, bool sortOrder)
        {
            var query = PrepareGetQuery(parameters);

            if (sortOrder)
                query = query.OrderBy(sortField);
            else
                query = query.OrderByDescending(sortField);

            return await query.ProjectTo<G>().ToListAsync();
        }
        public virtual async Task<EntityGetPagingDto<Y, G>> GetPaging(P parameters, string sortField, bool sortOrder,
            string sumField, int? first = null, int? rows = null)
        {
            var query = PrepareGetQuery(parameters);

            if (sortOrder)
                query = query.OrderBy(sortField);
            else
                query = query.OrderByDescending(sortField);

            /// Get records count.
            int? dataCount = await query.CountAsync();

            /// It takes the sum according to the value of <param name="sumField"></param>.
            var param = Expression.Parameter(typeof(D), "sum");
            var prop = Expression.Property(param, sumField);
            var expSum = Expression.Lambda<Func<D, double?>>(Expression.Convert(prop, typeof(double?)), param);
            double? sum = await query.SumAsync(expSum);

            if (first != null && rows != null)
                query = query
                    .Skip(first.Value)
                    .Take(rows.Value);

            var records = await query.ProjectTo<G>().ToListAsync();

            return new EntityGetPagingDto<Y, G>
            {
                DataCount = dataCount ?? 0,
                Sum = Math.Round(sum ?? 0, 2),
                Records = records == null || records.Count == 0 ? new List<G>() : records
            };
        }
    }
}
