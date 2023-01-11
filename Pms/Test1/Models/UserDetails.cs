using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Test1.Models
{
    [Table("UserDetails")]
    public class UserDetails
	{
		public UserDetails()
		{
		}
        [Key]
		public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDisabled { get; set; }
        public bool AdminAccess { get; set; }


    }
}

