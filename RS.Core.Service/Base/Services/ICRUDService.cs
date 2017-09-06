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
        Task<APIResult> Add(A model, Y userID, bool isCommit = true);
        Task<APIResult> Update(U model, Y? userID = null, bool isCommit = true, bool checkAuthorize = false);
        Task<APIResult> Delete(Y id, Y? userID = null, bool isCommit = true, bool checkAuthorize = false);
        Task<G> GetByID(Y id, Y? userID = null, bool isDeleted = false);
        Task<IList<AutoCompleteListVM<Y>>> AutoCompleteList(Y? id = null, string text = null);
    }
}
