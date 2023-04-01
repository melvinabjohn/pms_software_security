using System;
using System.ComponentModel.DataAnnotations;

namespace Test1.Models
{
	public class LoginRequest
	{
		public LoginRequest()
		{

		}
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }

    }
}

