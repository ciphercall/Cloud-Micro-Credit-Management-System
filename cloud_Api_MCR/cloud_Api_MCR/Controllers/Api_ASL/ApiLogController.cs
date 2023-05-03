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
    public class ApiLogController : ApiController
    {
        private DatabaseDbContext db;

        public ApiLogController()
        {
            db = new DatabaseDbContext();
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Log/SaveLog")]
        public object SaveLog([FromBody]ViewAslLog passModel)
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
                    AslLog aslLog = new AslLog();
                    //TimeZoneInfo timeZoneInfo;
                    //timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
                    //DateTime printDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                    var date = Convert.ToString(UserlogAddress.Timezone(DateTime.Now).ToString("dd-MMM-yyyy"));
                    var time = Convert.ToString(UserlogAddress.Timezone(DateTime.Now).ToString("hh:mm:ss tt"));

                    Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.Compid == passModel.Compid && n.Userid == passModel.Userid select n.Logslno).Max());
                    if (maxSerialNo == 0)
                    {
                        aslLog.Logslno = Convert.ToInt64("1");
                    }
                    else
                    {
                        aslLog.Logslno = maxSerialNo + 1;
                    }

                    aslLog.Compid = Convert.ToInt64(passModel.Compid);
                    aslLog.Userid = Convert.ToInt64(passModel.Userid);
                    aslLog.Logtype = passModel.Logtype;
                    aslLog.Logslno = aslLog.Logslno;
                    aslLog.Logdate = Convert.ToDateTime(date);
                    aslLog.Logtime = Convert.ToString(time);
                    aslLog.Logipno = UserlogAddress.IpAddress();
                    aslLog.Logltude = passModel.Logltude;
                    aslLog.Logdata = passModel.Logdata;
                    aslLog.Tableid = passModel.Tableid;
                    aslLog.Userpc = UserlogAddress.UserPc();
                    db.AslLogDbSet.Add(aslLog);
                    db.SaveChanges();

                    return new
                    {
                        data = passModel.Logdata,
                        success = true,
                        message = "Save data Successfully."
                    };
                }
                catch (Exception ex)
                {
                    return new
                    {
                        data = ex,
                        success = false,
                        message = "Save failed!"
                    };
                }
            }
            return new
            {
                success = false,
                message = "Authorized not permitted."
            };
        }





        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Log/DeleteLog")]
        public object DeleteLog([FromBody]ViewAslDelete passModel)
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
                    AslDelete aslDeleteLog = new AslDelete();
                    //TimeZoneInfo timeZoneInfo;
                    //timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
                    //DateTime printDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                    var date = Convert.ToString(UserlogAddress.Timezone(DateTime.Now).ToString("dd-MMM-yyyy"));
                    var time = Convert.ToString(UserlogAddress.Timezone(DateTime.Now).ToString("hh:mm:ss tt"));

                    Int64 maxSerialNo = Convert.ToInt64((from n in db.AslDeleteDbSet where n.Compid == passModel.Compid && n.Userid == passModel.Userid select n.Delslno).Max());
                    if (maxSerialNo == 0)
                    {
                        aslDeleteLog.Delslno = Convert.ToInt64("1");
                    }
                    else
                    {
                        aslDeleteLog.Delslno = maxSerialNo + 1;
                    }

                    aslDeleteLog.Compid = Convert.ToInt64(passModel.Compid);
                    aslDeleteLog.Userid = Convert.ToInt64(passModel.Userid);
                    aslDeleteLog.Delslno = aslDeleteLog.Delslno;
                    aslDeleteLog.Deldate = Convert.ToDateTime(date);
                    aslDeleteLog.Deltime = Convert.ToString(time);
                    aslDeleteLog.Delipno = UserlogAddress.IpAddress();
                    aslDeleteLog.Delltude = passModel.Delltude;
                    aslDeleteLog.Deldata = passModel.Deldata;
                    aslDeleteLog.Tableid = passModel.Tableid;
                    aslDeleteLog.Userpc = UserlogAddress.UserPc();
                    db.AslDeleteDbSet.Add(aslDeleteLog);
                    db.SaveChanges();

                    return new
                    {
                        data = passModel.Deldata,
                        success = true,
                        message = "Save data Successfully."
                    };
                }
                catch (Exception ex)
                {
                    return new
                    {
                        data = ex,
                        success = false,
                        message = "Save failed!"
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
        [System.Web.Http.Route("api/Log/UserLogDataList")]
        public object UserLogDataList(Int64 userCompanycode, Int64 usercode, Int64 selectCompanycode, Int64 selectUsercode, string fromDate , string toDate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            List<AslLog> nulllist = new List<AslLog>();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                DateTime fdate= Convert.ToDateTime(fromDate);
                DateTime tdate = Convert.ToDateTime(toDate);

                List<AslLog> list = new List<AslLog>();
                list = (from n in db.AslLogDbSet
                    where n.Userid == selectUsercode && n.Compid == selectCompanycode && n.Logdate >= fdate && n.Logdate <= tdate
                    select n).ToList();
                if (list.Count != 0)
                    return new
                    {
                        data = list,
                        success = true,
                        message = "Get data Successfully."
                    };
                else return new
                {
                    data = nulllist,
                    success = true,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = nulllist,
                success = false,
                message = "Authorized not permitted."
            };
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
