using System;
namespace PMS.Models
{
	public class LegacyAppLoginRequest
	{
		public LegacyAppLoginRequest()
		{
		}
		public int AppId { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }

    }
}

