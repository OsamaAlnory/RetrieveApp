﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace JsonWebService
{
    [DataContract]
    public class WsAdmin
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        
    }
}