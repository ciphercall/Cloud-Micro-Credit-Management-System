using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using cloud_Api_MCR.DataAccess.OTHERS;
using cloud_Api_MCR.Models.ASL_DTO;
using cloud_Api_MCR.Models.MCR_DTO;
using Newtonsoft.Json.Linq;

namespace cloud_Api_MCR.Controllers.MCR
{
    public class MemberLoanController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;
        private MemberController memberController;
        private SchemeController schemeController;
        private MemberSchemeController memberSchemeController;

        public MemberLoanController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                memberController = new MemberController();
                schemeController = new SchemeController();
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




        public ActionResult Index()
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "MemberLoan" && getModel.Actionname == "Index")
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
        public ActionResult Index(ViewMcrMLoan passModel, String command)
        {
            if (command == "Search")
            {
                TempData["passData_MonthYear"] = passModel.Transmy;
                TempData["passData_MemberLoanList"] = MemberLoanList(passModel.Transmy);
                return RedirectToAction("Index");
            }
            else // Create
            {
                return RedirectToAction("Create");
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "MemberLoan" && getModel.Actionname == "Index")
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


            string dateTimetoString = Convert.ToString(UserlogAddressforWeb.Timezone(DateTime.Now).ToString("dd-MMM-yyyy"));
            string getYear = dateTimetoString.Substring(9, 2);
            string getMonth = dateTimetoString.Substring(3, 3).ToUpper();
            string currrentTransMonth = getMonth + "-" + getYear;

            TempData["passData_SelectMemberList"] = memberController.MemberList();
            TempData["passData_SelectSchemeList"] = schemeController.SchemeList();
            TempData["passData_MemberLoanList"] = MemberLoanList(currrentTransMonth);
            return View();
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewMcrMLoan passModel)
        {
            if (ModelState.IsValid)
            {
                TempData["passData_MemberLoanList"] = MemberLoanList(passModel.Transmy);

                //Http Post - Create   
                String createUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberLoan/Create?userCompanycode=" + _loggedCompid +
                                          "&usercode=" + _loggedUserid;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                     new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),
                     new KeyValuePair<string, string>("Transmy",passModel.Transmy),
                     new KeyValuePair<string, string>("Transno",passModel.Transno.ToString()),

                     new KeyValuePair<string, string>("Transdt",passModel.Transdt.ToString("dd-MMM-yyyy")),
                     new KeyValuePair<string, string>("Schemeid",passModel.Schemeid),
                     new KeyValuePair<string, string>("Memberid",passModel.Memberid.ToString()),
                     new KeyValuePair<string, string>("Internid",passModel.Internid.ToString()),
                     new KeyValuePair<string, string>("Loanamt",passModel.Loanamt.ToString()),
                     new KeyValuePair<string, string>("Pchargert",passModel.Pchargert.ToString()),
                     new KeyValuePair<string, string>("Pchargeamt",passModel.Pchargeamt.ToString()),
                     new KeyValuePair<string, string>("Schargert",passModel.Schargert.ToString()),
                     new KeyValuePair<string, string>("Schargeamt",passModel.Schargeamt.ToString()),
                     new KeyValuePair<string, string>("Disburseamt",passModel.Disburseamt.ToString()),
                     new KeyValuePair<string, string>("Riskfundrt",passModel.Riskfundrt.ToString()),
                     new KeyValuePair<string, string>("Riskfundamt",passModel.Riskfundamt.ToString()),
                     new KeyValuePair<string, string>("Collectamt",passModel.Collectamt.ToString()),
                     new KeyValuePair<string, string>("Interestrt",passModel.Interestrt.ToString()),
                     new KeyValuePair<string, string>("Schemeiqty",passModel.Schemeiqty.ToString()),
                     new KeyValuePair<string, string>("Schemeefdt",passModel.Schemeefdt.ToString()),
                     new KeyValuePair<string, string>("Schemeetdt",passModel.Schemeetdt.ToString()),
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
                                    String memoNo="";
                                    var data = jobject["data"].ToString();
                                    if (data != "")
                                    {
                                        JObject jdata = JObject.Parse(data);
                                        memoNo = Convert.ToString(jdata["Transno"].ToString());
                                    }
                                    TempData["MemmberLoanCreateMessage"] = "Memo No: "+ memoNo + message;
                                    return RedirectToAction("Create");
                                }
                                else
                                {
                                    TempData["MemmberLoanCreateMessage"] = message;
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






        public ActionResult Edit(String monthYear, Int64 transNo)
        {
            ViewMcrMLoan viewModel = new ViewMcrMLoan();

            //Http Get - get Loan List
            String memberLoanInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberLoan/Information?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&transMonthYear=" + monthYear + "&transno=" + transNo;

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
                                    viewModel.Compid = (Int64)get["Compid"];
                                    viewModel.Transmy = (string)get["Transmy"];
                                    viewModel.Transno = (Int64)get["Transno"];
                                    viewModel.Transdt = (DateTime)get["Transdt"];

                                    DateTime transdt = (DateTime)get["Transdt"];
                                    string transdate= Convert.ToString(transdt.ToString("dd-MMM-yyyy"));
                                    viewModel.TransDate = transdate;

                                    viewModel.Schemeid = (string)get["Schemeid"];
                                    viewModel.Memberid = (Int64)get["Memberid"];
                                    viewModel.Internid = (Int64)get["Internid"];
                                    viewModel.Loanamt = (decimal)get["Loanamt"];
                                    viewModel.Pchargert = (decimal)get["Pchargert"];
                                    viewModel.Pchargeamt = (decimal)get["Pchargeamt"];
                                    viewModel.Schargert = (decimal)get["Schargert"];
                                    viewModel.Schargeamt = (decimal)get["Schargeamt"];
                                    viewModel.Disburseamt = (decimal) get["Disburseamt"];
                                    viewModel.Riskfundrt = (decimal)get["Riskfundrt"];
                                    viewModel.Riskfundamt = (decimal)get["Riskfundamt"];
                                    viewModel.Collectamt = (decimal)get["Collectamt"];
                                    viewModel.Interestrt = (decimal)get["Interestrt"];
                                    viewModel.Schemeiqty = (decimal)get["Schemeiqty"];

                                    DateTime fdt = (DateTime) get["Schemeefdt"];
                                    string fromDate = Convert.ToString(fdt.ToString("dd-MMM-yyyy"));
                                    viewModel.Schemeefdt = fromDate;

                                    DateTime tdt = (DateTime)get["Schemeetdt"];
                                    string toDate = Convert.ToString(tdt.ToString("dd-MMM-yyyy"));
                                    viewModel.Schemeetdt = toDate;
                                    viewModel.Remarks = (string)get["Remarks"];
                                }
                                
                                List<ViewMcrMember> memberList = (List<ViewMcrMember>)memberController.MemberList();
                                var findMemberName = (from m in memberList where m.Compid == _loggedCompid && m.Memberid == viewModel.Memberid select m).ToList();
                                foreach (var get1 in findMemberName)
                                {
                                    viewModel.MemberName = get1.Membernm;
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


            List<SelectListItem> getInternalIdList = new List<SelectListItem>();
            List<ViewMcrMScheme> memberSchemeList = (List<ViewMcrMScheme>)memberSchemeController.MemberSchemeList();

            var findInternalId = (from m in memberSchemeList where m.Compid == _loggedCompid && m.Schemeid == viewModel.Schemeid select m).ToList();
            foreach (var getModel in findInternalId)
            {
                getInternalIdList.Add(new SelectListItem { Text = getModel.Internid.ToString(), Value = Convert.ToString(getModel.Internid) });
            }
            ViewBag.InternalIdList = getInternalIdList;

            TempData["passData_SelectMemberList"] = memberController.MemberList();
            TempData["passData_SelectSchemeList"] = schemeController.SchemeList();
            return View(viewModel);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewMcrMLoan passModel)
        {
            if (ModelState.IsValid)
            {
                //Http Post - update Password 
                String updateUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberLoan/Update?userCompanycode=" + _loggedCompid +
                                          "&usercode=" + _loggedUserid;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                     new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),
                     new KeyValuePair<string, string>("Transmy",passModel.Transmy),
                     new KeyValuePair<string, string>("Transno",passModel.Transno.ToString()),

                     new KeyValuePair<string, string>("Transdt",passModel.Transdt.ToString("dd-MMM-yyyy")),
                     new KeyValuePair<string, string>("Schemeid",passModel.Schemeid),
                     new KeyValuePair<string, string>("Memberid",passModel.Memberid.ToString()),
                     new KeyValuePair<string, string>("Internid",passModel.Internid.ToString()),
                     new KeyValuePair<string, string>("Loanamt",passModel.Loanamt.ToString()),
                     new KeyValuePair<string, string>("Pchargert",passModel.Pchargert.ToString()),
                     new KeyValuePair<string, string>("Pchargeamt",passModel.Pchargeamt.ToString()),
                     new KeyValuePair<string, string>("Schargert",passModel.Schargert.ToString()),
                     new KeyValuePair<string, string>("Schargeamt",passModel.Schargeamt.ToString()),
                     new KeyValuePair<string, string>("Disburseamt",passModel.Disburseamt.ToString()),
                     new KeyValuePair<string, string>("Riskfundrt",passModel.Riskfundrt.ToString()),
                     new KeyValuePair<string, string>("Riskfundamt",passModel.Riskfundamt.ToString()),
                     new KeyValuePair<string, string>("Collectamt",passModel.Collectamt.ToString()),
                     new KeyValuePair<string, string>("Interestrt",passModel.Interestrt.ToString()),
                     new KeyValuePair<string, string>("Schemeiqty",passModel.Schemeiqty.ToString()),
                     new KeyValuePair<string, string>("Schemeefdt",passModel.Schemeefdt.ToString()),
                     new KeyValuePair<string, string>("Schemeetdt",passModel.Schemeetdt.ToString()),
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
                                    TempData["MemmberLoanMessage"] = message;
                                    return RedirectToAction("Index");
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






        public ActionResult Delete(String monthYear, Int64 transNo)
        {
            ViewMcrMLoan viewModel = new ViewMcrMLoan();

            //Http Get - get Loan List
            String memberLoanInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberLoan/List?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&transMonthYear=" + monthYear + "&transno=" + transNo;

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
                                    viewModel.Compid = (Int64)get["Compid"];
                                    viewModel.Transmy = (string)get["Transmy"];
                                    viewModel.Transno = (Int64)get["Transno"];
                                    viewModel.Transdt = (DateTime)get["Transdt"];

                                    DateTime transdt = (DateTime)get["Transdt"];
                                    string transdate = Convert.ToString(transdt.ToString("dd-MMM-yyyy"));
                                    viewModel.TransDate = transdate;

                                    viewModel.Schemeid = (string)get["Schemeid"];
                                    viewModel.Memberid = (Int64)get["Memberid"];
                                    viewModel.Internid = (Int64)get["Internid"];
                                    viewModel.Loanamt = (decimal)get["Loanamt"];
                                    viewModel.Pchargert = (decimal)get["Pchargert"];
                                    viewModel.Pchargeamt = (decimal)get["Pchargeamt"];
                                    viewModel.Schargert = (decimal)get["Schargert"];
                                    viewModel.Schargeamt = (decimal)get["Schargeamt"];
                                    viewModel.Disburseamt = (decimal)get["Disburseamt"];
                                    viewModel.Riskfundrt = (decimal)get["Riskfundrt"];
                                    viewModel.Riskfundamt = (decimal)get["Riskfundamt"];
                                    viewModel.Collectamt = (decimal)get["Collectamt"];
                                    viewModel.Interestrt = (decimal)get["Interestrt"];
                                    viewModel.Schemeiqty = (decimal)get["Schemeiqty"];

                                    DateTime fdt = (DateTime)get["Schemeefdt"];
                                    string fromDate = Convert.ToString(fdt.ToString("dd-MMM-yyyy"));
                                    viewModel.Schemeefdt = fromDate;

                                    DateTime tdt = (DateTime)get["Schemeetdt"];
                                    string toDate = Convert.ToString(tdt.ToString("dd-MMM-yyyy"));
                                    viewModel.Schemeetdt = toDate;
                                    viewModel.Remarks = (string)get["Remarks"];
                                }

                                List<ViewMcrMember> memberList = (List<ViewMcrMember>)memberController.MemberList();
                                var findMemberName = (from m in memberList where m.Compid == _loggedCompid && m.Memberid == viewModel.Memberid select m).ToList();
                                foreach (var get1 in findMemberName)
                                {
                                    viewModel.MemberName = get1.Membernm;
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
            TempData["passData_SelectSchemeList"] = schemeController.SchemeList();
            return View(viewModel);
        }







        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ViewMcrMLoan passModel)
        {
            if (ModelState.IsValid)
            {
                String companyCode = _loggedCompid.ToString();
                String userCode = _loggedUserid.ToString();

                //Http Post - update Password 
                String deleteUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberLoan/Delete?userCompanycode=" + companyCode +
                                          "&usercode=" + userCode;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {

                     new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),
                     new KeyValuePair<string, string>("Transmy",passModel.Transmy),
                     new KeyValuePair<string, string>("Transno",passModel.Transno.ToString()),

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
                                    TempData["MemmberLoanMessage"] = message;
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    ViewBag.DeleteMessage = message;
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







        //Get Member loan List
        public List<ViewMcrMLoan> MemberLoanList(String monthYear)
        {
            List<ViewMcrMLoan> memberLoanList = new List<ViewMcrMLoan>();
            //Http Get - get Member List
            String memberLoanInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberLoan/List?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid+ "&transMonthYear="+ monthYear;

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
                                    ViewMcrMLoan getModel = new ViewMcrMLoan();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transmy = (String)get["Transmy"];
                                    getModel.Transno = (Int64)get["Transno"];

                                    getModel.Transdt = (DateTime)get["Transdt"];
                                    getModel.Schemeid = (String)get["Schemeid"];
                                    getModel.Memberid = (Int64)get["Memberid"];
                                    getModel.Internid = (Int64)get["Internid"];
                                    getModel.Loanamt = (Decimal)get["Loanamt"];
                                    getModel.Pchargert = (Decimal)get["Pchargert"];
                                    getModel.Pchargeamt = (Decimal)get["Pchargeamt"];
                                    getModel.Schargert = (Decimal)get["Schargert"];
                                    getModel.Schargeamt = (Decimal)get["Schargeamt"];
                                    getModel.Disburseamt = (Decimal)get["Disburseamt"];
                                    getModel.Riskfundrt = (Decimal)get["Riskfundrt"];
                                    getModel.Riskfundamt = (Decimal)get["Riskfundamt"];
                                    getModel.Collectamt = (Decimal)get["Collectamt"];
                                    getModel.Interestrt = (Decimal)get["Interestrt"];
                                    getModel.Schemeiqty = (Decimal)get["Schemeiqty"];
                                    if (get["Schemeefdt"].ToString() != "")
                                    {
                                        DateTime fdt = (DateTime)get["Schemeefdt"];
                                        string fDate = Convert.ToString(fdt.ToString("dd-MMM-yyyy"));
                                        getModel.Schemeefdt = fDate;
                                    }
                                    if (get["Schemeetdt"].ToString() != "")
                                    {
                                        DateTime tdt = (DateTime)get["Schemeetdt"];
                                        string tDate = Convert.ToString(tdt.ToString("dd-MMM-yyyy"));
                                        getModel.Schemeetdt = tDate;
                                    }
                                    getModel.Remarks = (String)get["Remarks"];
                                    memberLoanList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return memberLoanList;
        }




        //Get Member loan List
        public List<ViewMcrMLoan> DateWiseMemberLoanList(String fromDate, String toDate)
        {
            List<ViewMcrMLoan> dateWiseMemberLoanList = new List<ViewMcrMLoan>();
            //Http Get - get Member List
            String memberLoanInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberLoan/DateWiseList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&fromDate=" + fromDate+ "&toDate=" + toDate;

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
                                    ViewMcrMLoan getModel = new ViewMcrMLoan();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transmy = (String)get["Transmy"];
                                    getModel.Transno = (Int64)get["Transno"];

                                    getModel.Transdt = (DateTime)get["Transdt"];
                                    getModel.Schemeid = (String)get["Schemeid"];
                                    getModel.Memberid = (Int64)get["Memberid"];
                                    getModel.Internid = (Int64)get["Internid"];
                                    getModel.Loanamt = (Decimal)get["Loanamt"];
                                    getModel.Pchargert = (Decimal)get["Pchargert"];
                                    getModel.Pchargeamt = (Decimal)get["Pchargeamt"];
                                    getModel.Schargert = (Decimal)get["Schargert"];
                                    getModel.Schargeamt = (Decimal)get["Schargeamt"];
                                    getModel.Disburseamt = (Decimal)get["Disburseamt"];
                                    getModel.Riskfundrt = (Decimal)get["Riskfundrt"];
                                    getModel.Riskfundamt = (Decimal)get["Riskfundamt"];
                                    getModel.Collectamt = (Decimal)get["Collectamt"];
                                    getModel.Interestrt = (Decimal)get["Interestrt"];
                                    getModel.Schemeiqty = (Decimal)get["Schemeiqty"];
                                    if (get["Schemeefdt"].ToString() != "")
                                    {
                                        DateTime fdt = (DateTime)get["Schemeefdt"];
                                        string fDate = Convert.ToString(fdt.ToString("dd-MMM-yyyy"));
                                        getModel.Schemeefdt = fDate;
                                    }
                                    if (get["Schemeetdt"].ToString() != "")
                                    {
                                        DateTime tdt = (DateTime)get["Schemeetdt"];
                                        string tDate = Convert.ToString(tdt.ToString("dd-MMM-yyyy"));
                                        getModel.Schemeetdt = tDate;
                                    }
                                    getModel.Remarks = (String)get["Remarks"];
                                    dateWiseMemberLoanList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return dateWiseMemberLoanList;
        }




        public List<ViewMcrMLoan> SchemeWiseMemberLoanList(String schemeId)
        {
            List<ViewMcrMLoan> schemeWiseMemberLoanList = new List<ViewMcrMLoan>();
            //Http Get - get Member List
            String memberLoanInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberLoan/SchemeWiseList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&schemeId=" + schemeId;

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
                                    ViewMcrMLoan getModel = new ViewMcrMLoan();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transmy = (String)get["Transmy"];
                                    getModel.Transno = (Int64)get["Transno"];

                                    getModel.Transdt = (DateTime)get["Transdt"];
                                    getModel.Schemeid = (String)get["Schemeid"];
                                    getModel.Memberid = (Int64)get["Memberid"];
                                    getModel.Internid = (Int64)get["Internid"];
                                    getModel.Loanamt = (Decimal)get["Loanamt"];
                                    getModel.Pchargert = (Decimal)get["Pchargert"];
                                    getModel.Pchargeamt = (Decimal)get["Pchargeamt"];
                                    getModel.Schargert = (Decimal)get["Schargert"];
                                    getModel.Schargeamt = (Decimal)get["Schargeamt"];
                                    getModel.Disburseamt = (Decimal)get["Disburseamt"];
                                    getModel.Riskfundrt = (Decimal)get["Riskfundrt"];
                                    getModel.Riskfundamt = (Decimal)get["Riskfundamt"];
                                    getModel.Collectamt = (Decimal)get["Collectamt"];
                                    getModel.Interestrt = (Decimal)get["Interestrt"];
                                    getModel.Schemeiqty = (Decimal)get["Schemeiqty"];
                                    if (get["Schemeefdt"].ToString() != "")
                                    {
                                        DateTime fdt = (DateTime)get["Schemeefdt"];
                                        string fDate = Convert.ToString(fdt.ToString("dd-MMM-yyyy"));
                                        getModel.Schemeefdt = fDate;
                                    }
                                    if (get["Schemeetdt"].ToString() != "")
                                    {
                                        DateTime tdt = (DateTime)get["Schemeetdt"];
                                        string tDate = Convert.ToString(tdt.ToString("dd-MMM-yyyy"));
                                        getModel.Schemeetdt = tDate;
                                    }
                                    getModel.Remarks = (String)get["Remarks"];
                                    schemeWiseMemberLoanList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return schemeWiseMemberLoanList;
        }





        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult DateChanged_getMonth(DateTime txtTransDate)//with Trans No Generation
        {
            string converttoString = Convert.ToString(txtTransDate.ToString("dd-MMM-yyyy"));
            string getYear = converttoString.Substring(9, 2);
            string getMonth = converttoString.Substring(3, 3).ToUpper();
            getMonth = getMonth + "-" + getYear;

            var result = new { month = getMonth };
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
            var findMemberId = (from m in memberSchemeList where m.Compid == _loggedCompid && m.Schemeid == txtSchemeId && m.Internid == txtInternId select m).ToList();
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
    }
}