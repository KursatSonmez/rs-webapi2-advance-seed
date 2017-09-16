using Moq;
using RS.Core.Data;
using RS.Core.Service.AutoMapper;

namespace RS.Core.Testing
{
    public class BaseUnitTest<Y>
        where Y:struct
    {
        protected IInMemoryUnitofWork<Y> IMUow;
        protected Mock<RSCoreDBContext> mockContext;

        public BaseUnitTest()
        {
            AutoMapperConfig<Y>.Configure();
            mockContext = new Mock<RSCoreDBContext>();
            IMUow = new InMemoryUnitofWork<Y>();
        }
    }
   
}
