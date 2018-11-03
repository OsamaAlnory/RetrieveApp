using System;
using System.Collections.Generic;
using System.Text;

namespace RetrieveApp.Database
{
    public class Binary
    {
        public Guests OWNER { get; set; }
        public Products PRODUCT { get; set; }
        public Admins ADMIN { get; set; }
        public int QUANTITY { get; set; }
    }
}
