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
    public class ApiMemberController : ApiController
    {
        private DatabaseDbContext db;

        public ApiMemberController()
        {
            db = new DatabaseDbContext();
        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Member/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMember passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrMember model = new McrMember();

                    Int64 maxMemberId = Convert.ToInt64((from m in db.McrMembersDbSet where m.Compid == userCompanycode select m.Memberid).DefaultIfEmpty().Max());
                    if (maxMemberId == 0)
                    {
                        model.Memberid = Convert.ToInt64(userCompanycode + "1030001");
                    }
                    else if (maxMemberId != 0)
                    {
                        Int64 maxLimit = Convert.ToInt64(userCompanycode + "1039999");
                        Int64 id = maxMemberId + 1;
                        if (id <= maxLimit)
                        {
                            model.Memberid = id;
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
                    model.Macgid = Convert.ToInt64(userCompanycode + "1030000"); 
                    model.Memberid = model.Memberid;
                    model.Membernm = passModel.Membernm;
                    model.Guardiannm = passModel.Guardiannm;
                    model.Mothernm = passModel.Mothernm;
                    model.Addrpre = passModel.Addrpre;
                    model.Addrper = passModel.Addrper;
                    model.Mobno1 = passModel.Mobno1;
                    model.Mobno2 = passModel.Mobno2;
                    model.Dob = passModel.Dob;
                    model.Age = passModel.Age;
                    model.Nationality = passModel.Nationality;
                    model.Nid = passModel.Nid;
                    model.Religion = passModel.Religion;
                    model.Occupation = passModel.Occupation;
                    model.Refnm = passModel.Refnm;
                    model.Refmid = passModel.Refmid;
                    model.Refcno = passModel.Refcno;
                    model.Sharecno = passModel.Sharecno;
                    model.Sharecdt = passModel.Sharecdt;
                    model.Areaid = passModel.Areaid;
                    model.Branchid = passModel.Branchid;
                    model.Acopdt = passModel.Acopdt;
                    model.Accldt = passModel.Accldt;
                    model.Status = passModel.Status;

                    model.Userpc = UserlogAddress.UserPc();
                    model.Insuserid = usercode;
                    model.Instime = UserlogAddress.Timezone(DateTime.Now);
                    model.Insipno = UserlogAddress.IpAddress();
                    model.Insltude = passModel.Insltude;
                    db.McrMembersDbSet.Add(model);
                    db.SaveChanges();


                    String areaName = "", branchName = "";
                    var findAreaName = (from m in db.McrAreasDbSet
                                        where m.Compid == userCompanycode && m.Areaid == passModel.Areaid
                                        select m).ToList();
                    foreach (var get in findAreaName)
                    {
                        areaName = get.Areanm;
                    }
                    var findBranchName = (from m in db.McrBranchesDbSet
                                          where m.Compid == userCompanycode && m.Branchid == passModel.Branchid
                                          select m).ToList();
                    foreach (var get in findBranchName)
                    {
                        branchName = get.Branchnm;
                    }

                    //Create Log Data Save in to ASL_LOG Table
                    AslLog aslLog = new AslLog();
                    aslLog.Compid = userCompanycode;
                    aslLog.Userid = usercode;
                    aslLog.Logtype = "CREATE";
                    aslLog.Logltude = passModel.Insltude;
                    aslLog.Logdata = Convert.ToString("Member Input page. Member Name: " + passModel.Membernm + ",\nGuardian Name: " + passModel.Guardiannm + ",\nMother Name: " + passModel.Mothernm + ",\nPresent Address: " + passModel.Addrpre + ",\nPermanent Address: " + passModel.Addrper + ",\nMobile(1): " + passModel.Mobno1 + ",\nMobile(2): " + passModel.Mobno2 + ",\nDate Of Birth: " + passModel.Dob + ",\nAge: " + passModel.Age + ",\nNationality: " + passModel.Nationality + ",\nN-ID: " + passModel.Nid + ",\nReligion: " + passModel.Religion + ",\nOccupation: " + passModel.Occupation + ",\nReference Name: " + passModel.Refnm + ",\nReference Member ID: " + passModel.Refmid + ",\nReference Contact NO: " + passModel.Refcno + ",\nShare Certificate No: " + passModel.Sharecno + ",\nShare Certificate Date: " + passModel.Sharecdt + ",\nArea Name: " + areaName + ",\nBranch Name: " + branchName + ",\nA/C Opening Date: " + passModel.Acopdt + ",\nA/C Closing Date: " + passModel.Accldt + ",\nStatus: " + passModel.Status + ".");
                    aslLog.Tableid = "MCR_MEMBER";
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
        [System.Web.Http.Route("api/Member/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMember passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();
            
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findMemberInfo = (from m in db.McrMembersDbSet where m.Compid == passModel.Compid && m.Macgid == passModel.Macgid && m.Memberid == passModel.Memberid select m).ToList();
                    if (findMemberInfo.Count == 1)
                    {
                        //update logic
                        foreach (var get in findMemberInfo)
                        {
                            get.Membernm = passModel.Membernm;
                            get.Guardiannm = passModel.Guardiannm;
                            get.Mothernm = passModel.Mothernm;
                            get.Addrpre = passModel.Addrpre;
                            get.Addrper = passModel.Addrper;
                            get.Mobno1 = passModel.Mobno1;
                            get.Mobno2 = passModel.Mobno2;
                            get.Dob = passModel.Dob;
                            get.Age = passModel.Age;
                            get.Nationality = passModel.Nationality;
                            get.Nid = passModel.Nid;
                            get.Religion = passModel.Religion;
                            get.Occupation = passModel.Occupation;
                            get.Refnm = passModel.Refnm;
                            get.Refmid = passModel.Refmid;
                            get.Refcno = passModel.Refcno;
                            get.Sharecno = passModel.Sharecno;
                            get.Sharecdt = passModel.Sharecdt;
                            get.Areaid = passModel.Areaid;
                            get.Branchid = passModel.Branchid;
                            get.Acopdt = passModel.Acopdt;
                            get.Accldt = passModel.Accldt;
                            get.Status = passModel.Status;

                            get.Userpc = UserlogAddress.UserPc();
                            get.Upduserid = usercode;
                            get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                            get.Updipno = UserlogAddress.IpAddress();
                            get.Updltude = passModel.Updltude;
                        }
                        db.SaveChanges();



                        String areaName = "", branchName = "";
                        var findAreaName = (from m in db.McrAreasDbSet
                                            where m.Compid == userCompanycode && m.Areaid == passModel.Areaid
                                            select m).ToList();
                        foreach (var get in findAreaName)
                        {
                            areaName = get.Areanm;
                        }
                        var findBranchName = (from m in db.McrBranchesDbSet
                                              where m.Compid == userCompanycode && m.Branchid == passModel.Branchid
                                              select m).ToList();
                        foreach (var get in findBranchName)
                        {
                            branchName = get.Branchnm;
                        }

                        //Update Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "UPDATE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = Convert.ToString("Member Input page. Member Name: " + passModel.Membernm + ",\nGuardian Name: " + passModel.Guardiannm + ",\nMother Name: " + passModel.Mothernm + ",\nPresent Address: " + passModel.Addrpre + ",\nPermanent Address: " + passModel.Addrper + ",\nMobile(1): " + passModel.Mobno1 + ",\nMobile(2): " + passModel.Mobno2 + ",\nDate Of Birth: " + passModel.Dob + ",\nAge: " + passModel.Age + ",\nNationality: " + passModel.Nationality + ",\nN-ID: " + passModel.Nid + ",\nReligion: " + passModel.Religion + ",\nOccupation: " + passModel.Occupation + ",\nReference Name: " + passModel.Refnm + ",\nReference Member ID: " + passModel.Refmid + ",\nReference Contact NO: " + passModel.Refcno + ",\nShare Certificate No: " + passModel.Sharecno + ",\nShare Certificate Date: " + passModel.Sharecdt + ",\nArea Name: " + areaName + ",\nBranch Name: " + branchName + ",\nA/C Opening Date: " + passModel.Acopdt + ",\nA/C Closing Date: " + passModel.Accldt + ",\nStatus: " + passModel.Status + ".");
                        aslLog.Tableid = "MCR_MEMBER";
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
        [System.Web.Http.Route("api/Member/Delete")]
        public object Delete(Int64 userCompanycode, Int64 usercode, [FromBody]ViewMcrMember passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    McrMember model = db.McrMembersDbSet.Find(passModel.Compid, passModel.Macgid,passModel.Memberid);
                    if (model != null)
                    {
                        String areaName = "", branchName = "";
                        var findAreaName = (from m in db.McrAreasDbSet where m.Compid == userCompanycode && m.Areaid == model.Areaid
                                select m).ToList();
                        foreach (var get in findAreaName)
                        {
                            areaName = get.Areanm;
                        }
                        var findBranchName = (from m in db.McrBranchesDbSet
                                            where m.Compid == userCompanycode && m.Branchid == model.Branchid
                                              select m).ToList();
                        foreach (var get in findBranchName)
                        {
                            branchName = get.Branchnm;
                        }


                        //Delete Log Data Save in to ASL_LOG Table
                        AslLog aslLog = new AslLog();
                        aslLog.Compid = userCompanycode;
                        aslLog.Userid = usercode;
                        aslLog.Logtype = "DELETE";
                        aslLog.Logltude = passModel.Updltude;
                        aslLog.Logdata = Convert.ToString("Member Input page. Member Name: " + model.Membernm + ",\nGuardian Name: " + model.Guardiannm + ",\nMother Name: " + model.Mothernm + ",\nPresent Address: " + model.Addrpre + ",\nPermanent Address: " + model.Addrper + ",\nMobile(1): " + model.Mobno1 + ",\nMobile(2): " + model.Mobno2 + ",\nDate Of Birth: " + model.Dob + ",\nAge: " + model.Age + ",\nNationality: " + model.Nationality + ",\nN-ID: " + model.Nid + ",\nReligion: " + model.Religion + ",\nOccupation: " + model.Occupation + ",\nReference Name: " + model.Refnm + ",\nReference Member ID: " + model.Refmid + ",\nReference Contact NO: " + model.Refcno + ",\nShare Certificate No: " + model.Sharecno + ",\nShare Certificate Date: " + model.Sharecdt + ",\nArea Name: " +areaName + ",\nBranch Name: " + branchName + ",\nA/C Opening Date: " + model.Acopdt + ",\nA/C Closing Date: " + model.Accldt + ",\nStatus: " + model.Status + ".");
                        aslLog.Tableid = "MCR_MEMBER";
                        Log.SaveLog(aslLog);

                        //Delete Log Data Save in to ASL_DELETE Table
                        AslDelete aslDeleteLog = new AslDelete();
                        aslDeleteLog.Compid = userCompanycode;
                        aslDeleteLog.Userid = usercode;
                        aslDeleteLog.Delltude = passModel.Updltude;
                        aslDeleteLog.Deldata = Convert.ToString("Member Input page. Member Name: " + model.Membernm + ",\nGuardian Name: " + model.Guardiannm + ",\nMother Name: " + model.Mothernm + ",\nPresent Address: " + model.Addrpre + ",\nPermanent Address: " + model.Addrper + ",\nMobile(1): " + model.Mobno1 + ",\nMobile(2): " + model.Mobno2 + ",\nDate Of Birth: " + model.Dob + ",\nAge: " + model.Age + ",\nNationality: " + model.Nationality + ",\nN-ID: " + model.Nid + ",\nReligion: " + model.Religion + ",\nOccupation: " + model.Occupation + ",\nReference Name: " + model.Refnm + ",\nReference Member ID: " + model.Refmid + ",\nReference Contact NO: " + model.Refcno + ",\nShare Certificate No: " + model.Sharecno + ",\nShare Certificate Date: " + model.Sharecdt + ",\nArea Name: " + areaName + ",\nBranch Name: " + branchName + ",\nA/C Opening Date: " + model.Acopdt + ",\nA/C Closing Date: " + model.Accldt + ",\nStatus: " + model.Status + ".");
                        aslDeleteLog.Tableid = "MCR_MEMBER";
                        Log.DeleteLog(aslDeleteLog);

                        //Delete From AslUserco
                        db.McrMembersDbSet.Remove(model);
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
        [System.Web.Http.Route("api/Member/List")]
        public object List(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrMember> list = new List<McrMember>();
                list = (from m in db.McrMembersDbSet where m.Compid == userCompanycode select m).ToList();
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
        [System.Web.Http.Route("api/Member/Information")]
        public object Information(Int64 userCompanycode, Int64 usercode, Int64 macgId, Int64 memberId)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<McrMember> list = new List<McrMember>();
                list = (from m in db.McrMembersDbSet where m.Compid == userCompanycode && m.Macgid == macgId && m.Memberid== memberId select m).ToList();
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
