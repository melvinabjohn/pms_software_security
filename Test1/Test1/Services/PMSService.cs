using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Test1.Models;


namespace Test1.Services
{
	public class PMSService : IPMSService
    {
        private readonly PMSDbContext _context;

        //private readonly IApplicationDbContext _context;

        //public PMSService(IApplicationDbContext context)
        //{
        //	_context = context;
        //}
        public PMSService(PMSDbContext context)
		{
			_context = context;
		}

		public bool UpdateUserStatus(UpdateUserStatusRequest updateRequest)
		{
			var user = _context.UserDetails.FirstOrDefault(x => x.UserId.Equals(updateRequest.UserId));
			if(user != null)
			{
                user.IsDisabled = updateRequest.isDisabled;
            }
			_context.SaveChangesAsync();
			return true;
		}

        public bool RequestAccess(AccessRequest accessRequest)
        {
            var user = _context.UserDetails.FirstOrDefault(x => x.UserId.Equals(accessRequest.UserId));
            if (user != null)
            {
                //make notification call or mail trigger
            }
            //_context.SaveChangesAsync();
            return true;
        }

        public LegacyApps GetLegacyAppPasswords(int userId)
        {
            var legacyApps = _context.LegacyAppPassword.Where(x => x.UserId.Equals(userId)).ToList();
            var appIds = legacyApps.Select(x => x.AppId);
            var appData = _context.LegacyAppData.Where(x=>appIds.Contains(x.AppId)).ToList();
            LegacyApps apps = new();
            List<LegacyAppDetail> appList = new();
            foreach (var app in legacyApps)
            {
                //decrypt the password
                var decryptedPass = DecryptPassword(app.Password);

                LegacyAppDetail application = new()
                {
                    AppId = app.AppId,
                    AppName = appData.FirstOrDefault(x => x.AppId == app.AppId).AppName,
                    Password = decryptedPass
                };

                appList.Add(application);
            }
            
            apps.legacyAppDetails = appList;
            return apps;
        }


        public bool UpdatePassword(PasswordUpdateRequest passRequest)
        {
            var masterPasswordData = _context.MasterPassword.FirstOrDefault(x => x.UserId.Equals(passRequest.UserId));
            var hashedPass = EncryptPassword(passRequest.Password);
            if (hashedPass != null)
            {
                masterPasswordData.Password = hashedPass;
                _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        /// <summary>
        /// Password encryption method
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string EncryptPassword(string password)
        {
            string message = "";
            byte[] encoded = new byte[password.Length];
            encoded = Encoding.UTF8.GetBytes(password);
            message = Convert.ToBase64String(encoded);
            return message;
        }

        public string DecryptPassword(string password)
        {
            byte[] data = Convert.FromBase64String(password);
            string message = Encoding.UTF8.GetString(data);
            return message;
        }

        public string GenerateHash(string password)
        {
            using var sha1 = SHA1.Create();
            return Convert.ToHexString(sha1.ComputeHash(Encoding.UTF8.GetBytes(password))).ToUpper();
        }

        /// <summary>
        /// Bulk Generate Passwords
        /// </summary>
        /// <param name="noOfPasswords"></param>
        /// <returns></returns>
        public List<string> BulkGeneratePassword(int noOfPasswords)
        {
            HttpClient client = new HttpClient();
            Policy policy = new Policy();
            List<string> passwords = new();

            using (StreamReader r = new StreamReader("PasswordPolicy.json"))
            {
                string json = r.ReadToEnd();
                policy = JsonSerializer.Deserialize<Policy>(json);
            }

            
            Random random = new Random();

            bool isDigit = policy.IsDigitRequired;
            bool isLower = policy.IsLowercaseRequired;
            bool isUpper = policy.IsUppercaseRequired;
            bool isNonAlphaNumeric = policy.IsNonAlphanumericRequired;

            while(passwords.Count() < noOfPasswords)
            {
                StringBuilder newPassword = new StringBuilder();
                while (newPassword.Length < policy.RequiredLength)
                {
                    char c = (char)random.Next(32, 126);

                    newPassword.Append(c);

                    if (char.IsDigit(c))
                        isDigit = false;
                    else if (char.IsLower(c))
                        isLower = false;
                    else if (char.IsUpper(c))
                        isUpper = false;
                    else if (!char.IsLetterOrDigit(c))
                        isNonAlphaNumeric = false;
                }

                if (isNonAlphaNumeric)
                    newPassword.Append((char)random.Next(33, 48));
                if (isDigit)
                    newPassword.Append((char)random.Next(48, 58));
                if (isLower)
                    newPassword.Append((char)random.Next(97, 123));
                if (isUpper)
                    newPassword.Append((char)random.Next(65, 91));

                var hashed = EncryptPassword(newPassword.ToString());
                var url = "https://api.pwnedpasswords.com/range/";
                var query = hashed.Substring(0,5);

                HttpResponseMessage response = client.GetAsync(url + query).Result;

                var leakedPasswords ="";
                if (response.IsSuccessStatusCode)
                {
                    leakedPasswords = response.Content.ReadAsStringAsync().Result;
                }
                var leakedPasswordsArray = leakedPasswords.Split("\r\n").ToList();
                var newpassWordsList = new List<string>();
                foreach(var i in leakedPasswordsArray)
                {
                    var hashAlone = i.Split(":")[0];
                    newpassWordsList.Add(query + hashAlone);
                }

                if (!leakedPasswordsArray.Contains(hashed))
                {
                    passwords.Add(hashed);
                }
                
            }

            return passwords;
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

        public bool UpdatePolicy(Policy policy)
        {
            policy.PolicyCreatedDate = DateTime.Now;
            string json = JsonSerializer.Serialize(policy);
            File.WriteAllText("PasswordPolicy.json", json);

            var response = _context.MasterPassword.ToList();
            response.ForEach(x =>
            {
                x.IsPasswordvalid = false;
            });
            return true;

        }

        /// <summary>
        /// disable user means=> UserDetaails.IsDisabled = 1
        /// </summary>
        /// <returns></returns>
        public bool DisableUsers()
        {
            Policy policy = new Policy();
            using (StreamReader r = new StreamReader("PasswordPolicy.json"))
            {
                string json = r.ReadToEnd();
                policy = JsonSerializer.Deserialize<Policy>(json);
            }
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

        public LoginResponse Login(LoginRequest loginRequest)
        {
            LoginResponse loginResponse = new() { loginStatus = false, token = "" };
            var user = _context.UserDetails.Where(x => x.Email == loginRequest.Email).FirstOrDefault();

            var masterPassword = _context.MasterPassword.FirstOrDefault(y => y.UserId == user.UserId);

            var loginPass = EncryptPassword(loginRequest.Password);

            var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
            var random = new Random();
            var resultToken = new string(
               Enumerable.Repeat(allChar, 30)
               .Select(token => token[random.Next(token.Length)]).ToArray());

            string authToken = resultToken.ToString();
            
            
            if (loginPass.Equals(masterPassword.Password))
            {
                loginResponse.loginStatus = true;
                loginResponse.token = resultToken;
                masterPassword.Token = authToken;
                masterPassword.LastResetTime = DateTime.Now;
                _context.SaveChanges();
            }
            return loginResponse;
        }
    }
}

