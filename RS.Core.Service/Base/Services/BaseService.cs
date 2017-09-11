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
using System.Threading.Tasks;

namespace RS.Core.Service
{
    public interface IBaseService<A,U,G,D,Y>:ICRUDService<A,U,G,Y>
        where Y:struct
        where D:Entity<Y>
        where U:EntityUpdateDto<Y>
        where G:EntityGetDto<Y>
    {
        D AddMapping(A entity, Y? fooId = default(Y?));
    }
    public class BaseService<A,U,G,D,Y>:IBaseService<A,U,G,D,Y>
        where Y : struct
        where D : Entity<Y>
        where U : EntityUpdateDto<Y>
        where G : EntityGetDto<Y>
    {
        protected EntityUnitofWork<Y> uow;
        public BaseService(EntityUnitofWork<Y> _uow)
        {
            uow = _uow;
        }
        public virtual D AddMapping(A model, Y? fooID = default(Y?))
        {
            D entity = Mapper.Map<D>(model);
            return entity;
        }
        public virtual async Task<APIResult> Add(A model,Y userID, bool isCommit = true, Y? fooID = default(Y?))
        {
            D entity = AddMapping(model, fooID);

            var pkType = default(Y);

            if(pkType is Guid)
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

            if(isCommit)
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
                    if ((object)(entity as ITableEntity<Y>).CreateBy != (object)userID.Value)
                        return new APIResult() { Data = model.ID, Message = Messages.GNW0001 };
                }

                (entity as ITableEntity<Y>).UpdateDT = DateTime.Now;
                (entity as ITableEntity<Y>).UpdateBy = userID.Value;
            }

            Mapper.Map(model, entity);

            if (isCommit)
                await uow.SaveChangesAsync();

            return new APIResult() { Message = Messages.Ok };
        }
        public virtual async Task<APIResult> Delete(Y id,Y? userID= default(Y?), bool isCommit=true, bool checkAuthorize = false)
        {
            D entity = await uow.Repository<D>().GetByID(id);

            if (entity == null)
                return new APIResult() { Data=id , Message = Messages.GNE0001 };

            if (entity is ITableEntity<Y>)
            {
                //Access Control
                if (userID != null && checkAuthorize)
                {
                    if ((object)(entity as ITableEntity<Y>).CreateBy != (object)userID.Value)
                        return new APIResult() { Message = Messages.GNW0001 };
                }

                 (entity as ITableEntity<Y>).UpdateDT = DateTime.Now;
                 (entity as ITableEntity<Y>).UpdateBy = userID.Value;
            }

            entity.IsDeleted = true;

            if(isCommit)
                await uow.SaveChangesAsync();

            return new APIResult() { Data=id, Message = Messages.Ok };
        }
        public virtual async Task<G> GetByID(Y id,Y? userID= default(Y?), bool isDeleted = false)
        {
            //İlgili kaydın, ilgili kullanıcıya ait olma durumunu kontrol etmektedir.
            if (userID != null)
            {
                var query = (IQueryable<ITableEntity<Y>>)uow.Repository<D>().Query(isDeleted);

                return await query.Where(x=>(object)x.ID==(object)id && (object)x.CreateBy== (object)userID.Value).Cast<D>().
                    ProjectTo<G>().FirstOrDefaultAsync();
            }

            return await uow.Repository<D>().Query(isDeleted).Where(x => (object)x.ID == (object)id).ProjectTo<G>().FirstOrDefaultAsync();
        }
        public virtual async Task<IList<AutoCompleteListVM<Y>>> AutoCompleteList(Y? id = default(Y?), string text = default(string))
        {
            var query = uow.Repository<D>().Query().ProjectTo<AutoCompleteList<Y>>().AsQueryable();

            var pkType = default(Y);

            if((pkType is Guid && !(id as Guid?).IsNullOrEmpty()) || id!=null)
                query = query.Where(x => (object)x.ID == (object)id);

            if (text != null)
                query = query.Where(x => x.Search.Contains(text));

            return await query.OrderBy(x=>x.Text).ProjectTo<AutoCompleteListVM<Y>>().ToListAsync();
        }
    }
}
