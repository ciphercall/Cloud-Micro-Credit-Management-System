using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Web.Http;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.ASL;
using cloud_Api_MCR.Models.ASL_DTO;

namespace cloud_Api_MCR.Controllers.Api_ASL
{
    public class ApiUserController : ApiController
    {
        private DatabaseDbContext db;

        public ApiUserController()
        {
            db = new DatabaseDbContext();
        }





        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/User/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewAslUserco passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            //Int64 companyCode = Convert.ToInt64(passModel.Compid);
            //Int64 usercode = Convert.ToInt64(passModel.Userid);
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    AslUserco model = new AslUserco();
                    Int64 maxUserId = Convert.ToInt64((from m in db.AslUsercoDbSet where m.Compid == passModel.Compid select m.Userid).DefaultIfEmpty().Max());
                    Int64 maximum = Convert.ToInt64(passModel.Compid + "99");

                    if (maxUserId == 0)
                    {
                        model.Userid = Convert.ToInt64(passModel.Compid + "01");
                    }
                    else if (maxUserId <= maximum)
                    {
                        model.Userid = maxUserId + 1;
                    }
                    else // maxUserId == maximum
                    {
                        return new
                        {
                            data = maxUserId,
                            success = false,
                            message = "Not possible entry!"
                        };
                    }

                    model.Compid = Convert.ToInt64(passModel.Compid);
                    model.Usernm = passModel.Usernm;
                    model.Deptnm = passModel.Deptnm;
                    model.Optp = passModel.Optp;
                    model.Address = passModel.Address;
                    model.Mobno = passModel.Mobno;
                    model.Emailid = passModel.Emailid;
                    model.Loginby = passModel.Loginby;
                    model.Loginid = passModel.Loginid;
                    model.Loginpw = passModel.Loginpw;
                    model.Timefr = passModel.Timefr;
                    model.Timeto = passModel.Timeto;
                    model.Status = passModel.Status;

                    model.Userpc = UserlogAddress.UserPc();
                    model.Insuserid = usercode;
                    model.Instime = UserlogAddress.Timezone(DateTime.Now);
                    model.Insipno = UserlogAddress.IpAddress();
                    model.Insltude = passModel.Insltude;
                    db.AslUsercoDbSet.Add(model);
                    db.SaveChanges();

                    //User row created in Token Table
                    AslToken aslToken = new AslToken();
                    aslToken.Compid = model.Compid;
                    aslToken.Userid = model.Userid;
                    aslToken.ExpireDate = UserlogAddress.Timezone(DateTime.Now);
                    db.AslTokenDbSet.Add(aslToken);
                    db.SaveChanges();

                    //Role database list add
                    var roleList = (from a in db.AslMenuDbSet select a).OrderBy(x => x.Serial);
                    foreach (AslMenu a in roleList)
                    {
                        String status = "", Insertr = "", updater = "", deleter = "";
                        if (passModel.Optp == "CADMIN") //Company Admin
                        {
                            status = "A";
                            Insertr = "A";
                            updater = "A";
                            deleter = "A";
                        }
                        else // UADMIN (User Admin), USER
                        {
                            status = "I";
                            Insertr = "I";
                            updater = "I";
                            deleter = "I";
                        }

                        AslRole role = new AslRole()
                        {
                            Compid = Convert.ToInt64(model.Compid),
                            Userid = Convert.ToInt64(model.Userid),
                            Moduleid = a.Moduleid,
                            Menutp = a.Menutp,
                            Menuid = a.Menuid,

                            Serial = a.Serial,
                            Status = status,
                            Insertr = Insertr,
                            Updater = updater,
                            Deleter = deleter,
                            Menuname = a.Menunm,
                            Actionname = a.Actionname,
                            Controllername = a.Controllername,

                            Userpc = UserlogAddress.UserPc(),
                            Upduserid = usercode,
                            Updtime = UserlogAddress.Timezone(DateTime.Now),
                            Updipno = UserlogAddress.IpAddress(),
                            Updltude = passModel.Updltude
                        };
                        db.AslRoleDbSet.Add(role);
                    }
                    db.SaveChanges();


