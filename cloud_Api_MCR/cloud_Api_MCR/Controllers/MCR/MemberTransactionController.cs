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
    public class MemberTransactionController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;
        private MemberController memberController;
        private SchemeController schemeController;
        private MemberSchemeController memberSchemeController;
        private AccountChartController accountChartController;
        public MemberTransactionController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                memberController = new MemberController();
                schemeController = new SchemeController();
                memberSchemeController = new MemberSchemeController();
                accountChartController = new AccountChartController();
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "MemberTransaction" && getModel.Actionname == "Create")
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
                                   where  m.Headcd != cash && m.Headcd != bank
                                   select m).ToList();
            foreach (var get in findAccountName)
            {
                ViewGlAcchart getModel = new ViewGlAcchart();
                getModel.Accountcd = get.Accountcd;
                getModel.Accountnm = get.Accountnm;
                accountsList.Add(get);
            }

            TempData["passData_SelectAccountNameList"] = accountsList;
            TempData["passData_SelectMemberList"] = memberController.MemberList();
            TempData["passData_SelectSchemeList"] = schemeController.SchemeList();

            return View();
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewMcrMTrans passModel)
        {
            if (ModelState.IsValid)
            {
                //Http Post - Create   
                String createUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberTransaction/Create/MemoNoWise?userCompanycode=" + _loggedCompid +
                                          "&usercode=" + _loggedUserid;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                     new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),
                     new KeyValuePair<string, string>("Transtp",passModel.Transtp.ToString()),
                     new KeyValuePair<string, string>("Transmy",passModel.Transmy.ToString()),
                     new KeyValuePair<string, string>("Transno",passModel.Transno.ToString()),
                     new KeyValuePair<string, string>("Transsl",passModel.Transsl.ToString()),

                     new KeyValuePair<string, string>("Transdt",passModel.Transdt.ToString()),
                     new KeyValuePair<string, string>("Transfor",passModel.Transfor),
                     new KeyValuePair<string, string>("Transmode",passModel.Transmode),
                     new KeyValuePair<string, string>("Glheadid",passModel.Glheadid.ToString()),
                     new KeyValuePair<string, string>("Schemeid",passModel.Schemeid),
                     new KeyValuePair<string, string>("Memberid",passModel.Memberid.ToString()),
                     new KeyValuePair<string, string>("Internid",passModel.Internid.ToString()),
                     new KeyValuePair<string, string>("Schemeid2",passModel.Schemeid2),
                     new KeyValuePair<string, string>("Memberid2",passModel.Memberid2.ToString()),
                     new KeyValuePair<string, string>("Internid2",passModel.Internid2.ToString()),
                     new KeyValuePair<string, string>("Amount",passModel.Amount.ToString()),
                     new KeyValuePair<string, string>("Remarks",passModel.Remarks),

                     new KeyValuePair<string, string>("Insltude",passModel.Insltude),
                };
                HttpContent query = new FormUrlEncodedContent(queries);
                using (HttpClient httpclient = new HttpClient())
                {
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                    using (HttpResponseMessage response = httpclient.PostAsync(createUrl, query).Result)
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
                                    TempData["MemmberTransactionCreateMessage"] = message;
                                    return RedirectToAction("Create");
                                }
                                else
                                {
                                    TempData["MemmberTransactionCreateMessage"] = message;
                                    return View();
                                }
                            }
                        }
                        else
                        {
                            ViewData["ErrorMessage"] = "Login failed.";
                            Session["HomePage"] = "Show home page";
                            return RedirectToAction("Index", "Logout");
                        }
                    }
                }
            }
            return View(passModel);
        }






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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "MemberTransaction" && getModel.Actionname == "Create")
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
            TempData["passData_SelectMemberList"] = memberController.MemberList();
            TempData["passData_SelectSchemeList"] = schemeController.SchemeList();
            return View();
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ViewMcrMTrans passModel)
        {
            if (ModelState.IsValid)
            {
                passModel.Transsl = 1;

                //Http Post - update 
                String updateUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberTransaction/Update?userCompanycode=" + _loggedCompid +
                                          "&usercode=" + _loggedUserid;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),
                     new KeyValuePair<string, string>("Transtp",passModel.Transtp.ToString()),
                     new KeyValuePair<string, string>("Transmy",passModel.Transmy.ToString()),
                     new KeyValuePair<string, string>("Transno",passModel.Transno.ToString()),
                     new KeyValuePair<string, string>("Transsl",passModel.Transsl.ToString()),

                     new KeyValuePair<string, string>("Transdt",passModel.Transdt.ToString()),
                     new KeyValuePair<string, string>("Transfor",passModel.Transfor),
                     new KeyValuePair<string, string>("Transmode",passModel.Transmode),
                     new KeyValuePair<string, string>("Glheadid",passModel.Glheadid.ToString()),
                     new KeyValuePair<string, string>("Schemeid",passModel.Schemeid),
                     new KeyValuePair<string, string>("Memberid",passModel.Memberid.ToString()),
                     new KeyValuePair<string, string>("Internid",passModel.Internid.ToString()),
                     new KeyValuePair<string, string>("Schemeid2",passModel.Schemeid2),
                     new KeyValuePair<string, string>("Memberid2",passModel.Memberid2.ToString()),
                     new KeyValuePair<string, string>("Internid2",passModel.Internid2.ToString()),
                     new KeyValuePair<string, string>("Amount",passModel.Amount.ToString()),
                     new KeyValuePair<string, string>("Remarks",passModel.Remarks),

                     new KeyValuePair<string, string>("Updltude",passModel.Updltude),
                };
                HttpContent query = new FormUrlEncodedContent(queries);
                using (HttpClient httpclient = new HttpClient())
                {
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                    using (HttpResponseMessage response = httpclient.PostAsync(updateUrl, query).Result)
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
                                    TempData["MemmberTransactionUpdateMessage"] = message;
                                    return RedirectToAction("Update");
                                }
                                else
                                {
                                    ViewBag.EditMessage = message;
                                    return View();
                                }
                            }
                        }
                        else
                        {
                            ViewData["ErrorMessage"] = "Login failed.";
                            Session["HomePage"] = "Show home page";
                            return RedirectToAction("Index", "Logout");
                        }
                    }
                }
            }
            return View(passModel);
        }








        public ActionResult Delete()
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "MemberTransaction" && getModel.Actionname == "Create")
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

            TempData["passData_SelectMemberList"] = memberController.MemberList();
            return View();
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ViewMcrMTrans passModel)
        {
            try
            {
                passModel.Transsl = 1;

                //Http Post - Delete company 
                String deleteUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberTransaction/Delete?userCompanycode=" + _loggedCompid +
                                          "&usercode=" + _loggedUserid;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                     new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),
                     new KeyValuePair<string, string>("Transtp",passModel.Transtp.ToString()),
                     new KeyValuePair<string, string>("Transmy",passModel.Transmy.ToString()),
                     new KeyValuePair<string, string>("Transno",passModel.Transno.ToString()),
                     new KeyValuePair<string, string>("Transsl",passModel.Transsl.ToString()),

                     new KeyValuePair<string, string>("Updltude",passModel.Updltude),
                };
                HttpContent query = new FormUrlEncodedContent(queries);
                using (HttpClient httpclient = new HttpClient())
                {
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                    using (HttpResponseMessage response = httpclient.PostAsync(deleteUrl, query).Result)
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
                                    TempData["MemmberTransactionDeleteMessage"] = message;
                                    return RedirectToAction("Delete");
                                }
                                else
                                {
                                    ViewBag.DeleteMessage = message;
                                    return RedirectToAction("Delete");
                                }
                            }
                        }
                        else
                        {
                            ViewData["ErrorMessage"] = "Login failed.";
                            Session["HomePage"] = "Show home page";
                            return RedirectToAction("Index", "Logout");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return View(passModel);
            }
        }






        //Get Type and Date Wise Member Transaction List
        public List<ViewMcrMTrans> TypeNdDateWiseMemberTransactionList(String transType, String fromDate, String toDate)
        {
            List<ViewMcrMTrans> dateWiseMemberTransactionList = new List<ViewMcrMTrans>();
            //Http Get - get Member List
            String memberLoanInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberTransaction/TypeDateWiseList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&transType=" + transType + "&fromDate=" + fromDate + "&toDate=" + toDate;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(memberLoanInfoUrl).Result)
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
                                    ViewMcrMTrans getModel = new ViewMcrMTrans();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transtp = (String)get["Transtp"];
                                    getModel.Transmy = (String)get["Transmy"];
                                    getModel.Transno = (Int64)get["Transno"];
                                    getModel.Transsl = (Int64)get["Transsl"];

                                    getModel.Transdt = (DateTime)get["Transdt"];
                                    getModel.Transfor = (String)get["Transfor"];
                                    getModel.Transmode = (String)get["Transmode"];
                                    if (get["Glheadid"].ToString()!="")
                                    {
                                        getModel.Glheadid = (Int64)get["Glheadid"];
                                    }
                                    getModel.Schemeid = (String)get["Schemeid"];
                                    if (get["Memberid"].ToString() != "")
                                    {
                                        getModel.Memberid = (Int64)get["Memberid"];
                                    }
                                    if (get["Internid"].ToString() != "")
                                    {
                                        getModel.Internid = (Int64)get["Internid"];
                                    }
                                    if (get["Schemeid2"].ToString() != "")
                                    {
                                        getModel.Schemeid2 = (String)get["Schemeid2"];
                                    }
                                    if (get["Memberid2"].ToString() != "")
                                    {
                                        getModel.Memberid2 = (Int64)get["Memberid2"];
                                    }
                                    if (get["Internid2"].ToString() != "")
                                    {
                                        getModel.Internid2 = (Int64)get["Internid2"];
                                    }
                                    getModel.Amount = Convert.ToDecimal(get["Amount"]);
                                    getModel.Remarks = (String)get["Remarks"];
                                    dateWiseMemberTransactionList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return dateWiseMemberTransactionList;
        }






        //Get Date Wise Member Transaction List
        public List<ViewMcrMTrans> DateWiseMemberTransactionList(String fromDate, String toDate)
        {
            List<ViewMcrMTrans> dateWiseMemberTransactionList = new List<ViewMcrMTrans>();
            //Http Get - get Member List
            String memberLoanInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberTransaction/DateWiseList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&fromDate=" + fromDate + "&toDate=" + toDate;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(memberLoanInfoUrl).Result)
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
                                    ViewMcrMTrans getModel = new ViewMcrMTrans();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transtp = (String)get["Transtp"];
                                    getModel.Transmy = (String)get["Transmy"];
                                    getModel.Transno = (Int64)get["Transno"];
                                    getModel.Transsl = (Int64)get["Transsl"];

                                    getModel.Transdt = (DateTime)get["Transdt"];
                                    getModel.Transfor = (String)get["Transfor"];
                                    getModel.Transmode = (String)get["Transmode"];
                                    if (get["Glheadid"].ToString() != "")
                                    {
                                        getModel.Glheadid = (Int64)get["Glheadid"];
                                    }
                                    getModel.Schemeid = (String)get["Schemeid"];
                                    if (get["Memberid"].ToString() != "")
                                    {
                                        getModel.Memberid = (Int64)get["Memberid"];
                                    }
                                    if (get["Internid"].ToString() != "")
                                    {
                                        getModel.Internid = (Int64)get["Internid"];
                                    }
                                    if (get["Schemeid2"].ToString() != "")
                                    {
                                        getModel.Schemeid2 = (String)get["Schemeid2"];
                                    }
                                    if (get["Memberid2"].ToString() != "")
                                    {
                                        getModel.Memberid2 = (Int64)get["Memberid2"];
                                    }
                                    if (get["Internid2"].ToString() != "")
                                    {
                                        getModel.Internid2 = (Int64)get["Internid2"];
                                    }
                                    getModel.Amount = Convert.ToDecimal(get["Amount"]);
                                    getModel.Remarks = (String)get["Remarks"];
                                    dateWiseMemberTransactionList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return dateWiseMemberTransactionList;
        }





        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult DateChanged_getMonth(DateTime txtTransDate, string txtTransType)//with Trans No Generation
        {
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






        public JsonResult InternalIdLoadAfterSchemeSelect(String schemeId)
        {
            List<SelectListItem> getInternalIdList = new List<SelectListItem>();
            List<ViewMcrMScheme> memberSchemeList = (List<ViewMcrMScheme>)memberSchemeController.MemberSchemeList();

            var findInternalId = (from m in memberSchemeList where m.Compid == _loggedCompid && m.Schemeid == schemeId select m).ToList();
            foreach (var getModel in findInternalId)
            {
                getInternalIdList.Add(new SelectListItem { Text = getModel.Internid.ToString(), Value = Convert.ToString(getModel.Internid) });
            }
            return Json(new SelectList(getInternalIdList, "Value", "Text"));
        }






        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetMemberName(Int64 txtInternId, string txtSchemeId)//with Trans No Generation
        {
            String memberName = "";
            Int64 memberid = 0;
            List<ViewMcrMScheme> memberSchemeList = (List<ViewMcrMScheme>)memberSchemeController.MemberSchemeList();
            var findMemberId = (from m in memberSchemeList where m.Compid == _loggedCompid && m.Schemeid == txtSchemeId && m.Internid== txtInternId select m).ToList();
            foreach (var getModel in findMemberId)
            {
                memberid = getModel.Memberid;
            }

            List<ViewMcrMember> memberList = (List<ViewMcrMember>)memberController.MemberList();
            var findMemberName = (from m in memberList where m.Compid == _loggedCompid && m.Memberid == memberid select m).ToList();
            foreach (var getModel in findMemberName)
            {
                memberName = getModel.Membernm;
            }

            var result = new { MemberName = memberName, MemberId = memberid };
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        public JsonResult Invoiceload(string theDate, string type)
        {
            DateTime transDate = Convert.ToDateTime(theDate);
            //Http Get - get Invoice
            List<SelectListItem> trans = new List<SelectListItem>();
            String getTransactionInvoiceInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberTransaction/GetTransNo?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&transType=" + type + "&transDate=" + transDate;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(getTransactionInvoiceInfoUrl).Result)
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
                                    ViewMcrMTrans getModel = new ViewMcrMTrans();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transno = (Int64)get["Transno"];
                                    trans.Add(new SelectListItem { Text = getModel.Transno.ToString(), Value = getModel.Transno.ToString() });
                                }
                            }
                        }
                    }
                }
            }
            return Json(new SelectList(trans, "Value", "Text"));
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
                                    if (get["Schemeid"].ToString() != "")
                                    {
                                        getModel.Schemeid = (String)get["Schemeid"];
                                    }
                                    if (get["Memberid"].ToString() != "")
                                    {
                                        getModel.Memberid = (Int64)get["Memberid"];
                                    }
                                    if (get["Internid"].ToString() != "")
                                    {
                                        getModel.Internid = (Int64)get["Internid"];
                                    }
                                    if (get["Glheadid"].ToString() != "")
                                    {
                                        getModel.Glheadid = (Int64)get["Glheadid"];
                                    }
                                    if (get["Schemeid2"].ToString() != "")
                                    {
                                        getModel.Schemeid2 = (String)get["Schemeid2"];
                                    }
                                    if (get["Memberid2"].ToString() != "")
                                    {
                                        getModel.Memberid2 = (Int64)get["Memberid2"];
                                    }
                                    if (get["Internid2"].ToString() != "")
                                    {
                                        getModel.Internid2 = (Int64)get["Internid2"];
                                    }
                                    getModel.Amount = (Decimal)get["Amount"];
                                    getModel.Remarks = (String)get["Remarks"];
                                }
                            }
                        }
                    }
                }
            }


            String memberNm = "", memberNm2 = "";
            List<ViewMcrMember> memberList = (List<ViewMcrMember>)memberController.MemberList();
            var findMemberName = (from m in memberList where m.Compid == _loggedCompid && m.Memberid == getModel.Memberid select m).ToList();
            foreach (var get1 in findMemberName)
            {
                memberNm = get1.Membernm;
            }

            var findMemberName2 = (from m in memberList where m.Compid == _loggedCompid && m.Memberid == getModel.Memberid2 select m).ToList();
            foreach (var get2 in findMemberName2)
            {
                memberNm2 = get2.Membernm;
            }

            var userResult = new
            {
                transFor = getModel.Transfor,
                transMode = getModel.Transmode,
                acountName = getModel.Glheadid,
                sheme = getModel.Schemeid,
                internalid = getModel.Internid,
                memberId = getModel.Memberid,
                memberName = memberNm,
                sheme2 = getModel.Schemeid2,
                internalid2 = getModel.Internid2,
                memberId2 = getModel.Memberid2,
                memberName2 = memberNm2,
                amount = getModel.Amount,
                remark = getModel.Remarks
            };
            return Json(userResult, JsonRequestBehavior.AllowGet);
        }
    }
}