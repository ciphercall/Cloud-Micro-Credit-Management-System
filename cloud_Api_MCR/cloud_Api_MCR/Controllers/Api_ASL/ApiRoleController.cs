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
    public class ApiRoleController : ApiController
    {
        private DatabaseDbContext db;

        public ApiRoleController()
        {
            db = new DatabaseDbContext();
        }



        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Role/RoleList")]
        public object RoleList(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            List<ViewAslRole> nulllist = new List<ViewAslRole>();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<AslRole> list = new List<AslRole>();
                list = (from m in db.AslRoleDbSet where m.Compid == userCompanycode && m.Userid == usercode select m).ToList();
                if (list.Count != 0)
                    return new
                    {
                        data = list,
                        success = true,
                        message = "Get data Successfully."
                    };
                else return new
                {
                    data = nulllist,
                    success = true,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = nulllist,
                success = false,
                message = "Authorized not permitted."
            };
        }



        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Role/CompanyWiseUserRoleList")]
        public object CompanyWiseUserRoleList(Int64 userCompanycode, Int64 usercode, Int64 selectCompanyCode, Int64 selectUserCode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            List<ViewAslRole> nulllist = new List<ViewAslRole>();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<ViewAslRole> list = new List<ViewAslRole>();
                var getData = (from m in db.AslRoleDbSet join n in db.AslMenumstDbSet on m.Moduleid equals n.Moduleid
                               where m.Compid == selectCompanyCode && m.Userid == selectUserCode
                               select new {m,n}).OrderBy(e => e.m.Moduleid).ThenBy(e => e.m.Menutp).ToList();
                foreach (var get in getData)
                {
                    ViewAslRole model = new ViewAslRole();
                    model.SelectCompId = get.m.Compid;
                    model.SelectUserId = get.m.Userid;
                    model.Moduleid = get.m.Moduleid;
                    model.Menutp = get.m.Menutp;
                    model.Menuid = get.m.Menuid;
                    model.Serial = get.m.Serial;
                    model.Status = get.m.Status;
                    model.Insertr = get.m.Insertr;
                    model.Updater = get.m.Updater;
                    model.Deleter = get.m.Deleter;
                    model.Menuname = get.m.Menuname;
                    model.Actionname = get.m.Actionname;
                    model.Controllername = get.m.Controllername;
                    
                    model.ModuleName = get.n.Modulenm;
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
                    data = nulllist,
                    success = true,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = nulllist,
                success = false,
                message = "Authorized not permitted."
            };
        }




        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Role/CompanyWisePermissionList")]
        public object CompanyWisePermissionList(Int64 userCompanycode, Int64 usercode,Int64 selectCompanyCode, string moduleId, string menuType)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            List<ViewAslRole> nulllist = new List<ViewAslRole>();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<AslRole> list = new List<AslRole>();
                Int64 companyAdminUserId = Convert.ToInt64(selectCompanyCode + "01");
                var getRoleInfo = (from m in db.AslRoleDbSet where m.Compid == selectCompanyCode && m.Userid== companyAdminUserId && m.Moduleid == moduleId && m.Menutp==menuType select m).OrderBy(x => x.Serial).ToList();
                if (getRoleInfo.Count != 0)
                {
                    foreach (var getData in getRoleInfo)
                    {
                        if (getData.Status == "A")
                        {
                            getData.Status = "Active";
                        }
                        else // if (getData.Status == "I")
                        {
                            getData.Status = "Inactive";
                        }
                        list.Add(getData);
                    }
                    return new
                    {
                        data = list,
                        success = true,
                        message = "Get data Successfully."
                    };
                }
                else return new
                {
                    data = nulllist,
                    success = false,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = nulllist,
                success = false,
                message = "Authorized not permitted."
            };
        }





        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Role/CompanyWisePermissionUpdate")]
        public object CompanyWisePermissionUpdate([FromBody]ViewAslRole passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            Int64 userCompanycode = Convert.ToInt64(passModel.Compid);
            Int64 usercode = Convert.ToInt64(passModel.Userid);
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    Int64 companyAdminUserId = Convert.ToInt64(passModel.SelectCompId + "01");
                    passModel.Status = passModel.Status == "Active" ? "A" : "I";
                    var findRoleInfo = (from m in db.AslRoleDbSet where m.Compid == passModel.SelectCompId && m.Userid == companyAdminUserId && m.Moduleid == passModel.Moduleid && m.Menutp == passModel.Menutp && m.Menuid== passModel.Menuid select m).ToList();
                    if (findRoleInfo.Count == 1)
                    {
                        foreach (var get in findRoleInfo)
                        {
                            get.Status = passModel.Status;
                            if (get.Status == "A")
                            {
                                get.Insertr = "A";
                                get.Updater = "A";
                                get.Deleter = "A";
                            }
                            else if (get.Status == "I")
                            {
                                get.Insertr = "I";
                                get.Updater = "I";
                                get.Deleter = "I";

                            }
                            get.Userpc = UserlogAddress.UserPc();
                            get.Upduserid = usercode;
                            get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                            get.Updipno = UserlogAddress.IpAddress();
                            get.Updltude = passModel.Updltude;
                        }
                        db.SaveChanges();
                    }

                    //Companies user statues update(if user exists, but not companyAdmin)
                    var companiesUsersRoleList = (from m in db.AslRoleDbSet where m.Compid == passModel.SelectCompId && m.Userid != companyAdminUserId && m.Moduleid == passModel.Moduleid && m.Menutp == passModel.Menutp && m.Menuid == passModel.Menuid select m).ToList();
                    foreach (var get in companiesUsersRoleList)
                    {
                        get.Status = "I";
                        get.Insertr = "I";
                        get.Updater = "I";
                        get.Deleter = "I";

                        get.Userpc = UserlogAddress.UserPc();
                        get.Upduserid = usercode;
                        get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                        get.Updipno = UserlogAddress.IpAddress();
                        get.Updltude = passModel.Updltude;
                    }
                    db.SaveChanges();

                    string menuName = "";
                    var findmenuName = (from m in db.AslMenuDbSet where m.Moduleid == passModel.Moduleid && m.Menutp == passModel.Menutp && m.Menuid == passModel.Menuid select m).ToList();
                    foreach (var get in findmenuName)
                    {
                        menuName = get.Menunm;
                    }

                    return new
                    {
                        data = menuName,
                        success = true,
                        message = "Update data Successfully."
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
        [System.Web.Http.Route("api/Role/UserWisePermissionList")]
        public object UserWisePermissionList(Int64 userCompanycode, Int64 usercode,Int64 selectUserCode, string moduleId, string menuType)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            List<ViewAslRole> nulllist = new List<ViewAslRole>();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<AslRole> list = new List<AslRole>();
                list = (from m in db.AslRoleDbSet where m.Compid == userCompanycode && m.Userid== selectUserCode && m.Moduleid == moduleId && m.Menutp == menuType select m).ToList();
                if (list.Count != 0)
                    return new
                    {
                        data = list,
                        success = true,
                        message = "Get data Successfully."
                    };
                else return new
                {
                    data = nulllist,
                    success = false,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = nulllist,
                success = false,
                message = "Authorized not permitted."
            };
        }






        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Role/UserWisePermissionUpdate")]
        public object UserWisePermissionUpdate([FromBody]ViewAslRole passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            Int64 companyCode = Convert.ToInt64(passModel.Compid);
            Int64 usercode = Convert.ToInt64(passModel.Userid);
            String logData = "",userName="",operationType="",moduleName="", menuName="",menuType="";
            if (TokenInfo.TokenCheck(companyCode, usercode, token))
            {
                try
                {
                    var findUserName = (from m in db.AslUsercoDbSet where m.Compid == companyCode && m.Userid == passModel.SelectUserId select m).ToList();
                    foreach (var get in findUserName)
                    {
                        userName = get.Usernm.ToString();
                        operationType = get.Optp.ToString();
                    }
                    var findModuleInfo = (from m in db.AslMenumstDbSet where m.Moduleid == passModel.Moduleid select m).ToList();
                    foreach (var get in findModuleInfo)
                    {
                        moduleName = get.Modulenm;
                    }
                    var findMenuInfo = (from m in db.AslMenuDbSet where m.Moduleid == passModel.Moduleid && m.Menutp == passModel.Menutp && m.Menuid == passModel.Menuid select m).ToList();
                    foreach (var get in findMenuInfo)
                    {
                        menuName = get.Menunm;
                    }
                    if (passModel.Menutp == "F")
                    {
                        menuType = "FORMS";
                    }
                    else // passModel.Menutp == "R"
                    {
                        menuType = "REPORTS";
                    }

                    var findUserRoleList = (from m in db.AslRoleDbSet where m.Compid == companyCode && m.Userid == passModel.SelectUserId && m.Moduleid == passModel.Moduleid && m.Menutp == passModel.Menutp && m.Menuid == passModel.Menuid select m).ToList();
                    foreach (var get in findUserRoleList)
                    {
                        get.Status = passModel.Status;
                        get.Insertr = passModel.Insertr;
                        get.Updater = passModel.Updater;
                        get.Deleter = passModel.Deleter;

                        get.Userpc = UserlogAddress.UserPc();
                        get.Upduserid = passModel.Upduserid;
                        get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                        get.Updipno = UserlogAddress.IpAddress();
                        get.Updltude = passModel.Updltude;
                        logData = Convert.ToString("Changed permission to this user(User Name): " + userName + ",\nOperation Type: " + operationType + ",\nModule Name: " + moduleName + ",\nMenu Name: " + menuName + ",\nMenu Type: " + menuType + ",\nStatus: " + get.Status + ",\nInsert: " + get.Insertr + ",\nUpdate: " + get.Updater+ ",\nDelete: " + get.Deleter + ".");
                    }
                    db.SaveChanges();

                    //Update Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = companyCode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "UPDATE";
                    aslLog.Logltude = passModel.Updltude;
                    aslLog.Logdata = logData;
                    aslLog.Tableid = "ASL_ROLE";
                    Log.SaveLog(aslLog);
                    return new
                    {
                        data = passModel,
                        success = true,
                        message = "Update data Successfully."
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


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
