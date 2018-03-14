using RS.Core.Service.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RS.Core.Service
{
    public interface ICRUDService<A, U, G, C, P, Y>
        where Y : struct
        where U : EntityUpdateDto<Y>
        where G : EntityGetDto<Y>
        where C : EntityGetDto<Y>
    {
        Task<APIResult> Add(A model, Y userID, bool isCommit = true);
        Task<APIResult> Update(U model, Y? userID = default(Y?), bool isCommit = true, bool checkAuthorize = false);
        Task<APIResult> Delete(Y id, Y? userID = default(Y?), bool isCommit = true, bool checkAuthorize = false);
        Task<C> GetByID(Y id, Y? userID = default(Y?), bool isDeleted = false);
        Task<IList<AutoCompleteListVM<Y>>> AutoCompleteList(P parameters,
            Y? id = default(Y?), string text = default(string));
        Task<IList<G>> Get(P parameters, string sortField, bool sortOrder);
        Task<EntityGetPagingDto<Y, G>> GetPaging(P parameters, string sortField, bool sortOrder,
           string sumField, int? first = null, int? rows = null);
    }
}
