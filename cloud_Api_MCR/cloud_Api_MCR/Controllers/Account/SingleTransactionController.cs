using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using cloud_Api_MCR.DataAccess.OTHERS;
using cloud_Api_MCR.Models.Account_DTO;
using cloud_Api_MCR.Models.ASL_DTO;
using Newtonsoft.Json.Linq;

namespace cloud_Api_MCR.Controllers.Account
{
    public class SingleTransactionController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;

        public SingleTransactionController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                _loggedCompid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
                _loggedUserid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
                _loggedToken = Convert.ToString(System.Web.HttpContext.Current.Session["loggedToken"].ToString());
                _loggedUserTp = Convert.ToString(System.Web.HttpContext.Current.Session["loggedUserType"].ToString());
                ViewData["HighLight_Menu_AccountForm"] = "highlight menu";
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "SingleTransaction" && getModel.Actionname == "Create")
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


            List<ViewGlCostPool> costPoolList = new List<ViewGlCostPool>();
            //Http Get - get Cost Pool List
            String costPoolmasterInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/CostPool/TotalList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(costPoolmasterInfoUrl).Result)
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
                                    ViewGlCostPool getModel = new ViewGlCostPool();
                                    getModel.Costpid = (Int64)get["Costpid"];
                                    getModel.Costpnm = (String)get["Costpnm"];
                                    costPoolList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            TempData["passData_SelectCostPool"] = costPoolList;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewGlStrans passModel, string command)
        {
            if (ModelState.IsValid)
            {
                //Http Post - Create Voucher  
                String createUrl = HttpClientBaseAddress.BaseAddress() + "api/SingleTransaction/Create?userCompanycode=" + _loggedCompid +
                                          "&usercode=" + _loggedUserid;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                     new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),
                     new KeyValuePair<string, string>("Transtp",passModel.Transtp),
                     new KeyValuePair<string, string>("Transmy",passModel.Transmy),
                     new KeyValuePair<string, string>("Transno",passModel.Transno.ToString()),

                     new KeyValuePair<string, string>("Transdt",passModel.Transdt.ToString("dd-MMM-yyyy")),
                     new KeyValuePair<string, string>("Transfor",passModel.Transfor),
                     new KeyValuePair<string, string>("Transmode",passModel.Transmode),
                     new KeyValuePair<string, string>("Costpid",passModel.Costpid.ToString()),
                     new KeyValuePair<string, string>("Creditcd",passModel.Creditcd.ToString()),
                     new KeyValuePair<string, string>("Debitcd",passModel.Debitcd.ToString()),
                     new KeyValuePair<string, string>("Chequeno",passModel.Chequeno),
                     new KeyValuePair<string, string>("Chequedt",passModel.Chequedt.ToString()),
                     new KeyValuePair<string, string>("Remarks",passModel.Remarks),
                     new KeyValuePair<string, string>("Amount",passModel.Amount.ToString()),
                     new KeyValuePair<string, string>("Chqpayto",passModel.Chqpayto),

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
                                    TempData["VoucherCreateMessage"] = message;
                                    if (command == "Create")
                                    {
                                        return RedirectToAction("Create");
                                    }
                                    else
                                    {
                                        TempData["VoucharReport"] = passModel;
                                        return RedirectToAction("_GetVoucharReport");
                                    }
                                    
                                }
                                else
                                {
                                    TempData["VoucherCreateMessage"] = message;
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "SingleTransaction" && getModel.Actionname == "Create")
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


            List<ViewGlCostPool> costPoolList = new List<ViewGlCostPool>();
            //Http Get - get Cost Pool List
            String costPoolmasterInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/CostPool/TotalList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(costPoolmasterInfoUrl).Result)
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
                                    ViewGlCostPool getModel = new ViewGlCostPool();
                                    getModel.Costpid = (Int64)get["Costpid"];
                                    getModel.Costpnm = (String)get["Costpnm"];
                                    costPoolList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            TempData["passData_SelectCostPool"] = costPoolList;
            return View();
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ViewGlStrans passModel, string command)
        {
            if (ModelState.IsValid)
            {
                //Http Post - update company 
                String updateUrl = HttpClientBaseAddress.BaseAddress() + "api/SingleTransaction/Update?userCompanycode=" + _loggedCompid +
                                          "&usercode=" + _loggedUserid;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),
                     new KeyValuePair<string, string>("Transtp",passModel.Transtp),
                     new KeyValuePair<string, string>("Transmy",passModel.Transmy),
                     new KeyValuePair<string, string>("Transno",passModel.Transno.ToString()),

                     new KeyValuePair<string, string>("Transdt",passModel.Transdt.ToString("dd-MMM-yyyy")),
                     new KeyValuePair<string, string>("Transfor",passModel.Transfor),
                     new KeyValuePair<string, string>("Transmode",passModel.Transmode),
                     new KeyValuePair<string, string>("Costpid",passModel.Costpid.ToString()),
                     new KeyValuePair<string, string>("Creditcd",passModel.Creditcd.ToString()),
                     new KeyValuePair<string, string>("Debitcd",passModel.Debitcd.ToString()),
                     new KeyValuePair<string, string>("Chequeno",passModel.Chequeno),
                     new KeyValuePair<string, string>("Chequedt",passModel.Chequedt.ToString()),
                     new KeyValuePair<string, string>("Remarks",passModel.Remarks),
                     new KeyValuePair<string, string>("Amount",passModel.Amount.ToString()),
                     new KeyValuePair<string, string>("Chqpayto",passModel.Chqpayto),

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
                                    TempData["VoucherUpdateMessage"] = message;
                                    if (command == "Update")
                                    {
                                        return RedirectToAction("Update");
                                    }
                                    else
                                    {
                                        TempData["VoucharReport"] = passModel;
                                        return RedirectToAction("_GetVoucharReport");
                                    }
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "SingleTransaction" && getModel.Actionname == "Create")
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


            List<ViewGlCostPool> costPoolList = new List<ViewGlCostPool>();
            //Http Get - get Cost Pool List
            String costPoolmasterInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/CostPool/TotalList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(costPoolmasterInfoUrl).Result)
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
                                    ViewGlCostPool getModel = new ViewGlCostPool();
                                    getModel.Costpid = (Int64)get["Costpid"];
                                    getModel.Costpnm = (String)get["Costpnm"];
                                    costPoolList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            TempData["passData_SelectCostPool"] = costPoolList;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ViewGlStrans passModel, string command)
        {
            try
            {
                //Http Post - update company 
                String updateUrl = HttpClientBaseAddress.BaseAddress() + "api/SingleTransaction/Delete?userCompanycode=" + _loggedCompid +
                                          "&usercode=" + _loggedUserid;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),
                     new KeyValuePair<string, string>("Transtp",passModel.Transtp),
                     new KeyValuePair<string, string>("Transmy",passModel.Transmy),
                     new KeyValuePair<string, string>("Transno",passModel.Transno.ToString()),

                     new KeyValuePair<string, string>("Transdt",passModel.Transdt.ToString("dd-MMM-yyyy")),
                     new KeyValuePair<string, string>("Transfor",passModel.Transfor),
                     new KeyValuePair<string, string>("Transmode",passModel.Transmode),
                     new KeyValuePair<string, string>("Costpid",passModel.Costpid.ToString()),
                     new KeyValuePair<string, string>("Creditcd",passModel.Creditcd.ToString()),
                     new KeyValuePair<string, string>("Debitcd",passModel.Debitcd.ToString()),
                     new KeyValuePair<string, string>("Chequeno",passModel.Chequeno),
                     new KeyValuePair<string, string>("Chequedt",passModel.Chequedt.ToString()),
                     new KeyValuePair<string, string>("Remarks",passModel.Remarks),
                     new KeyValuePair<string, string>("Amount",passModel.Amount.ToString()),
                     new KeyValuePair<string, string>("Chqpayto",passModel.Chqpayto),

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
                                    TempData["VoucherDeleteMessage"] = message;
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
            catch (Exception ex)
            {
                return View(passModel);
            }
        }






        //Get Account Single Transaction List
        public List<ViewGlStrans> AccountSingleTransactionList()
        {
            List<ViewGlStrans> list = new List<ViewGlStrans>();
            //Http Get - get List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/SingleTransaction/TotalList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;

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
                                    ViewGlStrans getModel = new ViewGlStrans();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transtp = (String)get["Transtp"];
                                    getModel.Transmy = (String)get["Transmy"];
                                    getModel.Transno = (Int64)get["Transno"];

                                    getModel.Transdt = (DateTime)get["Transdt"];
                                    getModel.Transfor = (String)get["Transfor"];
                                    getModel.Transmode = (String)get["Transmode"];
                                    getModel.Costpid = (Int64)get["Costpid"];
                                    getModel.Creditcd = (Int64)get["Creditcd"];
                                    getModel.Debitcd = (Int64)get["Debitcd"];
                                    getModel.Chequeno = (String)get["Chequeno"];
                                    if (get["Chequedt"].ToString()!="")
                                    {
                                        getModel.Chequedt = (DateTime)get["Chequedt"];
                                    }
                                    getModel.Remarks = (String)get["Remarks"];
                                    getModel.Amount = (Decimal)get["Amount"];
                                    getModel.Chqpayto = (String)get["Chqpayto"];
                                    list.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }







        //Get Account Single Transaction - Type and Date Wise List
        public List<ViewGlStrans> DateWiseList(String fromDate, String toDate)
        {
            List<ViewGlStrans> list = new List<ViewGlStrans>();
            //Http Get - get List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/SingleTransaction/DateWiseList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid+ "&fromdate=" + fromDate + "&toDate=" + toDate;

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
                                    ViewGlStrans getModel = new ViewGlStrans();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transtp = (String)get["Transtp"];
                                    getModel.Transmy = (String)get["Transmy"];
                                    getModel.Transno = (Int64)get["Transno"];

                                    getModel.Transdt = (DateTime)get["Transdt"];
                                    getModel.Transfor = (String)get["Transfor"];
                                    getModel.Transmode = (String)get["Transmode"];
                                    if (get["Costpid"].ToString() != "")
                                    {
                                        getModel.Costpid = (Int64)get["Costpid"];
                                    }
                                    if (get["Creditcd"].ToString() != "")
                                    {
                                        getModel.Creditcd = (Int64)get["Creditcd"];
                                    }
                                    if (get["Debitcd"].ToString() != "")
                                    {
                                        getModel.Debitcd = (Int64)get["Debitcd"];
                                    }
                                    getModel.Chequeno = (String)get["Chequeno"];
                                    if (get["Chequedt"].ToString() != "")
                                    {
                                        getModel.Chequedt = (DateTime)get["Chequedt"];
                                    }
                                    getModel.Remarks = (String)get["Remarks"];
                                    getModel.Amount = (Decimal)get["Amount"];
                                    getModel.Chqpayto = (String)get["Chqpayto"];
                                    list.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }








        public ActionResult _GetVoucharReport()
        {
            // Account Chart Master List
            List<ViewGlAcchartMst> accountChartMasterList = new List<ViewGlAcchartMst>();
            String accountChartMasterInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountChartMaster/TotalList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(accountChartMasterInfoUrl).Result)
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
                                    ViewGlAcchartMst getModel = new ViewGlAcchartMst();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Headcd = (Int64)get["Headcd"];
                                    getModel.Headtp = (int)get["Headtp"];
                                    getModel.Headnm = (String)get["Headnm"];
                                    accountChartMasterList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }


            // Account Chart List
            List<ViewGlAcchart> accountChartList = new List<ViewGlAcchart>();
            String accountChartInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountChart/TotalList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(accountChartInfoUrl).Result)
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
                                    ViewGlAcchart getModel = new ViewGlAcchart();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Headcd = (Int64)get["Headcd"];
                                    getModel.Headtp = (int)get["Headtp"];
                                    getModel.Accountcd = (Int64)get["Accountcd"];
                                    getModel.Accountnm = (string)get["Accountnm"];
                                    accountChartList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }

            TempData["AccountChartMasterList"] = accountChartMasterList;
            TempData["AccountChartList"] = accountChartList;
            ViewGlStrans model = (ViewGlStrans)TempData["VoucharReport"];
            
            String compNm = System.Web.HttpContext.Current.Session["loggedCompanyName"].ToString();
            String address = System.Web.HttpContext.Current.Session["loggedCompanyAddress"].ToString();
            String address2 = "";
            if (System.Web.HttpContext.Current.Session["loggedCompanyAddress2"] != null)
            {
                address2 = System.Web.HttpContext.Current.Session["loggedCompanyAddress2"].ToString();
            }

            ViewBag.CompanyName = compNm;
            ViewBag.CompanyAddress = address;
            ViewBag.CompanyAddress2 = address2;
            return View(model);
        }








        //JseonResult for DateChanged and get year.............................Start
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult DateChanged_getMonth(DateTime txtTransDate, string txtTransType)//with Trans No Generation
        {
            string converttoString = Convert.ToString(txtTransDate.ToString("dd-MMM-yyyy"));
            string getYear = converttoString.Substring(9, 2);
            string getMonth = converttoString.Substring(3, 3).ToUpper();
            getMonth = getMonth + "-" + getYear;

            String transno = "";
            //Http Get - get Find Max TransNo
            List<ViewGlAcchart> debitList = new List<ViewGlAcchart>();
            String accountChartInfoUrl1 = HttpClientBaseAddress.BaseAddress() + "api/SingleTransaction/FindMaxTransNo?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&transType=" + txtTransType + "&transDate=" + txtTransDate;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(accountChartInfoUrl1).Result)
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
        //JseonResult for DateChanged and get year.............................End





        //debitcd load
        public JsonResult Debitcdload(string type)
        {
            string company = Convert.ToString(_loggedCompid);
            string b = company + "101";
            string c = company + "102";

            Int64 matchingHead1 = Convert.ToInt64(b);
            Int64 matchingHead2 = Convert.ToInt64(c);

            //Http Get - get credit List
            List<ViewGlAcchart> debitList = new List<ViewGlAcchart>();
            String accountChartInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountChart/TotalList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(accountChartInfoUrl).Result)
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
                                    ViewGlAcchart getModel = new ViewGlAcchart();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Headcd = (Int64)get["Headcd"];
                                    getModel.Headtp = (int)get["Headtp"];
                                    getModel.Accountcd = (Int64)get["Accountcd"];
                                    getModel.Accountnm = (string)get["Accountnm"];
                                    debitList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }

            List<SelectListItem> debitcd = new List<SelectListItem>();
            if (type == "MREC")
            {
                var ans1 = (from n in debitList where n.Compid == _loggedCompid && (n.Headcd == matchingHead1 || n.Headcd == matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans1)
                {
                    debitcd.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            else if (type == "MPAY")
            {
                var ans2 = (from n in debitList where n.Compid == _loggedCompid && (n.Headcd != matchingHead1 && n.Headcd != matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans2)
                {
                    debitcd.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            else if (type == "CONT")
            {
                var ans3 = (from n in debitList where n.Compid == _loggedCompid && (n.Headcd == matchingHead1 || n.Headcd == matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans3)
                {
                    debitcd.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            else if (type == "JOUR")
            {
                var ans4 = (from n in debitList where n.Compid == _loggedCompid && (n.Headcd != matchingHead1 && n.Headcd != matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans4)
                {
                    debitcd.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            return Json(new SelectList(debitcd, "Value", "Text"));
        }





        //creditcd load
        public JsonResult Creditload(string type)
        {
            string company = Convert.ToString(_loggedCompid);
            string b = company + "101";
            string c = company + "102";

            Int64 matchingHead1 = Convert.ToInt64(b);
            Int64 matchingHead2 = Convert.ToInt64(c);


            //Http Get - get credit List
            List<ViewGlAcchart> creditList = new List<ViewGlAcchart>();
            String accountChartInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountChart/TotalList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(accountChartInfoUrl).Result)
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
                                    ViewGlAcchart getModel = new ViewGlAcchart();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Headcd = (Int64)get["Headcd"];
                                    getModel.Headtp = (int)get["Headtp"];
                                    getModel.Accountcd = (Int64)get["Accountcd"];
                                    getModel.Accountnm = (string)get["Accountnm"];
                                    creditList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }

            List<SelectListItem> creditcd = new List<SelectListItem>();
            if (type == "MREC")
            {
                var ans1 = (from n in creditList where n.Compid == _loggedCompid && (n.Headcd != matchingHead1 && n.Headcd != matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans1)
                {
                    creditcd.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            else if (type == "MPAY")
            {
                var ans2 = (from n in creditList where n.Compid == _loggedCompid && (n.Headcd == matchingHead1 || n.Headcd == matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans2)
                {
                    creditcd.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            else if (type == "CONT")
            {
                var ans2 = (from n in creditList where n.Compid == _loggedCompid && (n.Headcd == matchingHead1 || n.Headcd == matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans2)
                {
                    creditcd.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            else if (type == "JOUR")
            {
                var ans2 = (from n in creditList where n.Compid == _loggedCompid && (n.Headcd != matchingHead1 && n.Headcd != matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans2)
                {
                    creditcd.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            return Json(new SelectList(creditcd, "Value", "Text"));
        }





        public JsonResult CreditloadAfterDebitselect(string type, Int64 dvalue)
        {
            string company = Convert.ToString(_loggedCompid);
            string b = company + "101";
            string c = company + "102";

            Int64 matchingHead1 = Convert.ToInt64(b);
            Int64 matchingHead2 = Convert.ToInt64(c);

            //Http Get - get credit List
            List<ViewGlAcchart> creditList = new List<ViewGlAcchart>();
            String accountChartInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountChart/TotalList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(accountChartInfoUrl).Result)
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
                                    ViewGlAcchart getModel = new ViewGlAcchart();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Headcd = (Int64)get["Headcd"];
                                    getModel.Headtp = (int)get["Headtp"];
                                    getModel.Accountcd = (Int64)get["Accountcd"];
                                    getModel.Accountnm = (string)get["Accountnm"];
                                    creditList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }

            List<SelectListItem> creditcd = new List<SelectListItem>();
            if (type == "CONT")
            {
                var ans2 = (from n in creditList where n.Compid == _loggedCompid && (n.Headcd == matchingHead1 || n.Headcd == matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans2)
                {
                    if (x.Accountcd != dvalue)
                    {
                        creditcd.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                    }
                }
            }
            else if (type == "JOUR")
            {
                var ans2 = (from n in creditList where n.Compid == _loggedCompid && (n.Headcd != matchingHead1 && n.Headcd != matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans2)
                {
                    if (x.Accountcd != dvalue)
                    {
                        creditcd.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                    }
                }
            }
            else if (type == "MREC")
            {
                var ans1 = (from n in creditList where n.Compid == _loggedCompid && (n.Headcd != matchingHead1 && n.Headcd != matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans1)
                {
                    creditcd.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            else if (type == "MPAY")
            {
                var ans2 = (from n in creditList where n.Compid == _loggedCompid && (n.Headcd == matchingHead1 || n.Headcd == matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans2)
                {
                    creditcd.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            return Json(new SelectList(creditcd, "Value", "Text"));
        }





        public JsonResult Invoiceload(string theDate, string type)
        {
            DateTime transDate = Convert.ToDateTime(theDate);
            //Http Get - get Invoice
            List<ViewGlStrans> transNoList = new List<ViewGlStrans>();
            String getTransactionInvoiceInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/SingleTransaction/List?userCompanycode=" + _loggedCompid +
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
                                    ViewGlStrans getModel = new ViewGlStrans();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transno = (Int64)get["Transno"];
                                    transNoList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            List<SelectListItem> trans = new List<SelectListItem>();
            foreach (var f in transNoList)
            {
                trans.Add(new SelectListItem { Text = f.Transno.ToString(), Value = f.Transno.ToString() });
            }
            return Json(new SelectList(trans, "Value", "Text"));
        }








        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetTransactionInfo(string type, string transMonthYr, Int64 transno)
        {
            string transfor = "", transmode = "", chequeno = "", chequedate = "", remarks = "", debitaccount = "", creditaccount = "";
            Int64 costpoolid = 0, debitcd = 0, creditcd = 0, primaryid = 0;
            Decimal amount = 0;


            //Http Get - get Single transaction Information
            String getTransactionInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/SingleTransaction/Information?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&transType=" + type + "&transMonthYear=" + transMonthYr + "&transno=" + transno;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(getTransactionInfoUrl).Result)
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
                                    transfor = (String)get["Transfor"];
                                    transmode = (String)get["Transmode"];
                                    if (get["Costpid"].ToString() != "")
                                    {
                                        costpoolid = (Int64)get["Costpid"];
                                    }
                                    debitcd = (Int64)get["Debitcd"];
                                    creditcd = (Int64)get["Creditcd"];
                                    if (get["Chequeno"].ToString() != "")
                                    {
                                        chequeno = (String)get["Chequeno"];
                                    }
                                    //DateTime chqDate= (DateTime)get["Chequedt"];
                                    if (get["Chequedt"].ToString() != "")
                                    {
                                        DateTime chqDate = (DateTime)get["Chequedt"];
                                        string cDate = Convert.ToString(chqDate);
                                        DateTime date = DateTime.Parse(cDate);
                                        chequedate = date.ToString("dd-MMM-yyyy");
                                    }
                                    remarks = (String)get["Remarks"];
                                    amount = (Int64)get["Amount"];
                                }
                            }
                        }
                    }
                }
            }




            //Http Get - get account chart List
            List<ViewGlAcchart> accoutChartList = new List<ViewGlAcchart>();
            String accountChartInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountChart/TotalList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(accountChartInfoUrl).Result)
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
                                    ViewGlAcchart getModel = new ViewGlAcchart();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Headcd = (Int64)get["Headcd"];
                                    getModel.Headtp = (int)get["Headtp"];
                                    getModel.Accountcd = (Int64)get["Accountcd"];
                                    getModel.Accountnm = (string)get["Accountnm"];
                                    accoutChartList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }



            if (debitcd != 0)
            {
                var res = from x in accoutChartList where x.Compid == _loggedCompid && x.Accountcd == debitcd select new { x.Accountnm };
                foreach (var n in res)
                {
                    debitaccount = n.Accountnm;
                }
            }


            if (creditcd != 0)
            {
                var res2 = from x in accoutChartList where x.Compid == _loggedCompid && x.Accountcd == creditcd select new { x.Accountnm };
                foreach (var n in res2)
                {
                    creditaccount = n.Accountnm;
                }
            }


            string company = Convert.ToString(_loggedCompid);
            string b = company + "101";
            string c = company + "102";

            Int64 matchingHead1 = Convert.ToInt64(b);
            Int64 matchingHead2 = Convert.ToInt64(c);

            //For debit
            List<SelectListItem> debitlist = new List<SelectListItem>();
            if (type == "MREC")
            {
                var ans1 = (from n in accoutChartList where n.Compid == _loggedCompid && (n.Headcd == matchingHead1 || n.Headcd == matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans1)
                {
                    debitlist.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            else if (type == "MPAY")
            {
                var ans2 = (from n in accoutChartList where n.Compid == _loggedCompid && (n.Headcd != matchingHead1 && n.Headcd != matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans2)
                {
                    debitlist.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            else if (type == "CONT")
            {
                var ans3 = (from n in accoutChartList where n.Compid == _loggedCompid && (n.Headcd == matchingHead1 || n.Headcd == matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans3)
                {
                    debitlist.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            else if (type == "JOUR")
            {
                var ans4 = (from n in accoutChartList where n.Compid == _loggedCompid && (n.Headcd != matchingHead1 && n.Headcd != matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans4)
                {
                    debitlist.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }

            //For credit
            List<SelectListItem> creditlist = new List<SelectListItem>();
            if (type == "CONT")
            {
                var ans2 = (from n in accoutChartList where n.Compid == _loggedCompid && (n.Headcd == matchingHead1 || n.Headcd == matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans2)
                {
                    creditlist.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            else if (type == "JOUR")
            {
                var ans2 = (from n in accoutChartList where n.Compid == _loggedCompid && (n.Headcd != matchingHead1 && n.Headcd != matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans2)
                {
                    creditlist.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            else if (type == "MREC")
            {
                var ans1 = (from n in accoutChartList where n.Compid == _loggedCompid && (n.Headcd != matchingHead1 && n.Headcd != matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans1)
                {
                    creditlist.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }
            else if (type == "MPAY")
            {
                var ans2 = (from n in accoutChartList where n.Compid == _loggedCompid && (n.Headcd == matchingHead1 || n.Headcd == matchingHead2) select new { n.Accountcd, n.Accountnm }).OrderBy(x => x.Accountnm);
                foreach (var x in ans2)
                {
                    creditlist.Add(new SelectListItem { Text = x.Accountnm, Value = Convert.ToString(x.Accountcd) });
                }
            }

            var getresult = new
            {
                keyid = primaryid,
                For = transfor,
                date = chequedate,
                costpool = costpoolid,
                mode = transmode,
                firstdebittext = debitaccount,
                debitCD = debitcd,
                debit = debitlist,
                firstcredittext = creditaccount,
                creditCD = creditcd,
                credit = creditlist,
                cheque = chequeno,
                ChequeDate = chequedate,
                Remarks = remarks,
                Amount = amount
            };

            return Json(getresult, JsonRequestBehavior.AllowGet);
        }

    }
}