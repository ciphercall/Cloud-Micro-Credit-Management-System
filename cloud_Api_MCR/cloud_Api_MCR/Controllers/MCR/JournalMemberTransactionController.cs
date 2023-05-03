using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using cloud_Api_MCR.Controllers.Account;
using cloud_Api_MCR.DataAccess.OTHERS;
using cloud_Api_MCR.Models.Account_DTO;
using cloud_Api_MCR.Models.ASL_DTO;
using cloud_Api_MCR.Models.MCR_DTO;
using Newtonsoft.Json.Linq;

namespace cloud_Api_MCR.Controllers.MCR
{
    public class JournalMemberTransactionController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;
        private BranchController brunchController;
        private SchemeController schemeController;
        private AccountChartController accountChartController;
        private MemberController memberController;
        private MemberSchemeController memberSchemeController;

        public JournalMemberTransactionController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                brunchController = new BranchController();
                schemeController = new SchemeController();
                memberController = new MemberController();
                accountChartController = new AccountChartController();
                memberSchemeController = new MemberSchemeController();
                _loggedCompid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
                _loggedUserid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
                _loggedToken = Convert.ToString(System.Web.HttpContext.Current.Session["loggedToken"].ToString());
                _loggedUserTp = Convert.ToString(System.Web.HttpContext.Current.Session["loggedUserType"].ToString());
                ViewData["HighLight_Menu_BillingForm"] = "highlight menu";
            }
            else
            {
                RedirectToAction("Index", "Logout");
            }
        }





        public ActionResult Create()
        {
            //Http Get - get Role List and binding the users module wise menu
            String userRoleUrl = HttpClientBaseAddress.BaseAddress() + "api/Role/RoleList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;
            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(userRoleUrl).Result)
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
                                    ViewAslRole getModel = new ViewAslRole();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Userid = (Int64)get["Userid"];
                                    getModel.Status = (string)get["Status"];
                                    getModel.Insertr = (string)get["Insertr"];
                                    getModel.Updater = (string)get["Updater"];
                                    getModel.Deleter = (string)get["Deleter"];
                                    getModel.Menuname = (string)get["Menuname"];
                                    getModel.Actionname = (string)get["Actionname"];
                                    getModel.Controllername = (string)get["Controllername"];
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "JournalMemberTransaction" && getModel.Actionname == "Create")
                                    {
                                        TempData["UserRoleInfo"] = getModel;
                                        break;
                                    }
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

            List<ViewGlAcchart> accountsList = new List<ViewGlAcchart>();
            var getAccountChartList = accountChartController.AccountChartList();
            Int64 cash = Convert.ToInt64(_loggedCompid + "101"), bank = Convert.ToInt64(_loggedCompid + "102");
            var findAccountName = (from m in getAccountChartList
                                   where m.Headcd != cash && m.Headcd != bank
                                   select m).ToList();
            foreach (var get in findAccountName)
            {
                ViewGlAcchart getModel = new ViewGlAcchart();
                getModel.Accountcd = get.Accountcd;
                getModel.Accountnm = get.Accountnm;
                accountsList.Add(get);
            }

            TempData["passData_SelectAccountNameList"] = accountsList;
            TempData["passData_SelectSchemeList"] = schemeController.SchemeList();
            return View();
        }

        //[HttpPost]
        //public ActionResult Create(ViewMcrCollect model)
        //{
        //    return RedirectToAction("Create");
        //}




        public ActionResult Update()
        {
            //Http Get - get Role List and binding the users module wise menu
            String userRoleUrl = HttpClientBaseAddress.BaseAddress() + "api/Role/RoleList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;
            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(userRoleUrl).Result)
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
                                    ViewAslRole getModel = new ViewAslRole();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Userid = (Int64)get["Userid"];
                                    getModel.Status = (string)get["Status"];
                                    getModel.Insertr = (string)get["Insertr"];
                                    getModel.Updater = (string)get["Updater"];
                                    getModel.Deleter = (string)get["Deleter"];
                                    getModel.Menuname = (string)get["Menuname"];
                                    getModel.Actionname = (string)get["Actionname"];
                                    getModel.Controllername = (string)get["Controllername"];
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "JournalMemberTransaction" && getModel.Actionname == "Create")
                                    {
                                        TempData["UserRoleInfo"] = getModel;
                                        break;
                                    }
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

            List<ViewGlAcchart> accountsList = new List<ViewGlAcchart>();
            var getAccountChartList = accountChartController.AccountChartList();
            Int64 cash = Convert.ToInt64(_loggedCompid + "101"), bank = Convert.ToInt64(_loggedCompid + "102");
            var findAccountName = (from m in getAccountChartList
                                   where m.Headcd != cash && m.Headcd != bank
                                   select m).ToList();
            foreach (var get in findAccountName)
            {
                ViewGlAcchart getModel = new ViewGlAcchart();
                getModel.Accountcd = get.Accountcd;
                getModel.Accountnm = get.Accountnm;
                accountsList.Add(get);
            }

            TempData["passData_SelectAccountNameList"] = accountsList;
            TempData["passData_SelectSchemeList"] = schemeController.SchemeList();
            return View();
        }




        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult DateChanged_getMonth(DateTime txtTransDate)//with Trans No Generation
        {
            string txtTransType = "JOUR";
            string converttoString = Convert.ToString(txtTransDate.ToString("dd-MMM-yyyy"));
            string getYear = converttoString.Substring(9, 2);
            string getMonth = converttoString.Substring(3, 3).ToUpper();
            getMonth = getMonth + "-" + getYear;

            String transno = "";
            //Http Get - get Find Max TransNo
            String findMaxTransNoUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberTransaction/FindMaxTransNo?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&transType=" + txtTransType + "&transDate=" + txtTransDate;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(findMaxTransNoUrl).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            bool success;
                            string message = "";

                            var getresult = content.ReadAsStringAsync().Result;
                            JObject jobject = JObject.Parse(getresult);
                            var successMessage = jobject["success"].ToString();
                            message = jobject["message"].ToString();
                            success = successMessage.Equals("True") ? true : false;
                            if (success == true)
                            {
                                transno = jobject["data"].ToString();
                            }
                        }
                    }
                }
            }
            var result = new { month = getMonth, TransNo = transno };
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetMemberTransactionInformation(String transType, String monthYear, Int64 memoNo)
        {
            ViewMcrMTrans getModel = new ViewMcrMTrans();

            //Http Get - get User Information 
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberTransaction/Information?userCompanycode=" + _loggedCompid +
                               "&usercode=" + _loggedUserid + "&transType=" + transType + "&transMonthYear=" + monthYear + "&transno=" + memoNo + "&transSerial=" + 1;
            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(infoUrl).Result)
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
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transfor = (String)get["Transfor"];
                                    getModel.Transmode = (String)get["Transmode"];
                                }
                            }
                        }
                    }
                }
            }

            var userResult = new
            {
                transFor = getModel.Transfor,
                transMode = getModel.Transmode,
            };
            return Json(userResult, JsonRequestBehavior.AllowGet);
        }
    }
}