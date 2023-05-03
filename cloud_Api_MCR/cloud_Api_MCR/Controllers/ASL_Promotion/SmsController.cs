using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.DataAccess.OTHERS;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.ASL;
using cloud_Api_MCR.Models.ASL_DTO;

namespace cloud_Api_MCR.Controllers.ASL_Promotion
{
    public class SmsController : AppController
    {
        private DatabaseDbContext db = new DatabaseDbContext();

        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;


        public SmsController()
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
            //string userName = "asl-sms";
            //string usPss = "asl.123SMS@3412";
            
            var getSMSUserNamePass = from m in db.AslCompanyDbSet where m.Compid == _loggedCompid select new { m.Smsidp, m.Smspwp };
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
                if (model.GroupId == null && model.MobileNo == null)
                {
                    ViewBag.SMSMessage = "Select group name or Mobile number filed first!";
                    return View("Index");
                }


                WebClient chcksmsqtySndSms = new WebClient();
                //string userName = "asl-sms";
                //string usPss = "asl.123SMS@3412";

                var getSMSUserNamePass = from m in db.AslCompanyDbSet
                                         where m.Compid == model.CompId
                                         select new { m.Smsidp, m.Smspwp, m.Smssendernm };
                string userName = "", usPss = "", senderName = "";
                foreach (var VARIABLE in getSMSUserNamePass)
                {
                    senderName = VARIABLE.Smssendernm;
                    userName = VARIABLE.Smsidp;
                    usPss = VARIABLE.Smspwp;
                }

