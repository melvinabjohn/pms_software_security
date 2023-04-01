using System;
using PMS.Models;
using Test1.Models;

namespace PMS.Services
{
	public static class Validator
	{

		public static void PolicyValidator(Policy policy)
		{
            if (policy.AdminId <= 0)
            {
                throw new Exception("adminId must be a positive integer.");
            }
            if (string.IsNullOrEmpty(policy.LoginToken))
            {
                throw new Exception("loginToken cannot be empty.");
            }
            if (policy.PolicyId < 0)
            {
                throw new Exception("policyId must be a non-negative integer.");
            }
            if (string.IsNullOrEmpty(policy.PolicyName))
            {
                throw new Exception("policyName cannot be empty.");
            }
            if (policy.RequiredLength < 0)
            {
                throw new Exception("requiredLength must be a non-negative integer.");
            }
            if (policy.UppercasesRequired < 0)
            {
                throw new Exception("UppercasesRequired must be a non-negative integer.");
            }
            if (policy.LowercasesRequired < 0)
            {
                throw new Exception("LowercasesRequired must be a non-negative integer.");
            }
            if (policy.DigitsRequired < 0)
            {
                throw new Exception("DigitsRequired must be a non-negative integer.");
            }
            if (policy.NonAlphanumericsRequired < 0)
            {
                throw new Exception("NonAlphanumericsRequired must be a non-negative integer.");
            }
            if (policy.RequiredLength < (policy.NonAlphanumericsRequired + policy.DigitsRequired
                + policy.LowercasesRequired + policy.UppercasesRequired))
            {
                throw new Exception("Total Required Length must be greater than the sum of all individual requirements.");
            }
        }

        public static void UpdateUserValidate(UpdateUserStatusRequest updateUserStatusRequest)
        {
            if (updateUserStatusRequest.UserId <= 0)
            {
                throw new Exception("UserId must be a positive integer.");
            }
        }

        public static void AccessRequestValidate(AccessRequest request)
        {
            if (request.UserId <= 0)
            {
                throw new Exception("userId must be a positive integer.");
            }

            if (string.IsNullOrEmpty(request.Email) || !IsValidEmail(request.Email))
            {
                throw new Exception("email is invalid.");
            }

            if (string.IsNullOrEmpty(request.RequestMessage))
            {
                throw new Exception("requestMessage cannot be empty.");
            }
        }

        public static void UserIdValidate(int userId)
        {
            if (userId <= 0)
            {
                throw new Exception("userId must be a positive integer.");
            }
        }

        public static void UpdatePasswordRequestValidate(PasswordUpdateRequest passRequest)
        {
            if (passRequest.UserId <= 0)
            {
                throw new Exception("userId must be a positive integer.");
            }

            if (string.IsNullOrEmpty(passRequest.NewPassword))
            {
                throw new Exception("newPassword cannot be empty.");
            }

            if (string.IsNullOrEmpty(passRequest.OldPassword))
            {
                throw new Exception("oldPassword cannot be empty.");
            }

        }

        static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static void ValidateNoOfPasswords(int NoOfPasswords)
        {
            if (NoOfPasswords <= 0)
            {
                throw new Exception("NoOfPasswords must be a positive integer.");
            }
        }

        public static void ValidateMasterLoginRequest(LoginRequest loginRequest)
        {
            if (!IsValidEmail(loginRequest.Email))
            {
                throw new Exception("Invalid email address.");
            }

            if (string.IsNullOrEmpty(loginRequest.Password))
            {
                throw new Exception("Password cannot be empty.");
            }
        }

        public static void ValidateCheckPasswordLeak(PasswordLeakCheckRequest passwordLeakCheckRequest)
        {
            foreach (string password in passwordLeakCheckRequest.Passwords)
            {
                if (string.IsNullOrEmpty(password))
                {
                    throw new Exception("Password cannot be empty.");
                }
            }
        }
        public static void ValidateLegacyAppLoginRequest(LegacyAppLoginRequest userPassword)
        {
            if (userPassword.AppId <= 0)
            {
                throw new Exception("appId must be a positive integer.");
            }

            if (userPassword.UserId <= 0)
            {
                throw new Exception("userId must be a positive integer.");
            }

            if (string.IsNullOrEmpty(userPassword.Password))
            {
                throw new Exception("Password cannot be empty.");
            }
        }
        public static void ValidateUser(CreateUserRequest request)
        {
            if (string.IsNullOrEmpty(request.FirstName))
            {
                throw new Exception("FirstName cannot be empty.");
            }
            if (string.IsNullOrEmpty(request.LastName))
            {
                throw new Exception("LastName cannot be empty.");
            }
            if (!IsValidEmail(request.Email))
            {
                throw new Exception("Invalid email address.");
            }
            if (string.IsNullOrEmpty(request.Password))
            {
                throw new Exception("password cannot be empty.");
            }
        }
    }
}

