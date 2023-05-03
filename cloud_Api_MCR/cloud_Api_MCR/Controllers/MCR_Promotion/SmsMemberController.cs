using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.DataAccess.OTHERS;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.ASL;
using cloud_Api_MCR.Models.ASL_DTO;

namespace cloud_Api_MCR.Controllers.MCR_Promotion
{
    public class SmsMemberController : AppController
    {
        private DatabaseDbContext db = new DatabaseDbContext();


        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;


        public SmsMemberController()
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





        // GET: /SMS/
        public ActionResult Index()
        {
            WebClient chcksmsqtySndSms = new WebClient();
            var getSMSUserNamePass = from m in db.AslCompanyDbSet
                                     where m.Compid == _loggedCompid
                                     select new { m.Smsidp, m.Smspwp };
            string userName = "", usPss = "";
            foreach (var get in getSMSUserNamePass)
            {
                userName = get.Smsidp;
                usPss = get.Smspwp;
            }

            string qty = "";
            if (userName == null || usPss == null)
            {
                qty = "0";
            }
            else
            {
                qty = chcksmsqtySndSms.DownloadString("http://66.45.237.70/balancechk.php?username=" + userName + "&password=" + usPss);
            }
            TempData["CheckSMSQuantity"] = qty;
            return View();
        }






