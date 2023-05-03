using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using cloud_Api_MCR.DataAccess;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.DataAccess.OTHERS;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.ASL;
using cloud_Api_MCR.Models.ASL_DTO;
using Newtonsoft.Json.Linq;

namespace cloud_Api_MCR.Controllers.ASL
{
    public class CompanyController : Controller
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken;

        public CompanyController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                _loggedCompid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
                _loggedUserid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
                _loggedToken = Convert.ToString(System.Web.HttpContext.Current.Session["loggedToken"].ToString());
            }
            else
            {
                RedirectToAction("Index", "Logout");
            }
        }







        //Get Company List
        public List<ViewAslCompany> CompanyList()
        {
            List<ViewAslCompany> companyList = new List<ViewAslCompany>();
            //Http Get - get company List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/Company/CompanyList?userCompanycode=" + _loggedCompid +
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
                                    ViewAslCompany getModel = new ViewAslCompany();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Compnm = (string)get["Compnm"];
                                    getModel.Address = (string)get["Address"];
                                    getModel.Address2 = (string)get["Address2"];
                                    getModel.Contactno = (string)get["Contactno"];
                                    getModel.Emailid = (string)get["Emailid"];
                                    getModel.Webid = (string)get["Webid"];
                                    getModel.Status = (string)get["Status"];

                                    getModel.Emailidp = (string)get["Emailidp"];
                                    getModel.Emailpwp = (string)get["Emailpwp"];
                                    getModel.Smssendernm = (string)get["Smssendernm"];
                                    getModel.Smsidp = (string)get["Smsidp"];
                                    getModel.Smspwp = (string)get["Smspwp"];
                                    companyList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return companyList;
        }





        //Get Company List
        public List<ViewAslCompany> CompanyInfo(Int64 companyId)
        {
            List<ViewAslCompany> companyList = new List<ViewAslCompany>();
            //Http Get - get company List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/Company/CompanyInformation?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&selectCompanyCode=" + companyId;

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
                                    ViewAslCompany getModel = new ViewAslCompany();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Compnm = (string)get["Compnm"];
                                    getModel.Address = (string)get["Address"];
                                    getModel.Address2 = (string)get["Address2"];
                                    getModel.Contactno = (string)get["Contactno"];
                                    getModel.Emailid = (string)get["Emailid"];
                                    getModel.Webid = (string)get["Webid"];
                                    getModel.Status = (string)get["Status"];

                                    getModel.Emailidp = (string)get["Emailidp"];
                                    getModel.Emailpwp = (string)get["Emailpwp"];
                                    getModel.Smssendernm = (string)get["Smssendernm"];
                                    getModel.Smsidp = (string)get["Smsidp"];
                                    getModel.Smspwp = (string)get["Smspwp"];
                                    companyList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return companyList;
        }





        public ActionResult Index()
        {
            ViewData["HighLight_Menu_InformationForm"] = "Heigh Light Menu";
            List<ViewAslCompany> companyList = new List<ViewAslCompany>();

            //Http Get - get company List
            String companyInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/Company/CompanyList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(companyInfoUrl).Result)
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
                                    ViewAslCompany getModel = new ViewAslCompany();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Compnm = (string)get["Compnm"];
                                    getModel.Address = (string)get["Address"];
                                    getModel.Address2 = (string)get["Address2"];
                                    getModel.Contactno = (string)get["Contactno"];
                                    getModel.Emailid = (string)get["Emailid"];
                                    getModel.Webid = (string)get["Webid"];
                                    getModel.Status = (string)get["Status"];
                                    companyList.Add(getModel);
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
            return View(companyList);
        }








        public ActionResult Create()
        {
            ViewData["HighLight_Menu_InputForm"] = "Heigh Light Menu";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewAslCompany aslcompany)
        {
            if (ModelState.IsValid)
            {
                String companyCode = _loggedCompid.ToString();
                String userCode = _loggedUserid.ToString();

                //Http Post - create company
                String createCompanyInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/Company/Create?userCompanycode=" + companyCode +
                                          "&usercode=" + userCode;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                     new KeyValuePair<string, string>("Compnm",aslcompany.Compnm),
                     new KeyValuePair<string, string>("Address",aslcompany.Address),
                     new KeyValuePair<string, string>("Address2",aslcompany.Address2),
                     new KeyValuePair<string, string>("Contactno",aslcompany.Contactno),
                     new KeyValuePair<string, string>("Emailid",aslcompany.Emailid),
                     new KeyValuePair<string, string>("Webid",aslcompany.Webid),
                     new KeyValuePair<string, string>("Status",aslcompany.Status),
                     new KeyValuePair<string, string>("Emailidp",aslcompany.Emailidp),
                     new KeyValuePair<string, string>("Emailpwp",aslcompany.Emailpwp),
                     new KeyValuePair<string, string>("Smssendernm",aslcompany.Smssendernm),
                     new KeyValuePair<string, string>("Smsidp",aslcompany.Smsidp),
                     new KeyValuePair<string, string>("Smspwp",aslcompany.Smspwp),
                     new KeyValuePair<string, string>("Insltude",aslcompany.Insltude),
                };
                HttpContent query = new FormUrlEncodedContent(queries);
                using (HttpClient httpclient = new HttpClient())
                {
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                    using (HttpResponseMessage response = httpclient.PostAsync(createCompanyInfoUrl, query).Result)
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
                                    ViewBag.CreateMessage = message;

                                    ViewAslUserco users = new ViewAslUserco();

                                    var data = jobject["data"].ToString();
                                    if (data != "")
                                    {
                                        JObject jdata = JObject.Parse(data);
                                        users.Compid = Convert.ToInt64(jdata["Compid"].ToString());
                                    }
                                    users.Compnm = aslcompany.Compnm;
                                    users.Address = aslcompany.Address;
                                    users.Mobno = aslcompany.Contactno;
                                    users.Emailid = aslcompany.Emailid;
                                    users.Loginpw = aslcompany.Compnm.Substring(0, 3) + "asl" + "123%";

                                    TempData["UserInfo"] = users;
                                    return RedirectToAction("Create", "User");
                                }
                                else
                                {
                                    ViewBag.CreateMessage = message;
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
            return View(aslcompany);
        }






        public ActionResult Edit(short id = 0)
        {
            ViewData["HighLight_Menu_InformationForm"] = "Heigh Light Menu";
            ViewAslCompany viewAslCompany = new ViewAslCompany();

            //Http Get - get company List
            String companyInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/Company/CompanyInformation?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&selectCompanyCode=" + id;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(companyInfoUrl).Result)
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
                                    viewAslCompany.Compid = (Int64)get["Compid"];
                                    viewAslCompany.Compnm = (string)get["Compnm"];
                                    viewAslCompany.Address = (string)get["Address"];
                                    viewAslCompany.Address2 = (string)get["Address2"];
                                    viewAslCompany.Contactno = (string)get["Contactno"];
                                    viewAslCompany.Emailid = (string)get["Emailid"];
                                    viewAslCompany.Webid = (string)get["Webid"];
                                    viewAslCompany.Status = (string)get["Status"];
                                    viewAslCompany.Emailidp = (string)get["Emailidp"];
                                    viewAslCompany.Emailpwp = (string)get["Emailpwp"];
                                    viewAslCompany.Smssendernm = (string)get["Smssendernm"];
                                    viewAslCompany.Smsidp = (string)get["Smsidp"];
                                    viewAslCompany.Smspwp = (string)get["Smspwp"];
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
            return View(viewAslCompany);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewAslCompany aslcompany)
        {
            if (ModelState.IsValid)
            {
                String companyCode = _loggedCompid.ToString();
                String userCode = _loggedUserid.ToString();

                //Http Post - update Password 
                String updateCompanyInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/Company/Update?userCompanycode=" + companyCode +
                                          "&usercode=" + userCode;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                     new KeyValuePair<string, string>("Compid",aslcompany.Compid.ToString()),
                     new KeyValuePair<string, string>("Compnm",aslcompany.Compnm),
                     new KeyValuePair<string, string>("Address",aslcompany.Address),
                     new KeyValuePair<string, string>("Address2",aslcompany.Address2),
                     new KeyValuePair<string, string>("Contactno",aslcompany.Contactno),
                     new KeyValuePair<string, string>("Emailid",aslcompany.Emailid),
                     new KeyValuePair<string, string>("Webid",aslcompany.Webid),
                     new KeyValuePair<string, string>("Status",aslcompany.Status),
                     new KeyValuePair<string, string>("Emailidp",aslcompany.Emailidp),
                     new KeyValuePair<string, string>("Emailpwp",aslcompany.Emailpwp),
                     new KeyValuePair<string, string>("Smssendernm",aslcompany.Smssendernm),
                     new KeyValuePair<string, string>("Smsidp",aslcompany.Smsidp),
                     new KeyValuePair<string, string>("Smspwp",aslcompany.Smspwp),
                };
                HttpContent query = new FormUrlEncodedContent(queries);
                using (HttpClient httpclient = new HttpClient())
                {
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                    using (HttpResponseMessage response = httpclient.PostAsync(updateCompanyInfoUrl, query).Result)
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
                                    ViewBag.EditMessage = message;
                                    return View();
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
            ViewData["HighLight_Menu_InformationForm"] = "Heigh Light Menu";
            return View(aslcompany);
        }




        

        public ActionResult Details(short id = 0)
        {
            ViewData["HighLight_Menu_InformationForm"] = "Heigh Light Menu";
            ViewAslCompany viewAslCompany = new ViewAslCompany();

            //Http Get - get company List
            String companyInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/Company/CompanyInformation?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&selectCompanyCode=" + id;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(companyInfoUrl).Result)
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
                                    viewAslCompany.Compid = (Int64)get["Compid"];
                                    viewAslCompany.Compnm = (string)get["Compnm"];
                                    viewAslCompany.Address = (string)get["Address"];
                                    viewAslCompany.Address2 = (string)get["Address2"];
                                    viewAslCompany.Contactno = (string)get["Contactno"];
                                    viewAslCompany.Emailid = (string)get["Emailid"];
                                    viewAslCompany.Webid = (string)get["Webid"];
                                    viewAslCompany.Status = (string)get["Status"];
                                    viewAslCompany.Emailidp = (string)get["Emailidp"];
                                    viewAslCompany.Emailpwp = (string)get["Emailpwp"];
                                    viewAslCompany.Smssendernm = (string)get["Smssendernm"];
                                    viewAslCompany.Smsidp = (string)get["Smsidp"];
                                    viewAslCompany.Smspwp = (string)get["Smspwp"];
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
            return View(viewAslCompany);
        }





        public JsonResult Check_Compnm(string compnm)
        {
            bool checkresult = true;

            //Http Get - Check Password 
            String getCheckUrl = HttpClientBaseAddress.BaseAddress() + "api/Company/Check_CompanyName?userCompanycode=" + _loggedCompid +
                                     "&usercode=" + _loggedUserid + "&companyName=" + compnm;
            using (HttpClient httpclient = new HttpClient())
            {
                //httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(getCheckUrl).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            bool success;
                            string message = "", data = "";

                            var result = content.ReadAsStringAsync().Result;
                            JObject jobject = JObject.Parse(result);
                            var successMessage = jobject["success"].ToString();
                            message = jobject["message"].ToString();
                            data = jobject["data"].ToString();
                            success = successMessage.Equals("True") ? true : false;
                            if (success == true)
                            {
                                checkresult = false;
                            }
                        }
                    }
                    else
                    {
                    }
                }
            }
            return Json(checkresult, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Check_ContactNo(string contactNo)
        {
            bool checkresult = true;

            //Http Get - Check Password 
            String getCheckUrl = HttpClientBaseAddress.BaseAddress() + "api/Company/Check_ContactNo?userCompanycode=" + _loggedCompid +
                                     "&usercode=" + _loggedUserid + "&contactNo=" + contactNo;
            using (HttpClient httpclient = new HttpClient())
            {
                //httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(getCheckUrl).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            bool success;
                            string message = "", data = "";

                            var result = content.ReadAsStringAsync().Result;
                            JObject jobject = JObject.Parse(result);
                            var successMessage = jobject["success"].ToString();
                            message = jobject["message"].ToString();
                            data = jobject["data"].ToString();
                            success = successMessage.Equals("True") ? true : false;
                            if (success == true)
                            {
                                checkresult = false;
                            }
                        }
                    }
                    else
                    {
                    }
                }
            }
            return Json(checkresult, JsonRequestBehavior.AllowGet);
        }




        public JsonResult Check_EmailId(string emailId)
        {
            bool checkresult = true;

            //Http Get - Check Password 
            String getCheckUrl = HttpClientBaseAddress.BaseAddress() + "api/Company/Check_EmailId?userCompanycode=" + _loggedCompid +
                                     "&usercode=" + _loggedUserid + "&emailId=" + emailId;
            using (HttpClient httpclient = new HttpClient())
            {
                //httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(getCheckUrl).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            bool success;
                            string message = "", data = "";

                            var result = content.ReadAsStringAsync().Result;
                            JObject jobject = JObject.Parse(result);
                            var successMessage = jobject["success"].ToString();
                            message = jobject["message"].ToString();
                            data = jobject["data"].ToString();
                            success = successMessage.Equals("True") ? true : false;
                            if (success == true)
                            {
                                checkresult = false;
                            }
                        }
                    }
                    else
                    {
                    }
                }
            }
            return Json(checkresult, JsonRequestBehavior.AllowGet);
        }






        public JsonResult Check_EmailId_Promotional(string emailIdP)
        {
            bool checkresult = true;

            //Http Get - Check Password 
            String getCheckUrl = HttpClientBaseAddress.BaseAddress() + "api/Company/Check_Promotional_EmailId?userCompanycode=" + _loggedCompid +
                                     "&usercode=" + _loggedUserid + "&promotionalEmailId=" + emailIdP;
            using (HttpClient httpclient = new HttpClient())
            {
                //httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(getCheckUrl).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            bool success;
                            string message = "", data = "";

                            var result = content.ReadAsStringAsync().Result;
                            JObject jobject = JObject.Parse(result);
                            var successMessage = jobject["success"].ToString();
                            message = jobject["message"].ToString();
                            data = jobject["data"].ToString();
                            success = successMessage.Equals("True") ? true : false;
                            if (success == true)
                            {
                                checkresult = false;
                            }
                        }
                    }
                    else
                    {
                    }
                }
            }
            return Json(checkresult, JsonRequestBehavior.AllowGet);
        }

    }
}