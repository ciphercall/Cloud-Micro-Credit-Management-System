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
    public class UploadEditController : AppController
    {
        private DatabaseDbContext db = new DatabaseDbContext();


        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;


        public UploadEditController()
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




        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }



        private bool BdNumberValidate(string number)
        {
            try
            {
                if (number.Length > 13 || number.Length < 13)
                {
                    return false;
                }
                else
                {
                    string operatorCode = number.Substring(0, 5);
                    switch (operatorCode)
                    {
                        case "88018":
                        case "88017":
                        case "88019":
                        case "88016":
                        case "88011":
                        case "88015":
                            return true; //all of the operator in case is return true
                            break;
                        default:
                            return false; //other operator code return false
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        public AslLog aslLog = new AslLog();

        public void Insert_Exchange_LogData(UploadContactDTO model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.Compid == model.CompId && n.Userid == model.Insuserid select n.Logslno).Max());
            if (maxSerialNo == 0)
            {
                aslLog.Logslno = Convert.ToInt64("1");
            }
            else
            {
                aslLog.Logslno = maxSerialNo + 1;
            }

            aslLog.Compid = Convert.ToInt64(model.CompId);
            aslLog.Userid = Convert.ToInt64(model.Insuserid);
            aslLog.Logtype = "INSERT";
            aslLog.Logslno = aslLog.Logslno;
            aslLog.Logdate = Convert.ToDateTime(date);
            aslLog.Logtime = Convert.ToString(time);
            aslLog.Logipno = UserlogAddress.IpAddress();
            aslLog.Logltude = model.Insltude;
            aslLog.Tableid = "ASL_PCONTACTS";

            string email = "", mobile1 = "", mobile2 = "";
            if (IsValidEmail(model.Email))
            {
                email = model.Email;
            }
            if (BdNumberValidate(model.MobNo1))
            {
                mobile1 = model.MobNo1;
            }
            if (BdNumberValidate(model.MobNo2))
            {
                mobile2 = model.MobNo2;
            }

            aslLog.Logdata = Convert.ToString("Exchange contact information. Group Name: " + model.ToGroupNm + ",\nName: " + model.Name + ",\nEmail: " + email + ",\nMobile No 1: " + mobile1 + ",\nMobile No 2: " + mobile2 + ",\nAddress: " + model.Address + ".");
            aslLog.Userpc = UserlogAddress.UserPc();

            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }



        public void Update_LogData(UploadContactDTO model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.Compid == model.CompId && n.Userid == model.Insuserid select n.Logslno).Max());
            if (maxSerialNo == 0)
            {
                aslLog.Logslno = Convert.ToInt64("1");
            }
            else
            {
                aslLog.Logslno = maxSerialNo + 1;
            }

            aslLog.Compid = Convert.ToInt64(model.CompId);
            aslLog.Userid = Convert.ToInt64(model.Insuserid);
            aslLog.Logtype = "UPDATE";
            aslLog.Logslno = aslLog.Logslno;
            aslLog.Logdate = Convert.ToDateTime(date);
            aslLog.Logtime = Convert.ToString(time);
            aslLog.Logipno = UserlogAddress.IpAddress();
            aslLog.Logltude = model.Insltude;
            aslLog.Tableid = "ASL_PCONTACTS";

            string email = "", mobile1 = "", mobile2 = "";
            if (IsValidEmail(model.Email))
            {
                email = model.Email;
            }
            if (BdNumberValidate(model.MobNo1))
            {
                mobile1 = model.MobNo1;
            }
            if (BdNumberValidate(model.MobNo2))
            {
                mobile2 = model.MobNo2;
            }

            aslLog.Logdata = Convert.ToString("Update contact information. Group Name: " + model.ToGroupNm + ",\nName: " + model.Name + ",\nEmail: " + email + ",\nMobile No 1: " + mobile1 + ",\nMobile No 2: " + mobile2 + ",\nAddress: " + model.Address + ".");
            aslLog.Userpc = UserlogAddress.UserPc();

            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }


        public void Delete_Exchange_LogData(UploadContactDTO model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.Compid == model.CompId && n.Userid == model.Insuserid select n.Logslno).Max());
            if (maxSerialNo == 0)
            {
                aslLog.Logslno = Convert.ToInt64("1");
            }
            else
            {
                aslLog.Logslno = maxSerialNo + 1;
            }

            aslLog.Compid = Convert.ToInt64(model.CompId);
            aslLog.Userid = Convert.ToInt64(model.Insuserid);
            aslLog.Logtype = "DELETE";
            aslLog.Logslno = aslLog.Logslno;
            aslLog.Logdate = Convert.ToDateTime(date);
            aslLog.Logtime = Convert.ToString(time);
            aslLog.Logipno = UserlogAddress.IpAddress();
            aslLog.Logltude = model.Insltude;
            aslLog.Tableid = "ASL_PCONTACTS";

            aslLog.Logdata = Convert.ToString("Group wise contact information page. Group Name: " + model.ToGroupNm + ",\nName: " + model.Name + ",\nEmail: " + model.Email + ",\nMobile No 1: " + model.MobNo1 + ",\nMobile No 2: " + model.MobNo2 + ",\nAddress: " + model.Address + ".");
            aslLog.Userpc = UserlogAddress.UserPc();

            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }





        public AslDelete AslDelete = new AslDelete();

        // Delete ALL INFORMATION from TO ASL_DELETE Database Table.
        public void Delete_Exchange_LogDelete(UploadContactDTO model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslDeleteDbSet where n.Compid == model.CompId && n.Userid == model.Insuserid select n.Delslno).Max());
            if (maxSerialNo == 0)
            {
                AslDelete.Delslno = Convert.ToInt64("1");
            }
            else
            {
                AslDelete.Delslno = maxSerialNo + 1;
            }

            AslDelete.Compid = Convert.ToInt64(model.CompId);
            AslDelete.Userid = Convert.ToInt64(model.Insuserid);
            AslDelete.Delslno = AslDelete.Delslno;
            AslDelete.Deldate = Convert.ToDateTime(date);
            AslDelete.Deltime = Convert.ToString(time);
            AslDelete.Delipno = UserlogAddress.IpAddress();
            AslDelete.Delltude = model.Insltude;
            AslDelete.Tableid = "ASL_PCONTACTS";

            AslDelete.Deldata = Convert.ToString("Group wise contact information page. Group Name: " + model.ToGroupNm + ",\nName: " + model.Name + ",\nEmail: " + model.Email + ",\nMobile No 1: " + model.MobNo1 + ",\nMobile No 1: " + model.MobNo2 + ",\nAddress: " + model.Address + ".");
            AslDelete.Userpc = UserlogAddress.UserPc();

            db.AslDeleteDbSet.Add(AslDelete);
            db.SaveChanges();
        }









        //Get: /ExchangeContact/
        public ActionResult EditContact()
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