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
    public class PasswordController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken,_loggedUserType, _loggedUserStatus, _loggedCompanyStatus;

        public PasswordController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                _loggedToken = Convert.ToString(System.Web.HttpContext.Current.Session["loggedToken"].ToString());
                _loggedCompid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
                _loggedUserid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
                _loggedUserType = Convert.ToString(System.Web.HttpContext.Current.Session["loggedUserType"].ToString());
                _loggedUserStatus = Convert.ToString(System.Web.HttpContext.Current.Session["loggedUserStatus"].ToString());
                _loggedCompanyStatus = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyStatus"].ToString());
            }
            else
            {
                RedirectToAction("Index", "Logout");
            }
        }




        public ActionResult PasswordChangedForm()
        {
            if ((_loggedUserType == "SADMIN" || _loggedUserType == "CADMIN" || _loggedUserType == "UADMIN" || _loggedUserType == "USER") && _loggedUserStatus == "A" && (_loggedCompanyStatus == "A"|| _loggedCompanyStatus == ""))
            {

            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
            return View();
        }



        // GET: /Password/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordChangedForm(ViewPassword password)
        {
            String companyCode = _loggedCompid.ToString();
            String userCode = _loggedUserid.ToString();

            //Http Post - update Password 
            String updatePasswordUrl = HttpClientBaseAddress.BaseAddress() + "api/Password/Update";
            IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                     new KeyValuePair<string, string>("Compid",companyCode),
                     new KeyValuePair<string, string>("Userid",userCode),
                     new KeyValuePair<string, string>("OldPassword",password.OldPassword),
                     new KeyValuePair<string, string>("NewPassword",password.NewPassword),
                     new KeyValuePair<string, string>("ConfirmedPassword",password.ConfirmedPassword)
                };
            HttpContent query = new FormUrlEncodedContent(queries);
            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.PostAsync(updatePasswordUrl, query).Result)
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
                                ViewBag.PasswordMessage = message;
                                return View();
                            }
                            else
                            {
                                ViewBag.PasswordMessage = message;
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





        //Check Old Password 
        public JsonResult Check_Password(string oldPassword)
        {
            String companyCode = _loggedCompid.ToString();
            String userCode = _loggedUserid.ToString();
            bool checkresult = false;

            //Http Get - Check Password 
            String getPasswordCheckUrl = HttpClientBaseAddress.BaseAddress() + "api/Password/OldPasswordCheck?userCompanycode=" + companyCode +
                                     "&usercode=" + userCode + "&OldPassword=" + oldPassword;
            using (HttpClient httpclient = new HttpClient())
            {
                //httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(getPasswordCheckUrl).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            bool success;
                            string message = "",data="";

                            var result = content.ReadAsStringAsync().Result;
                            JObject jobject = JObject.Parse(result);
                            var successMessage = jobject["success"].ToString();
                            message = jobject["message"].ToString();
                            data = jobject["data"].ToString();
                            success = successMessage.Equals("True") ? true : false;
                            if (success == true)
                            {
                                checkresult = true;
                            }
                        }
                    }
                    else
                    {
                    }
                }
            }

            //var result = db.AslUsercoDbSet.Count(d => d.LOGINPW == oldpassword && d.Compid == compid && d.USERID == userid) != 0;
            return Json(checkresult, JsonRequestBehavior.AllowGet);
        }

       
    }
}