                    //Create Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "CREATE";
                    aslLog.Logltude = passModel.Updltude;
                    aslLog.Logdata = Convert.ToString("User Name: " + model.Usernm + ",\nDepartment Name: " + model.Deptnm + ",\nOperation Type: " + model.Optp + ",\nAddress: " + model.Address + ",\nMobile No: " + model.Mobno + ",\nEmail ID: " + model.Emailid + ",\nLogin BY: " + model.Loginby + ",\nLogin ID: " + model.Loginid + ",\nTime From: " + model.Timefr + ",\nTime To: " + model.Timeto + ",\nStatus: " + model.Status + ".");
                    aslLog.Tableid = "ASL_USERCO";
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
                        data = ex,
                        success = true,
                        message = "Save failed!"
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
        [System.Web.Http.Route("api/User/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewAslUserco passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();
            
            String logData = "";
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var mobileExists = db.AslUsercoDbSet.Count(d => d.Mobno == passModel.Mobno);
                    var emailIdExists = db.AslUsercoDbSet.Count(d => d.Emailid == passModel.Emailid);
                    var loginIdExists = db.AslUsercoDbSet.Count(d => d.Loginid == passModel.Loginid);

                    var findUserInfo = (from m in db.AslUsercoDbSet where m.Compid == passModel.Compid && m.Userid == passModel.Userid select m).ToList();
                    if (findUserInfo.Count == 1)
                    {
                        foreach (var get in findUserInfo)
                        {
                            if (mobileExists == 0)
                            {
                                get.Mobno = passModel.Mobno;
                            }
                            if (emailIdExists == 0)
                            {
                                get.Emailid = passModel.Emailid;
                            }
                            if (loginIdExists == 0)
                            {
                                get.Loginid = passModel.Loginid;
                            }

                            get.Usernm = passModel.Usernm;
                            get.Deptnm = passModel.Deptnm;
                            get.Optp = passModel.Optp;
                            get.Address = passModel.Address;
                            get.Loginby = passModel.Loginby;
                            get.Loginpw = passModel.Loginpw;
                            get.Timefr = passModel.Timefr;
                            get.Timeto = passModel.Timeto;
                            get.Status = passModel.Status;

                            get.Userpc = UserlogAddress.UserPc();
                            get.Upduserid = usercode;
                            get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                            get.Updipno = UserlogAddress.IpAddress();
                            get.Updltude = passModel.Updltude;
                            logData = Convert.ToString("User Name: " + get.Usernm + ",\nDepartment Name: " + get.Deptnm + ",\nOperation Type: " + get.Optp + ",\nAddress: " + get.Address + ",\nMobile No: " + get.Mobno + ",\nEmail ID: " + get.Emailid + ",\nLogin BY: " + get.Loginby + ",\nLogin ID: " + get.Loginid + ",\nTime From: " + get.Timefr + ",\nTime To: " + get.Timeto + ",\nStatus: " + get.Status + ".");
                        }
                        db.SaveChanges();


                        //Update Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "UPDATE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = logData;
                        aslLog.Tableid = "ASL_USERCO";
                        Log.SaveLog(aslLog);

                        return new
                        {
                            data = passModel,
                            success = true,
                            message = "Update data Successfully."
                        };
                    }
                    else
                    {
                        return new
                        {
                            success = true,
                            message = "Update failed!"
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
        [System.Web.Http.Route("api/User/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewAslUserco passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();
            
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    AslUserco model = db.AslUsercoDbSet.Find(passModel.Compid, passModel.Userid);

                    //Delete Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "DELETE";
                    aslLog.Logltude = passModel.Updltude;
                    aslLog.Logdata = Convert.ToString("User Name: " + model.Usernm + ",\nDepartment Name: " + model.Deptnm + ",\nOperation Type: " + model.Optp + ",\nAddress: " + model.Address + ",\nMobile No: " + model.Mobno + ",\nEmail ID: " + model.Emailid + ",\nLogin BY: " + model.Loginby + ",\nLogin ID: " + model.Loginid + ",\nTime From: " + model.Timefr + ",\nTime To: " + model.Timeto + ",\nStatus: " + model.Status + ".");
                    aslLog.Tableid = "ASL_USERCO";
                    Log.SaveLog(aslLog);

                    //Delete Log Data Save in to ASL_DELETE Table
                    AslDelete aslDeleteLog = new AslDelete();
                    aslDeleteLog.Compid = userCompanycode;
                    aslDeleteLog.Userid = usercode;
                    aslDeleteLog.Delltude = passModel.Updltude;
                    aslDeleteLog.Deldata = Convert.ToString("User Name: " + model.Usernm + ",\nDepartment Name: " + model.Deptnm + ",\nOperation Type: " + model.Optp + ",\nAddress: " + model.Address + ",\nMobile No: " + model.Mobno + ",\nEmail ID: " + model.Emailid + ",\nLogin BY: " + model.Loginby + ",\nLogin ID: " + model.Loginid + ",\nTime From: " + model.Timefr + ",\nTime To: " + model.Timeto + ",\nStatus: " + model.Status + ".");
                    aslDeleteLog.Tableid = "ASL_USERCO";
                    Log.DeleteLog(aslDeleteLog);

                    //Delete From AslUserco
                    db.AslUsercoDbSet.Remove(model);
                    db.SaveChanges();

                    //Role database list Delete
                    var roleList = (from sub in db.AslRoleDbSet select sub).Where(sub => sub.Compid == passModel.Compid && sub.Userid == passModel.Userid);
                    foreach (var n in roleList)
                    {
                        db.AslRoleDbSet.Remove(n);
                    }
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
        [System.Web.Http.Route("api/User/UserList")]
        public object UserList(Int64 userCompanycode, Int64 usercode,Int64 selectCompanyCode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            List<ViewAslUserco> nulllist = new List<ViewAslUserco>();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<AslUserco> list = new List<AslUserco>();
                list = (from m in db.AslUsercoDbSet where m.Compid == selectCompanyCode select m).ToList();
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
        [System.Web.Http.Route("api/User/UserListExceptCompanyAdmin")]
        public object UserListExceptCompanyAdmin(Int64 userCompanycode, Int64 usercode, Int64 selectCompanycode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            List<ViewAslUserco> nulllist = new List<ViewAslUserco>();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<AslUserco> list = new List<AslUserco>();
                list = (from m in db.AslUsercoDbSet where m.Compid == selectCompanycode && m.Optp != "CADMIN" select m).ToList();
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
        [System.Web.Http.Route("api/User/UserInformation")]
        public object UserInformation(Int64 userCompanycode, Int64 usercode, Int64 selectCompanyCode, Int64 selectUserCode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            List<ViewAslUserco> nulllist = new List<ViewAslUserco>();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<AslUserco> list = new List<AslUserco>();
                list = (from m in db.AslUsercoDbSet where m.Compid == selectCompanyCode && m.Userid == selectUserCode select m).ToList();
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
        [System.Web.Http.Route("api/User/Check_MobileNo")]
        public object Check_MobileNo(Int64 userCompanycode, Int64 usercode, String mobileNo)
        {
            int count = 0;
            var findMobileNo = (from m in db.AslUsercoDbSet where m.Mobno != mobileNo select m).ToList();
            foreach (var get in findMobileNo)
            {
                if(get.Mobno == mobileNo)
                {
                    count = 1;
                }
            }
            if (count == 1)
            {
                return new
                {
                    data = "1",
                    success = true,
                    message = "User mobile number already exists!"
                };
            }
            else
            {
                return new
                {
                    data = "0",
                    success = false,
                    message = "User mobile number does not match!"
                };
            }
        }



        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/User/Check_UserEmail")]
        public object Check_UserEmail(Int64 userCompanycode, Int64 usercode, String emailId)
        {
            int count = 0;
            var findEmailNumber = (from m in db.AslUsercoDbSet where m.Emailid != emailId select m).ToList();
            foreach (var get in findEmailNumber)
            {
                if (get.Emailid == emailId)
                {
                    count = 1;
                }
            }
            if (count == 1)
            {
                return new
                {
                    data = "1",
                    success = true,
                    message = "User email ID already exists!"
                };
            }
            else
            {
                return new
                {
                    data = "0",
                    success = false,
                    message = "User email ID does not match!"
                };
            }
        }



        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/User/Check_UserLogIn")]
        public object Check_UserLogIn(Int64 userCompanycode, Int64 usercode, String loginid)
        {
            int count = 0;
            var findLoginId = (from m in db.AslUsercoDbSet where m.Loginid != loginid select m).ToList();
            foreach (var get in findLoginId)
            {
                if (get.Loginid == loginid)
                {
                    count = 1;
                }
            }
            if (count == 1)
            {
                return new
                {
                    data = "1",
                    success = true,
                    message = "User Login ID already exists!"
                };
            }
            else
            {
                return new
                {
                    data = "0",
                    success = false,
                    message = "User Login ID does not match!"
                };
            }
        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


    }
}
