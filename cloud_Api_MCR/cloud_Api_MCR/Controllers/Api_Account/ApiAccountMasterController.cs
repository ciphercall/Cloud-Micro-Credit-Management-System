using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.Account;
using cloud_Api_MCR.Models.MCR;

namespace cloud_Api_MCR.Controllers.Api_Account
{
    public class ApiAccountMasterController : ApiController
    {
        private DatabaseDbContext db;

        public ApiAccountMasterController()
        {
            db = new DatabaseDbContext();
        }



        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/AccountMaster/TotalList")]
        public object TotalList(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlMaster> list = new List<GlMaster>();
                list = (from m in db.GlMasterDbSet where m.Compid == userCompanycode select m).ToList();
                if (list.Count != 0)
                    return new
                    {
                        data = list,
                        success = true,
                        message = "Get data Successfully."
                    };
                else return new
                {
                    data = "",
                    success = false,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = "",
                success = false,
                message = "Authorized not permitted."
            };
        }




        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/AccountMaster/DebitCdWiseList")]
        public object DebitCdWiseList(Int64 userCompanycode, Int64 usercode, Int64 debitcd)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlMaster> list = new List<GlMaster>();
                list = (from m in db.GlMasterDbSet where m.Compid == userCompanycode && m.Debitcd == debitcd select m).ToList();
                if (list.Count != 0)
                    return new
                    {
                        data = list,
                        success = true,
                        message = "Get data Successfully."
                    };
                else return new
                {
                    data = "",
                    success = false,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = "",
                success = false,
                message = "Authorized not permitted."
            };
        }




        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/AccountMaster/DateWiseList")]
        public object DateWiseList(Int64 userCompanycode, Int64 usercode, String fromdate, String toDate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                DateTime fDate = Convert.ToDateTime(fromdate);
                DateTime tDate = Convert.ToDateTime(toDate);

                List<GlMaster> list = new List<GlMaster>();
                list = (from m in db.GlMasterDbSet where m.Compid == userCompanycode && fDate <= m.Transdt && m.Transdt <= tDate select m).ToList();
                if (list.Count != 0)
                    return new
                    {
                        data = list,
                        success = true,
                        message = "Get data Successfully."
                    };
                else return new
                {
                    data = "",
                    success = false,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = "",
                success = false,
                message = "Authorized not permitted."
            };
        }





        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/AccountMaster/LessThanFromDateWiseList")]
        public object LessThanFromDateWiseList(Int64 userCompanycode, Int64 usercode, String fromdate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                DateTime fDate = Convert.ToDateTime(fromdate);

                List<GlMaster> list = new List<GlMaster>();
                list = (from m in db.GlMasterDbSet where m.Compid == userCompanycode && m.Transdt < fDate select m).ToList();
                if (list.Count != 0)
                    return new
                    {
                        data = list,
                        success = true,
                        message = "Get data Successfully."
                    };
                else return new
                {
                    data = "",
                    success = false,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = "",
                success = false,
                message = "Authorized not permitted."
            };
        }



        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/AccountMaster/LessThanEqualtoToDateWiseList")]
        public object LessThanEqualtoToDateWiseList(Int64 userCompanycode, Int64 usercode, String todate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                DateTime tDate = Convert.ToDateTime(todate);

                List<GlMaster> list = new List<GlMaster>();
                list = (from m in db.GlMasterDbSet where m.Compid == userCompanycode && m.Transdt <= tDate select m).ToList();
                if (list.Count != 0)
                    return new
                    {
                        data = list,
                        success = true,
                        message = "Get data Successfully."
                    };
                else return new
                {
                    data = "",
                    success = false,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = "",
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
