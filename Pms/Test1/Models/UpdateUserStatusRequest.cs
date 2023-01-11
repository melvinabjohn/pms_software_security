using System;
namespace Test1.Models
{
	public class UpdateUserStatusRequest
	{
		public int UserId { get; set; }
		public bool isDisabled { get; set; }
	}
}

