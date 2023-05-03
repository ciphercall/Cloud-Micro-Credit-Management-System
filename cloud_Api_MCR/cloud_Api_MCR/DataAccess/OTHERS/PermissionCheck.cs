using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using cloud_Api_MCR.Models.ASL_DTO;
using Newtonsoft.Json.Linq;

namespace cloud_Api_MCR.DataAccess.OTHERS
{
    public class PermissionCheck
    {
        public static bool ReportPermissionCheck(Int64 loggedCompid, Int64 loggedUserid, string loggedToken, String controllerName, String actionName)
        {
            try
            {
                //Http Get - get Role List and binding the users module wise menu
                String userRoleUrl = HttpClientBaseAddress.BaseAddress() + "api/Role/RoleList?userCompanycode=" + loggedCompid +
                                     "&usercode=" + loggedUserid;
                using (HttpClient httpclient = new HttpClient())
                {
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", loggedToken);
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
                                        getModel.Menutp = (string)get["Menutp"];
                                        getModel.Status = (string)get["Status"];
                                        getModel.Insertr = (string)get["Insertr"];
                                        getModel.Updater = (string)get["Updater"];
                                        getModel.Deleter = (string)get["Deleter"];
                                        getModel.Menuname = (string)get["Menuname"];
                                        getModel.Actionname = (string)get["Actionname"];
                                        getModel.Controllername = (string)get["Controllername"];
                                        if (getModel.Compid == loggedCompid && getModel.Userid == loggedUserid && getModel.Controllername == controllerName && getModel.Menutp=="R" && getModel.Actionname == actionName && getModel.Status == "A")
                                        {
                                            return true;
                                        }
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}