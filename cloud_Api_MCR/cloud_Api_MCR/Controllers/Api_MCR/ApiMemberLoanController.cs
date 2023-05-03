using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.ASL;
using cloud_Api_MCR.Models.MCR;
using cloud_Api_MCR.Models.MCR_DTO;

namespace cloud_Api_MCR.Controllers.Api_MCR
{
    public class ApiMemberLoanController : ApiController
    {
        private DatabaseDbContext db;

        public ApiMemberLoanController()
        {
            db = new DatabaseDbContext();
        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/MemberLoan/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMLoan passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrMLoan model = new McrMLoan();

                    string dateTimetoString = Convert.ToString(passModel.Transdt.ToString("dd-MMM-yyyy"));
                    string getYear = dateTimetoString.Substring(9, 2);
                    string getMonth = dateTimetoString.Substring(3, 3).ToUpper();
                    string transMonth = getMonth + "-" + getYear;

                    string dateTimetoString1 = Convert.ToString(passModel.Transdt.ToString("dd-MM-yyyy"));
                    string getyear = dateTimetoString1.Substring(6, 4);
                    string getmonth = dateTimetoString1.Substring(3, 2);
                    string halftransno = getyear + getmonth;

                    Int64 maxTranssl = Convert.ToInt64((from m in db.McrMLoansDbSet where m.Compid == userCompanycode && m.Transmy == transMonth select m.Transno).DefaultIfEmpty().Max());
                    if (maxTranssl == 0)
                    {
                        model.Transno = Convert.ToInt64(halftransno + "0001");
                    }
                    else if (maxTranssl != 0)
                    {
                        Int64 maxLimit = Convert.ToInt64(halftransno + "9999");
                        Int64 id = maxTranssl + 1;
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
                    model.Transmy = transMonth;
                    model.Transno = model.Transno;

                    model.Transdt = passModel.Transdt;
                    model.Schemeid = passModel.Schemeid;
                    model.Memberid = passModel.Memberid;
                    model.Internid = passModel.Internid;
                    model.Loanamt = passModel.Loanamt;
                    model.Pchargert = passModel.Pchargert;
                    model.Pchargeamt = passModel.Pchargeamt;
                    model.Schargert = passModel.Schargert;
                    model.Schargeamt = passModel.Schargeamt;
                    model.Disburseamt = passModel.Disburseamt;
                    model.Riskfundrt = passModel.Riskfundrt;
                    model.Riskfundamt = passModel.Riskfundamt;
                    model.Collectamt = passModel.Collectamt;
                    model.Interestrt = passModel.Interestrt;
                    model.Schemeiqty = passModel.Schemeiqty;
                    model.Schemeefdt = Convert.ToDateTime(passModel.Schemeefdt);
                    model.Schemeetdt = Convert.ToDateTime(passModel.Schemeetdt);
                    model.Remarks = passModel.Remarks;


                    model.Userpc = UserlogAddress.UserPc();
                    model.Insuserid = usercode;
                    model.Instime = UserlogAddress.Timezone(DateTime.Now);
                    model.Insipno = UserlogAddress.IpAddress();
                    model.Insltude = passModel.Insltude;
                    db.McrMLoansDbSet.Add(model);
                    db.SaveChanges();


                    string transDate = Convert.ToString(passModel.Transdt.ToString("dd-MMM-yyyy"));
                    String memberName = "";
                    var findMemberName = (from m in db.McrMembersDbSet
                                          where m.Compid == userCompanycode && m.Memberid == model.Memberid
                                          select m).ToList();
                    foreach (var get in findMemberName)
                    {
                        memberName = get.Membernm;
                    }

                    //Create Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "CREATE";
                    aslLog.Logltude = passModel.Insltude;
                    aslLog.Logdata = Convert.ToString("Member Loan Input page. Date: " + transDate + ",\nTrans-No: " + model.Transno + ",\nScheme: " + model.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + model.Internid + ",\nLoan Amount: " + model.Loanamt + ",\nProfit Charge(%)  : " + model.Pchargert + ",\nProfit Charge: " + model.Pchargeamt + ",\nService Charge(%): " + model.Schargert + ",\nService Charge: " + model.Schargeamt + ",\nDisburse Amount: " + model.Disburseamt + ",\nRisk Fund (%): " + model.Riskfundrt + ",\nRisk Fund: " + model.Riskfundamt + ",\nCollect Amount : " + model.Collectamt + ",\nInterest (%): " + model.Interestrt + ",\nScheme Qty: " + model.Schemeiqty + ",\nEffect From: " + model.Schemeefdt + ",\nEffect To: " + model.Schemeetdt + ",\nRemarks: " + model.Remarks + ".");
                    aslLog.Tableid = "MCR_MLOAN";
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
        [System.Web.Http.Route("api/MemberLoan/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMLoan passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findMemberLoanInfo = (from m in db.McrMLoansDbSet where m.Compid == userCompanycode && m.Transmy == passModel.Transmy && m.Transno == passModel.Transno select m).ToList();
                    if (findMemberLoanInfo.Count == 1)
                    {
                        //update logic
                        foreach (var get in findMemberLoanInfo)
                        {
                            get.Schemeid = passModel.Schemeid;
                            get.Memberid = passModel.Memberid;
                            get.Internid = passModel.Internid;
                            get.Loanamt = passModel.Loanamt;
                            get.Pchargert = passModel.Pchargert;
                            get.Pchargeamt = passModel.Pchargeamt;
                            get.Schargert = passModel.Schargert;
                            get.Schargeamt = passModel.Schargeamt;
                            get.Disburseamt = passModel.Disburseamt;
                            get.Riskfundrt = passModel.Riskfundrt;
                            get.Riskfundamt = passModel.Riskfundamt;
                            get.Collectamt = passModel.Collectamt;
                            get.Interestrt = passModel.Interestrt;
                            get.Schemeiqty = passModel.Schemeiqty;
                            get.Schemeefdt = Convert.ToDateTime(passModel.Schemeefdt);
                            get.Schemeetdt = Convert.ToDateTime(passModel.Schemeetdt);
                            get.Remarks = passModel.Remarks;

                            get.Userpc = UserlogAddress.UserPc();
                            get.Upduserid = usercode;
                            get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                            get.Updipno = UserlogAddress.IpAddress();
                            get.Updltude = passModel.Updltude;
                        }
                        db.SaveChanges();

                        string transDate = Convert.ToString(passModel.Transdt.ToString("dd-MMM-yyyy"));
                        String memberName = "";
                        var findMemberName = (from m in db.McrMembersDbSet
                                              where m.Compid == userCompanycode && m.Memberid == passModel.Memberid
                                              select m).ToList();
                        foreach (var get in findMemberName)
                        {
                            memberName = get.Membernm;
                        }

                        //Update Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "UPDATE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = Convert.ToString("Member Loan Input page. Date: " + transDate + ",\nTrans-No: " + passModel.Transno + ",\nScheme: " + passModel.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + passModel.Internid + ",\nLoan Amount: " + passModel.Loanamt + ",\nProfit Charge(%)  : " + passModel.Pchargert + ",\n Profit Charge: " + passModel.Pchargeamt + ",\nService Charge(%): " + passModel.Schargert + ",\nService Charge: " + passModel.Schargeamt + ",\nDisburse Amount: " + passModel.Disburseamt + ",\nRisk Fund (%): " + passModel.Riskfundrt + ",\nRisk Fund: " + passModel.Riskfundamt + ",\nCollect Amount : " + passModel.Collectamt + ",\nInterest (%): " + passModel.Interestrt + ",\nScheme Qty: " + passModel.Schemeiqty + ",\nEffect From: " + passModel.Schemeefdt + ",\nEffect To: " + passModel.Schemeetdt + ",\nRemarks: " + passModel.Remarks + ".");
                        aslLog.Tableid = "MCR_MLOAN";
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
        [System.Web.Http.Route("api/MemberLoan/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMLoan passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrMLoan model = db.McrMLoansDbSet.Find(passModel.Compid, passModel.Transmy, passModel.Transno);
                    if (model != null)
                    {
                        string transDate = Convert.ToString(model.Transdt.ToString("dd-MMM-yyyy"));
                        String memberName = "";
                        var findMemberName = (from m in db.McrMembersDbSet
                                              where m.Compid == userCompanycode && m.Memberid == model.Memberid
                                              select m).ToList();
                        foreach (var get in findMemberName)
                        {
                            memberName = get.Membernm;
                        }

                        //Delete Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "DELETE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = Convert.ToString("Member Loan Input page. Date: " + transDate + ",\nTransNo: " + model.Transno +  ",\nScheme: " + model.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + model.Internid + ",\nLoan Amount: " + model.Loanamt + ",\nProfit Charge(%)  : " + model.Pchargert + ",\n Profit Charge: " + model.Pchargeamt + ",\nService Charge(%): " + model.Schargert + ",\nService Charge: " + model.Schargeamt + ",\nDisburse Amount: " + model.Disburseamt + ",\nRisk Fund (%): " + model.Riskfundrt + ",\nRisk Fund: " + model.Riskfundamt + ",\nCollect Amount : " + model.Collectamt + ",\nInterest (%): " + model.Interestrt + ",\nScheme Qty: " + model.Schemeiqty + ",\nEffect From: " + model.Schemeefdt + ",\nEffect To: " + model.Schemeetdt + ",\nRemarks: " + model.Remarks + ".");
                        aslLog.Tableid = "MCR_MLOAN";
                        Log.SaveLog(aslLog);

                        //Delete Log Data Save in to ASL_DELETE Table
                        AslDelete aslDeleteLog = new AslDelete();
                        aslDeleteLog.Compid = userCompanycode;
                        aslDeleteLog.Userid = usercode;
                        aslDeleteLog.Delltude = passModel.Updltude;
                        aslDeleteLog.Deldata = Convert.ToString("Member Loan Input page. Date: " + transDate + ",\nTransNo: " + model.Transno + ",\nScheme: " + model.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + model.Internid + ",\nLoan Amount: " + model.Loanamt + ",\nProfit Charge(%)  : " + model.Pchargert + ",\n Profit Charge: " + model.Pchargeamt + ",\nService Charge(%): " + model.Schargert + ",\nService Charge: " + model.Schargeamt + ",\nDisburse Amount: " + model.Disburseamt + ",\nRisk Fund (%): " + model.Riskfundrt + ",\nRisk Fund: " + model.Riskfundamt + ",\nCollect Amount : " + model.Collectamt + ",\nInterest (%): " + model.Interestrt + ",\nScheme Qty: " + model.Schemeiqty + ",\nEffect From: " + model.Schemeefdt + ",\nEffect To: " + model.Schemeetdt + ",\nRemarks: " + model.Remarks + ".");
                        aslDeleteLog.Tableid = "MCR_MLOAN";
                        Log.DeleteLog(aslDeleteLog);

                        //Delete From AslUserco
                        db.McrMLoansDbSet.Remove(model);
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
        [System.Web.Http.Route("api/MemberLoan/List")]
        public object List(Int64 userCompanycode, Int64 usercode, String transMonthYear)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrMLoan> list = new List<McrMLoan>();
                list = (from m in db.McrMLoansDbSet where m.Compid == userCompanycode && m.Transmy == transMonthYear select m).ToList();
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
        [System.Web.Http.Route("api/MemberLoan/DateWiseList")]
        public object DateWiseList(Int64 userCompanycode, Int64 usercode, String fromDate, String toDate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                DateTime fdate = Convert.ToDateTime(fromDate);
                DateTime tdate = Convert.ToDateTime(toDate);

                List<McrMLoan> list = new List<McrMLoan>();
                list = (from m in db.McrMLoansDbSet where m.Compid == userCompanycode && m.Transdt >= fdate && m.Transdt <= tdate select m).ToList();
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
        [System.Web.Http.Route("api/MemberLoan/SchemeWiseList")]
        public object SchemeWiseList(Int64 userCompanycode, Int64 usercode, String schemeId)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrMLoan> list = new List<McrMLoan>();
                list = (from m in db.McrMLoansDbSet where m.Compid == userCompanycode && m.Schemeid == schemeId select m).ToList();
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
        [System.Web.Http.Route("api/MemberLoan/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, String transMonthYear, Int64 transno)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrMLoan> list = new List<McrMLoan>();
                list = (from m in db.McrMLoansDbSet where m.Compid == userCompanycode && m.Transmy == transMonthYear && m.Transno == transno select m).ToList();
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
