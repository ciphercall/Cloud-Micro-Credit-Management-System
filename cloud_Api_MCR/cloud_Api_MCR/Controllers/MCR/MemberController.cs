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
    public class MemberController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;
        private BranchController branchController;
        private AreaController areaController;
        public MemberController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                branchController = new BranchController();
                areaController = new AreaController();
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "Member" && getModel.Actionname == "Create")
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

            TempData["passData_SelectAreaList"] = areaController.AreaList();
            TempData["passData_SelectBranchList"] = branchController.BranchList();
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewMcrMember passModel)
        {
            if (ModelState.IsValid)
            {
                //Http Post - Create   
                String createUrl = HttpClientBaseAddress.BaseAddress() + "api/Member/Create?userCompanycode=" + _loggedCompid +
                                          "&usercode=" + _loggedUserid;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                     new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),

                     new KeyValuePair<string, string>("Membernm",passModel.Membernm),
                     new KeyValuePair<string, string>("Guardiannm",passModel.Guardiannm),
                     new KeyValuePair<string, string>("Mothernm",passModel.Mothernm),
                     new KeyValuePair<string, string>("Addrpre",passModel.Addrpre),
                     new KeyValuePair<string, string>("Addrper",passModel.Addrper),
                     new KeyValuePair<string, string>("Mobno1",passModel.Mobno1),
                     new KeyValuePair<string, string>("Mobno2",passModel.Mobno2),
                     new KeyValuePair<string, string>("Dob",passModel.Dob.ToString()),
                     new KeyValuePair<string, string>("Age",passModel.Age),
                     new KeyValuePair<string, string>("Nationality",passModel.Nationality),
                     new KeyValuePair<string, string>("Nid",passModel.Nid),
                     new KeyValuePair<string, string>("Religion",passModel.Religion),
                     new KeyValuePair<string, string>("Occupation",passModel.Occupation),
                     new KeyValuePair<string, string>("Refnm",passModel.Refnm),
                     new KeyValuePair<string, string>("Refmid",passModel.Refmid.ToString()),
                     new KeyValuePair<string, string>("Refcno",passModel.Refcno),
                     new KeyValuePair<string, string>("Sharecno",passModel.Sharecno),
                     new KeyValuePair<string, string>("Sharecdt",passModel.Sharecdt.ToString()),
                     new KeyValuePair<string, string>("Areaid",passModel.Areaid.ToString()),
                     new KeyValuePair<string, string>("Branchid",passModel.Branchid.ToString()),
                     new KeyValuePair<string, string>("Acopdt",passModel.Acopdt.ToString()),
                     new KeyValuePair<string, string>("Accldt",passModel.Accldt.ToString()),
                     new KeyValuePair<string, string>("Status",passModel.Status),

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
                                    TempData["MemmberCreateMessage"] = message;
                                    return RedirectToAction("Create");
                                }
                                else
                                {
                                    TempData["BranchCreateMessage"] = message;
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

            TempData["passData_SelectMemberList"] = MemberList();
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "Member" && getModel.Actionname == "Create")
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

            TempData["passData_SelectMemberList"] = MemberList();
            TempData["passData_SelectAreaList"] = areaController.AreaList();
            TempData["passData_SelectBranchList"] = branchController.BranchList();
            return View();
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ViewMcrMember passModel)
        {
            if (ModelState.IsValid)
            {
                passModel.Macgid = Convert.ToInt64(_loggedCompid + "1030000");

                //Http Post - update  
                String updateUrl = HttpClientBaseAddress.BaseAddress() + "api/Member/Update?userCompanycode=" + _loggedCompid +
                                          "&usercode=" + _loggedUserid;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),
                    new KeyValuePair<string, string>("Macgid",passModel.Macgid.ToString()),
                    new KeyValuePair<string, string>("Memberid",passModel.Memberid.ToString()),

                    new KeyValuePair<string, string>("Membernm",passModel.Membernm),
                     new KeyValuePair<string, string>("Guardiannm",passModel.Guardiannm),
                     new KeyValuePair<string, string>("Mothernm",passModel.Mothernm),
                     new KeyValuePair<string, string>("Addrpre",passModel.Addrpre),
                     new KeyValuePair<string, string>("Addrper",passModel.Addrper),
                     new KeyValuePair<string, string>("Mobno1",passModel.Mobno1),
                     new KeyValuePair<string, string>("Mobno2",passModel.Mobno2),
                     new KeyValuePair<string, string>("Dob",passModel.Dob.ToString()),
                     new KeyValuePair<string, string>("Age",passModel.Age),
                     new KeyValuePair<string, string>("Nationality",passModel.Nationality),
                     new KeyValuePair<string, string>("Nid",passModel.Nid),
                     new KeyValuePair<string, string>("Religion",passModel.Religion),
                     new KeyValuePair<string, string>("Occupation",passModel.Occupation),
                     new KeyValuePair<string, string>("Refnm",passModel.Refnm),
                     new KeyValuePair<string, string>("Refmid",passModel.Refmid.ToString()),
                     new KeyValuePair<string, string>("Refcno",passModel.Refcno),
                     new KeyValuePair<string, string>("Sharecno",passModel.Sharecno),
                     new KeyValuePair<string, string>("Sharecdt",passModel.Sharecdt.ToString()),
                     new KeyValuePair<string, string>("Areaid",passModel.Areaid.ToString()),
                     new KeyValuePair<string, string>("Branchid",passModel.Branchid.ToString()),
                     new KeyValuePair<string, string>("Acopdt",passModel.Acopdt.ToString()),
                     new KeyValuePair<string, string>("Accldt",passModel.Accldt.ToString()),
                     new KeyValuePair<string, string>("Status",passModel.Status),

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
                                    TempData["MemberUpdateMessage"] = message;
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "Member" && getModel.Actionname == "Create")
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

            TempData["passData_SelectMemberList"] = MemberList();
            TempData["passData_SelectAreaList"] = areaController.AreaList();
            TempData["passData_SelectBranchList"] = branchController.BranchList();
            return View();
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ViewMcrMember passModel)
        {
            try
            {
                passModel.Macgid = Convert.ToInt64(_loggedCompid + "1030000");


                //Http Post - Delete 
                String deleteUrl = HttpClientBaseAddress.BaseAddress() + "api/Member/Delete?userCompanycode=" + _loggedCompid +
                                          "&usercode=" + _loggedUserid;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                   new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),
                    new KeyValuePair<string, string>("Macgid",passModel.Macgid.ToString()),
                     new KeyValuePair<string, string>("Memberid",passModel.Memberid.ToString()),

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
                                    TempData["MemberDeleteMessage"] = message;
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






        //Get Member List
        public List<ViewMcrMember> MemberList()
        {
            List<ViewMcrMember> memberList = new List<ViewMcrMember>();
            //Http Get - get Member List
            String memberInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/Member/List?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(memberInfoUrl).Result)
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
                                    ViewMcrMember getModel = new ViewMcrMember();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Macgid = (Int64)get["Macgid"];
                                    getModel.Memberid = (Int64)get["Memberid"];

                                    getModel.Membernm = (String)get["Membernm"];
                                    getModel.Guardiannm = (String)get["Guardiannm"];
                                    getModel.Mothernm = (String)get["Mothernm"];
                                    getModel.Addrpre = (String)get["Addrpre"];
                                    getModel.Addrper = (String)get["Addrper"];
                                    getModel.Mobno1 = (String)get["Mobno1"];
                                    getModel.Mobno2 = (String)get["Mobno2"];
                                    if (get["Dob"].ToString() != "")
                                    {
                                        getModel.Dob = (DateTime)get["Dob"];
                                    }
                                    getModel.Age = (String)get["Age"];
                                    getModel.Nationality = (String)get["Nationality"];
                                    getModel.Nid = (String)get["Nid"];
                                    getModel.Religion = (String)get["Religion"];
                                    getModel.Occupation = (String)get["Occupation"];
                                    getModel.Refnm = (String)get["Refnm"];
                                    if (get["Refmid"].ToString() != "")
                                    {
                                        getModel.Refmid = (Int64)get["Refmid"];
                                    }
                                    getModel.Refcno = (String)get["Refcno"];
                                    getModel.Sharecno = (String)get["Sharecno"];
                                    if (get["Sharecdt"].ToString() != "")
                                    {
                                        getModel.Sharecdt = (DateTime)get["Sharecdt"];
                                    }
                                    if (get["Areaid"].ToString() != "")
                                    {
                                        getModel.Areaid = (Int64)get["Areaid"];
                                    }
                                    if (get["Branchid"].ToString() != "")
                                    {
                                        getModel.Branchid = (Int64)get["Branchid"];
                                    }
                                    if (get["Acopdt"].ToString() != "")
                                    {
                                        getModel.Acopdt = (DateTime)get["Acopdt"];
                                    }
                                    if (get["Accldt"].ToString() != "")
                                    {
                                        getModel.Accldt = (DateTime)get["Accldt"];
                                    }
                                    getModel.Status = (String)get["Status"];
                                    memberList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return memberList;
        }






        public JsonResult GetMemberInformation(Int64 changedDropDown)
        {
            ViewMcrMember getModel = new ViewMcrMember();
            Int64 macgid = Convert.ToInt64(_loggedCompid + "1030000");
            String dob = "", sharecdt = "", acopdt = "", accldt = "";

            //Http Get - get User Information 
            String userInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/Member/Information?userCompanycode=" + _loggedCompid +
                               "&usercode=" + _loggedUserid + "&macgId=" + macgid + "&memberId=" + changedDropDown;
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
                                    getModel.Macgid = (Int64)get["Macgid"];
                                    getModel.Memberid = (Int64)get["Memberid"];
                                    getModel.Membernm = (String)get["Membernm"];
                                    getModel.Guardiannm = (String)get["Guardiannm"];
                                    getModel.Mothernm = (String)get["Mothernm"];
                                    getModel.Addrpre = (String)get["Addrpre"];
                                    getModel.Addrper = (String)get["Addrper"];
                                    getModel.Mobno1 = (String)get["Mobno1"];
                                    getModel.Mobno2 = (String)get["Mobno2"];
                                    if (get["Dob"].ToString() != "")
                                    {
                                        dob = (String)get["Dob"];
                                        DateTime date = Convert.ToDateTime(dob);
                                        dob = date.ToString("dd-MMM-yyyy");
                                    }
                                    getModel.Age = (String)get["Age"];
                                    getModel.Nationality = (String)get["Nationality"];
                                    getModel.Nid = (String)get["Nid"];
                                    getModel.Religion = (String)get["Religion"];
                                    getModel.Occupation = (String)get["Occupation"];
                                    getModel.Refnm = (String)get["Refnm"];
                                    if (get["Refmid"].ToString() != "")
                                    {
                                        getModel.Refmid = (Int64)get["Refmid"];
                                    }
                                    getModel.Refcno = (String)get["Refcno"];
                                    getModel.Sharecno = (String)get["Sharecno"];
                                    if (get["Sharecdt"].ToString() != "")
                                    {
                                        sharecdt = (String)get["Sharecdt"];
                                        DateTime date = Convert.ToDateTime(sharecdt);
                                        sharecdt = date.ToString("dd-MMM-yyyy");
                                    }
                                    if (get["Areaid"].ToString() != "")
                                    {
                                        getModel.Areaid = (Int64)get["Areaid"];
                                    }
                                    if (get["Branchid"].ToString() != "")
                                    {
                                        getModel.Branchid = (Int64)get["Branchid"];
                                    }
                                    if (get["Acopdt"].ToString() != "")
                                    {
                                        acopdt = (String)get["Acopdt"];
                                        DateTime date = Convert.ToDateTime(acopdt);
                                        acopdt = date.ToString("dd-MMM-yyyy");
                                    }
                                    if (get["Accldt"].ToString() != "")
                                    {
                                        accldt = (String)get["Accldt"];
                                        DateTime date = Convert.ToDateTime(accldt);
                                        accldt = date.ToString("dd-MMM-yyyy");
                                    }
                                    getModel.Status = (String)get["Status"];
                                }
                            }
                        }
                    }
                }
            }
            var userResult = new
            {
                memberName = getModel.Membernm,
                guardiannm = getModel.Guardiannm,
                mothernm = getModel.Mothernm,
                addrpresent = getModel.Addrpre,
                addrpermanent = getModel.Addrper,
                mobno1 = getModel.Mobno1,
                mobno2 = getModel.Mobno2,
                dob1 = dob,
                age = getModel.Age,
                nationality = getModel.Nationality,
                nid = getModel.Nid,
                religion = getModel.Religion,
                occupation = getModel.Occupation,
                refname = getModel.Refnm,
                refmid = getModel.Refmid,
                refcno = getModel.Refcno,
                sharecno = getModel.Sharecno,
                sharecdt1 = sharecdt,
                areaid = getModel.Areaid,
                branchid = getModel.Branchid,
                acopdt1 = acopdt,
                accldt1 = accldt,
                status = getModel.Status,

            };
            return Json(userResult, JsonRequestBehavior.AllowGet);
        }

    }
}