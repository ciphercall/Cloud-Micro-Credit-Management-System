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
    public class MemberSchemeController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;
        private SchemeController schemeController;
        private MemberController memberController;

        public MemberSchemeController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                schemeController = new SchemeController();
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "MemberScheme" && getModel.Actionname == "Index")
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
            TempData["passData_SelectSchemeList"] = schemeController.SchemeList();
            return View();
        }







        //Get Member Scheme List
        public List<ViewMcrMScheme> MemberSchemeList()
        {
            List<ViewMcrMScheme> memberSchemeList = new List<ViewMcrMScheme>();
            //Http Get - get Member List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/MemberScheme/TotalList?userCompanycode=" + _loggedCompid +
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
                                    ViewMcrMScheme getModel = new ViewMcrMScheme();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Memberid = (Int64)get["Memberid"];
                                    getModel.Schemesl = (Int64)get["Schemesl"];
                                    getModel.Schemeid = (string)get["Schemeid"];
                                    getModel.Internid = (Int64)get["Internid"];

                                    if (get["Schemeefdt"].ToString()!="")
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
                                    getModel.Remarks = (string)get["Remarks"];
                                    getModel.Status = (string)get["Status"];
                                    memberSchemeList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return memberSchemeList;
        }
    }
}