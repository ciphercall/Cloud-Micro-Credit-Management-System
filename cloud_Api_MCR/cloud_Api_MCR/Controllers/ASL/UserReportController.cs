using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using cloud_Api_MCR.DataAccess.OTHERS;
using cloud_Api_MCR.Models.ASL_DTO;
using Newtonsoft.Json.Linq;

namespace cloud_Api_MCR.Controllers.ASL
{
    public class UserReportController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp, _loggedCompanyName, _loggedCompanyAddress, _loggedCompanyAddress2, _loggedCompanyContactno;

        public UserReportController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                _loggedCompid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
                _loggedUserid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
                _loggedToken = Convert.ToString(System.Web.HttpContext.Current.Session["loggedToken"].ToString());
                _loggedUserTp = Convert.ToString(System.Web.HttpContext.Current.Session["loggedUserType"].ToString());
                _loggedCompanyName = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyName"].ToString());
                _loggedCompanyAddress = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyAddress"].ToString());
                _loggedCompanyAddress2 = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyAddress2"].ToString());
                _loggedCompanyContactno = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyContactno"].ToString());
            }
            else
            {
                RedirectToAction("Index", "Logout");
            }
            ViewData["HighLight_Menu_UserReport"] = "High Light Menu";
        }





        public ActionResult Index()
        {
            List<ViewAslUserco> userList = new List<ViewAslUserco>();
            if (_loggedUserTp == "CADMIN" || _loggedUserTp == "UADMIN")
            {
                //Http Get - get User List 
                String userInfoUrl = null;
                if (_loggedUserTp == "CADMIN")
                {
                    userInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/User/UserList?userCompanycode=" + _loggedCompid +
                                    "&usercode=" + _loggedUserid + "&selectCompanyCode=" + _loggedCompid;
                }
                else
                {
                    userInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/User/UserListExceptCompanyAdmin?userCompanycode=" + _loggedCompid +
                                   "&usercode=" + _loggedUserid + "&selectCompanyCode=" + _loggedCompid;
                }
                using (HttpClient httpclient = new HttpClient())
                {
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                    using (HttpResponseMessage response = httpclient.GetAsync(userInfoUrl).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                bool success;
                                string message = "";

                                var result = content.ReadAsStringAsync().Result;
                                JObject jobject = JObject.Parse(result);
                                var successMessage = jobject["success"].ToString();
                                message = jobject["message"].ToString();
                                success = successMessage.Equals("True") ? true : false;
                                if (success == true)
                                {
                                    IList<JToken> results = jobject["data"].Children().ToList();
                                    foreach (JToken get in results)
                                    {
                                        ViewAslUserco getModel = new ViewAslUserco();
                                        getModel.Compid = (Int64)get["Compid"];
                                        getModel.Userid = (Int64)get["Userid"];
                                        getModel.Usernm = (String)get["Usernm"];
                                        getModel.Loginid = (String)get["Loginid"];
                                        userList.Add(getModel);
                                        TempData["passData_SelectUserList"] = userList;
                                    }
                                }
                                else
                                {
                                    ViewData["ErrorMessage"] = message;
                                    Session["HomePage"] = "Show home page";
                                    return RedirectToAction("Index", "Logout");
                                }
                            }
                        }
                    }
                }
            }
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ViewUserReport userReportModel)
        {
            if (ModelState.IsValid)
            {
                DateTime fdate = Convert.ToDateTime(userReportModel.FromDate);
                String fromDate = fdate.ToString("dd-MMM-yyyy");
                DateTime tdate = Convert.ToDateTime(userReportModel.ToDate);
                String toDate = tdate.ToString("dd-MMM-yyyy");

                List<ViewUserReport> userLogList = new List<ViewUserReport>();

                //Http Get - get User log Data List 
                String userLogUrl = HttpClientBaseAddress.BaseAddress() + "api/Log/UserLogDataList?userCompanycode=" + _loggedCompid +
                             "&usercode=" + _loggedUserid + "&selectCompanyCode=" + _loggedCompid + "&selectUsercode=" + userReportModel.UserId + "&fromDate=" + fromDate + "&toDate=" + toDate;

                using (HttpClient httpclient = new HttpClient())
                {
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                    using (HttpResponseMessage response = httpclient.GetAsync(userLogUrl).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                bool success = false;
                                string message = "";

                                var result = content.ReadAsStringAsync().Result;
                                JObject jobject = JObject.Parse(result);
                                var successMessage = jobject["success"].ToString();
                                message = jobject["message"].ToString();
                                success = successMessage.Equals("True") ? true : false;
                                if (success == true)
                                {
                                    IList<JToken> results = jobject["data"].Children().ToList();
                                    foreach (JToken get in results)
                                    {
                                        ViewUserReport getModel = new ViewUserReport();
                                        getModel.Compid = (Int64)get["Compid"];
                                        getModel.UserId = (Int64)get["Userid"];
                                        getModel.LogType = (String)get["Logtype"];
                                        DateTime logdate = (DateTime)get["Logdate"];
                                        getModel.LogDate = logdate.ToString("dd-MMM-yyyy");
                                        getModel.LogTime = (String)get["Logtime"];
                                        getModel.LogData = (String)get["Logdata"];
                                        userLogList.Add(getModel);
                                    }
                                    TempData["passData_UserLogList"] = userLogList;
                                    TempData["passData_userReportModel"] = userReportModel;
                                    return RedirectToAction("_GetUserLogResult");
                                }
                                else
                                {
                                    ViewData["ErrorMessage"] = message;
                                    Session["HomePage"] = "Show home page";
                                    return RedirectToAction("Index", "Logout");
                                }
                            }
                        }
                    }
                }
            }
            return View();
        }


        public ActionResult _GetUserLogResult()
        {
            var list = (List<ViewUserReport>)TempData["passData_UserLogList"];
            var model = (ViewUserReport)TempData["passData_userReportModel"];

            //From Date
            string fDate = model.FromDate.ToString();
            DateTime datefrom = DateTime.Parse(fDate);
            string fromDate = datefrom.ToString("dd-MMM-yyyy");
            //To Date
            string tDate = model.ToDate.ToString();
            DateTime dateto = DateTime.Parse(tDate);
            string toDate = dateto.ToString("dd-MMM-yyyy");
            
            String userNm = System.Web.HttpContext.Current.Session["loggedUserName"].ToString();
            
            ViewBag.CompanyName = _loggedCompanyName;
            ViewBag.CompanyAddress = _loggedCompanyAddress;
            ViewBag.CompanyAddress2 = _loggedCompanyAddress2;

            ViewBag.UserName = userNm;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            return View(list);
        }

    }
}