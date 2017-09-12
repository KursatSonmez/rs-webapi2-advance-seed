using RS.Core.Service.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RS.Core.Service
{
    public interface ICRUDService<A,U,G,Y>
        where Y : struct
        where U : EntityUpdateDto<Y>
        where G : EntityGetDto<Y>
    {
        Task<APIResult> Add(A model, Y userID, bool isCommit = true, Y? fooID = default(Y?));
        Task<APIResult> Update(U model, Y? userID = default(Y?), bool isCommit = true, bool checkAuthorize = false);
        Task<APIResult> Delete(Y id, Y? userID = default(Y?), bool isCommit = true, bool checkAuthorize = false);
        Task<G> GetByID(Y id, Y? userID = default(Y?), bool isDeleted = false);
        Task<IList<AutoCompleteListVM<Y>>> AutoCompleteList(Y? id = default(Y?), string text = default(string));
    }
}
