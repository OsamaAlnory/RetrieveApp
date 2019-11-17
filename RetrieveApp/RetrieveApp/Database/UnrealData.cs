using System;
using System.Collections.Generic;
using System.Text;

namespace RetrieveApp.Database
{
    public class UnrealData
    {
        public static void Load()
        {
            DBActions.admins.Add(new Admins
            {
                ID = "DL1000",
                Password = "123456",
                Address = "Storgatan Lund",
                SName = "Lidl"
            });
            DBActions.guests.Add(new Guests { ID = 52, Name = "Noober", Password = "123456" });
        }
    }
}