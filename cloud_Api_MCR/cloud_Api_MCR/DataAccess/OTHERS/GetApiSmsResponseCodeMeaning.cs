using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.DataAccess.OTHERS
{
    public static class GetApiSmsResponseCodeMeaning
    {
        // Send SMS - sending sucessfully or not
        public static string SmsResponseCodeMeaning(string responseCode)
        {
            string code = responseCode.Substring(0, 4);
            switch (code)
            {
                case "1101":
                    return "Request was successful";
                case "1001":
                    return "Number Error.";
                case "1002":
                    return "Sender Name Error.";
                case "1003":
                    return "Massage Error.";
                case "1004":
                    return "Paramiter Error.";
                case "1005":
                    return "User Name or Password Error.";
                case "1006":
                    return "Account Balance Error.";
                case "1007":
                    return "Account Validity Error.";
                case "1008":
                    return "Operator Status Error.";
                case "1009":
                    return "Account Status Error.";
                default:
                    return "Internal Server Error.";
            }
        }
    }
}