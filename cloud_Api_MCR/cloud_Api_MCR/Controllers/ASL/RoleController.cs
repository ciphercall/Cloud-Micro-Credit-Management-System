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
    public class RoleController : Controller
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;

        public RoleController()
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



        public ActionResult CompanyWiseUserRoleList()
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




        //CompanyWiseUserRoleList Table, this view table works partial
        public PartialViewResult _UserRoleInfo(Int64 cid, Int64 uid)
        {
            List<ViewAslRole> userRoleList = new List<ViewAslRole>();
            if (cid!=0 && uid != 0)
            {
                //Http Get - get company's user Role List
                String userRoleInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/Role/CompanyWiseUserRoleList?userCompanycode=" + _loggedCompid +
                                     "&usercode=" + _loggedUserid + "&selectCompanyCode=" + cid+ "&selectUserCode=" + uid;

                using (HttpClient httpclient = new HttpClient())
                {
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                    using (HttpResponseMessage response = httpclient.GetAsync(userRoleInfoUrl).Result)
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
                                        ViewAslRole userRole = new ViewAslRole();
                                        userRole.SelectCompId = (Int64)get["SelectCompId"];
                                        userRole.SelectUserId = (Int64)get["SelectUserId"];
                                        userRole.Moduleid = (string)get["Moduleid"];
                                        userRole.ModuleName = (string)get["ModuleName"];
                                        userRole.Menutp = (string)get["Menutp"];
                                        userRole.Menuid = (string)get["Menuid"];
                                        userRole.Serial = (Int64)get["Serial"];
                                        userRole.Status = (string)get["Status"];
                                        userRole.Insertr = (string)get["Insertr"];
                                        userRole.Updater = (string)get["Updater"];
                                        userRole.Deleter = (string)get["Deleter"];
                                        userRole.Menuname = (string)get["Menuname"];
                                        userRole.Actionname = (string)get["Actionname"];
                                        userRole.Controllername = (string)get["Controllername"];
                                        userRoleList.Add(userRole);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return PartialView("~/Views/Role/_UserRoleInfo.cshtml", userRoleList);
        }





        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ComapnyNameChanged(Int64 changedDropDown)
        {
            List<SelectListItem> getUserName = new List<SelectListItem>();
            //Http Get - get user List
            String userInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/User/UserList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&selectCompanyCode=" + changedDropDown;

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
                                    getUserName.Add(new SelectListItem { Text = viewAslUserco.Usernm, Value = viewAslUserco.Userid.ToString() });
                                }
                            }
                        }
                    }
                }
            }
            return Json(getUserName, JsonRequestBehavior.AllowGet);
        }


    }
}