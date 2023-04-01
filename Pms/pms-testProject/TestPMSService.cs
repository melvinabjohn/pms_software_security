using System;
using Microsoft.EntityFrameworkCore;
using Test1.Models;
using static pms_testProject.MockData.DataFixture<Moq.Mock>;
using Test1.Services;
using pms_testProject.MockData;
using Microsoft.Extensions.Configuration;
using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace pms_testProject
{
    public class TestPMSService
    {
        private readonly PMSService pmsService1;
        private DbContextOptions<PMSDbContext> dbContextOptions;
        private PMSDbContext dbContextOptions1;

        //private DbContextOptions<PMSDbContext> dbContextOptions1;
        
        private IConfigurationRoot _configuration;

        public TestPMSService()
        {
            #region old
            //var builder = new ConfigurationBuilder()
            //  .SetBasePath(Directory.GetCurrentDirectory())
            //  .AddJsonFile("appsettings.json");
            //_configuration = builder.Build();
            //dbContextOptions = new DbContextOptionsBuilder<PMSDbContext>()
            //    .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
            //    .Options;
            #endregion 

            var dbName = $"PMSDB";
            dbContextOptions = new DbContextOptionsBuilder<PMSDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            dbContextOptions1 = new PMSDbContext(dbContextOptions);
            pmsService1 = new PMSService(dbContextOptions1);
        }



        [Fact]
        public async Task ReadPolicyFromJson_Success_Test()
        {
            var policy = pmsService1.ReadPolicyFromJson();

            Assert.IsAssignableFrom<Policy?>(policy.Result);

        }

        [Fact]
        public async Task UpdateUserStatus_Success_Test()
        {

            UpdateUserStatusRequest request = MockDataReader.GetStubData<UpdateUserStatusRequest>("UpdateUserStatusRequest_Success");
            var res = pmsService1.UpdateUserStatus(request);

            Assert.True(res.Result == true);

        }

        [Fact]
        public async Task EncryptPassword_Returns_String()
        {

            var res = pmsService1.EncryptPassword("melvin");

            Assert.True("bWVsdmlu" == res);
        }

        
        //[Fact]
        //public async void UpdatePolicy_Returns_true()
        //{
        //    Policy policy = MockDataReader.GetStubData<Policy>("PolicyUpdate_Success");

        //    var res = pmsService1.UpdatePolicy(policy);

        //    Assert.True(true == res.Result);
        //}

        

        [Fact]
        public async void BulkGeneratePassword_Returns_Count()
        {
            
            var res = pmsService1.BulkGeneratePassword(5);

            Assert.True(5 == res.Count);
        }

        [Fact]
        public async void BulkGeneratePassword_Returns_DifferentCount()
        {

            var res = pmsService1.BulkGeneratePassword(6);

            Assert.False(5 == res.Count);
        }

        [Fact]
        public async void BulkGeneratePassword_Include_UpperAndLowerCase_Returns_Count()
        {
            Policy policy = MockDataReader.GetStubData<Policy>("Policy_Include_UpperAndLowerCase");

            await pmsService1.UpdatePolicy(policy);
            var res = pmsService1.BulkGeneratePassword(10);

            Assert.True(10 == res.Count);
            
        }

        [Fact]
        public async void BulkGeneratePassword_Include_DigitAndNonAlphaNumeric_Returns_Count()
        {
            Policy policy = MockDataReader.GetStubData<Policy>("Policy_Include_DigitAndNonAlphaNumericCase");

            await pmsService1.UpdatePolicy(policy);
            var res = pmsService1.BulkGeneratePassword(8);

            Assert.True(8 == res.Count);

        }

        //[Fact]
        //public async Task Login_ValidCredentials_ReturnsSuccessfulLogin()
        //{
        //    // Arrange
        //    var loginRequest = new LoginRequest
        //    {
        //        Email = "melvin@gmail.com",
        //        Password = "melvinasda"
        //    };

        //    // Act
        //    var result = await pmsService1.Login(loginRequest);

        //    // Assert
        //    Assert.True(result.loginStatus);
        //    Assert.NotEmpty(result.token);
        //}


        [Fact]
        public async void Login_Returns_False_And_EmptyToken()
        {
            LoginRequest login = MockDataReader.GetStubData<LoginRequest>("LoginRequest_Failure");

            var res = await pmsService1.Login(login);

            Assert.True(false == res.loginStatus);
            Assert.True("" == res.token);

        }


        [Fact]
        public void LeakedPasswordCheck_Returns_List()
        {

            var res = pmsService1.LeakedPasswordCheck("F796F");
            Assert.IsAssignableFrom<List<string>?>(res);
            
        }
    }
}

