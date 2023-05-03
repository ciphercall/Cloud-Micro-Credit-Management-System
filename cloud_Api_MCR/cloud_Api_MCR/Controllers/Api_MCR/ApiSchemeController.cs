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
    public class ApiSchemeController : ApiController
    {
        private DatabaseDbContext db;

        public ApiSchemeController()
        {
            db = new DatabaseDbContext();
        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Scheme/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrScheme passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token) && (passModel.Schemetp != null || passModel.Schemetp != "") && (passModel.Schemeid!=null || passModel.Schemeid!="") && passModel.Glheadid!=0)
            {
                try
                {
                    McrScheme model = new McrScheme();

                    Int64 maxSchemeCd = Convert.ToInt64((from m in db.McrSchemesDbSet where m.Compid == userCompanycode select m.Schemecd).DefaultIfEmpty().Max());
                    if (maxSchemeCd == 0)
                    {
                        model.Schemecd = Convert.ToInt64(userCompanycode + "01");
                    }
                    else if (maxSchemeCd != 0)
                    {
                        Int64 maxLimit = Convert.ToInt64(userCompanycode + "99");
                        Int64 id = maxSchemeCd + 1;
                        if (id <= maxLimit)
                        {
                            model.Schemecd = id;
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
                    model.Schemecd = model.Schemecd;
                    model.Schemeid = passModel.Schemeid;
                    model.Schemetp = passModel.Schemetp;
                    model.Glheadid = passModel.Glheadid;
                    model.GlIncomehid = passModel.GlIncomehid;
                    model.Remarks = passModel.Remarks;

                    model.Userpc = UserlogAddress.UserPc();
                    model.Insuserid = usercode;
                    model.Instime = UserlogAddress.Timezone(DateTime.Now);
                    model.Insipno = UserlogAddress.IpAddress();
                    model.Insltude = passModel.Insltude;
                    db.McrSchemesDbSet.Add(model);
                    db.SaveChanges();


                    String accountName = "";
                    var findAccountName = (from m in db.GlAcchartDbSet where m.Compid == userCompanycode && m.Accountcd == passModel.Glheadid select m).ToList();
                    foreach (var get in findAccountName)
                    {
                        accountName = get.Accountnm;
                    }


                    //Create Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "CREATE";
                    aslLog.Logltude = passModel.Insltude;
                    aslLog.Logdata = Convert.ToString("Scheme Input page. Scheme Name: " + passModel.Schemeid + ",\nScheme Type: " + passModel.Schemetp + ",\nAccount Name: " + accountName + ",\nRemarks: " + passModel.Remarks + ".");
                    aslLog.Tableid = "MCR_SCHEME";
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
        [System.Web.Http.Route("api/Scheme/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrScheme passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            String logData = "";
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token) && (passModel.Schemetp != null || passModel.Schemetp != "") && (passModel.Schemeid != null || passModel.Schemeid != "") && passModel.Glheadid != 0)
            {
                try
                {
                    var findSchemeInfo = (from m in db.McrSchemesDbSet where m.Compid == passModel.Compid && m.Schemecd == passModel.Schemecd select m).ToList();
                    if (findSchemeInfo.Count == 1)
                    {
                        var checkSchemeId = (from m in db.McrSchemesDbSet where m.Compid == passModel.Compid && m.Schemeid==passModel.Schemeid && m.Schemetp==passModel.Schemetp && m.Glheadid==passModel.Glheadid && m.GlIncomehid == passModel.GlIncomehid select m).ToList();
                        if (checkSchemeId.Count != 0)
                        {
                            return new
                            {
                                success = false,
                                message = "Data already exists!!"
                            };
                        }

                        String accountName = "";
                        var findAccountName = (from m in db.GlAcchartDbSet where m.Compid == userCompanycode && m.Accountcd == passModel.Glheadid select m).ToList();
                        foreach (var get in findAccountName)
                        {
                            accountName = get.Accountnm;
                        }
                        
                        //update logic
                        foreach (var get in findSchemeInfo)
                        {
                            get.Schemeid = passModel.Schemeid;
                            get.Schemetp = passModel.Schemetp;
                            get.Glheadid = passModel.Glheadid;
                            get.GlIncomehid = passModel.GlIncomehid;
                            get.Remarks = passModel.Remarks;

                            get.Userpc = UserlogAddress.UserPc();
                            get.Upduserid = usercode;
                            get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                            get.Updipno = UserlogAddress.IpAddress();
                            get.Updltude = passModel.Updltude;

                            logData = Convert.ToString("Scheme Input page. Scheme Name: " + passModel.Schemeid + ",\nScheme Type: " + passModel.Schemetp + ",\nAccount Name: " + accountName + ",\nRemarks: " + passModel.Remarks + ".");
                        }
                        db.SaveChanges();

                        //Update Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "UPDATE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = logData;
                        aslLog.Tableid = "MCR_SCHEME";
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
                        message = "No Data Found!!"
                    };
                }
            }
            return new
            {
                success = false,
                message = "Authorized not permitted!!"
            };
        }






        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Scheme/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrScheme passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrScheme model = db.McrSchemesDbSet.Find(passModel.Compid, passModel.Schemecd);
                    if (model != null)
                    {
                        String accountName = "";
                        var findAccountName = (from m in db.GlAcchartDbSet where m.Compid == userCompanycode && m.Accountcd== model.Glheadid select m).ToList();
                        foreach (var get in findAccountName)
                        {
                            accountName = get.Accountnm;
                        }
                        
                        //Delete Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "DELETE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = Convert.ToString("Scheme Input page. Scheme Name: " + model.Schemeid + ",\nScheme Type: " + model.Schemetp + ",\nAccount Name: " + accountName + ",\nRemarks: " + model.Remarks + ".");
                        aslLog.Tableid = "MCR_SCHEME";
                        Log.SaveLog(aslLog);

                        //Delete Log Data Save in to ASL_DELETE Table
                        AslDelete aslDeleteLog = new AslDelete();
                        aslDeleteLog.Compid = userCompanycode;
                        aslDeleteLog.Userid = usercode;
                        aslDeleteLog.Delltude = passModel.Updltude;
                        aslDeleteLog.Deldata = Convert.ToString("Scheme Input page. Scheme Name: " + model.Schemeid + ",\nScheme Type: " + model.Schemetp + ",\nAccount Name: " + accountName + ",\nRemarks: " + model.Remarks + ".");
                        aslDeleteLog.Tableid = "MCR_SCHEME";
                        Log.DeleteLog(aslDeleteLog);

                        //Delete From AslUserco
                        db.McrSchemesDbSet.Remove(model);
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
        [System.Web.Http.Route("api/Scheme/List")]
        public object List(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<ViewMcrScheme> list = new List<ViewMcrScheme>();
                var getData = (from m in db.McrSchemesDbSet where m.Compid == userCompanycode select m).ToList();
                foreach (var get in getData)
                {
                    ViewMcrScheme model = new ViewMcrScheme();
                    model.Compid = get.Compid; 
                    model.Schemecd = get.Schemecd;
                    model.Schemeid = get.Schemeid;
                    model.Schemetp = get.Schemetp;
                    model.Glheadid = get.Glheadid;
                    model.GlIncomehid = get.GlIncomehid;
                    model.Remarks = get.Remarks;
                    var findWorkerName = (from m in db.GlAcchartDbSet
                                          where m.Compid == userCompanycode && m.Accountcd == get.Glheadid
                                          select m).ToList();
                    foreach (var getfindWorkerName in findWorkerName)
                    {
                        model.GlHeadName = getfindWorkerName.Accountnm;
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
        [System.Web.Http.Route("api/Scheme/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, String schemeId)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrScheme> list = new List<McrScheme>();
                list = (from m in db.McrSchemesDbSet where m.Compid == userCompanycode && m.Schemeid == schemeId select m).ToList();
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
