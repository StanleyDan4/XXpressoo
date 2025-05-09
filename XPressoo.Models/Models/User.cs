using System;
using System.Collections.Generic;
using System.Text;

namespace Xpressoo.Models
{
    public class User
    {
        public int UserId { get; set; } = 1 ;
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}
