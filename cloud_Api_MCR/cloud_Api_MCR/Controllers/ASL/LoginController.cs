using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using cloud_Api_MCR.DataAccess.OTHERS;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.ASL_DTO;
using Newtonsoft.Json.Linq;

namespace cloud_Api_MCR.Controllers.ASL
{
    public class LoginController : Controller
    {
        private DatabaseDbContext db = new DatabaseDbContext();


        ////Datetime formet
        //IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        //TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        //private DateTime td;

        public LoginController()
        {
            //td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }


        public ActionResult Index()
        {
            Session["HomePage"] = "Show home page";
            return View();
        }


        [HttpPost]
        public ActionResult Index(ViewLoginModel model)
        {
            if (ModelState.IsValid)
            {
                string token = "", companyStatus = "", companyName="",companyAddress="",companyAddress2="", companyContactno="";
                ViewAslUserco viewModel = new ViewAslUserco();


                ////HttpClient setup 
                //var client = new HttpClient();
                //string baseUrl = System.Web.HttpContext.Current
                //                            .Request
                //                            .Url
                //                            .GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped);
                //client.BaseAddress = new Uri(baseUrl);


                //Http Post - get token 
                String getTokenUrl = HttpClientBaseAddress.BaseAddress() + "api/login/UserLogin";
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
                {
                     new KeyValuePair<string, string>("Loginid",model.Loginid),
                     new KeyValuePair<string, string>("Loginpw",model.Loginpw),
                     new KeyValuePair<string, string>("Logltude",model.Logltude)
                };
                HttpContent query = new FormUrlEncodedContent(queries);
                using (HttpClient httpclient = new HttpClient())
                {
                    using (HttpResponseMessage response = httpclient.PostAsync(getTokenUrl, query).Result)
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
                                    token = jobject["Token"].ToString();
                                    var data = jobject["data"].ToString();
                                    JObject jdata = JObject.Parse(data);
                                    model.Compid = Convert.ToInt64(jdata["CompanyId"].ToString());
                                    model.Userid = Convert.ToInt64(jdata["UserId"].ToString());
                                }
                                else
                                {
                                    ViewData["ErrorMessage"] = message;
                                    Session["HomePage"] = "Show home page";
                                    return View("Index");
                                }
                            }
                        }
                        else
                        {
                            ViewData["ErrorMessage"] = "Login failed.";
                            Session["HomePage"] = "Show home page";
                            return View("Index");
                        }
                    }
                }





                //Http Get - get User Information
                String userInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/User/UserInformation?userCompanycode=" + model.Compid +
                                     "&usercode=" + model.Userid+ "&selectCompanyCode=" + model.Compid + "&selectUserCode=" + model.Userid; //+ "&token=" + token;

