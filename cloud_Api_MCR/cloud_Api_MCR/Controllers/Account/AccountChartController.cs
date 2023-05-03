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
    public class AccountChartController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;

        public AccountChartController()
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



        public ActionResult AccountChartMasterInfo()
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "AccountChart" && getModel.Actionname == "AccountChartInfo")
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
            return View();
        }






        public ActionResult AccountChartInfo()
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "AccountChart" && getModel.Actionname == "AccountChartInfo")
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
            return View();
        }






        //Get Account Chart List
        public List<ViewGlAcchart> AccountChartList()
        {
            List<ViewGlAcchart> accountsList = new List<ViewGlAcchart>();
            //Http Get - get List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountChart/TotalList?userCompanycode=" + _loggedCompid +
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
                                    ViewGlAcchart getModel = new ViewGlAcchart();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Headcd = (Int64)get["Headcd"];
                                    getModel.Accountcd = (Int64)get["Accountcd"];
                                    getModel.Headtp = (int)get["Headtp"];
                                    getModel.Accountnm = (String)get["Accountnm"];
                                    getModel.Remarks = (String)get["Remarks"];
                                    accountsList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return accountsList;
        }






        //Get Account Chart Master List
        public List<ViewGlAcchartMst> AccountChartMasterList()
        {
            List<ViewGlAcchartMst> accountsList = new List<ViewGlAcchartMst>();
            //Http Get - get List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountChartMaster/TotalList?userCompanycode=" + _loggedCompid +
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
                                    ViewGlAcchartMst getModel = new ViewGlAcchartMst();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Headcd = (Int64)get["Headcd"];
                                    getModel.Headtp = (int)get["Headtp"];
                                    getModel.Headnm = (String)get["Headnm"];
                                    getModel.Remarks = (String)get["Remarks"];
                                    accountsList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return accountsList;
        }


        public JsonResult GetHeadName(string headType)
        {
            List<ViewGlAcchartMst> headList = new List<ViewGlAcchartMst>();
            //Http Get - get Module List
            String moduleInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountChartMaster/List?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid+ "&headType=" + headType;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(moduleInfoUrl).Result)
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
                                    getModel.Headcd = (Int64)get["Headcd"];
                                    getModel.Headnm = (string)get["Headnm"];
                                    headList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var f in headList)
            {
                list.Add(new SelectListItem { Text = f.Headnm.ToString(), Value = f.Headcd.ToString() });
            }
            return Json(new SelectList(list, "Value", "Text"));
        }






    }
}