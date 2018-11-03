using System;
using System.Collections.Generic;
using System.Text;

namespace RetrieveApp.Database
{
     public class Admins : Account
    {
        public string ID { get; set; }
        public string Password { get; set; }
        public string SName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Login { get; set; }
    }
}
