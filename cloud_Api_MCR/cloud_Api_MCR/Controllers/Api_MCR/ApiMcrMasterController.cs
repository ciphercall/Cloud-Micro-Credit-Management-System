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
    public class ApiMcrMasterController : ApiController
    {
        private DatabaseDbContext db;

        public ApiMcrMasterController()
        {
            db = new DatabaseDbContext();
        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/McrMaster/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMaster passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token) && passModel.Transno != 0)
            {
                try
                {
                    McrMaster model = new McrMaster();

                    string dateTimetoString = Convert.ToString(passModel.Transdt.ToString("dd-MMM-yyyy"));
                    string getYear = dateTimetoString.Substring(9, 2);
                    string getMonth = dateTimetoString.Substring(3, 3).ToUpper();
                    string transMonth = getMonth + "-" + getYear;
                    

                    Int64 maxTranssl = Convert.ToInt64((from m in db.McrMastersDbSet where m.Compid == userCompanycode && m.Transtp == passModel.Transtp && m.Transmy == transMonth && m.Transno == passModel.Transno select m.Transsl).DefaultIfEmpty().Max());
                    if (maxTranssl == 0)
                    {
                        model.Transsl = Convert.ToInt64("90001");
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
                    model.Transdt = Convert.ToDateTime(passModel.Transdt);
                    model.Schemeid = passModel.Schemeid;
                    model.Memberid = passModel.Memberid;
                    model.Internid = passModel.Internid;
                    model.Transdrcr = passModel.Transdrcr;
                    if (model.Transdrcr == "DEBIT")
                    {
                        model.Debitamt = passModel.Debitamt;
                    }
                    else //model.Transdrcr == "CREDIT"
                    {
                        model.Creditamt = passModel.Creditamt;
                    }
                    model.Transfor = passModel.Transfor;
                    model.Transmode = passModel.Transmode;
                    model.Tableid = passModel.Tableid;
                    model.Remarks = passModel.Remarks;
                    model.Remarks = passModel.Remarks;


                    model.Userpc = UserlogAddress.UserPc();
                    model.Insuserid = usercode;
                    model.Instime = UserlogAddress.Timezone(DateTime.Now);
                    model.Insipno = UserlogAddress.IpAddress();
                    model.Insltude = passModel.Insltude;
                    db.McrMastersDbSet.Add(model);
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

                    //Create Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "CREATE";
                    aslLog.Logltude = passModel.Insltude;
                    aslLog.Logdata = Convert.ToString("Master Input page. TransType: " + passModel.Transtp + ",\nDate: " + transDate + ",\nTransNo: " + passModel.Transno + ",\nTrans-Serail: " + passModel.Transsl + ",\nScheme: " + passModel.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + passModel.Internid + ",\nRemarks: " + passModel.Remarks + ".");
                    aslLog.Tableid = "MCR_MASTER";
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
        [System.Web.Http.Route("api/McrMaster/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMaster passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findMasterInfo = (from m in db.McrMastersDbSet where m.Compid == userCompanycode && m.Transtp == passModel.Transtp && m.Transmy == passModel.Transmy && m.Transno == passModel.Transno && m.Transsl == passModel.Transsl select m).ToList();
                    if (findMasterInfo.Count == 1)
                    {
                        //update logic
                        foreach (var get in findMasterInfo)
                        {
                            get.Schemeid = passModel.Schemeid;
                            get.Memberid = passModel.Memberid;
                            get.Internid = passModel.Internid;
                            get.Transdrcr = passModel.Transdrcr;
                            get.Transfor = passModel.Transfor;
                            get.Transmode = passModel.Transmode;
                            get.Debitamt = passModel.Debitamt;
                            get.Creditamt = passModel.Creditamt;
                            get.Tableid = passModel.Tableid;
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
                        aslLog.Logdata = Convert.ToString("Master Input page. TransType: " + passModel.Transtp + ",\nDate: " + transDate + ",\nTransNo: " + passModel.Transno + ",\nTrans-Serail: " + passModel.Transsl + ",\nScheme: " + passModel.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + passModel.Internid + ",\nRemarks: " + passModel.Remarks + ".");
                        aslLog.Tableid = "MCR_MASTER";
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
        [System.Web.Http.Route("api/McrMaster/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMaster passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrMaster model = db.McrMastersDbSet.Find(passModel.Compid, passModel.Transtp, passModel.Transmy, passModel.Transno, passModel.Transsl);
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
                        aslLog.Logdata = Convert.ToString("Master Input page. TransType: " + model.Transtp + ",\nDate: " + transDate + ",\nTransNo: " + model.Transno + ",\nTrans-Serail: " + model.Transsl + ",\nScheme: " + model.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + model.Internid + ",\nRemarks: " + model.Remarks + ".");
                        aslLog.Tableid = "MCR_MASTER";
                        Log.SaveLog(aslLog);

                        //Delete Log Data Save in to ASL_DELETE Table
                        AslDelete aslDeleteLog = new AslDelete();
                        aslDeleteLog.Compid = userCompanycode;
                        aslDeleteLog.Userid = usercode;
                        aslDeleteLog.Delltude = passModel.Updltude;
                        aslDeleteLog.Deldata = Convert.ToString("Master Input page. TransType: " + model.Transtp + ",\nDate: " + transDate + ",\nTransNo: " + model.Transno + ",\nTrans-Serail: " + model.Transsl + ",\nScheme: " + model.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + model.Internid + ",\nRemarks: " + model.Remarks + ".");
                        aslDeleteLog.Tableid = "MCR_MASTER";
                        Log.DeleteLog(aslDeleteLog);

                        //Delete From AslUserco
                        db.McrMastersDbSet.Remove(model);
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
        [System.Web.Http.Route("api/McrMaster/List")]
        public object List(Int64 userCompanycode, Int64 usercode, String transType, String transMonthYear, Int64 transno)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrMaster> list = new List<McrMaster>();
                list = (from m in db.McrMastersDbSet where m.Compid == userCompanycode && m.Transtp == transType && m.Transmy == transMonthYear && m.Transno == transno select m).ToList();
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
        [System.Web.Http.Route("api/McrMaster/AsOnDateAndSchemeWiseList")]
        public object AsOnDateAndSchemeWiseList(Int64 userCompanycode, Int64 usercode, String date, String schemeid)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                DateTime transDate = Convert.ToDateTime(date);

                List<McrMaster> list = new List<McrMaster>();
                list = (from m in db.McrMastersDbSet where m.Compid == userCompanycode && m.Transdt <= transDate && m.Schemeid == schemeid select m).ToList();
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
        [System.Web.Http.Route("api/McrMaster/DateWiseList")]
        public object DateWiseList(Int64 userCompanycode, Int64 usercode, String fromdate, String toDate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                DateTime fDate = Convert.ToDateTime(fromdate);
                DateTime tDate = Convert.ToDateTime(toDate);

                List<McrMaster> list = new List<McrMaster>();
                list = (from m in db.McrMastersDbSet where m.Compid == userCompanycode && fDate <= m.Transdt && m.Transdt <= tDate select m).ToList();
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
        [System.Web.Http.Route("api/McrMaster/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, String transType, String transMonthYear, Int64 transno, Int64 transSerial)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrMaster> list = new List<McrMaster>();
                list = (from m in db.McrMastersDbSet where m.Compid == userCompanycode && m.Transtp == transType && m.Transmy == transMonthYear && m.Transno == transno && m.Transsl == transSerial select m).ToList();
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
        [System.Web.Http.Route("api/McrMaster/FindMaxTransNo")]
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
                Int64 maxtransNo = Convert.ToInt64((from m in db.McrMastersDbSet where m.Compid == userCompanycode && m.Transtp == "OPEN" && m.Transmy == transMonth select m.Transno).DefaultIfEmpty().Max());
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
