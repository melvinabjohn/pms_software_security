using System;
namespace PMS.Models
{
	public class CreateUserRequest
	{
		public CreateUserRequest()
		{
		}

		public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
		public List<int> AppIds { get; set; }

    }
}

