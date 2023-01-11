using System;
namespace Test1.Models
{
	public class AccessRequest
	{
		public AccessRequest()
		{
		}
		public int UserId { get; set; }
        public string Email { get; set; }
        public string RequestMessage { get; set; }

    }
}