                using (HttpClient httpclient = new HttpClient())
                {
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
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
                                        viewModel.Compid = (Int64)get["Compid"];
                                        viewModel.Userid = (Int64)get["Userid"];
                                        viewModel.Optp = (string)get["Optp"];
                                        viewModel.Deptnm = (string)get["Deptnm"];
                                        viewModel.Usernm = (string)get["Usernm"];
                                        viewModel.Mobno = (string)get["Mobno"];
                                        viewModel.Emailid = (string)get["Emailid"];
                                        viewModel.Timefr = (string)get["Timefr"];
                                        viewModel.Timeto = (string)get["Timeto"];
                                        viewModel.Status = (string)get["Status"];
                                    }
                                }
                                else
                                {
                                    ViewData["ErrorMessage"] = message;
                                    Session["HomePage"] = "Show home page";
                                    return View("Index");
                                }
                            }
                        }
                    }
                }


                //Http Get - get company Information
                String companyInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/Company/CompanyInformation?userCompanycode=" + model.Compid +
                                     "&usercode=" + model.Userid+ "&selectCompanyCode="+model.Compid; //+ "&token=" + token;

                using (HttpClient httpclient = new HttpClient())
                {
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
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
                                        companyName = (string)get["Compnm"];
                                        companyAddress = (string)get["Address"];
                                        companyAddress2 = (string)get["Address2"];
                                        companyContactno = (string)get["Contactno"];
                                        companyStatus = (string)get["Status"];
                                    }
                                }
                            }
                        }
                    }
                }



                //Company Status Check
                if (companyStatus == "A" || companyStatus == "")
                {
                }
                else
                {
                    return RedirectToAction("Index", "Logout");
                }


                ////Check TimeFor & TimeTo for current datetime login
                model.Logtime = Convert.ToString(UserlogAddressforWeb.Timezone(DateTime.Now).ToString("HH:mm:ss"));
                TimeSpan logTimeSpan = TimeSpan.Parse(model.Logtime);
                TimeSpan timeForSpan = TimeSpan.Parse(viewModel.Timefr);
                TimeSpan timeToSpan = TimeSpan.Parse(viewModel.Timeto);
                if (timeForSpan <= logTimeSpan && logTimeSpan <= timeToSpan && viewModel.Status == "A")
                {

                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }


                Session["loggedToken"] = token;
                Session["loggedCompID"] = viewModel.Compid;
                Session["loggedUserID"] = viewModel.Userid;
                Session["loggedUserType"] = viewModel.Optp;
                Session["loggedUserName"] = viewModel.Usernm;
                Session["loggedUserStatus"] = viewModel.Status;
                Session["loggedCompanyStatus"] = companyStatus;
                Session["loggedCompanyName"] = companyName;
                Session["loggedCompanyAddress"] = companyAddress;
                Session["loggedCompanyAddress2"] = companyAddress2;
                Session["loggedCompanyContactno"] = companyContactno;
                

                ////Http Post - LogData Save
                //String postSaveLogDataUrl = HttpClientBaseAddress.BaseAddress() + "api/Log/SaveLog";
                //model.Logdata = Convert.ToString("User Name: " + viewModel.Usernm + ",\nDepartment Name: " + viewModel.Deptnm + ",\nOperation Type: " + viewModel.Optp + ",\nMobile No: " + viewModel.Mobno + ",\nEmail ID: " + viewModel.Emailid + ",\nLogin ID: " + model.Loginid + ",\nTime From: " + viewModel.Timefr + ",\nTime To: " + viewModel.Timeto + ",\nStatus: " + viewModel.Status + ".");
                //IEnumerable<KeyValuePair<string, string>> logQueries = new List<KeyValuePair<string, string>>()
                //{
                //     new KeyValuePair<string, string>("Compid",model.Compid.ToString()),
                //     new KeyValuePair<string, string>("Userid",model.Userid.ToString()),
                //     new KeyValuePair<string, string>("Logtype","LOGIN"),
                //     new KeyValuePair<string, string>("Logltude",model.Logltude),
                //     new KeyValuePair<string, string>("Logdata",model.Logdata)
                //};
                //HttpContent logQuery = new FormUrlEncodedContent(logQueries);
                //using (HttpClient httpclient = new HttpClient())
                //{
                //    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
                //    using (HttpResponseMessage response = httpclient.PostAsync(postSaveLogDataUrl, logQuery).Result)
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            using (HttpContent content = response.Content)
                //            {
                //                bool success;
                //                string message = "";

                //                var result = content.ReadAsStringAsync().Result;
                //                JObject jobject = JObject.Parse(result);
                //                var successMessage = jobject["success"].ToString();
                //                message = jobject["message"].ToString();
                //                success = successMessage.Equals("True") ? true : false;
                //                if (success == true)
                //                {

                //                }
                //            }
                //        }
                //    }
                //}



                if (viewModel.Optp == "SADMIN")
                {
                    return RedirectToAction("Index", "Company");
                }
                else
                {
                    return RedirectToAction("Index", "GraphView");
                }
            }
            Session["HomePage"] = "Show home page";
            return View("Index");
        }






        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}