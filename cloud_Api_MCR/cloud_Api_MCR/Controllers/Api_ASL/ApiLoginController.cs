using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.Api;
using cloud_Api_MCR.Models.ASL;
using cloud_Api_MCR.Models.ASL_DTO;

namespace cloud_Api_MCR.Controllers.Api_ASL
{
    public class ApiLoginController : ApiController
    {
        //// GET: api/ApiLogin
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/ApiLogin/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/ApiLogin
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/ApiLogin/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ApiLogin/5
        //public void Delete(int id)
        //{
        //}


        private DatabaseDbContext db;

        public ApiLoginController()
        {
            db = new DatabaseDbContext();
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Login/UserLogin")]
        public object UserLogin([FromBody]ViewLoginModel model)
        {
            List<String> nulllist = new List<String>();

            if (Login.UserIsValid(model.Loginid, model.Loginpw))
            {
                UserTokenModel.DataWithToken dataWithToken = new UserTokenModel.DataWithToken();
                dataWithToken.Data = TokenInfo.CollectUserInfo(model.Loginid, model.Loginpw);
                
                UserTokenModel.TokenUserInfo tokenUserInfo = new UserTokenModel.TokenUserInfo();
                tokenUserInfo.UserId = dataWithToken.Data.UserId;
                tokenUserInfo.UserName = dataWithToken.Data.UserName;
                tokenUserInfo.UserCreateDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);

                string token = TokenInfo.GenerateToken(tokenUserInfo);
                string tokendate = TokenInfo.TokenExpireDateIncrement(DateTime.Now, 1);
                //Generate token end
                var s = TokenInfo.SaveToken(dataWithToken.Data.CompanyId, dataWithToken.Data.UserId, token, tokendate, model.Loginpw);
                if (s)
                {
                    //Create Log Data Save in to ASL_LOG Table
                    Int64 companyCode= Convert.ToInt64(dataWithToken.Data.CompanyId);
                    Int64 userCode= Convert.ToInt64(dataWithToken.Data.UserId);
                    var findUserInfo =(from m in db.AslUsercoDbSet where m.Compid == companyCode && m.Userid == userCode select m).ToList();
                    foreach (var get in findUserInfo)
                    {
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = Convert.ToInt64(dataWithToken.Data.CompanyId);
                        aslLog.Userid = Convert.ToInt64(dataWithToken.Data.UserId);
                        aslLog.Logtype = "LOGIN";
                        aslLog.Logltude = model.Logltude;
                        aslLog.Logdata = Convert.ToString("User Name: " + get.Usernm + ",\nDepartment Name: " + get.Deptnm + ",\nOperation Type: " + get.Optp + ",\nAddress: " + get.Address + ",\nMobile No: " + get.Mobno + ",\nEmail ID: " + get.Emailid + ",\nLogin BY: " + get.Loginby + ",\nLogin ID: " + get.Loginid + ",\nTime From: " + get.Timefr + ",\nTime To: " + get.Timeto + ",\nStatus: " + get.Status + ".");
                        aslLog.Tableid = "ASL_USERCO";
                        Log.SaveLog(aslLog);
                    }
                 

                    return new
                    {
                        Token = token,
                        data = dataWithToken.Data,
                        success = true,
                        message = "Login Successfully."
                    };
                }
                else
                {
                    return new
                    {
                        Token = "",
                        data = nulllist,
                        success = false,
                        message = "Login failed."
                    };
                }
            }
            else
            {
                return new
                {
                    Token = "",
                    data = nulllist,
                    success = false,
                    message = "User id or Password not found."
                };
            }
        }

    }
}
