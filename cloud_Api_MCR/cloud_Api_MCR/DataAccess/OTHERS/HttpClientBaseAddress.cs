using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace cloud_Api_MCR.DataAccess.OTHERS
{
    public static class HttpClientBaseAddress
    {
        public static string BaseAddress()
        {
            //HttpClient setup 
            var client = new HttpClient();
            string baseUrl = System.Web.HttpContext.Current
                                        .Request
                                        .Url
                                        .GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped);
            client.BaseAddress = new Uri(baseUrl);
            String baseAddress = client.BaseAddress.ToString();
            return baseAddress;
        }
    }
}