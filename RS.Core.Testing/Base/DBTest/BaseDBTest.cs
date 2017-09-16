using RS.Core.Data;
using RS.Core.Service;
using RS.Core.Service.AutoMapper;

namespace RS.Core.Testing
{
    public class BaseDBTest<Y>
        where Y:struct
    {
        protected RSCoreDBContext _con;
        protected EntityUnitofWork<Y> _uow;

        public BaseDBTest()
        {
            AutoMapperConfig<Y>.Configure();
            _con = new RSCoreDBContext();
            _uow = new EntityUnitofWork<Y>(_con);
        }
    }
}
