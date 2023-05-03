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
    public class MemberNomineeController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;
        private MemberController memberController;
        public MemberNomineeController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                memberController = new MemberController();
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "MemberNominee" && getModel.Actionname == "Create")
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
        public ActionResult Create(ViewMcrMNominee passModel)
        {
            if (ModelState.IsValid)
            {
                //Http Post - Create   
                String createUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberNominee/Create?userCompanycode=" + _loggedCompid +
                                          "&usercode=" + _loggedUserid;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                     new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),
                     new KeyValuePair<string, string>("Memberid",passModel.Memberid.ToString()),

                     new KeyValuePair<string, string>("Nomineenm",passModel.Nomineenm),
                     new KeyValuePair<string, string>("Guardiannm",passModel.Guardiannm),
                     new KeyValuePair<string, string>("Mothernm",passModel.Mothernm),
                     new KeyValuePair<string, string>("Age",passModel.Age),
                     new KeyValuePair<string, string>("Relation",passModel.Relation),
                     new KeyValuePair<string, string>("Npcnt",passModel.Npcnt.ToString()),
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
                                    TempData["MemmberNomineeCreateMessage"] = message;
                                    return RedirectToAction("Create");
                                }
                                else
                                {
                                    TempData["MemmberNomineeCreateMessage"] = message;
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "MemberNominee" && getModel.Actionname == "Create")
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
        public ActionResult Update(ViewMcrMNominee passModel)
        {
            if (ModelState.IsValid)
            {
                //Http Post - update 
                String updateUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberNominee/Update?userCompanycode=" + _loggedCompid +
                                          "&usercode=" + _loggedUserid;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),
                    new KeyValuePair<string, string>("Memberid",passModel.Memberid.ToString()),
                    new KeyValuePair<string, string>("Nomineeid",passModel.Nomineeid.ToString()),

                    new KeyValuePair<string, string>("Nomineenm",passModel.Nomineenm),
                    new KeyValuePair<string, string>("Guardiannm",passModel.Guardiannm),
                    new KeyValuePair<string, string>("Mothernm",passModel.Mothernm),
                    new KeyValuePair<string, string>("Age",passModel.Age),
                    new KeyValuePair<string, string>("Relation",passModel.Relation),
                    new KeyValuePair<string, string>("Npcnt",passModel.Npcnt.ToString()),
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
                                    TempData["MemberNomineeUpdateMessage"] = message;
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "MemberNominee" && getModel.Actionname == "Create")
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
        public ActionResult Delete(ViewMcrMNominee passModel)
        {
            try
            {
                //Http Post - Delete company 
                String deleteUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberNominee/Delete?userCompanycode=" + _loggedCompid +
                                          "&usercode=" + _loggedUserid;
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                   new KeyValuePair<string, string>("Compid",_loggedCompid.ToString()),
                   new KeyValuePair<string, string>("Memberid",passModel.Memberid.ToString()),
                   new KeyValuePair<string, string>("Nomineeid",passModel.Nomineeid.ToString()),

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
                                    TempData["MemberNomineeDeleteMessage"] = message;
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





        //Get Member-Nominee List
        public List<ViewMcrMNominee> MemberNomineeList()
        {
            List<ViewMcrMNominee> memberNomineeList = new List<ViewMcrMNominee>();
            //Http Get - get Member List
            String memberNomineeInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberNominee/TotalList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(memberNomineeInfoUrl).Result)
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
                                    ViewMcrMNominee getModel = new ViewMcrMNominee();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Memberid = (Int64)get["Memberid"];
                                    getModel.Nomineeid = (Int64)get["Nomineeid"];
                                    getModel.Nomineenm = (String)get["Nomineenm"];
                                    getModel.Guardiannm = (String)get["Guardiannm"];
                                    getModel.Mothernm = (String)get["Mothernm"];
                                    getModel.Age = (String)get["Age"];
                                    getModel.Relation = (String)get["Relation"];
                                    if (get["Npcnt"].ToString() != "")
                                    {
                                        getModel.Npcnt = (Decimal)get["Npcnt"];
                                    }
                                    getModel.Remarks = (String)get["Remarks"];
                                    memberNomineeList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return memberNomineeList;
        }





        public JsonResult NomineeIdLoadAfterMemberSelect(Int64 memberId)
        {
            List<SelectListItem> getMemberList = new List<SelectListItem>();

            //Http Get - get Member wise Nominee List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberNominee/List?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid+ "&memberId=" + memberId;

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
                                    ViewMcrMNominee getModel = new ViewMcrMNominee();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Memberid = (Int64)get["Memberid"];
                                    getModel.Nomineeid = (Int64)get["Nomineeid"];
                                    getModel.Nomineenm = (string)get["Nomineenm"];
                                    getMemberList.Add(new SelectListItem { Text = getModel.Nomineenm, Value = Convert.ToString(getModel.Nomineeid) });
                                }
                            }
                        }
                    }
                }
            }
            
            return Json(new SelectList(getMemberList, "Value", "Text"));
        }






        public JsonResult GetMemberNomineeInformation(Int64 memberid,Int64 changedDropDown)
        {
            ViewMcrMNominee getModel = new ViewMcrMNominee();

            //Http Get - get User Information 
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberNominee/Information?userCompanycode=" + _loggedCompid +
                               "&usercode=" + _loggedUserid + "&memberId=" + memberid + "&nomineeId=" + changedDropDown;
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
                                    getModel.Memberid = (Int64)get["Memberid"];
                                    getModel.Nomineeid = (Int64)get["Nomineeid"];
                                    getModel.Nomineenm = (String)get["Nomineenm"];
                                    getModel.Guardiannm = (String)get["Guardiannm"];
                                    getModel.Mothernm = (String)get["Mothernm"];
                                    getModel.Age = (String)get["Age"];
                                    getModel.Relation = (String)get["Relation"];
                                    if (get["Npcnt"].ToString() != "")
                                    {
                                        getModel.Npcnt = (Decimal)get["Npcnt"];
                                    }
                                    getModel.Remarks = (String)get["Remarks"];
                                }
                            }
                        }
                    }
                }
            }
            var userResult = new
            {
                nomineeName = getModel.Nomineenm,
                guardiannm = getModel.Guardiannm,
                mothernm = getModel.Mothernm,
                age = getModel.Age,
                relation = getModel.Relation,
                npcent = getModel.Npcnt,
                remarks = getModel.Remarks,
            };
            return Json(userResult, JsonRequestBehavior.AllowGet);
        }

    }
}