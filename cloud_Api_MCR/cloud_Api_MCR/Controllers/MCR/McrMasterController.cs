using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using cloud_Api_MCR.DataAccess.OTHERS;
using cloud_Api_MCR.Models.MCR_DTO;
using Newtonsoft.Json.Linq;

namespace cloud_Api_MCR.Controllers.MCR
{
    public class McrMasterController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;
        private MemberController memberController;
        private SchemeController schemeController;
        private MemberSchemeController memberSchemeController;
        public McrMasterController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                memberController = new MemberController();
                schemeController = new SchemeController();
                memberSchemeController = new MemberSchemeController();
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




        //Get Date And Scheme Wise MCR_Master List
        public List<ViewMcrMaster> AsOnDateAndSchemeWiseMcrMasterList(String schemeId, String date)
        {
            List<ViewMcrMaster> dateAndSchemeWiseMcrMasterList = new List<ViewMcrMaster>();
            //Http Get - get Mcr Master List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/McrMaster/AsOnDateAndSchemeWiseList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&date=" + date + "&schemeid=" + schemeId;

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
                                    ViewMcrMaster getModel = new ViewMcrMaster();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transtp = (String)get["Transtp"];
                                    getModel.Transdt = (DateTime)get["Transdt"];
                                    getModel.Transmy = (String)get["Transmy"];
                                    getModel.Transno = (Int64)get["Transno"];
                                    getModel.Transsl = (Int64)get["Transsl"];

                                    getModel.Transdrcr = (String)get["Transdrcr"];
                                    getModel.Transfor = (String)get["Transfor"];
                                    getModel.Transmode = (String)get["Transmode"];
                                    if (get["Memberid"].ToString() != "")
                                    {
                                        getModel.Memberid = (Int64)get["Memberid"];
                                    }
                                    if (get["Schemeid"].ToString() != "")
                                    {
                                        getModel.Schemeid = (String)get["Schemeid"];
                                    }
                                    if (get["Internid"].ToString() != "")
                                    {
                                        getModel.Internid = (Int64)get["Internid"];
                                    }
                                    getModel.Debitamt = Convert.ToDecimal(get["Debitamt"]);
                                    getModel.Creditamt = Convert.ToDecimal(get["Creditamt"]);
                                    getModel.Remarks = (String)get["Remarks"];
                                    getModel.Tableid = (String)get["Tableid"];
                                    dateAndSchemeWiseMcrMasterList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return dateAndSchemeWiseMcrMasterList;
        }




        //Get Date Wise MCR_Master List
        public List<ViewMcrMaster> DateWiseMcrMasterList(String fromDate, String toDate)
        {
            List<ViewMcrMaster> dateWiseMcrMasterList = new List<ViewMcrMaster>();
            //Http Get - get Mcr Master List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/McrMaster/DateWiseList?userCompanycode=" + _loggedCompid +
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
                                    ViewMcrMaster getModel = new ViewMcrMaster();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transtp = (String)get["Transtp"];
                                    getModel.Transdt = (DateTime)get["Transdt"];
                                    getModel.Transmy = (String)get["Transmy"];
                                    getModel.Transno = (Int64)get["Transno"];
                                    getModel.Transsl = (Int64)get["Transsl"];

                                    getModel.Transdrcr = (String)get["Transdrcr"];
                                    getModel.Transfor = (String)get["Transfor"];
                                    getModel.Transmode = (String)get["Transmode"];
                                    if (get["Memberid"].ToString() != "")
                                    {
                                        getModel.Memberid = (Int64)get["Memberid"];
                                    }
                                    if (get["Schemeid"].ToString() != "")
                                    {
                                        getModel.Schemeid = (String)get["Schemeid"];
                                    }
                                    if (get["Internid"].ToString() != "")
                                    {
                                        getModel.Internid = (Int64)get["Internid"];
                                    }
                                    getModel.Debitamt = Convert.ToDecimal(get["Debitamt"]);
                                    getModel.Creditamt = Convert.ToDecimal(get["Creditamt"]);
                                    getModel.Remarks = (String)get["Remarks"];
                                    getModel.Tableid = (String)get["Tableid"];
                                    dateWiseMcrMasterList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return dateWiseMcrMasterList;
        }
    }
}