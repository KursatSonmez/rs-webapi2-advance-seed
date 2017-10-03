using RS.Core.Domain;
using RS.Core.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RS.Core.Lib;
using System.Data.Entity;
using AutoMapper.QueryableExtensions;

namespace RS.Core.Service
{
    public interface IAutoCodeLogService
    {
        Task Add(AutoCodeLog entity, bool isCommit = true);
        Task<IList<AutoCodeLogListDto>> GetList
            (string screenCode = null, Guid? generatedBy = null, DateTime? generationDate = null);
    }
    public class AutoCodeLogService : IAutoCodeLogService
    {
        private EntityUnitofWork<Guid> uow = null;
        public AutoCodeLogService(EntityUnitofWork<Guid> _uow)
        {
            uow = _uow;
        }

        public async Task Add(AutoCodeLog entity, bool isCommit = true)
        {
            entity.ID = Guid.NewGuid();

            uow.Repository<AutoCodeLog>().Add(entity);

            if (isCommit)
                await uow.SaveChangesAsync();
        }

        public async Task<IList<AutoCodeLogListDto>> GetList
            (string screenCode = null, Guid? generatedBy = null, DateTime? generationDate = null)
        {
            var query = uow.Repository<AutoCodeLog>().Query();

            if (screenCode != null)
                query = query.Where(x => x.AutoCode.ScreenCode.Contains(screenCode));
            if (!generatedBy.IsNullOrEmpty())
                query = query.Where(x => x.GeneratedBy == generatedBy);
            if (generationDate != null)
                query = query.Where(x => DbFunctions.DiffDays(generationDate, x.CodeGenerationDate) == 0);

            return await query.ProjectTo<AutoCodeLogListDto>().
                OrderByDescending(x=>x.Code).ToListAsync();
        }
    }
}