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

namespace cloud_Api_MCR.Controllers.Api_Account
{
    public class ApiAccountChartController : ApiController
    {
        private DatabaseDbContext db;

        public ApiAccountChartController()
        {
            db = new DatabaseDbContext();
        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/AccountChart/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlAcchart passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    GlAcchart model = new GlAcchart();
                    var result = db.GlAcchartDbSet.Count(d => d.Compid == userCompanycode && d.Headtp == passModel.Headtp && d.Headcd == passModel.Headcd && d.Accountnm == passModel.Accountnm);
                    if (result == 0)
                    {
                        Int64 maxAccountcd = Convert.ToInt64((from m in db.GlAcchartDbSet where m.Compid == userCompanycode && m.Headtp == passModel.Headtp && m.Headcd == passModel.Headcd select m.Accountcd).DefaultIfEmpty().Max());
                        if (maxAccountcd == 0)
                        {
                            model.Accountcd = Convert.ToInt64(passModel.Headcd + "0001");
                        }
                        else if (maxAccountcd != 0)
                        {
                            Int64 maxLimit = Convert.ToInt64(passModel.Headcd + "9999");
                            Int64 id = maxAccountcd + 1;
                            if (id <= maxLimit)
                            {
                                model.Accountcd = id;
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
                        model.Headcd = passModel.Headcd;
                        model.Accountcd = model.Accountcd;
                        model.Accountnm = passModel.Accountnm;
                        model.Remarks = passModel.Remarks;
                        model.Userpc = UserlogAddress.UserPc();
                        model.Insuserid = usercode;
                        model.Instime = UserlogAddress.Timezone(DateTime.Now);
                        model.Insipno = UserlogAddress.IpAddress();
                        model.Insltude = passModel.Insltude;
                        db.GlAcchartDbSet.Add(model);
                        db.SaveChanges();

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

                        String headName = "";
                        var findHeadName = (from m in db.GlAccharMstDbSet where m.Compid == passModel.Compid && m.Headtp == passModel.Headtp && m.Headcd == passModel.Headcd select m).ToList();
                        foreach (var get in findHeadName)
                        {
                            headName = get.Headnm.ToString();
                        }
                        //Create Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "CREATE";
                        aslLog.Logltude = passModel.Insltude;
                        aslLog.Logdata = Convert.ToString("Account Information page. Head Type:" + headType + ",Head Name: " + headName + ",Account Name: " + passModel.Accountnm + ",Remarks: " + passModel.Remarks + ".");
                        aslLog.Tableid = "GL_ACCHART";
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
                            message = "This Account name already exists."
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
        [System.Web.Http.Route("api/AccountChart/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlAcchart passModel)
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

                    String headName = "";
                    var findHeadName = (from m in db.GlAccharMstDbSet where m.Compid == passModel.Compid && m.Headtp == passModel.Headtp && m.Headcd == passModel.Headcd select m).ToList();
                    foreach (var get in findHeadName)
                    {
                        headName = get.Headnm.ToString();
                    }

                    var findAccountChartInfo = (from m in db.GlAcchartDbSet where m.Compid == passModel.Compid && m.Headcd == passModel.Headcd && m.Accountnm == passModel.Accountnm select m).ToList();
                    if (findAccountChartInfo.Count == 0)
                    {
                        var accountChartInfo = (from m in db.GlAcchartDbSet where m.Compid == passModel.Compid && m.Headcd == passModel.Headcd && m.Accountcd==passModel.Accountcd select m).ToList();
                        if (accountChartInfo.Count == 1)
                        {
                            foreach (var get in accountChartInfo)
                            {
                                get.Accountnm = passModel.Accountnm;
                                get.Remarks = passModel.Remarks;

                                get.Userpc = UserlogAddress.UserPc();
                                get.Upduserid = usercode;
                                get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                                get.Updipno = UserlogAddress.IpAddress();
                                get.Updltude = passModel.Updltude;

                                logData = Convert.ToString("Account Information page. Head Type:" + headType + ",Head Name: " + headName + ",Account Name: " + passModel.Accountnm + ",Remarks: " + passModel.Remarks + ".");
                            }
                            db.SaveChanges();

                            //Update Log Data Save in to ASL_LOG Table
                            AslLog aslLog = new AslLog();
                            aslLog.Compid = userCompanycode;
                            aslLog.Userid = usercode;
                            aslLog.Logtype = "UPDATE";
                            aslLog.Logltude = passModel.Updltude;
                            aslLog.Logdata = logData;
                            aslLog.Tableid = "GL_ACCHART";
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
                            message = "This Account name already exists."
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
        [System.Web.Http.Route("api/AccountChart/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlAcchart passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findChildData = (from m in db.GlMasterDbSet where m.Compid == userCompanycode && m.Debitcd == passModel.Accountcd select m).ToList();
                    if (findChildData.Count != 0)
                    {
                        return new
                        {
                            data = "",
                            success = false,
                            message = "Permission Denied! This data already used."
                        };
                    }


                    GlAcchart model = db.GlAcchartDbSet.Find(passModel.Compid, passModel.Headcd,passModel.Accountcd);

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

                    String headName = "";
                    var findHeadName = (from m in db.GlAccharMstDbSet where m.Compid == passModel.Compid && m.Headtp == passModel.Headtp && m.Headcd == passModel.Headcd select m).ToList();
                    foreach (var get in findHeadName)
                    {
                        headName = get.Headnm.ToString();
                    }

                    //Delete Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "DELETE";
                    aslLog.Logltude = passModel.Updltude;
                    aslLog.Logdata = Convert.ToString("Account Information page. Head Type:" + headType + ",Head Name: " + headName + ",Account Name: " + passModel.Accountnm + ",Remarks: " + passModel.Remarks + ".");
                    aslLog.Tableid = "GL_ACCHART";
                    Log.SaveLog(aslLog);

                    //Delete Log Data Save in to ASL_DELETE Table
                    AslDelete aslDeleteLog = new AslDelete();
                    aslDeleteLog.Compid = userCompanycode;
                    aslDeleteLog.Userid = usercode;
                    aslDeleteLog.Delltude = passModel.Updltude;
                    aslDeleteLog.Deldata = Convert.ToString("Account Information page. Head Type:" + headType + ",Head Name: " + headName + ",Account Name: " + passModel.Accountnm + ",Remarks: " + passModel.Remarks + ".");
                    aslDeleteLog.Tableid = "GL_ACCHART";
                    Log.DeleteLog(aslDeleteLog);

                    //Delete From AslUserco
                    db.GlAcchartDbSet.Remove(model);
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
        [System.Web.Http.Route("api/AccountChart/List")]
        public object List(Int64 userCompanycode, Int64 usercode, Int64 headType, Int64 headcd)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlAcchart> list = new List<GlAcchart>();
                list = (from m in db.GlAcchartDbSet where m.Compid == userCompanycode && m.Headtp == headType && m.Headcd== headcd select m).ToList();
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
        [System.Web.Http.Route("api/AccountChart/TypeWiseList")]
        public object TypeWiseList(Int64 userCompanycode, Int64 usercode, Int64 headType)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlAcchart> list = new List<GlAcchart>();
                list = (from m in db.GlAcchartDbSet where m.Compid == userCompanycode && m.Headtp == headType select m).ToList();
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
        [System.Web.Http.Route("api/AccountChart/TotalList")]
        public object TotalList(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlAcchart> list = new List<GlAcchart>();
                list = (from m in db.GlAcchartDbSet where m.Compid == userCompanycode select m).ToList();
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
        [System.Web.Http.Route("api/AccountChart/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, Int64 headType, Int64 headcd, Int64 accountcd)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlAcchart> list = new List<GlAcchart>();
                list = (from m in db.GlAcchartDbSet where m.Compid == userCompanycode && m.Headtp == headType && m.Headtp == headcd && m.Headcd== accountcd select m).ToList();
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
