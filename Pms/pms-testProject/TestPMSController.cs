using Microsoft.AspNetCore.Mvc;
using Test1.Controllers;
using static pms_testProject.MockData.DataFixture<Moq.Mock>;
using System.Net;
using Assert = Xunit.Assert;
using pms_testProject.MockData;
using Test1.Models;

namespace pms.Tests;

//[TestClass]
public class TestPMSController : IClassFixture<PMSServiceDataFixture>, IClassFixture<LoggerDataFixture>
{

    private readonly PMSController _controller;
    

    public TestPMSController(LoggerDataFixture logger, PMSServiceDataFixture PmsService)
    {
        _controller = new PMSController(logger.LogManagerMock.Object, PmsService.ServiceFacade.Object);
    }

    [Fact]
    public async void GetPolicy_ShouldReturn200()
    {
        //var mockRepo = new Mock<IPMSService>();
        var result = await _controller.GetPolicy() as OkObjectResult;
        
        Assert.True(result.StatusCode == (int)HttpStatusCode.OK);
    }

    [Fact]
    public async void UpdatePolicy_ShouldReturn200()
    {
        //PolicyUpdate_Success
        //var mockRepo = new Mock<IPMSService>();
        //Policy request = MockDataReader.GetStubData<Policy>("PolicyUpdate_Success");

        Policy pol = new()
        {
            PolicyId = 1,
            PolicyName = "asd",
            PolicyCreatedDate = DateTime.Now,
            DigitsRequired = 2,
            UppercasesRequired = 2,
            LowercasesRequired =2,
            NonAlphanumericsRequired =2,
            RequiredLength=8,
            LoginToken ="qwe",
            AdminId = 1
        };
        var result = await _controller.UpdatePolicy(pol) as OkObjectResult;

        Assert.True(result.StatusCode == (int)HttpStatusCode.OK);
    }

    ///Commenting the delete policy test because it will delete the existing policy and will reslut in failure of another test case
    ///
    //[Fact]
    //public async void DeletePolicy_ShouldReturn200()
    //{
    //    //var mockRepo = new Mock<IPMSService>();
    //    var result = await _controller.DeletePolicy() as OkObjectResult;

    //    Assert.True(result.StatusCode == (int)HttpStatusCode.OK);
    //}

    [Fact]
    public async void UpdateUserStatus_ShouldReturn200()
    {
        UpdateUserStatusRequest request = MockDataReader.GetStubData<UpdateUserStatusRequest>("UpdateUserStatusRequest_Success");

        var result = await _controller.UpdateUserStatus(request) as OkObjectResult;

        Assert.True(result.StatusCode == (int)HttpStatusCode.OK);
    }

    [Fact]
    public async void RequestAccess_ShouldReturn200()
    {
        AccessRequest request = MockDataReader.GetStubData<AccessRequest>("AccessRequest_Success");

        var result = await _controller.RequestAccess(request) as OkObjectResult;

        Assert.True(result.StatusCode == (int)HttpStatusCode.OK);
    }

    [Fact]
    public async void GetLegacyAppPasswordsReturn200()
    {
        var result = await _controller.GetLegacyAppPasswords(1) as OkObjectResult;

        Assert.True(result.StatusCode == (int)HttpStatusCode.OK);
    }

    [Fact]
    public async void UpdatePasswordReturn200()
    {
        PasswordUpdateRequest request = MockDataReader.GetStubData<PasswordUpdateRequest>("PasswordUpdateRequest_Success");
        var result = await _controller.UpdatePassword(request) as OkObjectResult;

        Assert.True(result.StatusCode == (int)HttpStatusCode.OK);
    }

    [Fact]
    public async void BulkGeneratePasswordReturn200()
    {
        var result = await _controller.BulkGeneratePassword(5) as OkObjectResult;

        Assert.True(result.StatusCode == (int)HttpStatusCode.OK);
    }

    [Fact]
    public async void UpdatePasswordsReturn200()
    {
        var result = await _controller.UpdatePasswords() as OkObjectResult;

        Assert.True(result.StatusCode == (int)HttpStatusCode.OK);
    }

    [Fact]
    public async void DisableUsersReturn200()
    {
        var result = await _controller.DisableUsers() as OkObjectResult;

        Assert.True(result.StatusCode == (int)HttpStatusCode.OK);
    }

    [Fact]
    public async void LoginReturn200()
    {
        LoginRequest request = MockDataReader.GetStubData<LoginRequest>("LoginRequest_Success");

        var result = await _controller.Login(request) as OkObjectResult;

        Assert.True(result.StatusCode == (int)HttpStatusCode.OK);
    }


}
