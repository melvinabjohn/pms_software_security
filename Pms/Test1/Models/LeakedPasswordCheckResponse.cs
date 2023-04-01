using System;
namespace PMS.Models
{
	public class LeakedPasswordCheckResponse
	{
		public LeakedPasswordCheckResponse()
		{
		}
		public List<PasswordCheckDetails> PasswordCheckDetail { get; set; }
	}

	public class PasswordCheckDetails
	{
		public string? Password { get; set; }
        public bool IsPasswordLeaked { get; set; }
        public bool IsPasswordValid { get; set; }


    }
}

