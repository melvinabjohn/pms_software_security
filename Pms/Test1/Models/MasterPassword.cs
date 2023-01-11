using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test1.Models
{
    [Table("MasterPassword")]
    public class MasterPassword
	{
		public MasterPassword()
		{

		}
		[Key]
		public int Id { get; set; }
		public int UserId { get; set; }
		public string Password { get; set; }
        public DateTime LastResetTime { get; set; }
        public bool IsPasswordvalid { get; set; }
        public string Token{ get; set; }

    }
}

