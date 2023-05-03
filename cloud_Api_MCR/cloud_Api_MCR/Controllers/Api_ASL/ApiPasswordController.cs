using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.ASL;
using cloud_Api_MCR.Models.ASL_DTO;

namespace cloud_Api_MCR.Controllers.Api_ASL
{
    public class ApiPasswordController : ApiController
    {
        private DatabaseDbContext db;

        public ApiPasswordController()
        {
            db = new DatabaseDbContext();
        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Password/Update")]
        public object Update([FromBody]ViewPassword passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            Int64 companyCode = Convert.ToInt64(passModel.Compid);
            Int64 usercode = Convert.ToInt64(passModel.Userid);
            if (TokenInfo.TokenCheck(companyCode, usercode, token))
            {
                try
                {
                    var findUserPassword = (from m in db.AslUsercoDbSet where m.Compid == companyCode && m.Userid == usercode select m).ToList();
                    if (findUserPassword.Count == 1)
                    {
                        foreach (var get in findUserPassword)
                        {
                            get.Loginpw = passModel.NewPassword;
                        }
                        db.SaveChanges();
                    }
                    return new
                    {
                        data = passModel,
                        success = true,
                        message = "Your password successfully updated."
                    };
                }
                catch (Exception ex)
                {
                    return new
                    {
                        success = false,
                        message = ex
                    };
                }
            }
            return new
            {
                success = false,
                message = "Authorized not permitted."
            };
        }





        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Password/OldPasswordCheck")]
        public object OldPasswordCheck(Int64 userCompanycode, Int64 usercode, String oldPassword)
        {
            var findUserPassword = (from m in db.AslUsercoDbSet where m.Compid == userCompanycode && m.Userid == usercode && m.Loginpw == oldPassword select m).ToList();
            if (findUserPassword.Count == 1)
            {
                return new
                {
                    data = "1",
                    success = true,
                    message = "Your old password match!"
                };
            }
            else
            {
                return new
                {
                    data = "0",
                    success = false,
                    message = "Your old password does not match!"
                };
            }
        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
