using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PMS;
using PMS.Models;
using Test1.Models;


namespace Test1.Services
{
	public class PMSService : IPMSService
    {
        private readonly PMSDbContext _context;

        public PMSService(PMSDbContext context)
		{
			_context = context;
		}


        public async Task<Policy> ReadPolicyFromJson()
        {

            Policy? policy = new();
            using (StreamReader r = new StreamReader("PasswordPolicy.json"))
            {
                string json = r.ReadToEnd();
                if (json is null)
                {
                    //policy = null;
                    return policy;
                }
                else
                {
                    policy = JsonSerializer.Deserialize<Policy>(json);
                    if (policy != null)
                    {
                        return policy;
                    }
                    else
                    {
                        throw new ArgumentNullException();
                    }
                }
                
            }
            //return policy != null ? policy : new Policy();

        }


		public async Task<bool> UpdateUserStatus(UpdateUserStatusRequest updateRequest)
		{
			var user = _context.UserDetails.FirstOrDefault(x => x.UserId.Equals(updateRequest.UserId));
			if(user != null)
			{
                user.IsDisabled = updateRequest.isDisabled;
            }
			await _context.SaveChangesAsync();
			return true;
		}

        public async Task<bool> RequestAccess(AccessRequest accessRequest)
        {
            var user = _context.UserDetails.FirstOrDefault(x => x.UserId.Equals(accessRequest.UserId));
            if (user != null)
            {
                //make notification call or mail trigger
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<LegacyApps> GetLegacyAppPasswords(int userId)
        {
            var legacyApps = _context.LegacyAppPassword.Where(x => x.UserId.Equals(userId)).ToList();
            var appIds = legacyApps.Select(x => x.AppId);
            var appData = _context.LegacyAppData.Where(x => appIds.Contains(x.AppId)).ToList();
            LegacyApps apps = new();
            List<LegacyAppDetail> appList = new();
            foreach (var app in legacyApps)
            {
                //decrypt the password
                var decryptedPass = DecryptPassword(app.Password);
                var appName = "";
                if(appData != null)
                {
                    var data = appData.FirstOrDefault(x => x.AppId == app.AppId);
                    appName = data != null? data.AppName : appName;
                };
                LegacyAppDetail application = new()
                {
                    AppId = app.AppId,
                    AppName = appName,
                    Password = decryptedPass
                };
                 
                appList.Add(application);
            }
            
            apps.legacyAppDetails = appList;
            return apps;
        }


        public async Task<bool> UpdatePassword(PasswordUpdateRequest passRequest)
        {
            var masterPasswordData = _context.MasterPassword.FirstOrDefault(x => x.UserId.Equals(passRequest.UserId));
            var hashedPass = EncryptPassword(passRequest.NewPassword);
            var oldPasshash = EncryptPassword(passRequest.OldPassword);
            if (hashedPass != null && masterPasswordData != null && masterPasswordData.Password == oldPasshash)
            {
                masterPasswordData.Password = hashedPass;
                masterPasswordData.LastResetTime = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        /// <summary>
        /// Password encryption method(base 64 encoding)
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string EncryptPassword(string? password)
        {
            try
            {
                string message = "";
                if (password != null)
                {
                    byte[] encoded = new byte[password.Length];
                    encoded = Encoding.UTF8.GetBytes(password);
                    message = Convert.ToBase64String(encoded);
                    return message;
                }
                else throw new NullReferenceException();

            }
            catch (NullReferenceException ex)
            {
                throw;
            }

        }
        /// <summary>
        /// password decruption
        /// </summary>
        /// <param name="password"></param>
        /// <returns>decrypted password as string</returns>
        public string DecryptPassword(string password)
        {
            byte[] data = Convert.FromBase64String(password);
            string message = Encoding.UTF8.GetString(data);
            return message;
        }

        public string GenerateHash(string password)
        {
            using var sha1 = SHA1.Create();
            //using var sha1 = SHA512.Create();
            return Convert.ToHexString(sha1.ComputeHash(Encoding.UTF8.GetBytes(password))).ToUpper();
        }

        /// <summary>
        /// Bulk Generation of Passwords
        /// </summary>
        /// <param name="noOfPasswords"></param>
        /// <returns></returns>
        public List<string> BulkGeneratePassword(int noOfPasswords)
        {           
            List<string> passwords = new();
            List<string> passwordsForCSV = new();
            Policy policy = ReadPolicyFromJson().Result;

            while (passwords.Count() < noOfPasswords)
            {
                #region old
                //bool isDigit = policy.IsDigitRequired;
                //bool isLower = policy.IsLowercaseRequired;
                //bool isUpper = policy.IsUppercaseRequired;
                //bool isNonAlphaNumeric = policy.IsNonAlphanumericRequired;
                //StringBuilder newPassword = new StringBuilder();
                //while (newPassword.Length < policy.RequiredLength)
                //{
                //    char c = (char)random.Next(32, 126);

                //    var flag = isDigit || isLower || isUpper || isNonAlphaNumeric;

                //    ///FLAG =FALSE WHEN all 4 are false


                //    if (!flag && char.IsDigit(c) && policy.IsDigitRequired) newPassword.Append(c);

                //    if (!flag && char.IsLower(c) && policy.IsLowercaseRequired) newPassword.Append(c);
                //    if (!flag && char.IsUpper(c) && policy.IsUppercaseRequired) newPassword.Append(c);
                //    if (!flag && !char.IsLetterOrDigit(c) && policy.IsNonAlphanumericRequired) newPassword.Append(c);


                //    if (char.IsDigit(c))

                //        if (isDigit)
                //        {
                //            newPassword.Append(c);
                //            isDigit = false;
                //        }
                //        else continue;

                //    else if (char.IsLower(c))
                //        if (isLower)
                //        {
                //            newPassword.Append(c);
                //            isLower = false;
                //        }
                //        else continue;

                //    else if (char.IsUpper(c))
                //        if (isUpper)
                //        {
                //            newPassword.Append(c);
                //            isUpper = false;
                //        }
                //        else continue;

                //    else if (!char.IsLetterOrDigit(c))
                //        if (isNonAlphaNumeric)
                //        {
                //            newPassword.Append(c);
                //            isNonAlphaNumeric = false;
                //        }
                //        else continue;



                #endregion
                int passwordLength = policy.RequiredLength;
                int uppercaseLength = policy.UppercasesRequired;
                int lowercaseLength = policy.LowercasesRequired;
                int specialLength = policy.NonAlphanumericsRequired;
                int digitLength = policy.DigitsRequired;

                string specialChars = "!@#$%^&*()_+-=[]{}|,./<>?;':\"";                

                string newPassword = "";

                Random random = new Random();
                //var random = RandomNumberGenerator.Create();

                for (int i = 0; i < uppercaseLength; i++)
                {
                    newPassword += (char)random.Next(65,91);
                }

                for (int i = 0; i < lowercaseLength; i++)
                {
                    newPassword += (char)random.Next(97, 123);
                }

                for (int i = 0; i < specialLength; i++)
                {
                    newPassword += specialChars[random.Next(0, specialChars.Length)];
                }

                for (int i = 0; i < digitLength; i++)
                {
                    newPassword += (char)random.Next(48, 58);
                }

                var remainingCharLen = passwordLength - (uppercaseLength + lowercaseLength + specialLength + digitLength);
                for (int i = 0; i < remainingCharLen; i++) 
                {
                    newPassword += (char)random.Next(48, 123);
                }

                newPassword = new string(newPassword.OrderBy(x => random.Next()).ToArray());

                var hashed = GenerateHash(newPassword.ToString());
                //var hashed = EncryptPassword(newPassword.ToString());

                var query = hashed.Substring(0, 5);
                var leakedPasswords = LeakedPasswordCheck(query);

                var newpassWordsList = new List<string>();
                foreach(var i in leakedPasswords)
                {
                    var hashAlone = i.Split(":")[0];
                    newpassWordsList.Add(query + hashAlone);
                }

                if (!leakedPasswords.Contains(hashed))
                {
                    //passwords.Add(hashed);
                    passwords.Add(EncryptPassword(newPassword.ToString()));
                    passwordsForCSV.Add(newPassword.ToString());
                }            
            }
            WriteToCsv(passwordsForCSV, "BulkGeneratedPasswords");
            return passwords;
        }


        public void WriteToCsv(List<string> passwords, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (string pass in passwords)
                {
                    writer.WriteLine(pass);
                }
            }
        }


        public List<string> LeakedPasswordCheck(string endUrl)
        {
            try
            {
                HttpClient client = new();
                var baseUrl = Constants.HaveIBeenPawnedUrl; ;
                HttpResponseMessage response = client.GetAsync(baseUrl + endUrl).Result;

                var leakedPasswordsString = "";
                if (response.IsSuccessStatusCode)
                {
                    leakedPasswordsString = response.Content.ReadAsStringAsync().Result;
                }
                var leakedPasswords = leakedPasswordsString.Split("\r\n").ToList();
                return leakedPasswords;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }


        public bool BulkAssignPasswords()
        {
            var legacyAppUsers = _context.LegacyAppPassword.ToList();
            var totalPasswordsRequired = legacyAppUsers.Count();
            var passwords = BulkGeneratePassword(totalPasswordsRequired);

            int i = 0;
            passwords.ForEach(x =>
            {
                legacyAppUsers[i].Password = x;
                legacyAppUsers[i].LastResetTime = DateTime.Now;
                i++;
            });
            _context.SaveChanges();
            return true;

        }

        public async Task<bool> UpdatePolicy(Policy policy)
        {
            try
            {
                policy.PolicyCreatedDate = DateTime.Now;
                string json = JsonSerializer.Serialize(policy);
                

                var userDetails = _context.UserDetails.FirstOrDefault(x => x.UserId == policy.AdminId);
                var isLoggedin = _context.MasterPassword.FirstOrDefault(x => x.UserId.Equals(policy.AdminId)
                && x.Token.Equals(policy.LoginToken));


                if(userDetails != null && userDetails.AdminAccess.Equals(true) && isLoggedin != null)
                {
                    File.WriteAllText("PasswordPolicy.json", json);
                    var response = _context.MasterPassword.ToList();
                    ///updating all user's password status as invalid with respect to the new policy
                    response.ForEach(x =>
                    {
                        x.IsPasswordvalid = false;
                    });
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// disable user means=> UserDetaails.IsDisabled = 1
        /// </summary>
        /// <returns></returns>
        public bool DisableUsers()
        {
            Policy policy = ReadPolicyFromJson().Result;

            var policyUpdatedDate = policy.PolicyCreatedDate;

            var usersToDisable =
                _context.MasterPassword
                .Where(x => x.LastResetTime <= policy.PolicyCreatedDate && policyUpdatedDate.AddDays(1) <= DateTime.Now)
                .ToList();

            var userIds = usersToDisable.Select(x => x.UserId);
            var users = _context.UserDetails.Where(x => userIds.Contains(x.UserId)).ToList();

            users.ForEach(x =>
            {
                x.IsDisabled = true;
            });
            _context.SaveChanges();
            return true;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            try
            {
                LoginResponse loginResponse = new() { loginStatus = false, token = "" };
                var user = _context.UserDetails.Where(x => x.Email == loginRequest.Email).FirstOrDefault();
                if(user != null)
                {
                    var masterPassword = _context.MasterPassword.FirstOrDefault(y => y.UserId == user.UserId);
                    if (masterPassword != null)
                    {
                        var loginPass = EncryptPassword(loginRequest.Password);

                        var authToken = GenerateToken();


                        if (loginPass.Equals(masterPassword.Password))
                        {
                            loginResponse.loginStatus = true;
                            loginResponse.token = authToken;
                            masterPassword.Token = authToken;
                            masterPassword.LastResetTime = DateTime.Now;
                            await _context.SaveChangesAsync();
                        }
                        return loginResponse;
                    }
                    
                }
                return loginResponse;

            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public string GenerateToken()
        {
            var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
            var random = new Random();
            var resultToken = new string(
               Enumerable.Repeat(allChar, 30)
               .Select(token => token[random.Next(token.Length)]).ToArray());

            return resultToken.ToString();
        }

        public async Task<LeakedPasswordCheckResponse> CheckPasswordLeak(PasswordLeakCheckRequest request)
        {
            LeakedPasswordCheckResponse data = new();
            List<PasswordCheckDetails> passwordDatas = new();
            foreach (var password in request.Passwords)
            {
                PasswordCheckDetails passwordData = new();
                var hashed = GenerateHash(password.ToString());
                var query = hashed.Substring(0, 5);
                var leakedPasswords = LeakedPasswordCheck(query);
                var isValidPassword = PolicyValidation(password);
                var newpassWordsList = new List<string>();
                foreach (var i in leakedPasswords)
                {
                    var hashAlone = i.Split(":")[0];
                    newpassWordsList.Add(query + hashAlone);
                }
                passwordData.Password = password;
                passwordData.IsPasswordLeaked = newpassWordsList.Contains(hashed) ? true : false;
                passwordData.IsPasswordValid = isValidPassword;
                passwordDatas.Add(passwordData);
                data.PasswordCheckDetail = passwordDatas;

            }

            return data;
        }

        public bool PolicyValidation(string password)
        {
            Policy policy = ReadPolicyFromJson().Result;
            int nonAlphanumericCount = 0;
            int digitCount = 0;
            int lowercaseCount = 0;
            int uppercaseCount = 0;

            foreach (char c in password)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    nonAlphanumericCount++;
                }
                else if (char.IsDigit(c))
                {
                    digitCount++;
                }
                else if (char.IsLower(c))
                {
                    lowercaseCount++;
                }
                else if (char.IsUpper(c))
                {
                    uppercaseCount++;
                }
            }

            return password.Length == policy.RequiredLength
                && nonAlphanumericCount >= policy.NonAlphanumericsRequired
                && digitCount >= policy.DigitsRequired
                && lowercaseCount >= policy.LowercasesRequired
                && uppercaseCount >= policy.UppercasesRequired;
        }

        public async Task<LegacyAppLoginResponse> LegacyAppLogin(LegacyAppLoginRequest legacyAppLoginRequest)
        {
            var legacyAppDetail = _context.LegacyAppPassword.FirstOrDefault(
                x => x.AppId.Equals(legacyAppLoginRequest.AppId) && x.UserId.Equals(legacyAppLoginRequest.UserId));
            
            LegacyAppLoginResponse response = new() { LoginStatus = false, Token = "" };
            if (legacyAppDetail != null)
            {
                var originalPassword = DecryptPassword(legacyAppDetail.Password);
                if (legacyAppLoginRequest.Password.Equals(originalPassword))
                {
                    var token = GenerateToken();
                    response.LoginStatus = true;
                    response.Token = token;
                    legacyAppDetail.LoginToken = token;
                    await _context.SaveChangesAsync();
                }
            }
            return response;
        }

        public async Task<string> CreateUser(CreateUserRequest user)
        {
            try
            {
                UserDetails newUser = new()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };
                _context.UserDetails.Add(newUser);
               
                await _context.SaveChangesAsync();
                var newUserJustCreated = _context.UserDetails.FirstOrDefault(x => x.Email.Equals(user.Email));
                if(newUserJustCreated != null)
                {
                    MasterPassword mp = new()
                    {
                        Password = EncryptPassword(user.Password),
                        UserId = newUserJustCreated.UserId,
                        LastResetTime = DateTime.Now,
                        IsPasswordvalid = true,
                        Token = ""
                    };
                    _context.MasterPassword.Add(mp);
                    await _context.SaveChangesAsync();
                    var passwords = BulkGeneratePassword(user.AppIds.Count);
                    int count = 0;

                    foreach (var appId in user.AppIds)
                    {
                        LegacyAppPassword legacyApp = new()
                        {
                            UserId = newUserJustCreated.UserId,
                            AppId = appId,
                            Password = passwords[count],
                            LastResetTime = DateTime.Now,
                            LoginToken = ""
                        };
                        _context.LegacyAppPassword.Add(legacyApp);
                        count++;

                    }
                    await _context.SaveChangesAsync();
                    return "user created";
                }
                return "User Creation failed";
                
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}

