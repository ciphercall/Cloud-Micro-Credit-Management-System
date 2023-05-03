using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.Api
{
    public class UserTokenModel
    {
        public class TokenUserInfo
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string UserCreateDate { get; set; }
        }

        public class UserInformation
        {
            public string CompanyId { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string LoginId { get; set; }
        }


        public class DataWithToken
        {
            public string Token { get; set; }
            public UserInformation Data { get; set; }
            public bool Success { get; set; }
            public string Message { get; set; }
        }

    }
}