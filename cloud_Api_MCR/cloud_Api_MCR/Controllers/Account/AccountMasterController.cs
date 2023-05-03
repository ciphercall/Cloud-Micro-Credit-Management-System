using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using cloud_Api_MCR.DataAccess.OTHERS;
using cloud_Api_MCR.Models.Account_DTO;
using cloud_Api_MCR.Models.MCR_DTO;
using Newtonsoft.Json.Linq;

namespace cloud_Api_MCR.Controllers.Account
{
    public class AccountMasterController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;

        public AccountMasterController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
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



        //Get Total GL_Master List
        public List<ViewGlMaster> TotalAccountMasterList()
        {
            List<ViewGlMaster> totalAccountMasterList = new List<ViewGlMaster>();
            //Http Get - get Mcr Master List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountMaster/TotalList?userCompanycode=" + _loggedCompid +
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
                                    ViewGlMaster getModel = new ViewGlMaster();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transtp = (String)get["Transtp"];
                                    getModel.Transmy = (String)get["Transmy"];
                                    getModel.Transno = (Int64)get["Transno"];
                                    getModel.Transsl = (Int64)get["Transsl"];

                                    getModel.Transdt = (DateTime)get["Transdt"];
                                    getModel.Transdrcr = (String)get["Transdrcr"];
                                    getModel.Transfor = (String)get["Transfor"];
                                    getModel.Transmode = (String)get["Transmode"];
                                    if (get["Costpid"].ToString() != "")
                                    {
                                        getModel.Costpid = (Int64)get["Costpid"];
                                    }
                                    if (get["Creditcd"].ToString() != "")
                                    {
                                        getModel.Creditcd = (Int64)get["Creditcd"];
                                    }
                                    if (get["Debitcd"].ToString() != "")
                                    {
                                        getModel.Debitcd = (Int64)get["Debitcd"];
                                    }
                                    getModel.Chequeno = (String)get["Chequeno"];


                                    if (get["Chequedt"].ToString() != "")
                                    {
                                        getModel.Chequedt = (DateTime)get["Chequedt"];
                                    }
                                    getModel.Remarks = (String)get["Remarks"];
                                    getModel.Debitamt = Convert.ToDecimal(get["Debitamt"]);
                                    getModel.Creditamt = Convert.ToDecimal(get["Creditamt"]);
                                    getModel.Tableid = (String)get["Tableid"];
                                    totalAccountMasterList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return totalAccountMasterList;
        }






        //Get DebitCd wise GL_Master List
        public List<ViewGlMaster> DebitcdWiseAccountMasterList(Int64 debitcd)
        {
            List<ViewGlMaster> debitcdWiseAccountMasterList = new List<ViewGlMaster>();
            //Http Get - get Mcr Master List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountMaster/DebitCdWiseList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&debitcd=" + debitcd;

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
                                    ViewGlMaster getModel = new ViewGlMaster();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transtp = (String)get["Transtp"];
                                    getModel.Transmy = (String)get["Transmy"];
                                    getModel.Transno = (Int64)get["Transno"];
                                    getModel.Transsl = (Int64)get["Transsl"];

                                    getModel.Transdt = (DateTime)get["Transdt"];
                                    getModel.Transdrcr = (String)get["Transdrcr"];
                                    getModel.Transfor = (String)get["Transfor"];
                                    getModel.Transmode = (String)get["Transmode"];
                                    if (get["Costpid"].ToString() != "")
                                    {
                                        getModel.Costpid = (Int64)get["Costpid"];
                                    }
                                    if (get["Creditcd"].ToString() != "")
                                    {
                                        getModel.Creditcd = (Int64)get["Creditcd"];
                                    }
                                    if (get["Debitcd"].ToString() != "")
                                    {
                                        getModel.Debitcd = (Int64)get["Debitcd"];
                                    }
                                    getModel.Chequeno = (String)get["Chequeno"];


                                    if (get["Chequedt"].ToString() != "")
                                    {
                                        getModel.Chequedt = (DateTime)get["Chequedt"];
                                    }
                                    getModel.Remarks = (String)get["Remarks"];
                                    getModel.Debitamt = Convert.ToDecimal(get["Debitamt"]);
                                    getModel.Creditamt = Convert.ToDecimal(get["Creditamt"]);
                                    getModel.Tableid = (String)get["Tableid"];
                                    debitcdWiseAccountMasterList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return debitcdWiseAccountMasterList;
        }






