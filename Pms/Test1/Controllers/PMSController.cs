using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using PMS.Models;
using PMS.Services;
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
        //ModelState.IsValid;
        try
        {
            var policy = await _pmsService.ReadPolicyFromJson();
            return Ok(policy);
        }
        catch (Exception)
        {
            return StatusCode(HttpContext.Response.StatusCode, "Internal server error");
        }
        
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
        try
        {
            var res = await _pmsService.UpdatePolicy(policy);
            Validator.PolicyValidator(policy);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return StatusCode(HttpContext.Response.StatusCode, ex.Message);
        }
       
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
        try
        {
            System.IO.File.Delete("PasswordPolicy.json");
            return Ok(true);
        }
        catch (Exception)
        {
            return StatusCode(HttpContext.Response.StatusCode, "Internal server error");
        }
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

        try
        {
            Validator.UpdateUserValidate(updateRequest);
            var res = await _pmsService.UpdateUserStatus(updateRequest);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return StatusCode(HttpContext.Response.StatusCode, ex.Message);
        }
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

        try
        {
            Validator.AccessRequestValidate(accessRequest);
            var res = await _pmsService.RequestAccess(accessRequest);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return StatusCode(HttpContext.Response.StatusCode, ex.Message);
        }
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

        try
        {
            Validator.UserIdValidate(userId);
            LegacyApps res = await _pmsService.GetLegacyAppPasswords(userId);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return StatusCode(HttpContext.Response.StatusCode, ex.Message);
        }
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

        try
        {
            Validator.UpdatePasswordRequestValidate(passRequest);
            bool res = await _pmsService.UpdatePassword(passRequest);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return StatusCode(HttpContext.Response.StatusCode, ex.Message);
        }
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

        try
        {
            Validator.ValidateNoOfPasswords(NoOfPasswords);
            List<string> res = _pmsService.BulkGeneratePassword(NoOfPasswords);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return StatusCode(HttpContext.Response.StatusCode, ex.Message);
        }
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

        try
        {
            bool res = _pmsService.BulkAssignPasswords();
            return Ok(res);
        }
        catch (Exception)
        {
            return StatusCode(HttpContext.Response.StatusCode, "Internal server error");
        }
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

        try
        {
            bool res = _pmsService.DisableUsers();
            return Ok(res);
        }
        catch (Exception)
        {
            return StatusCode(HttpContext.Response.StatusCode, "Internal server error");
        }
    }

    /// <summary>
    /// Users can Login to the PMS system inorder to view the legacy app credentials.
    /// </summary>
    /// <param name="loginrequest"></param>
    /// <returns>Login status along with a valid token</returns>
    [HttpPost]
    [Route("master/login")]
    [ProducesResponseType(typeof(LoginResponse), 200)]
    public async Task<IActionResult> Login(LoginRequest loginrequest)
    {
        try
        {
            Validator.ValidateMasterLoginRequest(loginrequest);
            var res = await _pmsService.Login(loginrequest);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return StatusCode(HttpContext.Response.StatusCode, ex.Message);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="passwordLeakCheckRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("password/leak")]
    [ProducesResponseType(typeof(LeakedPasswordCheckResponse), 200)]
    public async Task<IActionResult> CheckPasswordLeak(PasswordLeakCheckRequest passwordLeakCheckRequest)
    {
        try
        {
            Validator.ValidateCheckPasswordLeak(passwordLeakCheckRequest);
            var res = await _pmsService.CheckPasswordLeak(passwordLeakCheckRequest);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return StatusCode(HttpContext.Response.StatusCode, ex.Message);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="legacyAppLoginRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("legacyapp/login")]
    [ProducesResponseType(typeof(LegacyAppLoginResponse), 200)]
    public async Task<IActionResult> LegacyAppLogin(LegacyAppLoginRequest legacyAppLoginRequest)
    {
        try
        {
            Validator.ValidateLegacyAppLoginRequest(legacyAppLoginRequest);
            var res = await _pmsService.LegacyAppLogin(legacyAppLoginRequest);
            return Ok(res);
        }
        catch (Exception)
        {
            return StatusCode(HttpContext.Response.StatusCode, "Internal server error");
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="policy"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("user/create")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> CreateUser(CreateUserRequest user)
    {
        try
        {
            Validator.ValidateUser(user);
            var res = await _pmsService.CreateUser(user);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return StatusCode(HttpContext.Response.StatusCode, ex.Message);
        }

    }
}

