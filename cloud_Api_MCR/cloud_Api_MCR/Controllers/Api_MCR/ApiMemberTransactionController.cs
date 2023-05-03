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
    public class ApiMemberTransactionController : ApiController
    {
        private DatabaseDbContext db;

        public ApiMemberTransactionController()
        {
            db = new DatabaseDbContext();
        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/MemberTransaction/Create/MemoNoWise")]
        public object Create_MemoNoWise(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMTrans passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token) && passModel.Transno != 0)
            {
                try
                {
                    McrMTrans model = new McrMTrans();
                    
                    string dateTimetoString = Convert.ToString(passModel.Transdt.ToString("dd-MMM-yyyy"));
                    string getYear = dateTimetoString.Substring(9, 2);
                    string getMonth = dateTimetoString.Substring(3, 3).ToUpper();
                    string transMonth = getMonth + "-" + getYear;

                    string dateTimetoString1 = Convert.ToString(passModel.Transdt.ToString("dd-MM-yyyy"));
                    string getyear = dateTimetoString1.Substring(6, 4);
                    string getmonth = dateTimetoString1.Substring(3, 2);
                    string halftransno = getyear + getmonth;

                    //Transaction No / Memo No generation if Memo No Not Same

                    Int64 maxtransNo = Convert.ToInt64((from m in db.McrMTransesDbSet where m.Compid == userCompanycode && m.Transtp == passModel.Transtp && m.Transmy == transMonth select m.Transno).DefaultIfEmpty().Max());
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
                    model.Transmy = transMonth;
                    model.Transno = passModel.Transno;
                    model.Transsl = Convert.ToInt64("1");
                    model.Transdt = passModel.Transdt;
                    model.Transfor = passModel.Transfor;
                    model.Transmode = passModel.Transmode;
                    model.Glheadid = passModel.Glheadid;
                    model.Schemeid = passModel.Schemeid;
                    model.Memberid = passModel.Memberid;
                    model.Internid = passModel.Internid;
                    model.Schemeid2 = passModel.Schemeid2;
                    model.Memberid2 = passModel.Memberid2;
                    model.Internid2 = passModel.Internid2;
                    model.Amount = Convert.ToDecimal(passModel.Amount);
                    model.Remarks = passModel.Remarks;

                    model.Userpc = UserlogAddress.UserPc();
                    model.Insuserid = usercode;
                    model.Instime = UserlogAddress.Timezone(DateTime.Now);
                    model.Insipno = UserlogAddress.IpAddress();
                    model.Insltude = passModel.Insltude;
                    db.McrMTransesDbSet.Add(model);
                    db.SaveChanges();


                    string transDate = Convert.ToString(passModel.Transdt.ToString("dd-MMM-yyyy"));

                    String accountName = "", memberName = "", memberName2 = "";
                    Int64 cash = Convert.ToInt64(userCompanycode + "101"), bank = Convert.ToInt64(userCompanycode + "102");
                    var findAccountName = (from m in db.GlAcchartDbSet
                                           where m.Compid == userCompanycode && m.Accountcd == passModel.Glheadid && m.Headcd != cash && m.Headcd != bank
                                           select m).ToList();
                    foreach (var get in findAccountName)
                    {
                        accountName = get.Accountnm;
                    }
                    var findMemberName = (from m in db.McrMembersDbSet
                                          where m.Compid == userCompanycode && m.Memberid == passModel.Memberid
                                          select m).ToList();
                    foreach (var get in findMemberName)
                    {
                        memberName = get.Membernm;
                    }
                    var findMemberName2 = (from m in db.McrMembersDbSet
                                          where m.Compid == userCompanycode && m.Memberid == passModel.Memberid2
                                          select m).ToList();
                    foreach (var get in findMemberName2)
                    {
                        memberName2 = get.Membernm;
                    }

                    //Create Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "CREATE";
                    aslLog.Logltude = passModel.Insltude;
                    aslLog.Logdata = Convert.ToString("Member Transaction Input page. TransType: " + passModel.Transtp + ",\nDate: " + transDate + ",\nTransNo: " + passModel.Transno + ",\nTrans-Serail: " + passModel.Transsl + ",\nTransFor: " + passModel.Transfor + ",\nTrans Mode: " + passModel.Transmode + ",\nAccount Name: " + accountName + ",\nScheme: " + passModel.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + passModel.Internid + ",\nScheme (2): " + passModel.Schemeid2 + ",\nMember Name (2): " + memberName2 + ",\nInternal ID (2): " + passModel.Internid2 + ",\nRemarks: " + passModel.Remarks + ",\nAmount: " + passModel.Amount + ".");
                    aslLog.Tableid = "MCR_MTRANS";
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
        [System.Web.Http.Route("api/MemberTransaction/Create/MemoSerialWise")]
        public object Create_MemoSerialWise(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMTrans passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token) && passModel.Transno != 0)
            {
                try
                {
                    McrMTrans model = new McrMTrans();

                    string dateTimetoString = Convert.ToString(passModel.Transdt.ToString("dd-MMM-yyyy"));
                    string getYear = dateTimetoString.Substring(9, 2);
                    string getMonth = dateTimetoString.Substring(3, 3).ToUpper();
                    string transMonth = getMonth + "-" + getYear;
                    
                    
                    //Transaction Serial generation
                    Int64 maxTranssl = Convert.ToInt64((from m in db.McrMTransesDbSet where m.Compid == userCompanycode && m.Transtp == passModel.Transtp && m.Transmy == transMonth && m.Transno == passModel.Transno select m.Transsl).DefaultIfEmpty().Max());
                    if (maxTranssl == 0)
                    {
                        model.Transsl = Convert.ToInt64("1");
                    }
                    else if (maxTranssl != 0)
                    {
                        Int64 maxLimit = Convert.ToInt64("99999");
                        Int64 id = maxTranssl + 1;
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
                    model.Transtp = passModel.Transtp;
                    model.Transmy = transMonth;
                    model.Transno = passModel.Transno;
                    model.Transsl = model.Transsl;
                    model.Transdt = passModel.Transdt;
                    model.Transfor = passModel.Transfor;
                    model.Transmode = passModel.Transmode;
                    model.Glheadid = passModel.Glheadid;
                    model.Schemeid = passModel.Schemeid;
                    model.Memberid = passModel.Memberid;
                    model.Internid = passModel.Internid;
                    model.Schemeid2 = passModel.Schemeid2;
                    model.Memberid2 = passModel.Memberid2;
                    model.Internid2 = passModel.Internid2;
                    model.Amount = Convert.ToDecimal(passModel.Amount);
                    model.Remarks = passModel.Remarks;

                    model.Userpc = UserlogAddress.UserPc();
                    model.Insuserid = usercode;
                    model.Instime = UserlogAddress.Timezone(DateTime.Now);
                    model.Insipno = UserlogAddress.IpAddress();
                    model.Insltude = passModel.Insltude;
                    db.McrMTransesDbSet.Add(model);
                    db.SaveChanges();


                    string transDate = Convert.ToString(passModel.Transdt.ToString("dd-MMM-yyyy"));

                    String accountName = "", memberName = "", memberName2 = "";
                    Int64 cash = Convert.ToInt64(userCompanycode + "101"), bank = Convert.ToInt64(userCompanycode + "102");
                    var findAccountName = (from m in db.GlAcchartDbSet
                                           where m.Compid == userCompanycode && m.Accountcd == passModel.Glheadid && m.Headcd != cash && m.Headcd != bank
                                           select m).ToList();
                    foreach (var get in findAccountName)
                    {
                        accountName = get.Accountnm;
                    }
                    var findMemberName = (from m in db.McrMembersDbSet
                                          where m.Compid == userCompanycode && m.Memberid == passModel.Memberid
                                          select m).ToList();
                    foreach (var get in findMemberName)
                    {
                        memberName = get.Membernm;
                    }
                    var findMemberName2 = (from m in db.McrMembersDbSet
                                           where m.Compid == userCompanycode && m.Memberid == passModel.Memberid2
                                           select m).ToList();
                    foreach (var get in findMemberName2)
                    {
                        memberName2 = get.Membernm;
                    }

                    //Create Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "CREATE";
                    aslLog.Logltude = passModel.Insltude;
                    aslLog.Logdata = Convert.ToString("Member Transaction Input page. TransType: " + passModel.Transtp + ",\nDate: " + transDate + ",\nTransNo: " + passModel.Transno + ",\nTrans-Serail: " + passModel.Transsl + ",\nTransFor: " + passModel.Transfor + ",\nTrans Mode: " + passModel.Transmode + ",\nAccount Name: " + accountName + ",\nScheme: " + passModel.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + passModel.Internid + ",\nScheme (2): " + passModel.Schemeid2 + ",\nMember Name (2): " + memberName2 + ",\nInternal ID (2): " + passModel.Internid2 + ",\nRemarks: " + passModel.Remarks + ",\nAmount: " + passModel.Amount + ".");
                    aslLog.Tableid = "MCR_MTRANS";
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
        [System.Web.Http.Route("api/MemberTransaction/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMTrans passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findMemberTransactionInfo = (from m in db.McrMTransesDbSet where m.Compid == passModel.Compid && m.Transtp == passModel.Transtp && m.Transmy == passModel.Transmy && m.Transno == passModel.Transno && m.Transsl == passModel.Transsl select m).ToList();
                    if (findMemberTransactionInfo.Count == 1)
                    {
                        //update logic
                        foreach (var get in findMemberTransactionInfo)
                        {
                            get.Transfor = passModel.Transfor;
                            get.Transmode = passModel.Transmode;
                            get.Glheadid = passModel.Glheadid;
                            get.Schemeid = passModel.Schemeid;
                            get.Memberid = passModel.Memberid;
                            get.Internid = passModel.Internid;
                            get.Amount = passModel.Amount;
                            get.Schemeid2 = passModel.Schemeid2;
                            get.Memberid2 = passModel.Memberid2;
                            get.Internid2 = passModel.Internid2;
                            get.Remarks = passModel.Remarks;

                            get.Userpc = UserlogAddress.UserPc();
                            get.Upduserid = usercode;
                            get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                            get.Updipno = UserlogAddress.IpAddress();
                            get.Updltude = passModel.Updltude;
                        }
                        db.SaveChanges();

                        string transDate = Convert.ToString(passModel.Transdt.ToString("dd-MMM-yyyy"));

                        String accountName = "", memberName = "", memberName2 = "";
                        Int64 cash = Convert.ToInt64(userCompanycode + "101"), bank = Convert.ToInt64(userCompanycode + "102");
                        var findAccountName = (from m in db.GlAcchartDbSet
                                               where m.Compid == userCompanycode && m.Accountcd == passModel.Glheadid && m.Headcd != cash && m.Headcd != bank
                                               select m).ToList();
                        foreach (var get in findAccountName)
                        {
                            accountName = get.Accountnm;
                        }
                        var findMemberName = (from m in db.McrMembersDbSet
                                              where m.Compid == userCompanycode && m.Memberid == passModel.Memberid
                                              select m).ToList();
                        foreach (var get in findMemberName)
                        {
                            memberName = get.Membernm;
                        }
                        var findMemberName2 = (from m in db.McrMembersDbSet
                                               where m.Compid == userCompanycode && m.Memberid == passModel.Memberid2
                                               select m).ToList();
                        foreach (var get in findMemberName2)
                        {
                            memberName2 = get.Membernm;
                        }

                        //Update Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "UPDATE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = Convert.ToString("Member Transaction Input page. TransType: " + passModel.Transtp + ",\nDate: " + transDate + ",\nTransNo: " + passModel.Transno + ",\nTrans-Serail: " + passModel.Transsl + ",\nTransFor: " + passModel.Transfor + ",\nTrans Mode: " + passModel.Transmode + ",\nAccount Name: " + accountName + ",\nScheme: " + passModel.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + passModel.Internid + ",\nScheme (2): " + passModel.Schemeid2 + ",\nMember Name (2): " + memberName2 + ",\nInternal ID (2): " + passModel.Internid2 + ",\nRemarks: " + passModel.Remarks + ",\nAmount: " + passModel.Amount + ".");
                        aslLog.Tableid = "MCR_MTRANS";
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
        [System.Web.Http.Route("api/MemberTransaction/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMTrans passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrMTrans model = db.McrMTransesDbSet.Find(passModel.Compid, passModel.Transtp, passModel.Transmy, passModel.Transno, passModel.Transsl);
                    if (model != null)
                    {
                        string transDate = Convert.ToString(model.Transdt.ToString("dd-MMM-yyyy"));

                        String accountName = "", memberName = "", memberName2="";
                        Int64 cash = Convert.ToInt64(userCompanycode + "101"), bank = Convert.ToInt64(userCompanycode + "102");
                        var findAccountName = (from m in db.GlAcchartDbSet
                                               where m.Compid == userCompanycode && m.Accountcd == model.Glheadid && m.Headcd != cash && m.Headcd != bank
                                               select m).ToList();
                        foreach (var get in findAccountName)
                        {
                            accountName = get.Accountnm;
                        }
                        var findMemberName = (from m in db.McrMembersDbSet
                                              where m.Compid == userCompanycode && m.Memberid == model.Memberid
                                              select m).ToList();
                        foreach (var get in findMemberName)
                        {
                            memberName = get.Membernm;
                        }
                        var findMemberName2 = (from m in db.McrMembersDbSet
                                               where m.Compid == userCompanycode && m.Memberid == passModel.Memberid2
                                               select m).ToList();
                        foreach (var get in findMemberName2)
                        {
                            memberName2 = get.Membernm;
                        }

                        //Delete Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "DELETE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = Convert.ToString("Member Transaction Input page. TransType: " + model.Transtp + ",\nDate: " + transDate + ",\nTransNo: " + model.Transno + ",\nTrans-Serail: " + model.Transsl + ",\nTransFor: " + model.Transfor + ",\nTrans Mode: " + model.Transmode + ",\nAccount Name: " + accountName + ",\nScheme: " + model.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + model.Internid + ",\nScheme (2): " + model.Schemeid2 + ",\nMember Name (2): " + memberName2 + ",\nInternal ID (2): " + model.Internid2 + ",\nRemarks: " + model.Remarks + ",\nAmount: " + model.Amount + ".");
                        aslLog.Tableid = "MCR_MTRANS";
                        Log.SaveLog(aslLog);

                        //Delete Log Data Save in to ASL_DELETE Table
                        AslDelete aslDeleteLog = new AslDelete();
                        aslDeleteLog.Compid = userCompanycode;
                        aslDeleteLog.Userid = usercode;
                        aslDeleteLog.Delltude = passModel.Updltude;
                        aslDeleteLog.Deldata = Convert.ToString("Member Transaction Input page. TransType: " + model.Transtp + ",\nDate: " + transDate + ",\nTransNo: " + model.Transno + ",\nTrans-Serail: " + model.Transsl + ",\nTransFor: " + model.Transfor + ",\nTrans Mode: " + model.Transmode + ",\nAccount Name: " + accountName + ",\nScheme: " + model.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + model.Internid + ",\nScheme (2): " + model.Schemeid2 + ",\nMember Name (2): " + memberName2 + ",\nInternal ID (2): " + model.Internid2 + ",\nRemarks: " + model.Remarks + ",\nAmount: " + model.Amount + ".");
                        aslDeleteLog.Tableid = "MCR_MTRANS";
                        Log.DeleteLog(aslDeleteLog);

                        //Delete From AslUserco
                        db.McrMTransesDbSet.Remove(model);
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
        [System.Web.Http.Route("api/MemberTransaction/List")]
        public object List(Int64 userCompanycode, Int64 usercode, String transType, DateTime transDate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrMTrans> list = new List<McrMTrans>();
                list = (from m in db.McrMTransesDbSet where m.Compid == userCompanycode && m.Transtp == transType && m.Transdt == transDate select m).ToList();
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
        [System.Web.Http.Route("api/MemberTransaction/TypeDateWiseList")]
        public object TypeDateWiseList(Int64 userCompanycode, Int64 usercode, String transType, String fromDate, String toDate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                DateTime fdate = Convert.ToDateTime(fromDate);
                DateTime tdate = Convert.ToDateTime(toDate);

                List<McrMTrans> list = new List<McrMTrans>();
                list = (from m in db.McrMTransesDbSet where m.Compid == userCompanycode && m.Transtp == transType && m.Transdt >= fdate && m.Transdt <= tdate  select m).ToList();
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
        [System.Web.Http.Route("api/MemberTransaction/DateWiseList")]
        public object DateWiseList(Int64 userCompanycode, Int64 usercode, String fromDate, String toDate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                DateTime fdate = Convert.ToDateTime(fromDate);
                DateTime tdate = Convert.ToDateTime(toDate);

                List<McrMTrans> list = new List<McrMTrans>();
                list = (from m in db.McrMTransesDbSet where m.Compid == userCompanycode && m.Transdt >= fdate && m.Transdt <= tdate select m).ToList();
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
        [System.Web.Http.Route("api/MemberTransaction/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, String transType, String transMonthYear, Int64 transno, Int64 transSerial)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<ViewMcrMTrans> list = new List<ViewMcrMTrans>();
                if (transSerial == 1)
                {
                    var getData = (from m in db.McrMTransesDbSet where m.Compid == userCompanycode && m.Transtp == transType && m.Transmy == transMonthYear && m.Transno == transno && m.Transsl == transSerial select m).ToList();
                    foreach (var get in getData)
                    {
                        ViewMcrMTrans model = new ViewMcrMTrans();
                        model.Compid = get.Compid;
                        model.Transtp = get.Transtp;
                        model.Transmy = get.Transmy;
                        model.Transno = get.Transno;
                        model.Transsl = get.Transsl;
                        model.Transdt = get.Transdt;

                        model.Transfor = get.Transfor;
                        model.Transmode = get.Transmode;
                        model.Glheadid = get.Glheadid;
                        model.Schemeid = get.Schemeid;
                        model.Memberid = get.Memberid;
                        model.Internid = get.Internid;
                        model.Amount = get.Amount;
                        model.Remarks = get.Remarks;

                        var findMemberName = (from m in db.McrMembersDbSet
                                              where m.Compid == userCompanycode && m.Memberid == get.Memberid
                                              select m).ToList();
                        foreach (var getMember in findMemberName)
                        {
                            model.MemberName = getMember.Membernm;
                        }

                        var findGlHeadName = (from m in db.GlAcchartDbSet where m.Compid == userCompanycode && m.Accountcd==get.Glheadid select m).ToList();
                        foreach (var getMember in findGlHeadName)
                        {
                            model.GlheadName = getMember.Accountnm;
                        }
                        list.Add(model);
                    }
                }
                else
                {
                    var getData = (from m in db.McrMTransesDbSet where m.Compid == userCompanycode && m.Transtp == transType && m.Transmy == transMonthYear && m.Transno == transno select m).ToList();
                    foreach (var get in getData)
                    {
                        ViewMcrMTrans model = new ViewMcrMTrans();
                        model.Compid = get.Compid;
                        model.Transtp = get.Transtp;
                        model.Transmy = get.Transmy;
                        model.Transno = get.Transno;
                        model.Transsl = get.Transsl;
                        model.Transdt = get.Transdt;

                        model.Transfor = get.Transfor;
                        model.Transmode = get.Transmode;
                        model.Glheadid = get.Glheadid;
                        model.Schemeid = get.Schemeid;
                        model.Memberid = get.Memberid;
                        model.Internid = get.Internid;
                        model.Amount = get.Amount;
                        model.Remarks = get.Remarks;

                        var findMemberName = (from m in db.McrMembersDbSet
                                              where m.Compid == userCompanycode && m.Memberid == get.Memberid
                                              select m).ToList();
                        foreach (var getMember in findMemberName)
                        {
                            model.MemberName = getMember.Membernm;
                        }

                        var findGlHeadName = (from m in db.GlAcchartDbSet where m.Compid == userCompanycode && m.Accountcd == get.Glheadid select m).ToList();
                        foreach (var getMember in findGlHeadName)
                        {
                            model.GlheadName = getMember.Accountnm;
                        }
                        list.Add(model);
                    }
                }

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
        [System.Web.Http.Route("api/MemberTransaction/GetTransNo")]
        public object GetTransNo(Int64 userCompanycode, Int64 usercode, String transType, DateTime transDate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<ViewMcrMTrans> list = new List<ViewMcrMTrans>();
                var getTransNo = (from m in db.McrMTransesDbSet where m.Compid == userCompanycode && m.Transtp == transType && m.Transdt == transDate select new {m.Transno,m.Compid}).Distinct().ToList();
                foreach (var get in getTransNo)
                {
                    ViewMcrMTrans model = new ViewMcrMTrans();
                    model.Compid = get.Compid;
                    model.Transno = get.Transno;
                    list.Add(model);
                }
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
        [System.Web.Http.Route("api/MemberTransaction/FindMaxTransNo")]
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
                Int64 maxtransNo = Convert.ToInt64((from m in db.McrMTransesDbSet where m.Compid == userCompanycode && m.Transtp == transType && m.Transmy == transMonth select m.Transno).DefaultIfEmpty().Max());
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
