using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class ApiSingleTransactionController : ApiController
    {

        private DatabaseDbContext db;

        public ApiSingleTransactionController()
        {
            db = new DatabaseDbContext();
        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/SingleTransaction/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlStrans passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    GlStrans model = new GlStrans();

                    string dateTimetoString = Convert.ToString(passModel.Transdt.ToString("dd-MMM-yyyy"));
                    string getYear = dateTimetoString.Substring(9, 2);
                    string getMonth = dateTimetoString.Substring(3, 3).ToUpper();
                    string transMonth = getMonth + "-" + getYear;

                    string dateTimetoString1 = Convert.ToString(passModel.Transdt.ToString("dd-MM-yyyy"));
                    string getyear = dateTimetoString1.Substring(6, 4);
                    string getmonth = dateTimetoString1.Substring(3, 2);
                    string halftransno = getyear + getmonth;

                    Int64 maxtransNo = Convert.ToInt64((from m in db.GlStransDbSet where m.Compid == userCompanycode && m.Transtp == passModel.Transtp && m.Transmy == transMonth select m.Transno).DefaultIfEmpty().Max());
                    if (maxtransNo == 0)
                    {
                        model.Transno = Convert.ToInt64(halftransno + "0001");
                    }
                    else if (maxtransNo != 0)
                    {
                        Int64 maxLimit = Convert.ToInt64(halftransno + "9999");
                        Int64 id = maxtransNo + 1;
                        if (id <= maxLimit)
                        {
                            model.Transno = id;
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
                    model.Transtp = passModel.Transtp;
                    model.Transmy = passModel.Transmy;
                    model.Transno = model.Transno;
                    model.Transdt = passModel.Transdt;
                    model.Transfor = passModel.Transfor;
                    if (model.Transfor == "COSTPOOL")
                    {
                        model.Costpid = passModel.Costpid;
                    }
                    else
                    {
                        model.Costpid = null;
                    }
                    model.Transmode = passModel.Transmode;
                    if (model.Transmode == "A/C PAYEE CHEQUE" || model.Transmode == "CASH CHEQUE")
                    {
                        model.Chequeno = passModel.Chequeno;
                        model.Chequedt = passModel.Chequedt;
                    }
                    else
                    {
                        model.Chequeno = null;
                        model.Chequedt = null;
                    }
                    model.Creditcd = passModel.Creditcd;
                    model.Debitcd = passModel.Debitcd;
                    model.Remarks = passModel.Remarks;
                    model.Amount = passModel.Amount;
                    model.Chqpayto = passModel.Chqpayto;

                    model.Userpc = UserlogAddress.UserPc();
                    model.Insuserid = usercode;
                    model.Instime = UserlogAddress.Timezone(DateTime.Now);
                    model.Insipno = UserlogAddress.IpAddress();
                    model.Insltude = passModel.Insltude;
                    db.GlStransDbSet.Add(model);
                    db.SaveChanges();

                    //For Log Data Logic
                    string debitCdTxtFieldName = "", creditCdTxtFieldName = "";
                    if (model.Transtp == "MREC")
                    {
                        debitCdTxtFieldName = "Received To";
                        creditCdTxtFieldName = "Received From";
                    }
                    else if (model.Transtp == "MPAY")
                    {
                        debitCdTxtFieldName = "Paid To";
                        creditCdTxtFieldName = "Paid From";
                    }
                    else if (model.Transtp == "JOUR")
                    {
                        debitCdTxtFieldName = "Debited To";
                        creditCdTxtFieldName = "Credited To";
                    }
                    else if (model.Transtp == "CONT")
                    {
                        debitCdTxtFieldName = "Deposited To";
                        creditCdTxtFieldName = "Withdrawl From";
                    }

                    string costPoolName = "";
                    var getCostPoolName = (from m in db.GlCostPoolDbSet
                                           where m.Compid == userCompanycode && m.Costpid == model.Costpid
                                           select new { m.Costpnm }).Distinct().ToList();
                    foreach (var get in getCostPoolName)
                    {
                        costPoolName = get.Costpnm;
                    }

                    string debitcd = "";
                    var getDebitName = (from m in db.GlAcchartDbSet
                                        where m.Compid == userCompanycode && m.Accountcd == model.Debitcd
                                        select new { m.Accountnm }).Distinct().ToList();
                    foreach (var get in getDebitName)
                    {
                        debitcd = get.Accountnm;
                    }

                    string creditcd = "";
                    var getCreditName = (from n in db.GlAcchartDbSet
                                         where n.Compid == userCompanycode && n.Accountcd == model.Creditcd
                                         select new { n.Accountnm }).Distinct().ToList();
                    foreach (var get in getCreditName)
                    {
                        creditcd = get.Accountnm;
                    }
                    String logData = Convert.ToString("Voucher Input page. TransType: " + model.Transtp + ",\nDate: " + dateTimetoString + ",\nTransNo: " + model.Transno + ",\nTransFor: " + model.Transfor + ",\nCost Pool: " + costPoolName + ",\nTrans Mode: " + model.Transmode + ",\n" + debitCdTxtFieldName + ": " + debitcd + ",\n" + creditCdTxtFieldName + ": " + creditcd + ",\nCheque NO: " + model.Chequeno + ",\nCheque Date: " + model.Chequedt + ",\nRemarks: " + model.Remarks + ",\nAmount: " + model.Amount + ".");

                    //Create Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "CREATE";
                    aslLog.Logltude = passModel.Insltude;
                    aslLog.Logdata = logData;
                    aslLog.Tableid = "GL_STRANS";
                    Log.SaveLog(aslLog);

                    return new
                    {
                        data = model,
                        success = true,
                        message = "Save data Successfully."
                    };
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
        [System.Web.Http.Route("api/SingleTransaction/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlStrans passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();
            
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findSingleTransactionInfo = (from m in db.GlStransDbSet where m.Compid == passModel.Compid && m.Transtp == passModel.Transtp && m.Transmy == passModel.Transmy && m.Transno == passModel.Transno select m).ToList();
                    if (findSingleTransactionInfo.Count == 1)
                    {
                        //For Log Data Logic
                        string debitCdTxtFieldName = "", creditCdTxtFieldName = "";
                        if (passModel.Transtp == "MREC")
                        {
                            debitCdTxtFieldName = "Received To";
                            creditCdTxtFieldName = "Received From";
                        }
                        else if (passModel.Transtp == "MPAY")
                        {
                            debitCdTxtFieldName = "Paid To";
                            creditCdTxtFieldName = "Paid From";
                        }
                        else if (passModel.Transtp == "JOUR")
                        {
                            debitCdTxtFieldName = "Debited To";
                            creditCdTxtFieldName = "Credited To";
                        }
                        else if (passModel.Transtp == "CONT")
                        {
                            debitCdTxtFieldName = "Deposited To";
                            creditCdTxtFieldName = "Withdrawl From";
                        }

                        string costPoolName = "";
                        var getCostPoolName = (from m in db.GlCostPoolDbSet
                                               where m.Compid == userCompanycode && m.Costpid == passModel.Costpid
                                               select new { m.Costpnm }).Distinct().ToList();
                        foreach (var get in getCostPoolName)
                        {
                            costPoolName = get.Costpnm;
                        }
                        string debitcd = "";
                        var getDebitName = (from m in db.GlAcchartDbSet
                                            where m.Compid == userCompanycode && m.Accountcd == passModel.Debitcd
                                            select new { m.Accountnm }).Distinct().ToList();
                        foreach (var get in getDebitName)
                        {
                            debitcd = get.Accountnm;
                        }
                        string creditcd = "";
                        var getCreditName = (from n in db.GlAcchartDbSet
                                             where n.Compid == userCompanycode && n.Accountcd == passModel.Creditcd
                                             select new { n.Accountnm }).Distinct().ToList();
                        foreach (var get in getCreditName)
                        {
                            creditcd = get.Accountnm;
                        }
                        String logData = Convert.ToString("Voucher Input page. TransType: " + passModel.Transtp + ",\nDate: " + passModel.Transdt + ",\nTransNo: " + passModel.Transno + ",\nTransFor: " + passModel.Transfor + ",\nCost Pool: " + costPoolName + ",\nTrans Mode: " + passModel.Transmode + ",\n" + debitCdTxtFieldName + ": " + debitcd + ",\n" + creditCdTxtFieldName + ": " + creditcd + ",\nCheque NO: " + passModel.Chequeno + ",\nCheque Date: " + passModel.Chequedt + ",\nRemarks: " + passModel.Remarks + ",\nAmount: " + passModel.Amount + ".");



                        //update logic
                        foreach (var get in findSingleTransactionInfo)
                        {
                            get.Transfor = passModel.Transfor;
                            if (get.Transfor == "COSTPOOL")
                            {
                                get.Costpid = passModel.Costpid;
                            }
                            else
                            {
                                get.Costpid = null;
                            }
                            get.Transmode = passModel.Transmode;
                            if (get.Transmode == "A/C PAYEE CHEQUE" || get.Transmode == "CASH CHEQUE")
                            {
                                get.Chequeno = passModel.Chequeno;
                                get.Chequedt = passModel.Chequedt;
                            }
                            else
                            {
                                get.Chequeno = null;
                                get.Chequedt = null;
                            }
                            get.Creditcd = passModel.Creditcd;
                            get.Debitcd = passModel.Debitcd;
                            get.Remarks = passModel.Remarks;
                            get.Amount = passModel.Amount;
                            get.Chqpayto = passModel.Chqpayto;

                            get.Userpc = UserlogAddress.UserPc();
                            get.Upduserid = usercode;
                            get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                            get.Updipno = UserlogAddress.IpAddress();
                            get.Updltude = passModel.Updltude;
                        }
                        db.SaveChanges();

                        //Update Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "UPDATE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = logData;
                        aslLog.Tableid = "GL_STRANS";
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
        [System.Web.Http.Route("api/SingleTransaction/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlStrans passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    GlStrans model = db.GlStransDbSet.Find(passModel.Compid, passModel.Transtp, passModel.Transmy, passModel.Transno);
                    if (model != null)
                    {
                        //For Log Data Logic
                        string debitCdTxtFieldName = "", creditCdTxtFieldName = "";
                        if (passModel.Transtp == "MREC")
                        {
                            debitCdTxtFieldName = "Received To";
                            creditCdTxtFieldName = "Received From";
                        }
                        else if (passModel.Transtp == "MPAY")
                        {
                            debitCdTxtFieldName = "Paid To";
                            creditCdTxtFieldName = "Paid From";
                        }
                        else if (passModel.Transtp == "JOUR")
                        {
                            debitCdTxtFieldName = "Debited To";
                            creditCdTxtFieldName = "Credited To";
                        }
                        else if (passModel.Transtp == "CONT")
                        {
                            debitCdTxtFieldName = "Deposited To";
                            creditCdTxtFieldName = "Withdrawl From";
                        }

                        string costPoolName = "";
                        var getCostPoolName = (from m in db.GlCostPoolDbSet
                                               where m.Compid == userCompanycode && m.Costpid == model.Costpid
                                               select new { m.Costpnm }).Distinct().ToList();
                        foreach (var get in getCostPoolName)
                        {
                            costPoolName = get.Costpnm;
                        }
                        string debitcd = "";
                        var getDebitName = (from m in db.GlAcchartDbSet
                                            where m.Compid == userCompanycode && m.Accountcd == model.Debitcd
                                            select new { m.Accountnm }).Distinct().ToList();
                        foreach (var get in getDebitName)
                        {
                            debitcd = get.Accountnm;
                        }
                        string creditcd = "";
                        var getCreditName = (from n in db.GlAcchartDbSet
                                             where n.Compid == userCompanycode && n.Accountcd == model.Creditcd
                                             select new { n.Accountnm }).Distinct().ToList();
                        foreach (var get in getCreditName)
                        {
                            creditcd = get.Accountnm;
                        }
                        String logData = Convert.ToString("Voucher Input page. TransType: " + model.Transtp + ",\nDate: " + model.Transdt + ",\nTransNo: " + model.Transno + ",\nTransFor: " + model.Transfor + ",\nCost Pool: " + costPoolName + ",\nTrans Mode: " + model.Transmode + ",\n" + debitCdTxtFieldName + ": " + debitcd + ",\n" + creditCdTxtFieldName + ": " + creditcd + ",\nCheque NO: " + model.Chequeno + ",\nCheque Date: " + model.Chequedt + ",\nRemarks: " + model.Remarks + ",\nAmount: " + model.Amount + ".");


                        //Delete Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "DELETE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = logData;
                        aslLog.Tableid = "GL_STRANS";
                        Log.SaveLog(aslLog);

                        //Delete Log Data Save in to ASL_DELETE Table
                        AslDelete aslDeleteLog = new AslDelete();
                        aslDeleteLog.Compid = userCompanycode;
                        aslDeleteLog.Userid = usercode;
                        aslDeleteLog.Delltude = passModel.Updltude;
                        aslDeleteLog.Deldata = logData;
                        aslDeleteLog.Tableid = "GL_STRANS";
                        Log.DeleteLog(aslDeleteLog);

                        //Delete From AslUserco
                        db.GlStransDbSet.Remove(model);
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
        [System.Web.Http.Route("api/SingleTransaction/TotalList")]
        public object TotalList(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlStrans> list = new List<GlStrans>();
                list = (from m in db.GlStransDbSet where m.Compid == userCompanycode select m).ToList();
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
        [System.Web.Http.Route("api/SingleTransaction/List")]
        public object List(Int64 userCompanycode, Int64 usercode, String transType, DateTime transDate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlStrans> list = new List<GlStrans>();
                list = (from m in db.GlStransDbSet where m.Compid == userCompanycode && m.Transtp == transType && m.Transdt == transDate select m).ToList();
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
        [System.Web.Http.Route("api/SingleTransaction/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, String transType, String transMonthYear, Int64 transno)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlStrans> list = new List<GlStrans>();
                list = (from m in db.GlStransDbSet where m.Compid == userCompanycode && m.Transtp == transType && m.Transmy == transMonthYear && m.Transno== transno select m).ToList();
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





        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/SingleTransaction/DateWiseList")]
        public object DateWiseList(Int64 userCompanycode, Int64 usercode, String fromDate, String toDate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    DateTime fdate = Convert.ToDateTime(fromDate);
                    DateTime tDate = Convert.ToDateTime(toDate);

                    List<GlStrans> list = new List<GlStrans>();
                    list = (from m in db.GlStransDbSet where m.Compid == userCompanycode && m.Transdt >= fdate && m.Transdt <= tDate select m).ToList();
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
                data = "",
                success = false,
                message = "Authorized not permitted."
            };
        }





        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/SingleTransaction/FindMaxTransNo")]
        public object FindMaxTransNo(Int64 userCompanycode, Int64 usercode, String transType, DateTime transDate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                string dateTimetoString = Convert.ToString(transDate.ToString("dd-MMM-yyyy"));
                string getYear = dateTimetoString.Substring(9, 2);
                string getMonth = dateTimetoString.Substring(3, 3).ToUpper();
                string transMonth = getMonth + "-" + getYear;

                string dateTimetoString1 = Convert.ToString(transDate.ToString("dd-MM-yyyy"));
                string getyear = dateTimetoString1.Substring(6, 4);
                string getmonth = dateTimetoString1.Substring(3, 2);
                string halftransno = getyear + getmonth;

                Int64 transNo = 0;
                Int64 maxtransNo = Convert.ToInt64((from m in db.GlStransDbSet where m.Compid == userCompanycode && m.Transtp == transType && m.Transmy == transMonth select m.Transno).DefaultIfEmpty().Max());
                if (maxtransNo == 0)
                {
                    transNo = Convert.ToInt64(halftransno + "0001");
                }
                else if (maxtransNo != 0)
                {
                    Int64 maxLimit = Convert.ToInt64(halftransno + "9999");
                    Int64 id = maxtransNo + 1;
                    if (id <= maxLimit)
                    {
                        transNo = id;
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
                return new
                {
                    data = transNo,
                    success = true,
                    message = "Get data Successfully."
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
