using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Test1.Models;
using Test1.Services;

namespace Test1.Controllers;

[ApiController]
[Route("[controller]")]
public class PMSController : ControllerBase
{

    readonly IPMSService _pmsService;

    private readonly ILogger<PMSController> _logger;

    public PMSController(ILogger<PMSController> logger, IPMSService pMSService)
    {
        _logger = logger;
        _pmsService = pMSService;
    }


    /// <summary>
    /// Get the current policy Details
    /// </summary>
    /// <returns>Policy Class</returns>
    [HttpGet]
    [Route("policy")]
    [ProducesResponseType(typeof(Policy), 200)]
    public async Task<IActionResult> GetPolicy()
    {
        Policy source = new Policy();

        using (StreamReader r = new StreamReader("PasswordPolicy.json"))
        {
            string json = r.ReadToEnd();
            source = JsonSerializer.Deserialize<Policy>(json);
        }
        return Ok(source);
    }

    /// <summary>
    /// Update or Create a new policy
    /// </summary>
    /// <param name="policy"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("policy/update")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> UpdatePolicy(Policy policy)
    {
        string json = JsonSerializer.Serialize(policy);
        System.IO.File.WriteAllText("PasswordPolicy.json", json);

        return Ok(true);
    }

    [HttpDelete]
    [Route("policy/delete")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> DeletePolicy()
    {
        //string json = JsonSerializer.Serialize(null);
        //System.IO.File.WriteAllText("PasswordPolicy.json", json);
        System.IO.File.Delete("PasswordPolicy.json");
        return Ok(true);
    }


    [HttpPost]
    [Route("user/status")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> UpdateUserStatus(UpdateUserStatusRequest updateRequest)
    {
        
        var res = _pmsService.UpdateUserStatus(updateRequest);
        return Ok(res);
    }

    [HttpPost]
    [Route("access/request")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> RequestAccess(AccessRequest accessRequest)
    {
        
        var res = _pmsService.RequestAccess(accessRequest);
        return Ok(res);
    }

    [HttpGet]
    [Route("user/legacyapp/passwords")]
    [ProducesResponseType(typeof(LegacyApps), 200)]
    public async Task<IActionResult> GetLegacyAppPasswords(int userId)
    {

        LegacyApps res = _pmsService.GetLegacyAppPasswords(userId);
        return Ok(res);
    }


    [HttpPost]
    [Route("password/update")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> UpdatePassword(PasswordUpdateRequest passRequest)
    {

        bool res = _pmsService.UpdatePassword(passRequest);
        return Ok(res);
    }

    [HttpGet]
    [Route("password/generate/bulk")]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<IActionResult> BulkGeneratePassword(int NoOfPasswords)
    {

        List<string> res = _pmsService.BulkGeneratePassword(NoOfPasswords);
        return Ok(res);
    }

    [HttpGet]
    [Route("password/update/all")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> UpdatePasswords()
    {

        bool res = _pmsService.BulkAssignPasswords();
        return Ok(res);
    }

    [HttpGet]
    [Route("function/disable/user")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> DisableUsers()
    {

        bool res = _pmsService.DisableUsers();
        return Ok(res);
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(LoginResponse), 200)]
    public async Task<IActionResult> Login(LoginRequest loginrequest)
    {

        var res = _pmsService.Login(loginrequest);
        return Ok(res);
    }
}

