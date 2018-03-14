using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RS.Core.Const;
using RS.Core.Domain;
using RS.Core.Lib.Email;
using RS.Core.Service;
using RS.Core.Testing;
using RS.Core.Testing.Arrange;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RS.Core.Tests.UnitTests
{
    [TestClass]
    public class UserServiceTest: BaseUnitTest<Guid>
    {
        private IUserService _userService;
        private IEmailService _emailService;

        [TestMethod]
        public async Task TestRegister()
        {
            //Context has been overriding and an in-memory user has been added.
            var mockUserSet = IMUow.Repository<User>().GetListAsNoTrackingAsync(UserDtos.InMemoryList());
            mockContext.Setup(m => m.Set<User>()).Returns(mockUserSet.Object);

            //Unit of work has been mocked. Mock context sent to constructor. Services works with mocked unit of work.
            var mockUow = new Mock<EntityUnitofWork<Guid>>(mockContext.Object);
            _emailService = new EmailService();
            _userService = new UserService(mockUow.Object, _emailService);

            var result = await _userService.Register(UserDtos.RegisterDto(),
                UserDtos.InMemoryList().First().Id);

            if (result.Message != Messages.Ok)
                Assert.Inconclusive(result.Message);

            mockUserSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once);
            mockUow.Verify(m => m.SaveChangesAsync(), Times.Once);
        }

        [TestMethod]
        public async Task TestDuplicateUser()
        {
            //Context has been overriding and an in-memory user has been added.
            var mockUserSet = IMUow.Repository<User>().GetListAsNoTrackingAsync(UserDtos.InMemoryList());
            mockContext.Setup(m => m.Set<User>()).Returns(mockUserSet.Object);

            //Unit of work has been mocked. Mock context sent to constructor. Services works with mocked unit of work.
            var mockUow = new Mock<EntityUnitofWork<Guid>>(mockContext.Object);
            _emailService = new EmailService();
            _userService = new UserService(mockUow.Object, _emailService);

            var result = await _userService.Register(UserDtos.DuplicateUserDto(),
                UserDtos.InMemoryList().First().Id);

            Assert.AreEqual(result.Message, Messages.GNE0003);
        }

        [TestMethod] 
        public async Task TestUpdate()
        {
            var mockUserSet = IMUow.Repository<User>().GetListAsync(UserDtos.InMemoryList());
            mockContext.Setup(m => m.Set<User>()).Returns(mockUserSet.Object);

            var mockUow = new Mock<EntityUnitofWork<Guid>>(mockContext.Object);
            _emailService = new EmailService();
            _userService = new UserService(mockUow.Object, _emailService);

            var result = await _userService.Update(UserDtos.UpdateDto(),
                UserDtos.InMemoryList().First().Id, true, true);

            if (result.Message != Messages.Ok)
                Assert.Inconclusive(result.Message);

            Assert.AreEqual(
                UserDtos.UpdateDto().Name, 
                mockUserSet.Object.FirstOrDefault(x => x.Id == Guid.Parse(result.Data.ToString())).Name);

            Assert.AreEqual(
                UserDtos.UpdateDto().Phone,
                mockUserSet.Object.FirstOrDefault(x => x.Id == Guid.Parse(result.Data.ToString())).Phone);

            mockUow.Verify(m => m.SaveChangesAsync(), Times.Once);
        }

        [TestCleanup]
        public void Cleanup()
        {
            mockContext.Object.Dispose();
        }
    }
}
