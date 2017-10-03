using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RS.Core.Const;
using RS.Core.Domain;
using RS.Core.Service;
using RS.Core.Testing;
using RS.Core.Testing.Arrange;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace RS.Core.Tests.UnitTests
{
    [TestClass]
    public class AutoCodeServiceTest:BaseUnitTest<Guid>
    {
        private IAutoCodeService _autoCodeService;
        private IAutoCodeLogService _autoCodeLogService;

        [TestMethod]
        public async Task TestAutoCodeGenerate()
        {
            //Context has been overriding and an in-memory autocode data has been added.
            var mockAutoCodeSet = IMUow.Repository<AutoCode>().GetListAsync(AutoCodeDtos.InMemoryList());
            mockContext.Setup(m => m.Set<AutoCode>()).Returns(mockAutoCodeSet.Object);

            //Context has been overriding and add mocked AutoCodeLog DbSet.
            var mockAutoCodeLogSet = new Mock<DbSet<AutoCodeLog>>();
            mockContext.Setup(m => m.Set<AutoCodeLog>()).Returns(mockAutoCodeLogSet.Object);

            //Unit of work has been mocked. Mock context sent to constructor. Services works with mocked unit of work.
            var mockUow = new Mock<EntityUnitofWork<Guid>>(mockContext.Object);
            _autoCodeLogService = new AutoCodeLogService(mockUow.Object);
            _autoCodeService = new AutoCodeService(mockUow.Object, _autoCodeLogService);

            var result = await _autoCodeService.AutoCodeGenerate(
                AutoCodeDtos.ScreenCodeForAutomaticCodeGenerationTest,UserDtos.InMemoryList().First().ID);

            //Is the expected code the same as the generated code?
            Assert.AreEqual("CF-143-UT", result);

            //Has the last code number increased?
            Assert.AreEqual(
                143,
                mockAutoCodeSet.Object.
                    FirstOrDefault(x => x.ScreenCode == AutoCodeDtos.ScreenCodeForAutomaticCodeGenerationTest).LastCodeNumber);

            mockAutoCodeLogSet.Verify(m => m.Add(It.IsAny<AutoCodeLog>()), Times.Once);
            mockUow.Verify(m => m.SaveChangesAsync(), Times.Once);
        }

        [TestMethod]
        public void TestCheckCodeFormat()
        {
            var mockUow = new Mock<EntityUnitofWork<Guid>>(mockContext.Object);
            _autoCodeLogService = new AutoCodeLogService(mockUow.Object);
            AutoCodeService _autoCodeService = new AutoCodeService(mockUow.Object, _autoCodeLogService);

            var incorrectCodeFormat = _autoCodeService.CheckCodeFormat(AutoCodeDtos.IncorrectCodeFormat);
            var correctCodeFormat = _autoCodeService.CheckCodeFormat(AutoCodeDtos.CorrectCodeFormat);

            Assert.IsFalse(incorrectCodeFormat);
            Assert.IsTrue(correctCodeFormat);
        }

        [TestMethod]
        public void TestCheckScreenCode()
        {
            var mockUow = new Mock<EntityUnitofWork<Guid>>(mockContext.Object);
            _autoCodeLogService = new AutoCodeLogService(mockUow.Object);
            AutoCodeService _autoCodeService = new AutoCodeService(mockUow.Object, _autoCodeLogService);

            var incorrectScreenCode = _autoCodeService.CheckScreenCode(AutoCodeDtos.IncorrectScreenCode);
            var correctScrennCode = _autoCodeService.CheckScreenCode(ScreenCodes.TSC01);

            Assert.IsFalse(incorrectScreenCode);
            Assert.IsTrue(correctScrennCode);
        }

        [TestCleanup]
        public void Cleanup()
        {
            mockContext.Object.Dispose();
        }
    }
}
