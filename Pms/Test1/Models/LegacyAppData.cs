using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test1.Models
{
    [Table("LegacyAppData")]
    public class LegacyAppData
	{
        public LegacyAppData()
		{

		}

        [Key]
        public int AppId { get; set; }
        public string AppName { get; set; }

    }
}

