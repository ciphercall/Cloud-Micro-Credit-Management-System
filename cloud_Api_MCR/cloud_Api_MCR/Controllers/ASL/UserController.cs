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
    public class UserController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;

        public UserController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                _loggedCompid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
                _loggedUserid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
                _loggedToken = Convert.ToString(System.Web.HttpContext.Current.Session["loggedToken"].ToString());
                _loggedUserTp = Convert.ToString(System.Web.HttpContext.Current.Session["loggedUserType"].ToString());
            }
            else
            {
                RedirectToAction("Index", "Logout");
            }
        }






        //Get User List
        public List<ViewAslUserco> TotalUserListExceptCompanyAdmin(Int64 companyId)
        {
            List<ViewAslUserco> userList = new List<ViewAslUserco>();
            //Http Get - get user List
            String userInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/User/UserListExceptCompanyAdmin?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&selectCompanycode=" + companyId;

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
                                    ViewAslUserco viewAslUserco = new ViewAslUserco();
                                    viewAslUserco.Compid = (Int64)get["Compid"];
                                    viewAslUserco.Userid = (Int64)get["Userid"];
                                    viewAslUserco.Usernm = (string)get["Usernm"];
                                    viewAslUserco.Deptnm = (string)get["Deptnm"];
                                    viewAslUserco.Address = (string)get["Address"];
                                    viewAslUserco.Optp = (string)get["Optp"];
                                    viewAslUserco.Emailid = (string)get["Emailid"];
                                    viewAslUserco.Mobno = (string)get["Mobno"];
                                    viewAslUserco.Status = (string)get["Status"];
                                    viewAslUserco.Loginby = (string)get["Loginby"];
                                    viewAslUserco.Loginid = (string)get["Loginid"];
                                    viewAslUserco.Loginpw = (string)get["Loginpw"];
                                    viewAslUserco.Timefr = (string)get["Timefr"];
                                    viewAslUserco.Timeto = (string)get["Timeto"];
                                    viewAslUserco.Status = (string)get["Status"];
                                    userList.Add(viewAslUserco);
                                }
                            }
                        }
                    }
                }
            }
            return userList;
        }








        public ActionResult SearchCompanyList()
        {
            ViewData["HighLight_Menu_InformationForm"] = "High Light Menu";
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
                                    companyList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            TempData["passData_SearchCompanyList"] = companyList;
            return View();
        }






        //SearchCompanyList Table, this view table works partial
        public PartialViewResult _CompanyWiseUserInfo(Int64 cid)
        {
            List<ViewAslUserco> userList = new List<ViewAslUserco>();

            if (cid != 0)
            {
                //Http Get - get user List
                String userInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/User/UserList?userCompanycode=" + _loggedCompid +
                                     "&usercode=" + _loggedUserid + "&selectCompanyCode=" + cid;

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
                                        ViewAslUserco viewAslUserco = new ViewAslUserco();
                                        viewAslUserco.Compid = (Int64)get["Compid"];
                                        viewAslUserco.Userid = (Int64)get["Userid"];
                                        viewAslUserco.Usernm = (string)get["Usernm"];
                                        viewAslUserco.Deptnm = (string)get["Deptnm"];
                                        viewAslUserco.Address = (string)get["Address"];
                                        viewAslUserco.Optp = (string)get["Optp"];
                                        viewAslUserco.Emailid = (string)get["Emailid"];
                                        viewAslUserco.Mobno = (string)get["Mobno"];
                                        viewAslUserco.Status = (string)get["Status"];
                                        viewAslUserco.Loginby = (string)get["Loginby"];
                                        viewAslUserco.Loginid = (string)get["Loginid"];
                                        viewAslUserco.Loginpw = (string)get["Loginpw"];
                                        viewAslUserco.Timefr = (string)get["Timefr"];
                                        viewAslUserco.Timeto = (string)get["Timeto"];
                                        viewAslUserco.Status = (string)get["Status"];
                                        userList.Add(viewAslUserco);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return PartialView("~/Views/User/_CompanyWiseUserInfo.cshtml", userList);

        }






        public ActionResult SearchUserList()
        {
            ViewData["HighLight_Menu_InformationForm"] = "Heigh Light Menu";
            ViewData["HighLight_Menu_Settings"] = "High Light Menu";
            List<ViewAslUserco> userList = new List<ViewAslUserco>();

            //Http Get - get company List
            String userInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/User/UserList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&selectCompanyCode=" + _loggedCompid;

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
                                    getModel.Usernm = (string)get["Usernm"];
                                    getModel.Deptnm = (string)get["Deptnm"];
                                    getModel.Optp = (string)get["Optp"];
                                    getModel.Mobno = (string)get["Mobno"];
                                    getModel.Emailid = (string)get["Emailid"];
                                    getModel.Loginid = (string)get["Loginid"];
                                    getModel.Timefr = (string)get["Timefr"];
                                    getModel.Timeto = (string)get["Timeto"];
                                    getModel.Status = (string)get["Status"];
                                    userList.Add(getModel);
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
            return View(userList);
        }





        public ActionResult Create()
        {
            ViewData["HighLight_Menu_UserForm"] = "High Light Menu";
            //Pass TempData["UserInfo"] from Company Controller - Creation Method
            var dt = (ViewAslUserco)TempData["UserInfo"];
            if (_loggedUserTp == "CADMIN" || _loggedUserTp == "UADMIN" || _loggedUserTp == "USER")
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
                                        if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "User" && getModel.Actionname == "Create")
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
                                    RedirectToAction("Index", "Logout");
                                }
                            }
                        }
                    }
                }
            }

            return View(dt);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewAslUserco aslUserco)
        {
            if (ModelState.IsValid)
            {
                String companyCode = _loggedCompid.ToString();
                String userCode = _loggedUserid.ToString();

                //Http Post - create company
                String createCompanyInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/User/Create?userCompanycode=" + companyCode +
                                          "&usercode=" + userCode;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                     new KeyValuePair<string, string>("Compid",aslUserco.Compid.ToString()),
                     new KeyValuePair<string, string>("Usernm",aslUserco.Usernm),
                     new KeyValuePair<string, string>("Deptnm",aslUserco.Deptnm),
                     new KeyValuePair<string, string>("Optp",aslUserco.Optp),
                     new KeyValuePair<string, string>("Address",aslUserco.Address),
                     new KeyValuePair<string, string>("Emailid",aslUserco.Emailid),
                     new KeyValuePair<string, string>("Mobno",aslUserco.Mobno),
                     new KeyValuePair<string, string>("Loginby",aslUserco.Loginby),
                     new KeyValuePair<string, string>("Loginid",aslUserco.Loginid),
                     new KeyValuePair<string, string>("Loginpw",aslUserco.Loginpw),
                     new KeyValuePair<string, string>("Timefr",aslUserco.Timefr),
                     new KeyValuePair<string, string>("Timeto",aslUserco.Timeto),
                     new KeyValuePair<string, string>("Status",aslUserco.Status),
                     new KeyValuePair<string, string>("Insltude",aslUserco.Insltude),
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

                                    TempData["UserCreationMessage"] = "User Name: '" + aslUserco.Usernm + "' successfully registered! ";
                                    if (_loggedUserTp == "SADMIN")
                                    {
                                        return RedirectToAction("SearchCompanyList");
                                    }
                                    else
                                    {
                                        return RedirectToAction("Create", "User");
                                    }
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
            return View(aslUserco);
        }





        public ActionResult Update()
        {
            ViewData["HighLight_Menu_UserForm"] = "High Light Menu";

            List<ViewAslUserco> userList = new List<ViewAslUserco>();
            if (_loggedUserTp == "CADMIN" || _loggedUserTp == "UADMIN" || _loggedUserTp == "USER")
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
                                        if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "User" && getModel.Actionname == "Create")
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
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ViewAslUserco aslUserco, String command)
        {
            if (ModelState.IsValid)
            {
                //Http Post - update company 
                String updateUserInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/User/Update?userCompanycode=" + _loggedCompid +
                                          "&usercode=" + _loggedUserid;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                     new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),
                     new KeyValuePair<string, string>("Userid",aslUserco.Userid.ToString()),
                     new KeyValuePair<string, string>("Usernm",aslUserco.Usernm),
                     new KeyValuePair<string, string>("Deptnm",aslUserco.Deptnm),
                     new KeyValuePair<string, string>("Optp",aslUserco.Optp),
                     new KeyValuePair<string, string>("Address",aslUserco.Address),
                     new KeyValuePair<string, string>("Mobno",aslUserco.Mobno),
                     new KeyValuePair<string, string>("Emailid",aslUserco.Emailid),
                     new KeyValuePair<string, string>("Loginby",aslUserco.Loginby),
                     new KeyValuePair<string, string>("Loginid",aslUserco.Loginid),
                     new KeyValuePair<string, string>("Loginpw",aslUserco.Loginpw),
                     new KeyValuePair<string, string>("Timefr",aslUserco.Timefr),
                     new KeyValuePair<string, string>("Timeto",aslUserco.Timeto),
                     new KeyValuePair<string, string>("Status",aslUserco.Status),
                     new KeyValuePair<string, string>("Updltude",aslUserco.Updltude),
                };
                HttpContent query = new FormUrlEncodedContent(queries);
                using (HttpClient httpclient = new HttpClient())
                {
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                    using (HttpResponseMessage response = httpclient.PostAsync(updateUserInfoUrl, query).Result)
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
                                    TempData["UserUpdateMessage"] = message;
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
            return RedirectToAction("Update");
        }











        public JsonResult GetUserInformation(Int64 changedDropDown)
        {
            ViewAslUserco getModel = new ViewAslUserco();

            //Http Get - get User Information 
            String userInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/User/UserInformation?userCompanycode=" + _loggedCompid +
                               "&usercode=" + _loggedUserid + "&selectCompanyCode=" + _loggedCompid + "&selectUserCode=" + changedDropDown;
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
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Userid = (Int64)get["Userid"];
                                    getModel.Usernm = (String)get["Usernm"];
                                    getModel.Deptnm = (String)get["Deptnm"];
                                    getModel.Optp = (String)get["Optp"];
                                    getModel.Address = (String)get["Address"];
                                    getModel.Mobno = (String)get["Mobno"];
                                    getModel.Emailid = (String)get["Emailid"];
                                    getModel.Loginby = (String)get["Loginby"];
                                    getModel.Loginid = (String)get["Loginid"];
                                    getModel.Loginpw = (String)get["Loginpw"];
                                    getModel.Timefr = (String)get["Timefr"];
                                    getModel.Timeto = (String)get["Timeto"];
                                    getModel.Status = (String)get["Status"];
                                }
                            }
                        }
                    }
                }
            }
            var userResult = new
            {
                COMPID = getModel.Compid,
                USERNAME = getModel.Usernm,
                DEPTNM = getModel.Deptnm,
                OPTP = getModel.Optp,
                ADDRESS = getModel.Address,
                MOBNO = getModel.Mobno,
                EMAILID = getModel.Emailid,
                LOGINBY = getModel.Loginby,
                LOGINID = getModel.Loginid,
                LOGINPASSWORD = getModel.Loginpw,
                TIMERFOR = getModel.Timefr,
                TIMERTO = getModel.Timeto,
                STATUS = getModel.Status
            };
            return Json(userResult, JsonRequestBehavior.AllowGet);

        }









        public JsonResult Check_MobileNo(string mobNo)
        {
            bool checkresult = true;

            //Http Get - Check Password 
            String getCheckUrl = HttpClientBaseAddress.BaseAddress() + "api/User/Check_MobileNo?userCompanycode=" + _loggedCompid +
                                     "&usercode=" + _loggedUserid + "&mobileNo=" + mobNo;
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



        public JsonResult Check_UserEmail(string emailId)
        {
            bool checkresult = true;

            //Http Get - Check Password 
            String getCheckUrl = HttpClientBaseAddress.BaseAddress() + "api/User/Check_UserEmail?userCompanycode=" + _loggedCompid +
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




        public JsonResult Check_UserLogIn(string loginid)
        {
            bool checkresult = true;

            //Http Get - Check Password 
            String getCheckUrl = HttpClientBaseAddress.BaseAddress() + "api/User/Check_UserLogIn?userCompanycode=" + _loggedCompid +
                                     "&usercode=" + _loggedUserid + "&loginid=" + loginid;
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