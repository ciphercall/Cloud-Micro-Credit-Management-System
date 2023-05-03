using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using cloud_Api_MCR.DataAccess.OTHERS;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.ASL_DTO;
using Newtonsoft.Json.Linq;

namespace cloud_Api_MCR.Controllers
{
    public class AppController : Controller
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken,_loggedCompanyName;

        public AppController()
        {
            if (System.Web.HttpContext.Current.Session["loggedCompID"] != null && (System.Web.HttpContext.Current.Session["loggedCompanyStatus"].ToString() == "A" || System.Web.HttpContext.Current.Session["loggedCompanyStatus"].ToString() == ""))
            {
                _loggedCompid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
                _loggedUserid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
                _loggedToken = Convert.ToString(System.Web.HttpContext.Current.Session["loggedToken"].ToString());
                _loggedCompanyName = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyName"].ToString());
            }
            else
            {
                RedirectToAction("Index", "Logout");
            }
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            try
            {
                List<ViewAslRole> validUserFormList = new List<ViewAslRole>();
                List<ViewAslRole> validUserReportsList = new List<ViewAslRole>();
                List<ViewAslRole> validMicroCreditFormList = new List<ViewAslRole>();
                List<ViewAslRole> validMicroCreditReportsList = new List<ViewAslRole>();
                List<ViewAslRole> validAccountFormList = new List<ViewAslRole>();
                List<ViewAslRole> validAccountReportsList = new List<ViewAslRole>();
                List<ViewAslRole> validPromotionFormList = new List<ViewAslRole>();

                //Http Get - get Role List and binding the users module wise menu
                String companyWiseUserRoleUrl = HttpClientBaseAddress.BaseAddress() + "api/Role/RoleList?userCompanycode=" + _loggedCompid +
                                     "&usercode=" + _loggedUserid;

                using (HttpClient httpclient = new HttpClient())
                {
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                    using (HttpResponseMessage response = httpclient.GetAsync(companyWiseUserRoleUrl).Result)
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
                                        getModel.Moduleid = (string)get["Moduleid"];
                                        getModel.Menutp = (string)get["Menutp"];
                                        getModel.Menuid = (string)get["Menuid"];
                                        getModel.Serial = (Int64)get["Serial"];

                                        getModel.Status = (string)get["Status"];
                                        getModel.Insertr = (string)get["Insertr"];
                                        getModel.Updater = (string)get["Updater"];
                                        getModel.Deleter = (string)get["Deleter"];
                                        getModel.Menuname = (string)get["Menuname"];
                                        getModel.Actionname = (string)get["Actionname"];
                                        getModel.Controllername = (string)get["Controllername"];

                                        if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Status == "A" && getModel.Menutp == "F" && getModel.Moduleid == "01")
                                        {
                                            validUserFormList.Add(getModel);
                                        }
                                        else if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Status == "A" && getModel.Menutp == "R" && getModel.Moduleid == "01")
                                        {
                                            validUserReportsList.Add(getModel);
                                        }
                                        else if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Status == "A" && getModel.Menutp == "F" && getModel.Moduleid == "02")
                                        {
                                            validMicroCreditFormList.Add(getModel);
                                        }
                                        else if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Status == "A" && getModel.Menutp == "R" && getModel.Moduleid == "02")
                                        {
                                            validMicroCreditReportsList.Add(getModel);
                                        }
                                        else if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Status == "A" && getModel.Menutp == "F" && getModel.Moduleid == "03")
                                        {
                                            validAccountFormList.Add(getModel);
                                        }
                                        else if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Status == "A" && getModel.Menutp == "R" && getModel.Moduleid == "03")
                                        {
                                            validAccountReportsList.Add(getModel);
                                        }
                                        else if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Status == "A" && getModel.Menutp == "F" && getModel.Moduleid == "04")
                                        {
                                            validPromotionFormList.Add(getModel);
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


                ViewData["validUserForm"] = validUserFormList.OrderBy(e=>e.Serial).ToList();
                ViewData["validUserReports"] = validUserReportsList.OrderBy(e => e.Serial).ToList();
                ViewData["validMicroCreditForm"] = validMicroCreditFormList.OrderBy(e => e.Serial).ToList();
                ViewData["validMicroCreditReports"] = validMicroCreditReportsList.OrderBy(e => e.Serial).ToList();
                ViewData["validAccountForm"] = validAccountFormList.OrderBy(e => e.Serial).ToList();
                ViewData["validAccountReports"] = validAccountReportsList.OrderBy(e => e.Serial).ToList();
                ViewData["validPromotionForm"] = validPromotionFormList.OrderBy(e => e.Serial).ToList();

                ViewData["CompanyName"] = _loggedCompanyName;


                //ViewData["validUserForm"] = from c in db.AslRoleDbSet
                //                            where (c.Userid == userid && c.Compid == comid && c.Status == "A" && c.Menutp == "F" && c.Moduleid == "01")
                //                            select c;

                //ViewData["validUserReports"] = from c in db.AslRoleDbSet
                //                               where (c.Userid == userid && c.Compid == comid && c.Status == "A" && c.Menutp == "R" && c.Moduleid == "01")
                //                               select c;

                //ViewData["validBillingForm"] = (from c in db.AslRoleDbSet
                //                                where (c.Userid == userid && c.Compid == comid && c.Status == "A" && c.MENUTP == "F" && c.MODULEID == "02")
                //                                select c).OrderBy(x => x.SERIAL);

                //ViewData["validBillingReports"] = (from c in db.AslRoleDbSet
                //                                   where (c.Userid == userid && c.Compid == comid && c.Status == "A" && c.MENUTP == "R" && c.MODULEID == "02")
                //                                   select c).OrderBy(x => x.SERIAL);

                //ViewData["validAccountForm"] = (from c in db.AslRoleDbSet
                //                                where (c.Userid == userid && c.Compid == comid && c.Status == "A" && c.MENUTP == "F" && c.MODULEID == "03")
                //                                select c).OrderBy(x => x.SERIAL);

                //ViewData["validAccountReports"] = (from c in db.AslRoleDbSet
                //                                   where (c.Userid == userid && c.Compid == comid && c.Status == "A" && c.MENUTP == "R" && c.MODULEID == "03")
                //                                   select c).OrderBy(x => x.SERIAL);

                //ViewData["validPromotionForm"] = (from c in db.AslRoleDbSet
                //                                  where (c.Userid == userid && c.Compid == comid && c.Status == "A" && c.MENUTP == "F" && c.MODULEID == "04")
                //                                  select c).OrderBy(x => x.SERIAL);

                
            }
            catch (Exception ex)
            {
                RedirectToAction("Index", "Logout");
            }
        }
    }
}