        [HttpPost]
        public ActionResult Index(MailDTO model)
        {
            if (ModelState.IsValid)
            {
                if (model.Select==null)
                {
                    ViewBag.SMS_MemberMessage = "Select group name filed first!";
                    return View("Index");
                }


                WebClient chcksmsqtySndSms = new WebClient();
                //string userName = "asl-sms";
                //string usPss = "asl.123SMS@3412";

                var getSMSUserNamePass = from m in db.AslCompanyDbSet
                                         where m.Compid == model.CompId
                                         select new { m.Smsidp, m.Smspwp, m.Smssendernm };
                string userName = "", usPss = "", senderName = "";
                foreach (var get in getSMSUserNamePass)
                {
                    senderName = get.Smssendernm;
                    userName = get.Smsidp;
                    usPss = get.Smspwp;
                }

                if (userName != null && usPss != null && senderName != null)
                {
                    try
                    {
                        string qty = chcksmsqtySndSms.DownloadString("http://66.45.237.70/balancechk.php?username=" + userName + "&password=" + usPss);
                        TempData["CheckSMSQuantity"] = qty;

                        WebClient cardsms = new WebClient();
                        string cashStatus = "";




                        //All Member contact list add in ASL_PSMS table. 
                        if (model.Select == "Member" )
                        {
                            var getUploadContactListForMobileNo1 = (from m in db.McrMembersDbSet where m.Compid == model.CompId && m.Mobno1!=null select new { m.Mobno1 }).Distinct().ToList();
                            foreach (var addList in getUploadContactListForMobileNo1)
                            {
                                if (BdNumberValidation.NumberValidate(addList.Mobno1))
                                {
                                    string mobileNo = addList.Mobno1;
                                    Insert_ASL_PSMS_Table(model, mobileNo);
                                }
                            }

                            var getUploadContactListForMobileNo2 = (from m in db.McrMembersDbSet where m.Compid == model.CompId && m.Mobno2 != null select new { m.Mobno2 }).Distinct().ToList();
                            foreach (var addList in getUploadContactListForMobileNo2)
                            {
                                if (BdNumberValidation.NumberValidate(addList.Mobno2))
                                {
                                    string mobileNo = addList.Mobno2;
                                    Insert_ASL_PSMS_Table(model, mobileNo);
                                }
                            }
                        }


                        //Scheme Wise Member contact list add in ASL_PSMS table. 
                        if (model.SchemeId != "")
                        {
                            var getMemberId  = (from m in db.McrMSchemesDbSet where m.Compid == model.CompId && m.Schemeid == model.SchemeId select new { m.Memberid }).Distinct().ToList();
                            foreach (var getMId in getMemberId)
                            {
                                var getUploadContactListForMobileNo1 = (from m in db.McrMembersDbSet where m.Compid == model.CompId && m.Memberid== getMId.Memberid && m.Mobno1 != null select new { m.Mobno1 }).Distinct().ToList();
                                foreach (var addList in getUploadContactListForMobileNo1)
                                {
                                    if (BdNumberValidation.NumberValidate(addList.Mobno1))
                                    {
                                        string mobileNo = addList.Mobno1;
                                        Insert_ASL_PSMS_Table(model, mobileNo);
                                    }
                                }

                                var getUploadContactListForMobileNo2 = (from m in db.McrMembersDbSet where m.Compid == model.CompId && m.Memberid == getMId.Memberid && m.Mobno2 != null select new { m.Mobno2 }).Distinct().ToList();
                                foreach (var addList in getUploadContactListForMobileNo2)
                                {
                                    if (BdNumberValidation.NumberValidate(addList.Mobno2))
                                    {
                                        string mobileNo = addList.Mobno2;
                                        Insert_ASL_PSMS_Table(model, mobileNo);
                                    }
                                }
                            }
                        }




                        //Area Wise Member contact list add in ASL_PSMS table. 
                        if (model.AreaId != null)
                        {
                            var getMemberId = (from m in db.McrMembersDbSet where m.Compid == model.CompId && m.Areaid == model.AreaId select new { m.Memberid }).Distinct().ToList();
                            foreach (var getMId in getMemberId)
                            {
                                var getUploadContactListForMobileNo1 = (from m in db.McrMembersDbSet where m.Compid == model.CompId && m.Memberid == getMId.Memberid && m.Mobno1 != null select new { m.Mobno1 }).Distinct().ToList();
                                foreach (var addList in getUploadContactListForMobileNo1)
                                {
                                    if (BdNumberValidation.NumberValidate(addList.Mobno1))
                                    {
                                        string mobileNo = addList.Mobno1;
                                        Insert_ASL_PSMS_Table(model, mobileNo);
                                    }
                                }

                                var getUploadContactListForMobileNo2 = (from m in db.McrMembersDbSet where m.Compid == model.CompId && m.Memberid == getMId.Memberid && m.Mobno2 != null select new { m.Mobno2 }).Distinct().ToList();
                                foreach (var addList in getUploadContactListForMobileNo2)
                                {
                                    if (BdNumberValidation.NumberValidate(addList.Mobno2))
                                    {
                                        string mobileNo = addList.Mobno2;
                                        Insert_ASL_PSMS_Table(model, mobileNo);
                                    }
                                }
                            }
                        }



                        string currentDate = UserlogAddressforWeb.Timezone(DateTime.Now).ToString("yyyy-MM-dd");
                        DateTime transdate = Convert.ToDateTime(currentDate.Substring(0, 10));
                        Int64 year = Convert.ToInt64(UserlogAddressforWeb.Timezone(DateTime.Now).ToString("yyyy"));
                        var find_ASLPSMS = (from m in db.AslPromotionalSmsDbSet where m.Compid == model.CompId && m.TransDt == transdate && m.Transyy == year && m.Status == "PENDING" select m).ToList();
                        Int64 countSms = 0;
                        foreach (var x in find_ASLPSMS)
                        {
                            if (model.Language == "ENG")
                                cashStatus = cardsms.DownloadString("http://66.45.237.70/api.php?username=" + userName +
                                                   "&password=" + usPss + "&number=" + x.MobNo + "&sender=" + senderName + "&type=0&message=" + x.Message);
                            else
                                cashStatus = cardsms.DownloadString("http://66.45.237.70/api.php?username=" + userName +
                                                   "&password=" + usPss + "&number=" + x.MobNo + "&sender=" + senderName + "&type=2&message=" + x.Message);

                            if (GetApiSmsResponseCodeMeaning.SmsResponseCodeMeaning(cashStatus) == "Request was successful")
                            {
                                countSms++;
                                Update_ASL_PSMS_Table_SendingEmail(x, model);
                            }
                        }

                        if (countSms != 0)
                        {
                            TempData["SMS_MemberMessage"] = "Sending Done in " + countSms + " sms.";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.SMS_MemberMessage = "Sms not sent.";
                            return View("Index");
                        }

                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                        ViewBag.SMS_MemberMessage = "Sending Failed!!";
                        return View();
                    }

                }
                else
                {
                    ViewBag.SMS_MemberMessage = "Your SMS id not registered!!";
                    return View("Index");
                }

            }
            return View();
        }






