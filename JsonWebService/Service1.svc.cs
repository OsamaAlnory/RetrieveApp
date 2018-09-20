using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;

namespace JsonWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        //Get data from Sql Server
        public List<WsAdmin> GetAllAdmins()
        {
            try
            {
                 RetrieveDataContext dc  = new RetrieveDataContext();
                List<WsAdmin> results = new List<WsAdmin>();
                foreach (AdminLogin cust in dc.AdminLogins)
                {
                    results.Add(new WsAdmin()
                    {
                        Username = cust.Username,
                        Password = cust.Password,
                        
                    });
                }
                return results;
            }
            catch (Exception ex)
            {
                //  Return any exception messages back to the Response header
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                return null;
            }
        }

         


    }
}
