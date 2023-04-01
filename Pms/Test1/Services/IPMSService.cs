using System;
using PMS.Models;
using Test1.Models;

namespace Test1.Services
{
	public interface IPMSService
	{
        Task<Policy> ReadPolicyFromJson();

        Task<bool> UpdateUserStatus(UpdateUserStatusRequest updateRequest);
        
        Task<bool> RequestAccess(AccessRequest updateRequest);

        Task<LegacyApps> GetLegacyAppPasswords(int userId);

        Task<bool> UpdatePassword(PasswordUpdateRequest passRequest);

        List<string> BulkGeneratePassword(int noOfPasswords);

        bool BulkAssignPasswords();

        Task<bool> UpdatePolicy(Policy policy);

        bool DisableUsers();

        Task<LoginResponse> Login(LoginRequest loginRequest);

        string DecryptPassword(string str);

        string EncryptPassword(string str);

        Task<LeakedPasswordCheckResponse> CheckPasswordLeak(PasswordLeakCheckRequest request);
        Task<LegacyAppLoginResponse> LegacyAppLogin(LegacyAppLoginRequest legacyAppLoginRequest);

        Task<string>CreateUser (CreateUserRequest user);

    }
}

