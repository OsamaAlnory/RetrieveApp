using System;
using System.Collections.Generic;
using System.Text;

namespace RetrieveApp.Database
{
    public class Products
    {
        public int ID { get; set; }
        public string AdminID { get; set; }
        public string PName { get; set; }
        public float OldPrice { get; set; }
        public float NewPrice { get; set; }
        public int Quantity { get; set; }
        public TimeSpan ExpireTime { get; set; }
        public byte[] Image { get; set; }
        //public int Guest { get; set; }
        public string Description { get; set; }
    }
}
