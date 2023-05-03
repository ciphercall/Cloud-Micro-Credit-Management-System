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
using cloud_Api_MCR.Models.MCR;
using cloud_Api_MCR.Models.MCR_DTO;

namespace cloud_Api_MCR.Controllers.Api_MCR
{
    public class ApiBranchController : ApiController
    {
        private DatabaseDbContext db;

        public ApiBranchController()
        {
            db = new DatabaseDbContext();
        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Branch/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrBranch passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrBranch model = new McrBranch();

                    Int64 maxBranchNo = Convert.ToInt64((from m in db.McrBranchesDbSet where m.Compid == userCompanycode select m.Branchid).DefaultIfEmpty().Max());
                    if (maxBranchNo == 0)
                    {
                        model.Branchid = Convert.ToInt64(userCompanycode + "01");
                    }
                    else if (maxBranchNo != 0)
                    {
                        Int64 maxLimit = Convert.ToInt64(userCompanycode + "99");
                        Int64 id = maxBranchNo + 1;
                        if (id <= maxLimit)
                        {
                            model.Branchid = id;
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
                    model.Branchid = model.Branchid;
                    model.Branchnm = passModel.Branchnm;
                    model.Address = passModel.Address;
                    model.Contactno = passModel.Contactno;
                    model.Remarks = passModel.Remarks;

                    model.Userpc = UserlogAddress.UserPc();
                    model.Insuserid = usercode;
                    model.Instime = UserlogAddress.Timezone(DateTime.Now);
                    model.Insipno = UserlogAddress.IpAddress();
                    model.Insltude = passModel.Insltude;
                    db.McrBranchesDbSet.Add(model);
                    db.SaveChanges();


                    //Create Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "CREATE";
                    aslLog.Logltude = passModel.Insltude;
                    aslLog.Logdata = Convert.ToString("Branch Input page. Branch Name: " + model.Branchnm + ",\nAddress: " + model.Address + ",\nContact No: " + model.Contactno + ",\nRemarks: " + model.Remarks + ".");
                    aslLog.Tableid = "MCR_BRANCH";
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
        [System.Web.Http.Route("api/Branch/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrBranch passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            String logData = "";
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findBranchInfo = (from m in db.McrBranchesDbSet where m.Compid == userCompanycode && m.Branchid == passModel.Branchid select m).ToList();
                    if (findBranchInfo.Count == 1)
                    {
                        //update logic
                        foreach (var get in findBranchInfo)
                        {
                            get.Branchnm = passModel.Branchnm;
                            get.Address = passModel.Address;
                            get.Contactno = passModel.Contactno;
                            get.Remarks = passModel.Remarks;

                            get.Userpc = UserlogAddress.UserPc();
                            get.Upduserid = usercode;
                            get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                            get.Updipno = UserlogAddress.IpAddress();
                            get.Updltude = passModel.Updltude;

                            logData = Convert.ToString("Branch Input page. Branch Name: " + passModel.Branchnm + ",\nAddress: " + passModel.Address + ",\nContact No: " + passModel.Contactno + ",\nRemarks: " + passModel.Remarks + ".");
                        }
                        db.SaveChanges();

                        //Update Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "UPDATE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = logData;
                        aslLog.Tableid = "MCR_BRANCH";
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
        [System.Web.Http.Route("api/Branch/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrBranch passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrBranch model = db.McrBranchesDbSet.Find(passModel.Compid, passModel.Branchid);
                    if (model != null)
                    {
                        //Delete Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "DELETE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = Convert.ToString("Branch Input page. Branch Name: " + model.Branchnm + ",\nAddress: " + model.Address + ",\nContact No: " + model.Contactno + ",\nRemarks: " + model.Remarks + ".");
                        aslLog.Tableid = "MCR_BRANCH";
                        Log.SaveLog(aslLog);

                        //Delete Log Data Save in to ASL_DELETE Table
                        AslDelete aslDeleteLog = new AslDelete();
                        aslDeleteLog.Compid = userCompanycode;
                        aslDeleteLog.Userid = usercode;
                        aslDeleteLog.Delltude = passModel.Updltude;
                        aslDeleteLog.Deldata = Convert.ToString("Branch Input page. Branch Name: " + model.Branchnm + ",\nAddress: " + model.Address + ",\nContact No: " + model.Contactno + ",\nRemarks: " + model.Remarks + ".");
                        aslDeleteLog.Tableid = "MCR_BRANCH";
                        Log.DeleteLog(aslDeleteLog);

                        //Delete From AslUserco
                        db.McrBranchesDbSet.Remove(model);
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
        [System.Web.Http.Route("api/Branch/List")]
        public object List(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrBranch> list = new List<McrBranch>();
                list = (from m in db.McrBranchesDbSet where m.Compid == userCompanycode select m).ToList();
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
        [System.Web.Http.Route("api/Branch/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, Int64 branchId)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrBranch> list = new List<McrBranch>();
                list = (from m in db.McrBranchesDbSet where m.Compid == userCompanycode && m.Branchid == branchId select m).ToList();
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
