using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.Account;
using cloud_Api_MCR.Models.Account_DTO;
using cloud_Api_MCR.Models.ASL;

namespace cloud_Api_MCR.Controllers.Api_Account
{
    public class ApiClosingBalanceController : ApiController
    {
        private DatabaseDbContext db;

        public ApiClosingBalanceController()
        {
            db = new DatabaseDbContext();
        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/ClosingBalance/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlMaster passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    GlMaster model = new GlMaster();

                    DateTime transdt = Convert.ToDateTime(passModel.Transdt);
                    string dateTimetoString = Convert.ToString(transdt.ToString("dd-MMM-yyyy"));
                    string getYear = dateTimetoString.Substring(9, 2);
                    string getMonth = dateTimetoString.Substring(3, 3).ToUpper();
                    string transMonth = getMonth + "-" + getYear;

                    string dateTimetoString1 = Convert.ToString(transdt.ToString("dd-MM-yyyy"));
                    string getyear = dateTimetoString1.Substring(6, 4);
                    string getmonth = dateTimetoString1.Substring(3, 2);
                    string fultransno = getyear + getmonth + "0001";

                    var result = db.GlMasterDbSet.Count(d => d.Compid == userCompanycode && d.Transtp == "OPEN" && d.Transmy == transMonth && d.Transdt == passModel.Transdt && d.Debitcd== passModel.Debitcd);
                    if (result == 0)
                    {
                        Int64 maxtransSerial = Convert.ToInt64((from m in db.GlMasterDbSet where m.Compid == userCompanycode && m.Transtp == "OPEN" && m.Transmy == transMonth && m.Transdt == passModel.Transdt select m.Transsl).DefaultIfEmpty().Max());
                        if (maxtransSerial == 0)
                        {
                            model.Transsl = Convert.ToInt64("50001");
                        }
                        else if (maxtransSerial != 0)
                        {
                            Int64 maxLimit = Convert.ToInt64("59999");
                            Int64 id = maxtransSerial + 1;
                            if (id <= maxLimit)
                            {
                                model.Transsl = id;
                            }
                            else
                            {
                                return new
                                {
                                    data = "",
                                    success = false,
                                    message = "Maximum Limit overloaded."
                                };
                            }
                        }
                        model.Compid = userCompanycode;
                        model.Transtp = "OPEN";
                        model.Transmy = transMonth;
                        model.Transno = Convert.ToInt64(fultransno);
                        model.Transsl = model.Transsl;
                        model.Transdt = Convert.ToDateTime(passModel.Transdt);
                        model.Debitcd = passModel.Debitcd;
                        model.Debitamt = passModel.Debitamt;
                        model.Creditamt = passModel.Creditamt;

                        model.Userpc = UserlogAddress.UserPc();
                        model.Insuserid = usercode;
                        model.Instime = UserlogAddress.Timezone(DateTime.Now);
                        model.Insipno = UserlogAddress.IpAddress();
                        model.Insltude = passModel.Insltude;
                        db.GlMasterDbSet.Add(model);
                        db.SaveChanges();

                        //For Log Data Logic
                        string accountname = "";
                        var getDebitName = (from m in db.GlAcchartDbSet
                                            where m.Compid == userCompanycode && m.Accountcd == model.Debitcd
                                            select new { m.Accountnm }).Distinct().ToList();
                        foreach (var get in getDebitName)
                        {
                            accountname = get.Accountnm;
                        }

                        string transDate = "";
                        if (passModel.Transdt != null)
                        {
                            string convertDate = Convert.ToString(passModel.Transdt);
                            DateTime dateTime = DateTime.Parse(convertDate);
                            transDate = dateTime.ToString("dd-MMM-yyyy");
                        }

                        String logData = Convert.ToString("Closing Balance Page." + ",\nDate: " + transDate + ",\nAccount Name: " + accountname + ",\nDebit Amount :" + model.Debitamt + ",\nCredit Amount" + model.Creditamt + ".");

                        //Create Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "CREATE";
                        aslLog.Logltude = passModel.Insltude;
                        aslLog.Logdata = logData;
                        aslLog.Tableid = "GL_MASTER";
                        Log.SaveLog(aslLog);

                        return new
                        {
                            data = model,
                            success = true,
                            message = "Save data Successfully."
                        };
                    }
                    else
                    {
                        return new
                        {
                            data = "",
                            success = true,
                            message = "This Account name already exists."
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new
                    {
                        data = "",
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





        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/ClosingBalance/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlMaster passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();
            
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    Int64 debitCd = 0;
                    var glMasterInfo = (from m in db.GlMasterDbSet where m.Compid == passModel.Compid && m.Transtp == passModel.Transtp && m.Transmy == passModel.Transmy && m.Transno == passModel.Transno && m.Transsl== passModel.Transsl select m).ToList();
                    if (glMasterInfo.Count == 1)
                    {
                        foreach (var get in glMasterInfo)
                        {
                            get.Debitamt = passModel.Debitamt;
                            get.Creditamt = passModel.Creditamt;

                            get.Userpc = UserlogAddress.UserPc();
                            get.Upduserid = usercode;
                            get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                            get.Updipno = UserlogAddress.IpAddress();
                            get.Updltude = passModel.Updltude;

                            debitCd = Convert.ToInt64(get.Debitcd);
                        }
                        db.SaveChanges();

                        //For Log Data Logic
                        string accountname = "";
                        var getDebitName = (from m in db.GlAcchartDbSet where m.Compid == userCompanycode && m.Accountcd == debitCd select new { m.Accountnm }).Distinct().ToList();
                        foreach (var get in getDebitName)
                        {
                            accountname = get.Accountnm;
                        }

                        string transDate = "";
                        if (passModel.Transdt != null)
                        {
                            string convertDate = Convert.ToString(passModel.Transdt);
                            DateTime dateTime = DateTime.Parse(convertDate);
                            transDate = dateTime.ToString("dd-MMM-yyyy");
                        }

                        String logData = Convert.ToString("Closing Balance Page." + ",\nDate: " + transDate + ",\nAccount Name: " + accountname + ",\nDebit Amount :" + passModel.Debitamt + ",\nCredit Amount" + passModel.Creditamt + ".");

                        //Update Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "UPDATE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = logData;
                        aslLog.Tableid = "GL_MASTER";
                        Log.SaveLog(aslLog);

                        return new
                        {
                            data = passModel,
                            success = true,
                            message = "Update data Successfully."
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new
                    {
                        data = ex,
                        success = false,
                        message = "No Data Found."
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
        [System.Web.Http.Route("api/ClosingBalance/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlMaster passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    GlMaster model = db.GlMasterDbSet.Find(passModel.Compid, passModel.Transtp, passModel.Transmy, passModel.Transno,passModel.Transsl);
                    if (model != null)
                    {
                        //For Log Data Logic
                        string accountname = "";
                        var getDebitName = (from m in db.GlAcchartDbSet where m.Compid == userCompanycode && m.Accountcd == model.Debitcd select new { m.Accountnm }).Distinct().ToList();
                        foreach (var get in getDebitName)
                        {
                            accountname = get.Accountnm;
                        }

                        string transDate = "";
                        if (passModel.Transdt != null)
                        {
                            string convertDate = Convert.ToString(passModel.Transdt);
                            DateTime dateTime = DateTime.Parse(convertDate);
                            transDate = dateTime.ToString("dd-MMM-yyyy");
                        }

                        String logData = Convert.ToString("Closing Balance Page." + ",\nDate: " + transDate + ",\nAccount Name: " + accountname + ",\nDebit Amount :" + passModel.Debitamt + ",\nCredit Amount" + passModel.Creditamt + ".");


                        //Delete Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "DELETE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = logData;
                        aslLog.Tableid = "GL_MASTER";
                        Log.SaveLog(aslLog);

                        //Delete Log Data Save in to ASL_DELETE Table
                        AslDelete aslDeleteLog = new AslDelete();
                        aslDeleteLog.Compid = userCompanycode;
                        aslDeleteLog.Userid = usercode;
                        aslDeleteLog.Delltude = passModel.Updltude;
                        aslDeleteLog.Deldata = logData;
                        aslDeleteLog.Tableid = "GL_MASTER";
                        Log.DeleteLog(aslDeleteLog);

                        //Delete From AslUserco
                        db.GlMasterDbSet.Remove(model);
                        db.SaveChanges();

                        return new
                        {
                            data = model,
                            success = true,
                            message = "Delete data Successfully."
                        };
                    }
                    else
                    {
                        return new
                        {
                            data = "",
                            success = false,
                            message = "No Data Found."
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new
                    {
                        data = ex,
                        success = false,
                        message = "No Data Found."
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
        [System.Web.Http.Route("api/ClosingBalance/List")]
        public object List(Int64 userCompanycode, Int64 usercode, DateTime transDate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<ViewGlMaster> list = new List<ViewGlMaster>();
                var getData = (from glMaster in db.GlMasterDbSet
                        join account in db.GlAcchartDbSet on glMaster.Compid equals account.Compid where glMaster.Compid == userCompanycode && account.Compid == userCompanycode &&
                        glMaster.Transdt == transDate && glMaster.Transtp == "OPEN"  && glMaster.Debitcd==account.Accountcd  select new
                        {
                            glMaster,
                            account,
                        }).ToList();
                foreach (var get in getData)
                {
                    ViewGlMaster model= new ViewGlMaster();
                    model.Compid = get.glMaster.Compid;
                    model.Transtp = get.glMaster.Transtp;
                    model.Transmy = get.glMaster.Transmy;
                    model.Transno = get.glMaster.Transno;
                    model.Transsl = get.glMaster.Transsl;
                    model.Transdt = get.glMaster.Transdt;
                    model.Debitcd = get.glMaster.Debitcd;
                    model.Debitamt = Convert.ToDecimal(get.glMaster.Debitamt);
                    model.Creditamt = Convert.ToDecimal(get.glMaster.Creditamt);
                    model.AccountName = get.account.Accountnm;
                    list.Add(model);
                }
                if (list.Count != 0)
                    return new
                    {
                        data = list.OrderBy(e=>e.AccountName).ToList(),
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
        [System.Web.Http.Route("api/ClosingBalance/DateWiseList")]
        public object DateWiseList(Int64 userCompanycode, Int64 usercode, DateTime transDate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlMaster> list = new List<GlMaster>();
                list = (from m in db.GlMasterDbSet where m.Compid == userCompanycode && m.Transdt == transDate select m).ToList();
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
        [System.Web.Http.Route("api/ClosingBalance/GetOpeningDate")]
        public object GetOpeningDate(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                String getDate = "";
                var list = (from m in db.GlMasterDbSet where m.Compid == userCompanycode && m.Transtp == "OPEN" select new {m.Compid,m.Transdt}).Distinct().ToList();
                foreach (var get in list)
                {
                    DateTime dateTime = Convert.ToDateTime(get.Transdt);
                    getDate = dateTime.ToString("dd-MMM-yyyy");
                }
                if (list.Count != 0)
                    return new
                    {
                        data = getDate,
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
        [System.Web.Http.Route("api/ClosingBalance/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, String transType, String transMonthYear, Int64 transno, Int64 transSerial)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlMaster> list = new List<GlMaster>();
                list = (from m in db.GlMasterDbSet where m.Compid == userCompanycode && m.Transtp == transType && m.Transmy == transMonthYear && m.Transno == transno && m.Transsl == transSerial select m).ToList();
                if (list.Count == 1)
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
