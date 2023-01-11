using System;
namespace Test1.Models
{
	public class PasswordUpdateRequest
	{
		public PasswordUpdateRequest()
		{
		}

		public int UserId { get; set; }
        public string Password { get; set; }

    }
}

