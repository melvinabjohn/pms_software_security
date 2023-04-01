using Microsoft.Extensions.Logging;
using Moq;
using Test1.Controllers;
using Test1.Models;
using Test1.Services;

namespace pms_testProject.MockData
{
    public partial class DataFixture<T> where T : Mock
    {
        protected T Mock { get; private set; }

        protected DataFixture(T mock)
        {
            Mock = mock;

        }
        ~DataFixture()
        {
            Mock.Reset();
        }

        public class LoggerDataFixture : DataFixture<Mock<ILogger<PMSController>>>
        {
            public LoggerDataFixture() : base(new Mock<ILogger<PMSController>>())
            {

            }
            public Mock<ILogger<PMSController>> LogManagerMock { get { return Mock; } }
        }

        public class PMSServiceDataFixture : DataFixture<Mock<IPMSService>>
        {
            public PMSServiceDataFixture() : base(new Mock<IPMSService>())
            {
            }
            public Mock<IPMSService> ServiceFacade { get { return Mock; } }
        }

        //public class PMSDbContextDataFixture : DataFixture<Mock<PMSDbContext>>
        //{
        //    public PMSDbContextDataFixture() : base(new Mock<PMSDbContext>())
        //    {
        //    }
        //    public Mock<PMSDbContext> DBFacade { get { return Mock; } }
        //}
    }
}
