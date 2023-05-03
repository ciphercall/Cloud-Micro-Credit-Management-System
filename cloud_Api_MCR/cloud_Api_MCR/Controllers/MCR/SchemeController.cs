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
    public class SchemeController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;
        private AccountChartController accountChartController;

        public SchemeController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "Scheme" && getModel.Actionname == "Index")
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
            
            TempData["passData_SelectAccountList"] = accountChartController.AccountChartList();
            return View();
        }





        //Get Scheme List
        public List<ViewMcrScheme> SchemeList()
        {
            List<ViewMcrScheme> schemeList = new List<ViewMcrScheme>();
            //Http Get - get Member List
            String memberInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/Scheme/List?userCompanycode=" + _loggedCompid +
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
                                    ViewMcrScheme getModel = new ViewMcrScheme();
                                    getModel.Schemecd = (Int64)get["Schemecd"];
                                    getModel.Schemeid = (String)get["Schemeid"];
                                    getModel.Schemetp = (String)get["Schemetp"];
                                    getModel.Glheadid = (Int64)get["Glheadid"];
                                    getModel.GlIncomehid = (Int64)get["GlIncomehid"];
                                    getModel.Remarks = (String)get["Remarks"];
                                    getModel.GlHeadName = (String)get["GlHeadName"];
                                    schemeList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return schemeList;
        }

    }
}