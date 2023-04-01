using System;
namespace PMS.Models
{
    public class PasswordLeakCheckRequest
    {
        public PasswordLeakCheckRequest()
        {

        }
        public List<string> Passwords { get; set; }
    }
}

