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
    public class MenuPermissionController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;

        private UserController userController;
        private MenuController menuController;

        public MenuPermissionController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                userController = new UserController();
                menuController = new MenuController();
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



        // GET: Company Wise MenuPermission
        public ActionResult CompanyWiseIndex()
        {
            List<ViewAslCompany> companyList = new List<ViewAslCompany>();
            //Http Get - get Comapny List
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
            


            List<ViewAslMenumst> menuList = new List<ViewAslMenumst>();
            //Http Get - get Module List
            String moduleInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/Module/ModuleList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid;

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
                                    ViewAslMenumst getModel = new ViewAslMenumst();
                                    getModel.Moduleid = (string)get["Moduleid"];
                                    getModel.Modulenm = (string)get["Modulenm"];
                                    menuList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }

            TempData["passData_SelectCompanyList"] = companyList;
            TempData["passData_SelectModuleList"] = menuList;
            return View();
        }







        // GET: User Wise MenuPermission
        public ActionResult UserWiseIndex()
        {
            TempData["passData_SelectUserListExceptCompanyAdmin"] = userController.TotalUserListExceptCompanyAdmin(_loggedCompid);
            TempData["passData_SelectModuleList"] = menuController.TotalModuleList();
            return View();
        }



    }
}