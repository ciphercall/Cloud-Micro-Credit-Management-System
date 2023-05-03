using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace cloud_Api_MCR.DataAccess.API
{
    public static class UserlogAddress
    {
        public static DateTime Timezone(DateTime datetime)
        {
            //Datetime formet
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            var PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            return PrintDate;
        }

        public static string IpAddress()
        {

            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            return ipAddress.ToString();
        }

        public static string UserPc()
        {
            return System.Net.Dns.GetHostName(); 
        }
    }
}