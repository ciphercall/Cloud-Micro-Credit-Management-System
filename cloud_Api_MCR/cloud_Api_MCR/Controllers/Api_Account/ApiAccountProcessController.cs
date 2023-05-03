using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.DataAccess.API.AccountProcess;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.Account_DTO;
using cloud_Api_MCR.Models.ASL;

namespace cloud_Api_MCR.Controllers.Api_Account
{
    public class ApiAccountProcessController : ApiController
    {
        private DatabaseDbContext db;
        private Int64 _countProcess;

        public ApiAccountProcessController()
        {
            db = new DatabaseDbContext();
            _countProcess = 0;
        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/ApiAccountProcess/Process")]
        public object Process(Int64 userCompanycode, Int64 usercode, [FromBody] ViewGlMaster passModel)
        {
            try
            {
                var re = Request;
                var headers = re.Headers.Authorization.Parameter;
                string token = headers.ToString();

                if (TokenInfo.TokenCheck(userCompanycode, usercode, token) && passModel.Transdt != null)
                {
                    //GL Strans Process from DataAccess folder(class GlStransProcess)
                    string glStrans = "";
                    glStrans = GlStransProcess.Process(userCompanycode, usercode, passModel);
                    if (glStrans == "True")
                    {
                        _countProcess++;
                    }


                    //Create Log Data Save in to ASL_LOG Table
                    string username = "";
                    var userNameFind = (from n in db.AslUsercoDbSet where n.Compid == userCompanycode && n.Userid == usercode select n).ToList();
                    foreach (var name in userNameFind)
                    {
                        username = name.Usernm;
                    }
                    string convertDate = Convert.ToString(passModel.Transdt);
                    DateTime myDateTime = DateTime.Parse(convertDate);
                    string transDate = myDateTime.ToString("dd-MMM-yyyy");
                    String logdata = Convert.ToString("Process: Process to Account." + "\nTime: " + UserlogAddress.Timezone(DateTime.Now) + ",\nDate: " + transDate + ",\nUserName: " + username + ".");

                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "INSERT";
                    aslLog.Logltude = passModel.Insltude;
                    aslLog.Logdata = logdata;
                    aslLog.Tableid = "GL_MASTER PROCESS";
                    Log.SaveLog(aslLog);


                    if (_countProcess != 0)
                    {
                        return new
                        {
                            data = passModel,
                            success = true,
                            message = "Processing Done."
                        };
                    }
                    else
                    {
                        return new
                        {
                            data = passModel,
                            success = true,
                            message = "Process would not done."
                        };
                    }
                }
                return new
                {
                    success = false,
                    message = "Authorized not permitted."
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    data = ex,
                    success = false,
                    message = "Error."
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
