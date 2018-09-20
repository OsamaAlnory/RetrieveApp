using System;
using System.Collections.Generic;
using System.Text;

namespace RetrieveApp.Database
{
    public class Guests : Account
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
