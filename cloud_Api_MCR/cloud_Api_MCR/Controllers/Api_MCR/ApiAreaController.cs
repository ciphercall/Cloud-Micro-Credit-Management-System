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
    public class ApiAreaController : ApiController
    {
        private DatabaseDbContext db;

        public ApiAreaController()
        {
            db = new DatabaseDbContext();
        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Area/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrArea passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrArea model = new McrArea();

                    Int64 maxBranchNo = Convert.ToInt64((from m in db.McrAreasDbSet where m.Compid == userCompanycode && m.Branchid == passModel.Branchid select m.Areaid).DefaultIfEmpty().Max());
                    if (maxBranchNo == 0)
                    {
                        model.Areaid = Convert.ToInt64(passModel.Branchid + "01");
                    }
                    else if (maxBranchNo != 0)
                    {
                        Int64 maxLimit = Convert.ToInt64(passModel.Branchid + "99");
                        Int64 id = maxBranchNo + 1;
                        if (id <= maxLimit)
                        {
                            model.Areaid = id;
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
                    model.Branchid = passModel.Branchid;
                    model.Areaid = model.Areaid;
                    model.Areanm = passModel.Areanm;
                    model.Authpnm = passModel.Authpnm;
                    model.Fwid = passModel.Fwid;
                    model.Remarks = passModel.Remarks;

                    model.Userpc = UserlogAddress.UserPc();
                    model.Insuserid = usercode;
                    model.Instime = UserlogAddress.Timezone(DateTime.Now);
                    model.Insipno = UserlogAddress.IpAddress();
                    model.Insltude = passModel.Insltude;
                    db.McrAreasDbSet.Add(model);
                    db.SaveChanges();


                    String fieldWorkerName = "";
                    var findWorkerName = (from m in db.GlAcchartDbSet
                                        where m.Compid == userCompanycode && m.Accountcd == passModel.Fwid
                                        select m).ToList();
                    foreach (var get in findWorkerName)
                    {
                        fieldWorkerName = get.Accountnm;
                    }

                    //Create Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "CREATE";
                    aslLog.Logltude = passModel.Insltude;
                    aslLog.Logdata = Convert.ToString("Area Input page. Area Name: " + passModel.Areanm + ",\nAuthorized Person Name: " + passModel.Authpnm + ",\n Field Worker Name: " + fieldWorkerName + ",\nRemarks: " + passModel.Remarks + ".");
                    aslLog.Tableid = "MCR_AREA";
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
        [System.Web.Http.Route("api/Area/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrArea passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();
            
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findBranchInfo = (from m in db.McrAreasDbSet where m.Compid == passModel.Compid && m.Branchid == passModel.Branchid && m.Areaid == passModel.Areaid select m).ToList();
                    if (findBranchInfo.Count == 1)
                    {
                        //update logic
                        foreach (var get in findBranchInfo)
                        {
                            get.Areanm = passModel.Areanm;
                            get.Authpnm = passModel.Authpnm;
                            get.Fwid = passModel.Fwid;
                            get.Remarks = passModel.Remarks;

                            get.Userpc = UserlogAddress.UserPc();
                            get.Upduserid = usercode;
                            get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                            get.Updipno = UserlogAddress.IpAddress();
                            get.Updltude = passModel.Updltude;
                        }
                        db.SaveChanges();


                        String fieldWorkerName = "";
                        var findWorkerName = (from m in db.GlAcchartDbSet
                                            where m.Compid == userCompanycode && m.Accountcd == passModel.Fwid
                                            select m).ToList();
                        foreach (var get in findWorkerName)
                        {
                            fieldWorkerName = get.Accountnm;
                        }
                        //Update Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "UPDATE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = Convert.ToString("Area Input page. Area Name: " + passModel.Areanm + ",\nAuthorized Person Name: " + passModel.Authpnm + ",\n Field Worker Name: " + fieldWorkerName + ",\nRemarks: " + passModel.Remarks + ".");
                        aslLog.Tableid = "MCR_AREA";
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
        [System.Web.Http.Route("api/Area/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrArea passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrArea model = db.McrAreasDbSet.Find(passModel.Compid, passModel.Branchid, passModel.Areaid);
                    if (model != null)
                    {
                        String fieldWorkerName = "";
                        var findWorkerName = (from m in db.GlAcchartDbSet
                                            where m.Compid == userCompanycode && m.Accountcd == model.Fwid
                                            select m).ToList();
                        foreach (var get in findWorkerName)
                        {
                            fieldWorkerName = get.Accountnm;
                        }


                        //Delete Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "DELETE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = Convert.ToString("Area Input page. Area Name: " + model.Areanm + ",\nAuthorized Person Name: " + model.Authpnm + ",\n Field Worker Name: " + fieldWorkerName + ",\nRemarks: " + model.Remarks + ".");
                        aslLog.Tableid = "MCR_AREA";
                        Log.SaveLog(aslLog);

                        //Delete Log Data Save in to ASL_DELETE Table
                        AslDelete aslDeleteLog = new AslDelete();
                        aslDeleteLog.Compid = userCompanycode;
                        aslDeleteLog.Userid = usercode;
                        aslDeleteLog.Delltude = passModel.Updltude;
                        aslDeleteLog.Deldata = Convert.ToString("Area Input page. Area Name: " + model.Areanm + ",\nAuthorized Person Name: " + model.Authpnm + ",\n Field Worker Name: " + fieldWorkerName + ",\nRemarks: " + model.Remarks + ".");
                        aslDeleteLog.Tableid = "MCR_AREA";
                        Log.DeleteLog(aslDeleteLog);

                        //Delete From AslUserco
                        db.McrAreasDbSet.Remove(model);
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
        [System.Web.Http.Route("api/Area/List")]
        public object List(Int64 userCompanycode, Int64 usercode, Int64 branchId)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<ViewMcrArea> list = new List<ViewMcrArea>();
                var getData = (from m in db.McrAreasDbSet where m.Compid == userCompanycode && m.Branchid == branchId select m).ToList();
                foreach (var get in getData)
                {
                    ViewMcrArea model = new ViewMcrArea();
                    model.Compid = get.Compid;
                    model.Branchid = get.Branchid;
                    model.Areaid = get.Areaid;
                    model.Areanm = get.Areanm;
                    model.Authpnm = get.Authpnm;
                    model.Fwid = get.Fwid;
                    model.Remarks = get.Remarks;
                    var findWorkerName = (from m in db.GlAcchartDbSet
                                          where m.Compid == userCompanycode && m.Accountcd == get.Fwid
                                          select m).ToList();
                    foreach (var getfindWorkerName in findWorkerName)
                    {
                        model.FieldWorkersName = getfindWorkerName.Accountnm;
                    }
                    list.Add(model);
                }
                if (list.Count != 0)
                    return new
                    {
                        data = list.OrderBy(e => e.Areanm).ToList(),
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
        [System.Web.Http.Route("api/Area/TotalList")]
        public object TotalList(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrArea> list = new List<McrArea>();
                list = (from m in db.McrAreasDbSet where m.Compid == userCompanycode select m).OrderBy(e=>e.Areanm).ToList();
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
        [System.Web.Http.Route("api/Area/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, Int64 branchId, Int64 areaId)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrArea> list = new List<McrArea>();
                list = (from m in db.McrAreasDbSet where m.Compid == userCompanycode && m.Branchid == branchId && m.Areaid == areaId select m).ToList();
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
