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
    public class ApiMemberNomineeController : ApiController
    {
        private DatabaseDbContext db;

        public ApiMemberNomineeController()
        {
            db = new DatabaseDbContext();
        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/MemberNominee/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMNominee passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrMNominee model = new McrMNominee();

                    Int64 maxNomineeId = Convert.ToInt64((from m in db.McrMNomineesDbSet where m.Compid == userCompanycode && m.Memberid == passModel.Memberid select m.Nomineeid).DefaultIfEmpty().Max());
                    if (maxNomineeId == 0)
                    {
                        model.Nomineeid = Convert.ToInt64(passModel.Memberid + "1");
                    }
                    else if (maxNomineeId != 0)
                    {
                        Int64 maxLimit = Convert.ToInt64(passModel.Memberid + "9");
                        Int64 id = maxNomineeId + 1;
                        if (id <= maxLimit)
                        {
                            model.Nomineeid = id;
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
                    model.Nomineeid = model.Nomineeid;
                    model.Nomineenm = passModel.Nomineenm;
                    model.Guardiannm = passModel.Guardiannm;
                    model.Mothernm = passModel.Mothernm;
                    model.Age = passModel.Age;
                    model.Relation = passModel.Relation;
                    model.Npcnt = passModel.Npcnt;
                    model.Remarks = passModel.Remarks;

                    model.Userpc = UserlogAddress.UserPc();
                    model.Insuserid = usercode;
                    model.Instime = UserlogAddress.Timezone(DateTime.Now);
                    model.Insipno = UserlogAddress.IpAddress();
                    model.Insltude = passModel.Insltude;
                    db.McrMNomineesDbSet.Add(model);
                    db.SaveChanges();

                    //Create Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "CREATE";
                    aslLog.Logltude = passModel.Insltude;
                    aslLog.Logdata = Convert.ToString("Member Nominee Input page. Nominee Name: " + passModel.Nomineenm + ",\nGuardian Name: " + passModel.Guardiannm + ",\nMother Name: " + passModel.Mothernm + ",\nAge: " + passModel.Age + ",\nRelation: " + passModel.Relation + ",\nNominee (%): " + passModel.Npcnt + ",\nRemarks: " + passModel.Remarks + ".");
                    aslLog.Tableid = "MCR_MNOMINEE";
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
        [System.Web.Http.Route("api/MemberNominee/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMNominee passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findMemberNomineeInfo = (from m in db.McrMNomineesDbSet where m.Compid == passModel.Compid && m.Memberid == passModel.Memberid && m.Nomineeid == passModel.Nomineeid select m).ToList();
                    if (findMemberNomineeInfo.Count == 1)
                    {
                        //update logic
                        foreach (var get in findMemberNomineeInfo)
                        {
                            get.Nomineenm = passModel.Nomineenm;
                            get.Guardiannm = passModel.Guardiannm;
                            get.Mothernm = passModel.Mothernm;
                            get.Age = passModel.Age;
                            get.Relation = passModel.Relation;
                            get.Npcnt = passModel.Npcnt;
                            get.Remarks = passModel.Remarks;

                            get.Userpc = UserlogAddress.UserPc();
                            get.Upduserid = usercode;
                            get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                            get.Updipno = UserlogAddress.IpAddress();
                            get.Updltude = passModel.Updltude;
                        }
                        db.SaveChanges();

                        //Update Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "UPDATE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = Convert.ToString("Member Nominee Input page. Nominee Name: " + passModel.Nomineenm + ",\nGuardian Name: " + passModel.Guardiannm + ",\nMother Name: " + passModel.Mothernm + ",\nAge: " + passModel.Age + ",\nRelation: " + passModel.Relation + ",\nNominee (%): " + passModel.Npcnt + ",\nRemarks: " + passModel.Remarks + ".");
                        aslLog.Tableid = "MCR_MNOMINEE";
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
        [System.Web.Http.Route("api/MemberNominee/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMNominee passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrMNominee model = db.McrMNomineesDbSet.Find(passModel.Compid, passModel.Memberid, passModel.Nomineeid);
                    if (model != null)
                    {
                        //Delete Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "DELETE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = Convert.ToString("Member Nominee Input page. Nominee Name: " + model.Nomineenm + ",\nGuardian Name: " + model.Guardiannm + ",\nMother Name: " + model.Mothernm + ",\nAge: " + model.Age + ",\nRelation: " + model.Relation + ",\nNominee (%): " + model.Npcnt + ",\nRemarks: " + model.Remarks + ".");
                        aslLog.Tableid = "MCR_MNOMINEE";
                        Log.SaveLog(aslLog);

                        //Delete Log Data Save in to ASL_DELETE Table
                        AslDelete aslDeleteLog = new AslDelete();
                        aslDeleteLog.Compid = userCompanycode;
                        aslDeleteLog.Userid = usercode;
                        aslDeleteLog.Delltude = passModel.Updltude;
                        aslDeleteLog.Deldata = Convert.ToString("Member Nominee Input page. Nominee Name: " + model.Nomineenm + ",\nGuardian Name: " + model.Guardiannm + ",\nMother Name: " + model.Mothernm + ",\nAge: " + model.Age + ",\nRelation: " + model.Relation + ",\nNominee (%): " + model.Npcnt + ",\nRemarks: " + model.Remarks + ".");
                        aslDeleteLog.Tableid = "MCR_MNOMINEE";
                        Log.DeleteLog(aslDeleteLog);

                        //Delete From AslUserco
                        db.McrMNomineesDbSet.Remove(model);
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
        [System.Web.Http.Route("api/MemberNominee/List")]
        public object List(Int64 userCompanycode, Int64 usercode, Int64 memberId)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrMNominee> list = new List<McrMNominee>();
                list = (from m in db.McrMNomineesDbSet where m.Compid == userCompanycode && m.Memberid == memberId select m).ToList();
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
        [System.Web.Http.Route("api/MemberNominee/TotalList")]
        public object TotalList(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrMNominee> list = new List<McrMNominee>();
                list = (from m in db.McrMNomineesDbSet where m.Compid == userCompanycode select m).ToList();
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
        [System.Web.Http.Route("api/MemberNominee/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, Int64 memberId, Int64 nomineeId)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrMNominee> list = new List<McrMNominee>();
                list = (from m in db.McrMNomineesDbSet where m.Compid == userCompanycode && m.Memberid == memberId && m.Nomineeid == nomineeId select m).ToList();
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
