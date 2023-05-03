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
using cloud_Api_MCR.Models.ASL_DTO;

namespace cloud_Api_MCR.Controllers.Api_Account
{
    public class ApiAccountChartMasterController : ApiController
    {
        private DatabaseDbContext db;

        public ApiAccountChartMasterController()
        {
            db = new DatabaseDbContext();
        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/AccountChartMaster/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlAcchartMst passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    GlAcchartMst model = new GlAcchartMst();
                    var result = db.GlAccharMstDbSet.Count(d => d.Compid == userCompanycode && d.Headtp == passModel.Headtp && d.Headnm == passModel.Headnm);
                    if (result == 0)
                    {
                        Int64 maxHeadcd = Convert.ToInt64((from m in db.GlAccharMstDbSet where m.Compid== userCompanycode && m.Headtp == passModel.Headtp select m.Headcd).DefaultIfEmpty().Max());
                        String headtp = passModel.Headtp.ToString();
                        if (maxHeadcd == 0)
                        {
                            model.Headcd = Convert.ToInt64(userCompanycode + headtp + "01");
                        }
                        else if (maxHeadcd != 0)
                        {
                            Int64 maxLimit = Convert.ToInt64(userCompanycode + headtp + "99");
                            Int64 id = maxHeadcd + 1;
                            if (id <= maxLimit)
                            {
                                model.Headcd = id;
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
                        model.Headtp = passModel.Headtp;
                        model.Headnm = passModel.Headnm;
                        model.Remarks = passModel.Remarks;
                        model.Userpc = UserlogAddress.UserPc();
                        model.Insuserid = usercode;
                        model.Instime = UserlogAddress.Timezone(DateTime.Now);
                        model.Insipno = UserlogAddress.IpAddress();
                        model.Insltude = passModel.Insltude;
                        db.GlAccharMstDbSet.Add(model);
                        db.SaveChanges();

                        String headType = "";
                        if (passModel.Headtp==1)
                        {
                            headType = "ASSET";
                        }
                        else if (passModel.Headtp == 2)
                        {
                            headType = "LIABILITY";
                        }
                        else if (passModel.Headtp == 3)
                        {
                            headType = "INCOME";
                        }
                        else if (passModel.Headtp == 4)
                        {
                            headType = "EXPENDITURE";
                        }
                        //Create Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "CREATE";
                        aslLog.Logltude = passModel.Insltude;
                        aslLog.Logdata = Convert.ToString("Account Head page. Head Type:" + headType + ",Head Name: " + passModel.Headnm +",Remarks: "+passModel.Remarks+ ".");
                        aslLog.Tableid = "GL_ACCHARTMST";
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
                            message = "This Head name already exists."
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
        [System.Web.Http.Route("api/AccountChartMaster/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlAcchartMst passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            String logData = "";
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    String headType = "";
                    if (passModel.Headtp == 1)
                    {
                        headType = "ASSET";
                    }
                    else if (passModel.Headtp == 2)
                    {
                        headType = "LIABILITY";
                    }
                    else if (passModel.Headtp == 3)
                    {
                        headType = "INCOME";
                    }
                    else if (passModel.Headtp == 4)
                    {
                        headType = "EXPENDITURE";
                    }

                    var findAccountChartMasterInfo = (from m in db.GlAccharMstDbSet where m.Compid == passModel.Compid  && m.Headtp == passModel.Headtp && m.Headnm == passModel.Headnm && m.Remarks==passModel.Remarks select m).ToList();
                    if (findAccountChartMasterInfo.Count == 0)
                    {
                        var accountChartMasterInfo = (from m in db.GlAccharMstDbSet where m.Compid == passModel.Compid && m.Headcd == passModel.Headcd select m).ToList();
                        if (accountChartMasterInfo.Count == 1)
                        {
                            foreach (var get in accountChartMasterInfo)
                            {
                                get.Headtp = passModel.Headtp;
                                get.Headnm = passModel.Headnm;
                                get.Remarks = passModel.Remarks;

                                get.Userpc = UserlogAddress.UserPc();
                                get.Upduserid = usercode;
                                get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                                get.Updipno = UserlogAddress.IpAddress();
                                get.Updltude = passModel.Updltude;

                                logData = Convert.ToString("Account Head page. Head Type:" + headType + ",Head Name: " + passModel.Headnm + ",Remarks: " + passModel.Remarks + ".");
                            }
                            db.SaveChanges();
                            
                            //Update Log Data Save in to ASL_LOG Table
                            AslLog aslLog = new AslLog();
                            aslLog.Compid = userCompanycode;
                            aslLog.Userid = usercode;
                            aslLog.Logtype = "UPDATE";
                            aslLog.Logltude = passModel.Updltude;
                            aslLog.Logdata = logData;
                            aslLog.Tableid = "GL_ACCHARTMST";
                            Log.SaveLog(aslLog);

                            return new
                            {
                                data = passModel,
                                success = true,
                                message = "Update data Successfully."
                            };
                        }
                    }
                    else
                    {
                        return new
                        {
                            data = "",
                            success = true,
                            message = "This Head name already exists."
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
        [System.Web.Http.Route("api/AccountChartMaster/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlAcchartMst passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findChildData = (from m in db.GlAcchartDbSet where m.Compid == userCompanycode && m.Headcd == passModel.Headcd select m).ToList();
                    if (findChildData.Count != 0)
                    {
                        return new
                        {
                            data = "",
                            success = false,
                            message = "Permission Denied! Child data already exists."
                        };
                    }

                    GlAcchartMst model = db.GlAccharMstDbSet.Find(passModel.Compid, passModel.Headcd);
                    String headType = "";
                    if (passModel.Headtp == 1)
                    {
                        headType = "ASSET";
                    }
                    else if (passModel.Headtp == 2)
                    {
                        headType = "LIABILITY";
                    }
                    else if (passModel.Headtp == 3)
                    {
                        headType = "INCOME";
                    }
                    else if (passModel.Headtp == 4)
                    {
                        headType = "EXPENDITURE";
                    }


                    //Delete Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "DELETE";
                    aslLog.Logltude = passModel.Updltude;
                    aslLog.Logdata = Convert.ToString("Account Head page. Head Type:" + headType + ",Head Name: " + passModel.Headnm + ",Remarks: " + passModel.Remarks + ".");
                    aslLog.Tableid = "GL_ACCHARTMST";
                    Log.SaveLog(aslLog);

                    //Delete Log Data Save in to ASL_DELETE Table
                    AslDelete aslDeleteLog = new AslDelete();
                    aslDeleteLog.Compid = userCompanycode;
                    aslDeleteLog.Userid = usercode;
                    aslDeleteLog.Delltude = passModel.Updltude;
                    aslDeleteLog.Deldata = Convert.ToString("Account Head page. Head Type:" + headType + ",Head Name: " + passModel.Headnm + ",Remarks: " + passModel.Remarks + ".");
                    aslDeleteLog.Tableid = "GL_ACCHARTMST";
                    Log.DeleteLog(aslDeleteLog);

                    //Delete From AslUserco
                    db.GlAccharMstDbSet.Remove(model);
                    db.SaveChanges();

                    return new
                    {
                        data = passModel,
                        success = true,
                        message = "Delete data Successfully."
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
                success = false,
                message = "Authorized not permitted."
            };
        }




        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/AccountChartMaster/List")]
        public object List(Int64 userCompanycode, Int64 usercode, Int64 headType)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();
            
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlAcchartMst> list = new List<GlAcchartMst>();
                list = (from m in db.GlAccharMstDbSet where m.Compid==userCompanycode && m.Headtp==headType select m).ToList();
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
        [System.Web.Http.Route("api/AccountChartMaster/TotalList")]
        public object TotalList(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlAcchartMst> list = new List<GlAcchartMst>();
                list = (from m in db.GlAccharMstDbSet where m.Compid == userCompanycode select m).ToList();
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
        [System.Web.Http.Route("api/AccountChartMaster/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, Int64 headcd)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();
            
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlAcchartMst> list = new List<GlAcchartMst>();
                list = (from m in db.GlAccharMstDbSet where m.Compid == userCompanycode && m.Headtp==headcd select m).ToList();
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
