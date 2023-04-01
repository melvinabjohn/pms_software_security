using System;
namespace Test1.Models
{
	public class PasswordUpdateRequest
	{
		public PasswordUpdateRequest()
		{
		}

		public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }


    }
}

