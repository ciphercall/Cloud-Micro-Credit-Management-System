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
    public class ApiCostPoolController : ApiController
    {
        private DatabaseDbContext db;

        public ApiCostPoolController()
        {
            db = new DatabaseDbContext();
        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CostPool/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlCostPool passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    GlCostPool model = new GlCostPool();
                    var result = db.GlCostPoolDbSet.Count(d => d.Compid == userCompanycode && d.Costcid == passModel.Costcid && d.Costpnm == passModel.Costpnm);
                    if (result == 0)
                    {
                        Int64 maxCostpid = Convert.ToInt64((from m in db.GlCostPoolDbSet where m.Compid == userCompanycode && m.Costcid == passModel.Costcid select m.Costpid).DefaultIfEmpty().Max());
                        if (maxCostpid == 0)
                        {
                            model.Costpid = Convert.ToInt64(passModel.Costcid + "0001");
                        }
                        else if (maxCostpid != 0)
                        {
                            Int64 maxLimit = Convert.ToInt64(passModel.Costcid + "9999");
                            Int64 id = maxCostpid + 1;
                            if (id <= maxLimit)
                            {
                                model.Costpid = id;
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
                        model.Costcid = passModel.Costcid;
                        model.Costpnm = passModel.Costpnm;
                        model.Costpsid = passModel.Costpsid;
                        model.Remarks = passModel.Remarks;
                        model.Userpc = UserlogAddress.UserPc();
                        model.Insuserid = usercode;
                        model.Instime = UserlogAddress.Timezone(DateTime.Now);
                        model.Insipno = UserlogAddress.IpAddress();
                        model.Insltude = passModel.Insltude;
                        db.GlCostPoolDbSet.Add(model);
                        db.SaveChanges();
                        
                        String groupName = "";
                        var findgroupName = (from m in db.GlCostPmstDbSet where m.Compid == passModel.Compid && m.Costcid == passModel.Costcid select m).ToList();
                        foreach (var get in findgroupName)
                        {
                            groupName = get.Costcnm.ToString();
                        }
                        //Create Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "CREATE";
                        aslLog.Logltude = passModel.Insltude;
                        aslLog.Logdata = Convert.ToString("Cost pool information page. Group:" + groupName + ",Cost Head Name: " + passModel.Costpnm + ",Cost Pool: " + passModel.Costpsid + ",Remarks: " + passModel.Remarks + ".");
                        aslLog.Tableid = "GL_COSTPOOL";
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
                            message = "This cost head name already exists."
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
        [System.Web.Http.Route("api/CostPool/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlCostPool passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            String logData = "";
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    String groupName = "";
                    var findgroupName = (from m in db.GlCostPmstDbSet where m.Compid == passModel.Compid && m.Costcid == passModel.Costcid select m).ToList();
                    foreach (var get in findgroupName)
                    {
                        groupName = get.Costcnm.ToString();
                    }

                    var findCostPoolInfo = (from m in db.GlCostPoolDbSet where m.Compid == passModel.Compid && m.Costcid == passModel.Costcid && m.Costpnm == passModel.Costpnm && m.Costpsid==passModel.Costpsid && m.Remarks==passModel.Remarks select m).ToList();
                    if (findCostPoolInfo.Count == 0)
                    {
                        var costPoolInfo = (from m in db.GlCostPoolDbSet where m.Compid == passModel.Compid && m.Costcid == passModel.Costcid && m.Costpid == passModel.Costpid select m).ToList();
                        if (costPoolInfo.Count == 1)
                        {
                            foreach (var get in costPoolInfo)
                            {
                                get.Costpnm = passModel.Costpnm;
                                get.Costpsid = passModel.Costpsid;
                                get.Remarks = passModel.Remarks;

                                get.Userpc = UserlogAddress.UserPc();
                                get.Upduserid = usercode;
                                get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                                get.Updipno = UserlogAddress.IpAddress();
                                get.Updltude = passModel.Updltude;

                                logData = Convert.ToString("Cost pool information page. Group:" + groupName + ",Cost Head Name: " + passModel.Costpnm + ",Cost Pool: " + passModel.Costpsid + ",Remarks: " + passModel.Remarks + ".");
                            }
                            db.SaveChanges();

                            //Update Log Data Save in to ASL_LOG Table
                            AslLog aslLog = new AslLog();
                            aslLog.Compid = userCompanycode;
                            aslLog.Userid = usercode;
                            aslLog.Logtype = "UPDATE";
                            aslLog.Logltude = passModel.Updltude;
                            aslLog.Logdata = logData;
                            aslLog.Tableid = "GL_COSTPOOL";
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
                            message = "This cost head name already exists."
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
        [System.Web.Http.Route("api/CostPool/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlCostPool passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findChildData = (from m in db.GlMasterDbSet where m.Compid == userCompanycode && m.Costpid == passModel.Costpid select m).ToList();
                    if (findChildData.Count != 0)
                    {
                        return new
                        {
                            data = "",
                            success = false,
                            message = "Permission Denied! This data already used."
                        };
                    }


                    GlCostPool model = db.GlCostPoolDbSet.Find(passModel.Compid, passModel.Costcid, passModel.Costpid);

                    String groupName = "";
                    var findgroupName = (from m in db.GlCostPmstDbSet where m.Compid == passModel.Compid && m.Costcid == passModel.Costcid select m).ToList();
                    foreach (var get in findgroupName)
                    {
                        groupName = get.Costcnm.ToString();
                    }

                    //Delete Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "DELETE";
                    aslLog.Logltude = passModel.Updltude;
                    aslLog.Logdata = Convert.ToString("Cost pool information page. Group:" + groupName + ",Cost Head Name: " + passModel.Costpnm + ",Cost Pool: " + passModel.Costpsid + ",Remarks: " + passModel.Remarks + ".");
                    aslLog.Tableid = "GL_COSTPOOL";
                    Log.SaveLog(aslLog);

                    //Delete Log Data Save in to ASL_DELETE Table
                    AslDelete aslDeleteLog = new AslDelete();
                    aslDeleteLog.Compid = userCompanycode;
                    aslDeleteLog.Userid = usercode;
                    aslDeleteLog.Delltude = passModel.Updltude;
                    aslDeleteLog.Deldata = Convert.ToString("Cost pool information page. Group:" + groupName + ",Cost Head Name: " + passModel.Costpnm + ",Cost Pool: " + passModel.Costpsid + ",Remarks: " + passModel.Remarks + ".");
                    aslDeleteLog.Tableid = "GL_COSTPOOL";
                    Log.DeleteLog(aslDeleteLog);

                    //Delete From AslUserco
                    db.GlCostPoolDbSet.Remove(model);
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
        [System.Web.Http.Route("api/CostPool/List")]
        public object List(Int64 userCompanycode, Int64 usercode, Int64 costcid)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlCostPool> list = new List<GlCostPool>();
                list = (from m in db.GlCostPoolDbSet where m.Compid == userCompanycode && m.Costcid == costcid select m).ToList();
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
        [System.Web.Http.Route("api/CostPool/TotalList")]
        public object TotalList(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlCostPool> list = new List<GlCostPool>();
                list = (from m in db.GlCostPoolDbSet where m.Compid == userCompanycode select m).ToList();
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
        [System.Web.Http.Route("api/CostPool/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, Int64 costcid, Int64 costpid)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlCostPool> list = new List<GlCostPool>();
                list = (from m in db.GlCostPoolDbSet where m.Compid == userCompanycode && m.Costcid == costcid && m.Compid== costpid select m).ToList();
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