        //Get Date Wise GL_Master List
        public List<ViewGlMaster> DateWiseAccountMasterList(String fromDate, String toDate)
        {
            List<ViewGlMaster> dateWiseAccountMasterList = new List<ViewGlMaster>();
            //Http Get - get Mcr Master List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountMaster/DateWiseList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&fromdate=" + fromDate + "&toDate=" + toDate;

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
                                    ViewGlMaster getModel = new ViewGlMaster();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transtp = (String)get["Transtp"];
                                    getModel.Transmy = (String)get["Transmy"];
                                    getModel.Transno = (Int64)get["Transno"];
                                    getModel.Transsl = (Int64)get["Transsl"];

                                    getModel.Transdt = (DateTime)get["Transdt"];
                                    getModel.Transdrcr = (String)get["Transdrcr"];
                                    getModel.Transfor = (String)get["Transfor"];
                                    getModel.Transmode = (String)get["Transmode"];
                                    if (get["Costpid"].ToString() != "")
                                    {
                                        getModel.Costpid = (Int64)get["Costpid"];
                                    }
                                    if (get["Creditcd"].ToString() != "")
                                    {
                                        getModel.Creditcd = (Int64)get["Creditcd"];
                                    }
                                    if (get["Debitcd"].ToString() != "")
                                    {
                                        getModel.Debitcd = (Int64)get["Debitcd"];
                                    }
                                    getModel.Chequeno = (String)get["Chequeno"];


                                    if (get["Chequedt"].ToString() != "")
                                    {
                                        getModel.Chequedt = (DateTime)get["Chequedt"];
                                    }
                                    getModel.Remarks = (String)get["Remarks"];
                                    getModel.Debitamt = Convert.ToDecimal(get["Debitamt"]);
                                    getModel.Creditamt = Convert.ToDecimal(get["Creditamt"]);
                                    getModel.Tableid = (String)get["Tableid"];
                                    dateWiseAccountMasterList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return dateWiseAccountMasterList;
        }

        
        //Get Less than From Date Wise GL_Master List
        public List<ViewGlMaster> LessThanFromDateWiseAccountMasterList(String fromDate)
        {
            List<ViewGlMaster> list = new List<ViewGlMaster>();
            //Http Get - get Mcr Master List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountMaster/LessThanFromDateWiseList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&fromdate=" + fromDate;

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
                                    ViewGlMaster getModel = new ViewGlMaster();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transtp = (String)get["Transtp"];
                                    getModel.Transmy = (String)get["Transmy"];
                                    getModel.Transno = (Int64)get["Transno"];
                                    getModel.Transsl = (Int64)get["Transsl"];

                                    getModel.Transdt = (DateTime)get["Transdt"];
                                    getModel.Transdrcr = (String)get["Transdrcr"];
                                    getModel.Transfor = (String)get["Transfor"];
                                    getModel.Transmode = (String)get["Transmode"];
                                    if (get["Costpid"].ToString() != "")
                                    {
                                        getModel.Costpid = (Int64)get["Costpid"];
                                    }
                                    if (get["Creditcd"].ToString() != "")
                                    {
                                        getModel.Creditcd = (Int64)get["Creditcd"];
                                    }
                                    if (get["Debitcd"].ToString() != "")
                                    {
                                        getModel.Debitcd = (Int64)get["Debitcd"];
                                    }
                                    getModel.Chequeno = (String)get["Chequeno"];


                                    if (get["Chequedt"].ToString() != "")
                                    {
                                        getModel.Chequedt = (DateTime)get["Chequedt"];
                                    }
                                    getModel.Remarks = (String)get["Remarks"];
                                    getModel.Debitamt = Convert.ToDecimal(get["Debitamt"]);
                                    getModel.Creditamt = Convert.ToDecimal(get["Creditamt"]);
                                    getModel.Tableid = (String)get["Tableid"];
                                    list.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }




        
        //Get Less than Equal to To-Date Wise GL_Master List
        public List<ViewGlMaster> LessThanEqualtoToDateWiseAccountMasterList(String toDate)
        {
            List<ViewGlMaster> list = new List<ViewGlMaster>();
            //Http Get - get Mcr Master List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountMaster/LessThanEqualtoToDateWiseList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&todate=" + toDate;

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
                                    ViewGlMaster getModel = new ViewGlMaster();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transtp = (String)get["Transtp"];
                                    getModel.Transmy = (String)get["Transmy"];
                                    getModel.Transno = (Int64)get["Transno"];
                                    getModel.Transsl = (Int64)get["Transsl"];

                                    getModel.Transdt = (DateTime)get["Transdt"];
                                    getModel.Transdrcr = (String)get["Transdrcr"];
                                    getModel.Transfor = (String)get["Transfor"];
                                    getModel.Transmode = (String)get["Transmode"];
                                    if (get["Costpid"].ToString() != "")
                                    {
                                        getModel.Costpid = (Int64)get["Costpid"];
                                    }
                                    if (get["Creditcd"].ToString() != "")
                                    {
                                        getModel.Creditcd = (Int64)get["Creditcd"];
                                    }
                                    if (get["Debitcd"].ToString() != "")
                                    {
                                        getModel.Debitcd = (Int64)get["Debitcd"];
                                    }
                                    getModel.Chequeno = (String)get["Chequeno"];


                                    if (get["Chequedt"].ToString() != "")
                                    {
                                        getModel.Chequedt = (DateTime)get["Chequedt"];
                                    }
                                    getModel.Remarks = (String)get["Remarks"];
                                    getModel.Debitamt = Convert.ToDecimal(get["Debitamt"]);
                                    getModel.Creditamt = Convert.ToDecimal(get["Creditamt"]);
                                    getModel.Tableid = (String)get["Tableid"];
                                    list.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }



    }
}