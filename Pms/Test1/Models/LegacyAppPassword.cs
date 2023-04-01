using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test1.Models
{
    [Table("LegacyAppPassword")]
    public class LegacyAppPassword
	{
		public LegacyAppPassword()
		{
		}

        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int AppId { get; set; }
        public string Password { get; set; }
        public DateTime LastResetTime { get; set; }
        public string LoginToken { get; set; }

    }
}