                if (userName != null && usPss != null && senderName != null)
                {
                    try
                    {
                        string qty = chcksmsqtySndSms.DownloadString("http://66.45.237.70/balancechk.php?username=" + userName + "&password=" + usPss);
                        TempData["CheckSMSQuantity"] = qty;

                        WebClient cardsms = new WebClient();
                        string cashStatus = "";

                        //Current group contact list add in ASL_PEMAIL table. 
                        if (model.GroupId != null)
                        {
                            var getUploadContactList = (from m in db.AslPromotionalContactDbSet where m.Compid == model.CompId && m.GroupId == model.GroupId select new { m.MobNo1, m.MobNo2 }).ToList();
                            foreach (var addList in getUploadContactList)
                            {
                                if (BdNumberValidation.NumberValidate(addList.MobNo1))
                                {
                                    string mobileNo = addList.MobNo1;
                                    Insert_ASL_PSMS_Table(model, mobileNo);
                                }
                                if (BdNumberValidation.NumberValidate(addList.MobNo2))
                                {
                                    string mobileNo = addList.MobNo2;
                                    Insert_ASL_PSMS_Table(model, mobileNo);
                                }
                            }
                        }


                        //model.ToEmail list add in ASL_PEMAIL Table.
                        if (model.MobileNo != null)
                        {
                            string[] multi = model.MobileNo.Split(',');
                            foreach (var multiemailId in multi)
                            {
                                if (multiemailId != "" && BdNumberValidation.NumberValidate(multiemailId))
                                {
                                    string mobileNo = multiemailId;
                                    Insert_ASL_PSMS_Table(model, mobileNo);
                                }
                            }
                        }

                        string currentDate = UserlogAddress.Timezone(DateTime.Now).ToString("yyyy-MM-dd");
                        DateTime transdate = Convert.ToDateTime(currentDate.Substring(0, 10));
                        Int64 year = Convert.ToInt64(UserlogAddress.Timezone(DateTime.Now).ToString("yyyy"));
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

                        //  if ((cashStatus == "-1") || (cashStatus == "-2") || (cashStatus == "-3") || (cashStatus == "-5") ||
                        //(cashStatus == "-6") || (cashStatus == "-10") || (cashStatus == "-11") ||
                        //(cashStatus == "-13") || (cashStatus == "-22") || (cashStatus == "-23") || (cashStatus == "-26") ||
                        //(cashStatus == "-27") || (cashStatus == "-28") || (cashStatus == "-29") || (cashStatus == "-30") ||
                        //(cashStatus == "-33") || (cashStatus == "-34") || (cashStatus == "-99"))
                        //  {
                        //      ViewBag.SMSMessage = "Sms not sent.";
                        //      return View();
                        //  }
                        //  else
                        //  {
                        //      TempData["SMSMessage"] = "Sms sent.";
                        //      return RedirectToAction("Index");
                        //  }


                        if (countSms != 0)
                        {
                            TempData["SMSMessage"] = "Sending Done in " + countSms + " sms.";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.SMSMessage = "Sms not sent.";
                            return View("Index");
                        }

                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                        ViewBag.SMSMessage = "Sending Failed!!";
                        return View();
                    }
                }
                else
                {
                    ViewBag.SMSMessage = "Your SMS id not registered!!";
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
                    smsLogData.SentTm = UserlogAddress.Timezone(DateTime.Now);
                    smsLogData.Userpc = UserlogAddress.UserPc();
                    smsLogData.Updipno = UserlogAddress.IpAddress();
                    smsLogData.Updtime = UserlogAddress.Timezone(DateTime.Now);
                    smsLogData.Upduserid = currentMailBody.Insuserid;
                    smsLogData.Updltude = Convert.ToString(currentMailBody.Insltude);
                }
                db.SaveChanges();
            }
        }










        // GET: /Mail/
        public ActionResult getPendingSMS()
        {
            WebClient chcksmsqtySndSms = new WebClient();
            //string userName = "asl-sms";
            //string usPss = "asl.123SMS@3412";
            
            var getSMSUserNamePass = from m in db.AslCompanyDbSet where m.Compid == _loggedCompid select new { m.Smsidp, m.Smspwp };
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
        public ActionResult getPendingSMS(PendingMailSmsDTO model, string command)
        {
            if (ModelState.IsValid)
            {
                DateTime transDate = Convert.ToDateTime(model.TransDt);
                if (command == "Search")
                {
                    List<AslPromotionalSms> pendingList = new List<AslPromotionalSms>();
                    var findPendingList =
                        (from m in db.AslPromotionalSmsDbSet
                         where m.Compid == model.CompId && m.TransDt == transDate && m.Status == "PENDING"
                         select m).ToList();
                    foreach (var x in findPendingList)
                    {
                        pendingList.Add(new AslPromotionalSms { MobNo = x.MobNo.ToString(), Message = x.Message.ToString(), Status = x.Status.ToString() });
                        ViewData["PendingList_SMS"] = pendingList;
                    }
                    return View();
                }
                else if (command == "Send")
                {
                    try
                    {
                        WebClient chcksmsqtySndSms = new WebClient();
                        //string userName = "asl-sms";
                        //string usPss = "asl.123SMS@3412";

                        var getSMSUserNamePass = from m in db.AslCompanyDbSet where m.Compid == model.CompId select new { m.Smsidp, m.Smspwp, m.Smssendernm };
                        string userName = "", usPss = "", senderName = "";
                        foreach (var getData in getSMSUserNamePass)
                        {
                            userName = getData.Smsidp;
                            usPss = getData.Smspwp;
                            senderName = getData.Smssendernm;
                        }


                        if (userName == null || usPss == null)
                        {
                            ViewBag.PendingSMSMessage = "Your Email id not registered!!";
                            return View();
                        }

                        string qty = chcksmsqtySndSms.DownloadString("http://66.45.237.70/balancechk.php?username=" + userName + "&password=" + usPss);
                        TempData["CheckSMSQuantity"] = qty;

                        WebClient cardsms = new WebClient();
                        string cashStatus = "";
                        var findPendingList = (from m in db.AslPromotionalSmsDbSet where m.Compid == model.CompId && m.TransDt == transDate && m.Status == "PENDING" select m).ToList();
                        Int64 count = 0;
                        foreach (var x in findPendingList)
                        {
                            count++;
                            if (x.Language == "ENG")
                                cashStatus = cardsms.DownloadString("http://66.45.237.70/api.php?username=" + userName +
                                                "&password=" + usPss + "&number=" + x.MobNo + "&sender=" + senderName + "&type=0&message=" + x.Message);
                            else // if (x.LANGUAGE == "BNG")
                                cashStatus = cardsms.DownloadString("http://66.45.237.70/api.php?username=" + userName +
                                                   "&password=" + usPss + "&number=" + x.MobNo + "&sender=" + senderName + "&type=2&message=" + x.Message);

                            if (GetApiSmsResponseCodeMeaning.SmsResponseCodeMeaning(cashStatus) == "Request was successful")
                            {
                                count++;
                                MailDTO pendingSMSBody = new MailDTO();
                                pendingSMSBody.Insuserid = model.Upduserid;
                                pendingSMSBody.Insltude = model.Updltude;
                                Update_ASL_PSMS_Table_SendingEmail(x, pendingSMSBody);
                            }

                        }
                        ViewBag.PendingSMSMessage = "Send Succesfully " + count + " pending sms.";
                        return View();
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                        ViewBag.PendingSMSMessage = "Sending Failed!!";
                        return View();
                    }
                }
            }
            return View("getPendingSMS");
        }






        //tag-it autocomplete
        public JsonResult TagSearch_tagit(string term, string compid)
        {
            var companyid = Convert.ToInt16(compid);
            var mobileNO1 = (from p in db.AslPromotionalContactDbSet
                             where p.Compid == companyid && p.MobNo1 != null
                             select p.MobNo1).ToList();
            var mobileNO2 = (from p in db.AslPromotionalContactDbSet
                             where p.Compid == companyid && p.MobNo2 != null
                             select p.MobNo2).ToList();
            var tags = mobileNO1.Union(mobileNO2).Distinct().ToList();

            return this.Json(tags.Where(t => t.StartsWith(term)),
                       JsonRequestBehavior.AllowGet);

        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}