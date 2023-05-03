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
    public class ApiCollectController : ApiController
    {
        private DatabaseDbContext db;

        public ApiCollectController()
        {
            db = new DatabaseDbContext();
        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Collect/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrCollect passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token) && passModel.Transno != 0 && passModel.Branchid != 0 && passModel.Fwid != 0)
            {
                try
                {
                    McrCollect model = new McrCollect();
                    
                    string dateTimetoString = Convert.ToString(passModel.Transdt.ToString("dd-MMM-yyyy"));
                    string getYear = dateTimetoString.Substring(9, 2);
                    string getMonth = dateTimetoString.Substring(3, 3).ToUpper();
                    string transMonth = getMonth + "-" + getYear;
                    
                    Int64 maxTranssl = Convert.ToInt64((from m in db.McrCollectsDbSet where m.Compid == userCompanycode && m.Transmy == transMonth && m.Transno == passModel.Transno select m.Transsl).DefaultIfEmpty().Max());
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
                    model.Transmy = transMonth;
                    model.Transno = passModel.Transno;
                    model.Transsl = model.Transsl;
                    model.Transdt = passModel.Transdt;
                    model.Branchid = passModel.Branchid;
                    model.Fwid = passModel.Fwid;
                    model.Schemeid = passModel.Schemeid;
                    model.Memberid = passModel.Memberid;
                    model.Internid = passModel.Internid;
                    model.Amount = passModel.Amount;
                    model.Remarks = passModel.Remarks;

                    model.Userpc = UserlogAddress.UserPc();
                    model.Insuserid = usercode;
                    model.Instime = UserlogAddress.Timezone(DateTime.Now);
                    model.Insipno = UserlogAddress.IpAddress();
                    model.Insltude = passModel.Insltude;
                    db.McrCollectsDbSet.Add(model);
                    db.SaveChanges();


                    string transDate = Convert.ToString(passModel.Transdt.ToString("dd-MMM-yyyy"));

                    String fieldWorkerName = "", branchName = "", memberName = "";
                    var findBranchName = (from m in db.McrBranchesDbSet
                                          where m.Compid == userCompanycode && m.Branchid == passModel.Branchid
                                          select m).ToList();
                    foreach (var get in findBranchName)
                    {
                        branchName = get.Branchnm;
                    }
                    var findWorkerName = (from m in db.GlAcchartDbSet
                                          where m.Compid == userCompanycode && m.Accountcd == passModel.Fwid
                                          select m).ToList();
                    foreach (var get in findWorkerName)
                    {
                        fieldWorkerName = get.Accountnm;
                    }
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
                    aslLog.Logdata = Convert.ToString("Collect Input page. Date: " + transDate + ",\nBranch Name: " + branchName + ",\nField Worker Name: " + fieldWorkerName + ",\nScheme: " + passModel.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + passModel.Internid + ",\nAmount: " + passModel.Amount + ",\nRemarks: " + passModel.Remarks + ".");
                    aslLog.Tableid = "MCR_COLLECT";
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
        [System.Web.Http.Route("api/Collect/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrCollect passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findCollectInfo = (from m in db.McrCollectsDbSet where m.Compid == passModel.Compid && m.Transmy == passModel.Transmy && m.Transno == passModel.Transno && m.Transsl== passModel.Transsl select m).ToList();
                    if (findCollectInfo.Count == 1)
                    {
                        //update logic
                        foreach (var get in findCollectInfo)
                        {
                            get.Schemeid = passModel.Schemeid;
                            get.Memberid = passModel.Memberid;
                            get.Internid = passModel.Internid;
                            get.Amount = passModel.Amount;
                            get.Remarks = passModel.Remarks;

                            get.Userpc = UserlogAddress.UserPc();
                            get.Upduserid = usercode;
                            get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                            get.Updipno = UserlogAddress.IpAddress();
                            get.Updltude = passModel.Updltude;
                        }
                        db.SaveChanges();

                        string transDate = Convert.ToString(passModel.Transdt.ToString("dd-MMM-yyyy"));

                        String fieldWorkerName = "", branchName = "", memberName = "";
                        var findBranchName = (from m in db.McrBranchesDbSet
                                              where m.Compid == userCompanycode && m.Branchid == passModel.Branchid
                                              select m).ToList();
                        foreach (var get in findBranchName)
                        {
                            branchName = get.Branchnm;
                        }
                        var findWorkerName = (from m in db.GlAcchartDbSet
                                              where m.Compid == userCompanycode && m.Accountcd == passModel.Fwid
                                              select m).ToList();
                        foreach (var get in findWorkerName)
                        {
                            fieldWorkerName = get.Accountnm;
                        }
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
                        aslLog.Logdata = Convert.ToString("Collect Input page. Date: " + transDate + ",\nBranch Name: " + branchName + ",\nField Worker Name: " + fieldWorkerName + ",\nScheme: " + passModel.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + passModel.Internid + ",\nAmount: " + passModel.Amount + ",\nRemarks: " + passModel.Remarks + ".");
                        aslLog.Tableid = "MCR_COLLECT";
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
        [System.Web.Http.Route("api/Collect/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrCollect passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrCollect model = db.McrCollectsDbSet.Find(passModel.Compid, passModel.Transmy, passModel.Transno,passModel.Transsl);
                    if (model != null)
                    {
                        string transDate = Convert.ToString(model.Transdt.ToString("dd-MMM-yyyy"));

                        String fieldWorkerName = "", branchName = "",memberName="";
                        var findBranchName = (from m in db.McrBranchesDbSet
                                              where m.Compid == userCompanycode && m.Branchid == model.Branchid
                                              select m).ToList();
                        foreach (var get in findBranchName)
                        {
                            branchName = get.Branchnm;
                        }
                        var findWorkerName = (from m in db.GlAcchartDbSet
                                            where m.Compid == userCompanycode && m.Accountcd == model.Fwid
                                            select m).ToList();
                        foreach (var get in findWorkerName)
                        {
                            fieldWorkerName = get.Accountnm;
                        }
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
                        aslLog.Logdata = Convert.ToString("Collect Input page. Date: " + transDate + ",\nBranch Name: " + branchName + ",\nField Worker Name: " + fieldWorkerName + ",\nScheme: " + model.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + model.Internid + ",\nAmount: " + model.Amount + ",\nRemarks: " + model.Remarks + ".");
                        aslLog.Tableid = "MCR_COLLECT";
                        Log.SaveLog(aslLog);

                        //Delete Log Data Save in to ASL_DELETE Table
                        AslDelete aslDeleteLog = new AslDelete();
                        aslDeleteLog.Compid = userCompanycode;
                        aslDeleteLog.Userid = usercode;
                        aslDeleteLog.Delltude = passModel.Updltude;
                        aslDeleteLog.Deldata = Convert.ToString("Collect Input page. Date: " + transDate + ",\nBranch Name: " + branchName + ",\nField Worker Name: " + fieldWorkerName + ",\nScheme: " + model.Schemeid + ",\nMember Name: " + memberName + ",\nInternal ID: " + model.Internid + ",\nAmount: " + model.Amount + ",\nRemarks: " + model.Remarks + ".");
                        aslDeleteLog.Tableid = "MCR_COLLECT";
                        Log.DeleteLog(aslDeleteLog);

                        //Delete From AslUserco
                        db.McrCollectsDbSet.Remove(model);
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
        [System.Web.Http.Route("api/Collect/List")]
        public object List(Int64 userCompanycode, Int64 usercode, String transMonthYear, Int64 transno)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<ViewMcrCollect> list = new List<ViewMcrCollect>();
                var getData = (from m in db.McrCollectsDbSet where m.Compid == userCompanycode && m.Transmy == transMonthYear && m.Transno == transno select m).ToList();
                foreach (var get in getData)
                {
                    ViewMcrCollect model = new ViewMcrCollect();
                    model.Compid = get.Compid;
                    model.Transmy = get.Transmy;
                    model.Transno = get.Transno;
                    model.Transsl = get.Transsl;
                    model.Transdt = get.Transdt;
                    model.Branchid = get.Branchid;
                    model.Fwid = get.Fwid;
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
        [System.Web.Http.Route("api/Collect/DateWiseTransNoList")]
        public object DateWiseTransNoList(Int64 userCompanycode, Int64 usercode, String transDate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                DateTime date = Convert.ToDateTime(transDate);
                var list = (from m in db.McrCollectsDbSet where m.Compid == userCompanycode && m.Transdt == date select new {m.Compid,m.Transno}).Distinct().ToList();
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
        [System.Web.Http.Route("api/Collect/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, String transMonthYear, Int64 transno, Int64 transSerial)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrCollect> list = new List<McrCollect>();
                list = (from m in db.McrCollectsDbSet where m.Compid == userCompanycode && m.Transmy == transMonthYear && m.Transno == transno && m.Transsl == transSerial select m).ToList();
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
        [System.Web.Http.Route("api/Collect/FindMaxTransNo")]
        public object FindMaxTransNo(Int64 userCompanycode, Int64 usercode, DateTime transDate)
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
                Int64 maxtransNo = Convert.ToInt64((from m in db.McrCollectsDbSet where m.Compid == userCompanycode  && m.Transmy == transMonth select m.Transno).DefaultIfEmpty().Max());
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
