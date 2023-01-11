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
        var policy = _pmsService.ReadPolicyFromJson();
        return Ok(policy);
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
        var res = _pmsService.UpdatePolicy(policy);

        return Ok(res);
    }

    /// <summary>
    /// An api to delete the existing policy.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// To update a user status to enable state. Only admin is authorised to access this api.
    /// </summary>
    /// <param name="updateRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("user/status")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> UpdateUserStatus(UpdateUserStatusRequest updateRequest)
    {
        
        var res = _pmsService.UpdateUserStatus(updateRequest);
        return Ok(res);
    }

    /// <summary>
    /// User can request an admin to enable his account if it gets disabled due to non adherence to new policy.
    /// </summary>
    /// <param name="accessRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("access/request")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> RequestAccess(AccessRequest accessRequest)
    {
        
        var res = _pmsService.RequestAccess(accessRequest);
        return Ok(res);
    }

    /// <summary>
    /// User can view legacy app details including passwords assigned by PMS system.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("user/legacyapp/passwords")]
    [ProducesResponseType(typeof(LegacyApps), 200)]
    public async Task<IActionResult> GetLegacyAppPasswords(int userId)
    {

        LegacyApps res = _pmsService.GetLegacyAppPasswords(userId);
        return Ok(res);
    }

    /// <summary>
    /// User can update master password. (PMS login password)
    /// </summary>
    /// <param name="passRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("password/update")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> UpdatePassword(PasswordUpdateRequest passRequest)
    {

        bool res = _pmsService.UpdatePassword(passRequest);
        return Ok(res);
    }

    /// <summary>
    /// PMS generates the required number of passwords as per the policy defined by admin.
    /// </summary>
    /// <param name="NoOfPasswords"></param>
    /// <returns>A list of passwords that are not leaked</returns>
    [HttpGet]
    [Route("password/generate/bulk")]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<IActionResult> BulkGeneratePassword(int NoOfPasswords)
    {

        List<string> res = _pmsService.BulkGeneratePassword(NoOfPasswords);
        return Ok(res);
    }

    /// <summary>
    /// Updates all the legacy app passwords of every user with compliance to the policy defined.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("password/update/all")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> UpdatePasswords()
    {

        bool res = _pmsService.BulkAssignPasswords();
        return Ok(res);
    }

    /// <summary>
    /// Disable all users who have not updated the master password even after 24 hours of updation of a policy.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("function/disable/user")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> DisableUsers()
    {

        bool res = _pmsService.DisableUsers();
        return Ok(res);
    }

    /// <summary>
    /// Users can Login to the PMS system inorder to view the legacy app credentials.
    /// </summary>
    /// <param name="loginrequest"></param>
    /// <returns>Login status along with a valid token</returns>
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(LoginResponse), 200)]
    public async Task<IActionResult> Login(LoginRequest loginrequest)
    {

        var res = _pmsService.Login(loginrequest);
        return Ok(res);
    }
}

