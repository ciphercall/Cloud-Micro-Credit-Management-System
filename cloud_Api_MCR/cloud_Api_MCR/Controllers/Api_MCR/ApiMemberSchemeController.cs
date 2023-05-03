using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.ASL;
using cloud_Api_MCR.Models.MCR;
using cloud_Api_MCR.Models.MCR_DTO;

namespace cloud_Api_MCR.Controllers.Api_MCR
{
    public class ApiMemberSchemeController : ApiController
    {
        private DatabaseDbContext db;

        public ApiMemberSchemeController()
        {
            db = new DatabaseDbContext();
        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/MemberScheme/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMScheme passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrMScheme model = new McrMScheme();

                    Int64 maxSchemeSl = Convert.ToInt64((from m in db.McrMSchemesDbSet where m.Compid == userCompanycode && m.Memberid==passModel.Memberid select m.Schemesl).DefaultIfEmpty().Max());
                    if (maxSchemeSl == 0)
                    {
                        model.Schemesl = Convert.ToInt64(passModel.Memberid + "01");
                    }
                    else if (maxSchemeSl != 0)
                    {
                        Int64 maxLimit = Convert.ToInt64(passModel.Memberid + "99");
                        Int64 id = maxSchemeSl + 1;
                        if (id <= maxLimit)
                        {
                            model.Schemesl = id;
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
                    model.Memberid = passModel.Memberid;
                    model.Schemesl = model.Schemesl;
                    model.Schemeid = passModel.Schemeid;
                    model.Internid = passModel.Internid;
                    if (passModel.Schemeefdt != null)
                    {
                        model.Schemeefdt = Convert.ToDateTime(passModel.Schemeefdt);
                    }
                    if (passModel.Schemeetdt!=null)
                    {
                        model.Schemeetdt = Convert.ToDateTime(passModel.Schemeetdt);
                    }
                    model.Remarks = passModel.Remarks;
                    model.Status = passModel.Status;
                    
                    model.Userpc = UserlogAddress.UserPc();
                    model.Insuserid = usercode;
                    model.Instime = UserlogAddress.Timezone(DateTime.Now);
                    model.Insipno = UserlogAddress.IpAddress();
                    model.Insltude = passModel.Insltude;
                    db.McrMSchemesDbSet.Add(model);
                    db.SaveChanges();

                    string fromDate = "", toDate = "";
                    if (passModel.Schemeefdt != null)
                    {
                        DateTime fdate = Convert.ToDateTime(passModel.Schemeefdt);
                        fromDate = fdate.ToString("dd-MMM-yyyy");
                    }
                    if (passModel.Schemeetdt != null)
                    {
                        DateTime tdate = Convert.ToDateTime(passModel.Schemeetdt);
                        toDate = tdate.ToString("dd-MMM-yyyy");
                    }

                    //Create Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "CREATE";
                    aslLog.Logltude = passModel.Insltude;
                    aslLog.Logdata = Convert.ToString("Member Scheme Input page. Scheme: " + passModel.Schemeid + ",\nInternal ID: " + passModel.Internid +",\nFrom Date:"+ fromDate + ",\nTo Date:" + toDate + ",\nRemarks: " + passModel.Remarks + ",\nStatus: " + passModel.Status + ".");
                    aslLog.Tableid = "MCR_MSCHEME";
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
        [System.Web.Http.Route("api/MemberScheme/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMScheme passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findMemberSchemeInfo = (from m in db.McrMSchemesDbSet where m.Compid == passModel.Compid  && m.Memberid == passModel.Memberid && m.Schemesl == passModel.Schemesl select m).ToList();
                    if (findMemberSchemeInfo.Count == 1)
                    {
                        //update logic
                        foreach (var get in findMemberSchemeInfo)
                        {
                            get.Schemeid = passModel.Schemeid;
                            get.Internid = passModel.Internid;
                            if (passModel.Schemeefdt != null)
                            {
                                get.Schemeefdt = Convert.ToDateTime(passModel.Schemeefdt);
                            }
                            if (passModel.Schemeetdt!=null)
                            {
                                get.Schemeetdt = Convert.ToDateTime(passModel.Schemeetdt);
                            }
                           
                            get.Remarks = passModel.Remarks;
                            if (passModel.Status == "")
                            {
                                get.Status = "I";
                            }
                            else if(passModel.Status == "I" || passModel.Status == "A")
                            {
                                get.Status = passModel.Status;
                            }
                            else
                            {
                                get.Status = "I";
                            }

                            get.Userpc = UserlogAddress.UserPc();
                            get.Upduserid = usercode;
                            get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                            get.Updipno = UserlogAddress.IpAddress();
                            get.Updltude = passModel.Updltude;
                        }
                        db.SaveChanges();


                        string fromDate = "", toDate = "";
                        if (passModel.Schemeefdt != null)
                        {
                            DateTime fdate = Convert.ToDateTime(passModel.Schemeefdt);
                            fromDate = fdate.ToString("dd-MMM-yyyy");
                        }
                        if (passModel.Schemeetdt != null)
                        {
                            DateTime tdate = Convert.ToDateTime(passModel.Schemeetdt);
                            toDate = tdate.ToString("dd-MMM-yyyy");
                        }

                        //Update Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "UPDATE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = Convert.ToString("Member Scheme Input page. Scheme: " + passModel.Schemeid + ",\nInternal ID: " + passModel.Internid + ",\nFrom Date:" + fromDate + ",\nTo Date:" + toDate + ",\nRemarks: " + passModel.Remarks + ",\nStatus: " + passModel.Status + ".");
                        aslLog.Tableid = "MCR_MSCHEME";
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
        [System.Web.Http.Route("api/MemberScheme/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMScheme passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrMScheme model = db.McrMSchemesDbSet.Find(passModel.Compid, passModel.Memberid, passModel.Schemesl);
                    if (model != null)
                    {
                        string fromDate = "", toDate = "";
                        if (passModel.Schemeefdt != null)
                        {
                            DateTime fdate = Convert.ToDateTime(model.Schemeefdt);
                            fromDate = fdate.ToString("dd-MMM-yyyy");
                        }
                        if (passModel.Schemeetdt != null)
                        {
                            DateTime tdate = Convert.ToDateTime(model.Schemeetdt);
                            toDate = tdate.ToString("dd-MMM-yyyy");
                        }


                        //Delete Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "DELETE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = Convert.ToString("Member Scheme Input page. Scheme: " + model.Schemeid + ",\nInternal ID: " + model.Internid + ",\nFrom Date:" + fromDate + ",\nTo Date:" + toDate + ",\nRemarks: " + model.Remarks + ",\nStatus: " + model.Status + ".");
                        aslLog.Tableid = "MCR_MSCHEME";
                        Log.SaveLog(aslLog);

                        //Delete Log Data Save in to ASL_DELETE Table
                        AslDelete aslDeleteLog = new AslDelete();
                        aslDeleteLog.Compid = userCompanycode;
                        aslDeleteLog.Userid = usercode;
                        aslDeleteLog.Delltude = passModel.Updltude;
                        aslDeleteLog.Deldata = Convert.ToString("Member Scheme Input page. Scheme: " + model.Schemeid + ",\nInternal ID: " + model.Internid + ",\nFrom Date:" + fromDate + ",\nTo Date:" + toDate + ",\nRemarks: " + model.Remarks + ",\nStatus: " + model.Status + ".");
                        aslDeleteLog.Tableid = "MCR_MSCHEME";
                        Log.DeleteLog(aslDeleteLog);

                        //Delete From AslUserco
                        db.McrMSchemesDbSet.Remove(model);
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
        [System.Web.Http.Route("api/MemberScheme/List")]
        public object List(Int64 userCompanycode, Int64 usercode, Int64 memberId)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<ViewMcrMScheme> list = new List<ViewMcrMScheme>();
                var getData = (from m in db.McrMSchemesDbSet where m.Compid == userCompanycode && m.Memberid == memberId select m).ToList();
                foreach (var get in getData)
                {
                    ViewMcrMScheme model = new ViewMcrMScheme();
                    model.Compid = get.Compid;
                    model.Memberid = get.Memberid;
                    model.Schemesl = get.Schemesl;
                    model.Schemeid = get.Schemeid;
                    model.Internid = get.Internid;

                    string fDate = "", tDate = "";
                    if (get.Schemeefdt != null)
                    {
                        DateTime fdt = Convert.ToDateTime(get.Schemeefdt);
                        fDate = Convert.ToString(fdt.ToString("dd-MMM-yyyy"));
                    }
                    if (get.Schemeetdt != null)
                    {
                        DateTime tdt = Convert.ToDateTime(get.Schemeetdt);
                        tDate = Convert.ToString(tdt.ToString("dd-MMM-yyyy"));
                    }
                    model.Schemeefdt = fDate;
                    model.Schemeetdt = tDate;
                    model.Remarks = get.Remarks;
                    if (get.Status=="A")
                    {
                        model.Status = "ACTIVE";
                    }
                    else if (get.Status == "I")
                    {
                        model.Status = "INACTIVE";
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
        [System.Web.Http.Route("api/MemberScheme/TotalList")]
        public object TotalList(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<ViewMcrMScheme> list = new List<ViewMcrMScheme>();
                var getData = (from m in db.McrMSchemesDbSet where m.Compid == userCompanycode select m).ToList();
                foreach (var get in getData)
                {
                    ViewMcrMScheme model = new ViewMcrMScheme();
                    model.Compid = get.Compid;
                    model.Memberid = get.Memberid;
                    model.Schemesl = get.Schemesl;
                    model.Schemeid = get.Schemeid;
                    model.Internid = get.Internid;
                    string fDate = "", tDate = "";
                    if (get.Schemeefdt != null)
                    {
                        DateTime fdt = Convert.ToDateTime(get.Schemeefdt);
                        fDate = Convert.ToString(fdt.ToString("dd-MMM-yyyy"));
                    }
                    if (get.Schemeetdt != null)
                    {
                        DateTime tdt = Convert.ToDateTime(get.Schemeetdt);
                        tDate = Convert.ToString(tdt.ToString("dd-MMM-yyyy"));
                    }
                    model.Schemeefdt = fDate;
                    model.Schemeetdt = tDate;
                    model.Remarks = get.Remarks;
                    if (get.Status == "A")
                    {
                        model.Status = "ACTIVE";
                    }
                    else if (get.Status == "I")
                    {
                        model.Status = "INACTIVE";
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
        [System.Web.Http.Route("api/MemberScheme/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, Int64 memberId, Int64 schemeSl)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrMScheme> list = new List<McrMScheme>();
                list = (from m in db.McrMSchemesDbSet where m.Compid == userCompanycode && m.Memberid == memberId && m.Schemesl == schemeSl select m).ToList();
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