        public void Insert_ASL_PSMS_Table(MailDTO currentMailBody, string mobileNo)
        {
            AslPromotionalSms smsLogData = new AslPromotionalSms();
            smsLogData.Compid = Convert.ToInt64(currentMailBody.CompId);
            string currentDate = UserlogAddress.Timezone(DateTime.Now).ToString("dd-MMM-yyyy");
            smsLogData.TransDt = Convert.ToDateTime(currentDate);
            smsLogData.Transyy = Convert.ToInt64(UserlogAddress.Timezone(DateTime.Now).ToString("yyyy"));

            string year = UserlogAddress.Timezone(DateTime.Now).ToString("yyyy");
            string last2Digit_inYEAR = year.Substring(2, 2);
            Int64 max = Convert.ToInt64(currentMailBody.CompId + last2Digit_inYEAR + "9999");
            try
            {
                Int64 maxTransNO = Convert.ToInt64((from n in db.AslPromotionalSmsDbSet where n.Compid == currentMailBody.CompId && n.Transyy == smsLogData.Transyy select n.TransNo).Max());
                if (maxTransNO == 0)
                {
                    smsLogData.TransNo = Convert.ToInt64(currentMailBody.CompId + last2Digit_inYEAR + "0001");
                }
                else if (maxTransNO <= max)
                {
                    smsLogData.TransNo = maxTransNO + 1;
                }
            }
            catch
            {
                smsLogData.TransNo = Convert.ToInt64(currentMailBody.CompId + last2Digit_inYEAR + "0001");
            }

            if (mobileNo != null)
            {
                smsLogData.MobNo = mobileNo;
            }

            smsLogData.Language = currentMailBody.Language;
            smsLogData.Message = currentMailBody.Body;
            smsLogData.Status = "PENDING";
            //smsLogData.SENTTM = null;

            smsLogData.Userpc = UserlogAddress.UserPc();
            smsLogData.Insipno = UserlogAddress.IpAddress();
            smsLogData.Instime = Convert.ToDateTime(UserlogAddress.Timezone(DateTime.Now));
            smsLogData.Insuserid = currentMailBody.Insuserid;
            smsLogData.Insltude = Convert.ToString(currentMailBody.Insltude);
            db.AslPromotionalSmsDbSet.Add(smsLogData);
            db.SaveChanges();
        }


        public void Update_ASL_PSMS_Table_SendingEmail(AslPromotionalSms model, MailDTO currentMailBody)
        {
            var updateTable =
                (from m in db.AslPromotionalSmsDbSet where m.Id == model.Id && m.Compid == model.Compid select m).ToList();
            if (updateTable.Count == 1)
            {
                foreach (AslPromotionalSms smsLogData in updateTable)
                {
                    smsLogData.Status = "SENT";
                    smsLogData.SentTm = Convert.ToDateTime(UserlogAddress.Timezone(DateTime.Now));
                    smsLogData.Userpc = UserlogAddress.UserPc();
                    smsLogData.Updipno = UserlogAddress.IpAddress();
                    smsLogData.Updtime = Convert.ToDateTime(UserlogAddress.Timezone(DateTime.Now));
                    smsLogData.Upduserid = currentMailBody.Insuserid;
                    smsLogData.Updltude = Convert.ToString(currentMailBody.Insltude);
                }
                db.SaveChanges();
            }
        }








        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}