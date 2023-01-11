using System;
using Test1.Models;

namespace Test1.Services
{
	public interface IPMSService
	{
        bool UpdateUserStatus(UpdateUserStatusRequest updateRequest);
        
        bool RequestAccess(AccessRequest updateRequest);

        LegacyApps GetLegacyAppPasswords(int userId);

        bool UpdatePassword(PasswordUpdateRequest passRequest);

        List<string> BulkGeneratePassword(int noOfPasswords);

        bool BulkAssignPasswords();

        bool UpdatePolicy(Policy policy);

        bool DisableUsers();

        LoginResponse Login(LoginRequest loginRequest);
    }
}

