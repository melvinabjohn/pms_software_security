using System;
using System.ComponentModel.DataAnnotations;

namespace Test1.Models
{

	public class Policy
	{
        [Required]
        public int AdminId { get; set; }
        [Required]
        public string LoginToken { get; set; }

        [Required]
        [Range(0, 100)]
        public int PolicyId { get; set; }
        public string PolicyName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PolicyCreatedDate { get; set; }
        [Required]
        public int RequiredLength { get; set; }
        [Required]
        public int NonAlphanumericsRequired { get; set; }
        [Required]
        public int DigitsRequired { get; set; }
        [Required]
        public int LowercasesRequired { get; set; }
        [Required]
        public int UppercasesRequired { get; set; }

    }

}

