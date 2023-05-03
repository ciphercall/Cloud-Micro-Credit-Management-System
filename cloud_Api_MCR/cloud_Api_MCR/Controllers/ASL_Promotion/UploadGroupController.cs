using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.ASL;
using cloud_Api_MCR.Models.ASL_DTO;

namespace cloud_Api_MCR.Controllers.ASL_Promotion
{
    public class UploadGroupController : AppController
    {
        private DatabaseDbContext db = new DatabaseDbContext();


        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;


        public UploadGroupController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                _loggedCompid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
                _loggedUserid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
                _loggedToken = Convert.ToString(System.Web.HttpContext.Current.Session["loggedToken"].ToString());
                _loggedUserTp = Convert.ToString(System.Web.HttpContext.Current.Session["loggedUserType"].ToString());
                ViewData["HighLight_Menu_PromotionForm"] = "highlight menu";
            }
            else
            {
                RedirectToAction("Index", "Logout");
            }
        }




        public AslLog aslLog = new AslLog();


        // SAVE ALL INFORMATION from grid(MediMaster data) TO Asl_lOG Database Table.
        public void Insert_UploadGroup_LogData(UploadGroupDTO model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.Compid == model.Compid && n.Userid == model.Insuserid select n.Logslno).Max());
            if (maxSerialNo == 0)
            {
                aslLog.Logslno = Convert.ToInt64("1");
            }
            else
            {
                aslLog.Logslno = maxSerialNo + 1;
            }


            aslLog.Compid = Convert.ToInt64(model.Compid);
            aslLog.Userid = Convert.ToInt64(model.Insuserid);
            aslLog.Logtype = "INSERT";
            aslLog.Logslno = aslLog.Logslno;
            aslLog.Logdate = Convert.ToDateTime(date);
            aslLog.Logtime = Convert.ToString(time);
            aslLog.Logipno = model.Insipno;
            aslLog.Logltude = model.Insltude;
            aslLog.Tableid = "ASL_PGROUPS";
            aslLog.Logdata = Convert.ToString("Contact Group Information Page. Group name: " + model.GroupNm + ".");
            aslLog.Userpc = model.Userpc;

            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }





        // Edit ALL INFORMATION from Grid(MediMaster data) TO Asl_lOG Database Table.
        public void update_UploadGroup_LogData(UploadGroupDTO model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.Compid == model.Compid && n.Userid == model.Insuserid select n.Logslno).Max());
            if (maxSerialNo == 0)
            {
                aslLog.Logslno = Convert.ToInt64("1");
            }
            else
            {
                aslLog.Logslno = maxSerialNo + 1;
            }

            aslLog.Compid = Convert.ToInt64(model.Compid);
            aslLog.Userid = Convert.ToInt64(model.Insuserid);
            aslLog.Logtype = "UPDATE";
            aslLog.Logslno = aslLog.Logslno;
            aslLog.Logdate = Convert.ToDateTime(date);
            aslLog.Logtime = Convert.ToString(time);
            aslLog.Logipno = UserlogAddress.IpAddress();
            aslLog.Logltude = model.Insltude;
            aslLog.Tableid = "ASL_PGROUPS";
            aslLog.Logdata = Convert.ToString("Contact Group Information Page. Group name: " + model.GroupNm + ".");
            aslLog.Userpc = UserlogAddress.UserPc();

            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }






        // Delete ALL INFORMATION from Grid(MediMaster data) TO Asl_lOG Database Table.
        public void Delete_UploadGroup_LogData(UploadGroupDTO model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.Compid == model.Compid && n.Userid == model.Insuserid select n.Logslno).Max());
            if (maxSerialNo == 0)
            {
                aslLog.Logslno = Convert.ToInt64("1");
            }
            else
            {
                aslLog.Logslno = maxSerialNo + 1;
            }

            aslLog.Compid = Convert.ToInt64(model.Compid);
            aslLog.Userid = Convert.ToInt64(model.Insuserid);
            aslLog.Logtype = "DELETE";
            aslLog.Logslno = aslLog.Logslno;
            aslLog.Logdate = Convert.ToDateTime(date);
            aslLog.Logtime = Convert.ToString(time);
            aslLog.Logipno = UserlogAddress.IpAddress();
            aslLog.Logltude = model.Insltude;
            aslLog.Tableid = "ASL_PGROUPS";
            aslLog.Logdata = Convert.ToString("Contact Group Information Page. Group name: " + model.GroupNm + ".");
            aslLog.Userpc = UserlogAddress.UserPc();

            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }


        public AslDelete AslDelete = new AslDelete();

        // Delete ALL INFORMATION from MediMaster TO ASL_DELETE Database Table.
        public void Delete_UploadGroup_LogDelete(UploadGroupDTO model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslDeleteDbSet where n.Compid == model.Compid && n.Userid == model.Insuserid select n.Delslno).Max());
            if (maxSerialNo == 0)
            {
                AslDelete.Delslno = Convert.ToInt64("1");
            }
            else
            {
                AslDelete.Delslno = maxSerialNo + 1;
            }

            AslDelete.Compid = Convert.ToInt64(model.Compid);
            AslDelete.Userid = Convert.ToInt64(model.Insuserid);
            AslDelete.Delslno = AslDelete.Delslno;
            AslDelete.Deldate = Convert.ToDateTime(date);
            AslDelete.Deltime = Convert.ToString(time);
            AslDelete.Delipno = UserlogAddress.IpAddress();
            AslDelete.Delltude = model.Insltude;
            AslDelete.Tableid = "ASL_PGROUPS";
            AslDelete.Deldata = Convert.ToString("Contact Group Information Page. Group name: " + model.GroupNm + ".");
            AslDelete.Userpc = UserlogAddress.UserPc();

            db.AslDeleteDbSet.Add(AslDelete);
            db.SaveChanges();
        }






        // GET: /UploadGroupContact/
        public ActionResult GroupView()
        {
            return View();
        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}