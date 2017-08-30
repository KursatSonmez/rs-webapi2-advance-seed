using RS.Core.Service.DTOs;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace RS.Core.Service
{
    public interface ICRUDService<A,U,G,Y>
        where Y : struct
        where U : EntityUpdateDto<Y>
        where G : EntityGetDto<Y>
    {
        Task<APIResult> Add(A model, Guid UserID, bool isCommit = true);
        Task<APIResult> Update(U model, Guid? UserID = null, bool isCommit = true, bool checkAuthorize = false);
        Task<APIResult> Delete(Y id, Guid? UserID = null, bool isCommit = true, bool checkAuthorize = false);
        G GetByID(Y id, Guid? UserID = null, bool isDeleted = false);
        IList<AutoCompleteListVM<Y>> AutoCompleteList(Y? id = null, string Text = null);
    }
}
