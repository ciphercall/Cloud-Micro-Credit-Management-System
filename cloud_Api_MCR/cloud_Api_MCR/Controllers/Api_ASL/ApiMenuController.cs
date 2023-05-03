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
using Microsoft.SqlServer.Server;

namespace cloud_Api_MCR.Controllers.Api_ASL
{
    public class ApiMenuController : ApiController
    {
        private DatabaseDbContext db;

        public ApiMenuController()
        {
            db = new DatabaseDbContext();
        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Menu/Create")]
        public object Create([FromBody]ViewAslMenu passModel)
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
                    AslMenu model = new AslMenu();
                    var maxData = Convert.ToString((from n in db.AslMenuDbSet where n.Moduleid == passModel.Moduleid && n.Menutp == passModel.Menutp select n.Menuid).Max());
                    if (maxData == null)
                    {
                        model.Menuid = Convert.ToString(passModel.Menutp + passModel.Moduleid + "01");
                    }
                    else //maxData != null
                    {
                        var subString = Convert.ToString((from n in db.AslMenuDbSet where n.Moduleid == passModel.Moduleid && n.Menutp == passModel.Menutp select n.Menuid.Substring(3, 2)).Max());
                        string id = Convert.ToString(subString);
                        int maxMenuid = int.Parse(id) + 1;

                        if (maxMenuid < 10)
                        {
                            model.Menuid = Convert.ToString(passModel.Menutp + passModel.Moduleid + "0" + maxMenuid);
                        }
                        else if (maxMenuid < 100)
                        {
                            model.Menuid = Convert.ToString(passModel.Menutp + passModel.Moduleid + maxMenuid);
                        }
                        else
                        {
                            return new
                            {
                                data = model.Menuid,
                                success = false,
                                message = "Menu entry not possible."
                            };
                        }
                    }
                    model.Moduleid = passModel.Moduleid;
                    model.Menutp = passModel.Menutp;
                    model.Serial = passModel.Serial;
                    model.Menunm = passModel.Menunm;
                    model.Actionname = passModel.Actionname;
                    model.Controllername = passModel.Controllername;
                    model.Userpc = UserlogAddress.UserPc();
                    model.Insuserid = usercode;
                    model.Instime = UserlogAddress.Timezone(DateTime.Now);
                    model.Insipno = UserlogAddress.IpAddress();
                    model.Insltude = passModel.Insltude;
                    db.AslMenuDbSet.Add(model);
                    db.SaveChanges();

                    //When Super Admin (SADMIN) create a New Menu List (From/Report) then the Role database list add for all user. 
                    var userList = from a in db.AslUsercoDbSet where a.Optp != "SADMIN" select a;
                    foreach (AslUserco a in userList)
                    {
                        string status = "", insert = "", update = "", delete = "";
                        if (a.Optp != "CADMIN")
                        {
                            status = "I";
                            insert = "I";
                            update = "I";
                            delete = "I";
                        }
                        else
                        {
                            status = "A";
                            insert = "A";
                            update = "A";
                            delete = "A";
                        }

                        AslRole role = new AslRole()
                        {
                            Compid = Convert.ToInt64(a.Compid),
                            Userid = Convert.ToInt64(a.Userid),
                            Moduleid = model.Moduleid,
                            Menuid = model.Menuid,
                            Menutp = model.Menutp,
                            Serial = model.Serial,
                            Status = status,
                            Insertr = insert,
                            Updater = update,
                            Deleter = delete,
                            Menuname = passModel.Menunm,
                            Actionname = passModel.Actionname,
                            Controllername = passModel.Controllername,

                            Userpc = UserlogAddress.UserPc(),
                            Insuserid = usercode,
                            Insipno = UserlogAddress.IpAddress(),
                            Instime = UserlogAddress.Timezone(DateTime.Now),
                            Insltude = passModel.Insltude
                    };
                        db.AslRoleDbSet.Add(role);
                    }
                    db.SaveChanges();
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
        [System.Web.Http.Route("api/Menu/Update")]
        public object Update([FromBody]ViewAslMenu passModel)
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
                    var findMenuInfo = (from m in db.AslMenuDbSet where m.Moduleid == passModel.Moduleid && m.Menutp == passModel.Menutp && m.Menuid == passModel.Menuid select m).ToList();
                    if (findMenuInfo.Count == 1)
                    {
                        foreach (var get in findMenuInfo)
                        {
                            get.Serial = passModel.Serial;
                            get.Menunm = passModel.Menunm;
                            get.Actionname = passModel.Actionname;
                            get.Controllername = passModel.Controllername;

                            get.Userpc = UserlogAddress.UserPc();
                            get.Upduserid = usercode;
                            get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                            get.Updipno = UserlogAddress.IpAddress();
                            get.Updltude = passModel.Updltude;
                        }
                        db.SaveChanges();
                    }

                    //all ASL-ROLE data modified
                    var roleList = (from m in db.AslRoleDbSet where m.Moduleid == passModel.Moduleid && m.Menutp == passModel.Menutp && m.Menuid == passModel.Menuid select m).ToList();
                    foreach (var get in roleList)
                    {
                        get.Serial = passModel.Serial;
                        get.Menuname = passModel.Menunm;
                        get.Actionname = passModel.Actionname;
                        get.Controllername = passModel.Controllername;

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
        [System.Web.Http.Route("api/Menu/Delete")]
        public object Delete([FromBody]ViewAslMenu passModel)
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
                    AslMenu model = db.AslMenuDbSet.Find(passModel.Moduleid, passModel.Menutp, passModel.Menuid);

                    //Delete all information from AslRole Table,when it match to the ModuleID, MenuID, MenuType
                    var roleList = (from sub in db.AslRoleDbSet select sub)
                        .Where(sub => sub.Moduleid == model.Moduleid && sub.Menutp == passModel.Menutp && sub.Menuid == passModel.Menuid).ToList();
                    foreach (var n in roleList)
                    {
                        db.AslRoleDbSet.Remove(n);
                    }
                    db.SaveChanges();

                    //Delete From AslMenu
                    db.AslMenuDbSet.Remove(model);
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
        [System.Web.Http.Route("api/Menu/MenuList")]
        public object MenuList(Int64 userCompanycode, Int64 usercode, string moduleId, string menuType)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            List<ViewAslMenu> nulllist = new List<ViewAslMenu>();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<AslMenu> list = new List<AslMenu>();
                list = (from m in db.AslMenuDbSet where m.Moduleid == moduleId && m.Menutp == menuType select m).OrderBy(x=>x.Serial).ToList();
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







        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Menu/MenuInformation")]
        public object MenuInformation(Int64 companyCode, Int64 usercode, string moduleId, string menuType, string menuId)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            List<ViewAslMenu> nulllist = new List<ViewAslMenu>();

            if (TokenInfo.TokenCheck(companyCode, usercode, token))
            {
                List<AslMenu> list = new List<AslMenu>();
                list = (from m in db.AslMenuDbSet where m.Moduleid == moduleId && m.Menutp == menuType && m.Menuid == menuId select m).ToList();
                if (list.Count == 1)
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



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


    }
}
