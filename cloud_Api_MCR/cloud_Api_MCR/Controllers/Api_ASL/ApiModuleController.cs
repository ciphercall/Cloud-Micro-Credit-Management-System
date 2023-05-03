using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.ASL;
using cloud_Api_MCR.Models.ASL_DTO;

namespace cloud_Api_MCR.Controllers.Api_ASL
{
    public class ApiModuleController : ApiController
    {
        private DatabaseDbContext db;

        public ApiModuleController()
        {
            db = new DatabaseDbContext();
        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Module/Create")]
        public object Create([FromBody]ViewAslMenumst passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            Int64 companyCode = Convert.ToInt64(passModel.Compid);
            Int64 usercode = Convert.ToInt64(passModel.Userid);
            if (TokenInfo.TokenCheck(companyCode, usercode, token))
            {
                try
                {
                    AslMenumst model = new AslMenumst();
                    var result = db.AslMenumstDbSet.Count(d => d.Modulenm == passModel.Modulenm);
                    if (result == 0)
                    {
                        var maxModuleId = Convert.ToString(db.AslMenumstDbSet.Max(s => s.Moduleid));
                        if (maxModuleId == null)
                        {
                            model.Moduleid = Convert.ToString("01");
                        }
                        else if (maxModuleId != "")
                        {
                            int moduleid = int.Parse(maxModuleId) + 1;
                            if (moduleid < 10)
                            {
                                model.Moduleid = Convert.ToString("0" + moduleid);
                            }
                            else if (moduleid < 100)
                            {
                                model.Moduleid = Convert.ToString(moduleid);
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
                        
                        model.Modulenm = passModel.Modulenm;
                        model.Userpc = UserlogAddress.UserPc();
                        model.Insuserid = usercode;
                        model.Instime = UserlogAddress.Timezone(DateTime.Now);
                        model.Insipno = UserlogAddress.IpAddress();
                        model.Insltude = passModel.Insltude;
                        db.AslMenumstDbSet.Add(model);
                        db.SaveChanges();

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
                            success = false,
                            message = "This module name already exists."
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
        [System.Web.Http.Route("api/Module/Update")]
        public object Update([FromBody]ViewAslMenumst passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            Int64 companyCode = Convert.ToInt64(passModel.Compid);
            Int64 usercode = Convert.ToInt64(passModel.Userid);
            if (TokenInfo.TokenCheck(companyCode, usercode, token))
            {
                try
                {
                    var checkModuleName = (from m in db.AslMenumstDbSet where m.Modulenm == passModel.Modulenm select m).ToList();
                    if (checkModuleName.Count == 0)
                    {
                        var findModuleInfo = (from m in db.AslMenumstDbSet where m.Moduleid == passModel.Moduleid select m).ToList();
                        if (findModuleInfo.Count == 1)
                        {
                            foreach (var get in findModuleInfo)
                            {
                                get.Modulenm = passModel.Modulenm;

                                get.Userpc = UserlogAddress.UserPc();
                                get.Upduserid = usercode;
                                get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                                get.Updipno = UserlogAddress.IpAddress();
                                get.Updltude = passModel.Updltude;
                            }
                            db.SaveChanges();
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
                            message = "Module name already exists."
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
        [System.Web.Http.Route("api/Module/Delete")]
        public object Delete([FromBody]ViewAslMenumst passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            Int64 companyCode = Convert.ToInt64(passModel.Compid);
            Int64 usercode = Convert.ToInt64(passModel.Userid);
            if (TokenInfo.TokenCheck(companyCode, usercode, token))
            {
                try
                {
                    AslMenumst model = db.AslMenumstDbSet.Find(passModel.Moduleid);
                    
                    //Delete all information from AslMenu Table,when it match to the Module ID
                    var menuList = (from sub in db.AslMenuDbSet select sub).Where(sub => sub.Moduleid == model.Moduleid);
                    foreach (var n in menuList)
                    {
                        db.AslMenuDbSet.Remove(n);
                    }
                    db.SaveChanges();

                    //Delete all information from AslRole Table,when it match to the Module ID
                    var roleList = (from sub in db.AslRoleDbSet select sub).Where(sub => sub.Moduleid == model.Moduleid);
                    foreach (var n in roleList)
                    {
                        db.AslRoleDbSet.Remove(n);
                    }
                    db.SaveChanges();

                    //Delete From AslMenumst
                    db.AslMenumstDbSet.Remove(model);
                    db.SaveChanges();
                    return new
                    {
                        data = passModel.Modulenm,
                        success = true,
                        message = "Delete data Successfully."
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





        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Module/ModuleList")]
        public object ModuleList(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();
            
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<AslMenumst> list = new List<AslMenumst>();
                list = (from m in db.AslMenumstDbSet select m).ToList();
                if (list.Count !=0)
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
        [System.Web.Http.Route("api/Module/ModuleInformation")]
        public object ModuleInformation(Int64 companyCode, Int64 usercode, string moduleid)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();
            
            if (TokenInfo.TokenCheck(companyCode, usercode, token))
            {
                List<AslMenumst> list = new List<AslMenumst>();
                list = (from m in db.AslMenumstDbSet where m.Moduleid== moduleid select m).ToList();
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
