using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RS.Core.Const;
using RS.Core.Lib;
using RS.Core.Domain;
using RS.Core.Service.DTOs;
using System.ComponentModel;
using System.Data.Entity;

namespace RS.Core.Service
{
    ///ToDo:
    /// checkAutorize read in webconfig
    /// Itableentityler model degıl entity olacak.
    ///Createby ve updateby generic type alacak (Y)

    public interface IBaseService<A,U,G,D,Y>:ICRUDService<A,U,G,Y>
        where Y:struct
        where D:Entity<Y>
        where U:EntityUpdateDto<Y>
        where G:EntityGetDto<Y>
    {
        D AddMapping(A entity);
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
        public virtual D AddMapping(A model)
        {
            D entity = Mapper.Map<D>(model);
            return entity;
        }
        public virtual async Task<APIResult> Add(A model,Y UserID, bool isCommit = true)
        {
            D entity = AddMapping(model);

            var pkType = default(Y);

            if(pkType is Guid)
            {
                if ((entity.ID as Guid?).IsNullOrEmpty())
                    entity.ID = (Y)TypeDescriptor.GetConverter(typeof(Y)).ConvertFromInvariantString(Guid.NewGuid().ToString());
            }

            if (model is ITableEntity<Y>)
            {
                (model as ITableEntity<Y>).CreateBy = UserID;
                (model as ITableEntity<Y>).CreateDT = DateTime.Now;      
            }
                
            uow.Repository<D>().Add(entity);

            if(isCommit)
                await uow.SaveChangesAsync();

            return new APIResult { Data = entity.ID, Message = Messages.Ok };
        }
        public virtual async Task<APIResult> Update(U model, Y? UserID = null, bool isCommit = true, bool checkAuthorize = false)
        {
            D entity = await uow.Repository<D>().GetByID(model.ID);

            if (model == null)
                return new APIResult() { Data = model.ID, Message = Messages.GNE0001 };

            if (model is ITableEntity<Y>)
            {
                ///Access Control
                if (UserID != null && checkAuthorize)
                {
                    if ((object)(model as ITableEntity<Y>).CreateBy != (object)UserID.Value)
                        return new APIResult() { Data = model.ID, Message = Messages.GNW0001 };
                }

                (model as ITableEntity<Y>).UpdateDT = DateTime.Now;
                (model as ITableEntity<Y>).UpdateBy = UserID.Value;
            }

            Mapper.Map(entity, model);

            if (isCommit)
                await uow.SaveChangesAsync();

            return new APIResult() { Message = Messages.Ok };
        }
        public virtual async Task<APIResult> Delete(Y id,Y? UserID=null,bool isCommit=true, bool checkAuthorize = false)
        {
            D entity = await uow.Repository<D>().GetByID(id);

            if (entity == null)
                return new APIResult() { Data=id , Message = Messages.GNE0001 };

            if (entity is ITableEntity<Y>)
            {
                //Access Control
                if (UserID != null && checkAuthorize)
                {
                    if ((object)(entity as ITableEntity<Y>).CreateBy != (object)UserID.Value)
                        return new APIResult() { Message = Messages.GNW0001 };
                }

                 (entity as ITableEntity<Y>).UpdateDT = DateTime.Now;
                 (entity as ITableEntity<Y>).UpdateBy = UserID.Value;
            }

            entity.IsDeleted = true;

            if(isCommit)
                await uow.SaveChangesAsync();

            return new APIResult() { Data=id, Message = Messages.Ok };
        }
        public virtual async Task<G> GetByID(Y id,Y? UserID=null, bool isDeleted = false)
        {
            //İlgili kaydın, ilgili kullanıcıya ait olma durumunu kontrol etmektedir.
            if (UserID != null)
            {
                var query = (IQueryable<ITableEntity<Y>>)uow.Repository<D>().Query(isDeleted);

                return await query.Where(x=>(object)x.ID==(object)id && (object)x.CreateBy== (object)UserID.Value).Cast<D>().
                    ProjectTo<G>().FirstOrDefaultAsync();
            }

            return await uow.Repository<D>().Query(isDeleted).Where(x => (object)x.ID == (object)id).ProjectTo<G>().FirstOrDefaultAsync();
        }
        public virtual async Task<IList<AutoCompleteListVM<Y>>> AutoCompleteList(Y? id = null, string Text = null)
        {
            var query = uow.Repository<D>().Query().ProjectTo<AutoCompleteList<Y>>().AsQueryable();

            var pkType = default(Y);

            if((pkType is Guid && !(id as Guid?).IsNullOrEmpty()) || id!=null)
                query = query.Where(x => (object)x.ID == (object)id);

            if (Text != null)
                query = query.Where(x => x.Search.Contains(Text));

            return await query.OrderBy(x=>x.Text).ProjectTo<AutoCompleteListVM<Y>>().ToListAsync();
        }
    }
}
