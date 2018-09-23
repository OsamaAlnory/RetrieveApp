using System;
using System.Collections.Generic;
using System.Text;

namespace RetrieveApp.Database
{
    public class Products
    {
        public string AdminID { get; set; }
        public string PName { get; set; }
        public float OldPrice { get; set; }
        public float NewPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpireTime { get; set; }
        public object Image { get; set; }
        public int Guest { get; set; }
        public string Description { get; set; }
    }
}
