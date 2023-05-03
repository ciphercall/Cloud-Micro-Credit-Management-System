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
    public class ApiCostPoolMasterController : ApiController
    {

        private DatabaseDbContext db;

        public ApiCostPoolMasterController()
        {
            db = new DatabaseDbContext();
        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CostPoolMaster/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlCostPmst  passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    GlCostPmst model = new GlCostPmst();
                    var result = db.GlCostPmstDbSet.Count(d => d.Compid == userCompanycode &&  d.Costcnm == passModel.Costcnm);
                    if (result == 0)
                    {
                        Int64 maxCostcid = Convert.ToInt64((from m in db.GlCostPmstDbSet where m.Compid == userCompanycode select m.Costcid).DefaultIfEmpty().Max());
                        if (maxCostcid == 0)
                        {
                            model.Costcid = Convert.ToInt64(userCompanycode + "001");
                        }
                        else if (maxCostcid != 0)
                        {
                            Int64 maxLimit = Convert.ToInt64(userCompanycode + "999");
                            Int64 id = maxCostcid + 1;
                            if (id <= maxLimit)
                            {
                                model.Costcid = id;
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
                        model.Costcnm = passModel.Costcnm;
                        model.Userpc = UserlogAddress.UserPc();
                        model.Insuserid = usercode;
                        model.Instime = UserlogAddress.Timezone(DateTime.Now);
                        model.Insipno = UserlogAddress.IpAddress();
                        model.Insltude = passModel.Insltude;
                        db.GlCostPmstDbSet.Add(model);
                        db.SaveChanges();

                        //Create Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "CREATE";
                        aslLog.Logltude = passModel.Insltude;
                        aslLog.Logdata = Convert.ToString("Cost Pool Group page. Cost group name:" + passModel.Costcnm +".");
                        aslLog.Tableid = "GL_COSTPMST";
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
        [System.Web.Http.Route("api/CostPoolMaster/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlCostPmst passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            String logData = "";
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findCostPoolMasterInfo = (from m in db.GlCostPmstDbSet where m.Compid == passModel.Compid && m.Costcnm == passModel.Costcnm select m).ToList();
                    if (findCostPoolMasterInfo.Count == 0)
                    {
                        var costPoolMasterInfo = (from m in db.GlCostPmstDbSet where m.Compid == passModel.Compid && m.Costcid == passModel.Costcid select m).ToList();
                        if (costPoolMasterInfo.Count == 1)
                        {
                            foreach (var get in costPoolMasterInfo)
                            {
                                get.Costcnm = passModel.Costcnm;

                                get.Userpc = UserlogAddress.UserPc();
                                get.Upduserid = usercode;
                                get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                                get.Updipno = UserlogAddress.IpAddress();
                                get.Updltude = passModel.Updltude;

                                logData = Convert.ToString("Cost Pool Group page. Cost group name:" + passModel.Costcnm + ".");
                            }
                            db.SaveChanges();

                            //Update Log Data Save in to ASL_LOG Table
                            AslLog aslLog = new AslLog();
                            aslLog.Compid = userCompanycode;
                            aslLog.Userid = usercode;
                            aslLog.Logtype = "UPDATE";
                            aslLog.Logltude = passModel.Updltude;
                            aslLog.Logdata = logData;
                            aslLog.Tableid = "GL_COSTPMST";
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
        [System.Web.Http.Route("api/CostPoolMaster/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewGlCostPmst passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findChildData = (from m in db.GlCostPoolDbSet where m.Compid == userCompanycode && m.Costcid == passModel.Costcid select m).ToList();
                    if (findChildData.Count != 0)
                    {
                        return new
                        {
                            data = "",
                            success = false,
                            message = "Permission Denied! Child data already exists."
                        };
                    }

                    GlCostPmst model = db.GlCostPmstDbSet.Find(passModel.Compid, passModel.Costcid);
                    
                    //Delete Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "DELETE";
                    aslLog.Logltude = passModel.Updltude;
                    aslLog.Logdata = Convert.ToString("Cost Pool Group page. Cost group name:" + passModel.Costcnm + ".");
                    aslLog.Tableid = "GL_COSTPMST";
                    Log.SaveLog(aslLog);

                    //Delete Log Data Save in to ASL_DELETE Table
                    AslDelete aslDeleteLog = new AslDelete();
                    aslDeleteLog.Compid = userCompanycode;
                    aslDeleteLog.Userid = usercode;
                    aslDeleteLog.Delltude = passModel.Updltude;
                    aslDeleteLog.Deldata = Convert.ToString("Cost Pool Group page. Cost group name:" + passModel.Costcnm + ".");
                    aslDeleteLog.Tableid = "GL_COSTPMST";
                    Log.DeleteLog(aslDeleteLog);

                    //Delete From AslUserco
                    db.GlCostPmstDbSet.Remove(model);
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
        [System.Web.Http.Route("api/CostPoolMaster/List")]
        public object List(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlCostPmst> list = new List<GlCostPmst>();
                list = (from m in db.GlCostPmstDbSet where m.Compid == userCompanycode select m).ToList();
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
        [System.Web.Http.Route("api/CostPoolMaster/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, Int64 costcid)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<GlCostPmst> list = new List<GlCostPmst>();
                list = (from m in db.GlCostPmstDbSet where m.Compid == userCompanycode && m.Costcid == costcid select m).ToList();
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
