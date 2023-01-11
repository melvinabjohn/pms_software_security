using System;
namespace Test1.Models
{

	public class Policy
	{

		public int PolicyId { get; set; }
        public string PolicyName { get; set; }

        public DateTime PolicyCreatedDate { get; set; }

        public int RequiredLength { get; set; }

        public bool IsNonAlphanumericRequired { get; set; }
        public bool IsDigitRequired { get; set; }
        public bool IsLowercaseRequired { get; set; }
        public bool IsUppercaseRequired { get; set; }

        //public int RequiredLength { get; set; }
        //public int RequiredLength { get; set; }

    }

}

