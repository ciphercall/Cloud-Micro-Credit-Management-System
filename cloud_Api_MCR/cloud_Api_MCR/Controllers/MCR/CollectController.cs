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
    public class CollectController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;
        private BranchController brunchController;
        private SchemeController schemeController;
        private AccountChartController accountChartController;
        private MemberController memberController;
        private MemberSchemeController memberSchemeController;

        public CollectController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                brunchController = new BranchController();
                schemeController = new SchemeController();
                memberController = new MemberController();
                accountChartController = new AccountChartController();
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




        public ActionResult Create()
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "Collect" && getModel.Actionname == "Create")
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

            TempData["passData_SelectBrunchList"] = brunchController.BranchList();
            TempData["passData_SelectFieldWorkersList"] = accountChartController.AccountChartList();
            TempData["passData_SelectSchemeList"] = schemeController.SchemeList();
            TempData["passData_SelectMemberList"] = memberController.MemberList();
            return View();
        }


        [HttpPost]
        public ActionResult Create(ViewMcrCollect model)
        {
            model.Compid = _loggedCompid;
            TempData["ViewMcrCollectModel"] = model;
            return RedirectToAction("_GetCollectionInvoice");
        }



        public ActionResult Update()
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
                                    if (getModel.Compid == _loggedCompid && getModel.Userid == _loggedUserid && getModel.Controllername == "Collect" && getModel.Actionname == "Create")
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

            TempData["passData_SelectBrunchList"] = brunchController.BranchList();
            TempData["passData_SelectFieldWorkersList"] = accountChartController.AccountChartList();
            TempData["passData_SelectSchemeList"] = schemeController.SchemeList();
            TempData["passData_SelectMemberList"] = memberController.MemberList();
            return View();
        }

        
        [HttpPost]
        public ActionResult Update(ViewMcrCollect model)
        {
            model.Compid = _loggedCompid;
            TempData["ViewMcrCollectModel"] = model;
            return RedirectToAction("_GetCollectionInvoice");
        }





        //Collection Invoice
        public ActionResult _GetCollectionInvoice()
        {
            ViewMcrCollect passModel = (ViewMcrCollect)TempData["ViewMcrCollectModel"];
            var reportModel = new ViewMcrListModel();
            reportModel.BranchList = brunchController.BranchList();
            reportModel.AccountChartList = accountChartController.AccountChartList();
            reportModel.MembersList = memberController.MemberList();
            reportModel.CollectionList = MonthMemoNoWiseCollectionList(passModel.Transmy, passModel.Transno);
            return View(reportModel);
        }





        //Get Collection Invoice List
        public List<ViewMcrCollect> MonthMemoNoWiseCollectionList(String monthyear, Int64 memoNo)
        {
            List<ViewMcrCollect> monthMemoNoWiseCollectionList = new List<ViewMcrCollect>();
            //Http Get - get List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/Collect/List?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&transMonthYear=" + monthyear + "&transno=" + memoNo;

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
                                    ViewMcrCollect getModel = new ViewMcrCollect();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transmy = (String)get["Transmy"];
                                    getModel.Transno = (Int64)get["Transno"];
                                    getModel.Transsl = (Int64)get["Transsl"];

                                    getModel.Transdt = (DateTime)get["Transdt"];
                                    if (get["Branchid"].ToString() != "")
                                    {
                                        getModel.Branchid = (Int64)get["Branchid"];
                                    }
                                    if (get["Fwid"].ToString() != "")
                                    {
                                        getModel.Fwid = (Int64)get["Fwid"];
                                    }
                                    getModel.Schemeid = (String)get["Schemeid"];
                                    if (get["Memberid"].ToString() != "")
                                    {
                                        getModel.Memberid = (Int64)get["Memberid"];
                                    }
                                    if (get["Internid"].ToString() != "")
                                    {
                                        getModel.Internid = (Int64)get["Internid"];
                                    }
                                    getModel.Amount = (Decimal)get["Amount"];
                                    getModel.Remarks = (String)get["Remarks"];
                                    monthMemoNoWiseCollectionList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return monthMemoNoWiseCollectionList;
        }










        //JseonResult for DateChanged and get year.............................Start
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult DateChanged_getMonth(DateTime txtTransDate)//with Trans No Generation
        {
            string converttoString = Convert.ToString(txtTransDate.ToString("dd-MMM-yyyy"));
            string getYear = converttoString.Substring(9, 2);
            string getMonth = converttoString.Substring(3, 3).ToUpper();
            getMonth = getMonth + "-" + getYear;

            String transno = "";
            //Http Get - get Find Max TransNo
            List<ViewGlAcchart> debitList = new List<ViewGlAcchart>();
            String accountChartInfoUrl1 = HttpClientBaseAddress.BaseAddress() + "api/Collect/FindMaxTransNo?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&transDate=" + txtTransDate;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(accountChartInfoUrl1).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            bool success;
                            string message = "";

                            var getresult = content.ReadAsStringAsync().Result;
                            JObject jobject = JObject.Parse(getresult);
                            var successMessage = jobject["success"].ToString();
                            message = jobject["message"].ToString();
                            success = successMessage.Equals("True") ? true : false;
                            if (success == true)
                            {
                                transno = jobject["data"].ToString();
                            }
                        }
                    }
                }
            }
            var result = new { month = getMonth, TransNo = transno };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //JseonResult for DateChanged and get year.............................End








        public JsonResult Invoiceload(string theDate)
        {
            DateTime transDate = Convert.ToDateTime(theDate);

            //Http Get - get Invoice
            List<ViewMcrCollect> transNoList = new List<ViewMcrCollect>();
            String getTransactionInvoiceInfoUrl = HttpClientBaseAddress.BaseAddress() + "api/Collect/DateWiseTransNoList?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&transDate=" + transDate;

            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loggedToken);
                using (HttpResponseMessage response = httpclient.GetAsync(getTransactionInvoiceInfoUrl).Result)
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
                                    ViewMcrCollect getModel = new ViewMcrCollect();
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Transno = (Int64)get["Transno"];
                                    transNoList.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            List<SelectListItem> trans = new List<SelectListItem>();
            foreach (var f in transNoList)
            {
                trans.Add(new SelectListItem { Text = f.Transno.ToString(), Value = f.Transno.ToString() });
            }
            return Json(new SelectList(trans, "Value", "Text"));
        }







        public JsonResult GetCollectInformation(String transDate, String transNo)
        {
            String getBranchName = "", getFieldWorkersName = "";

            DateTime date = Convert.ToDateTime(transDate);
            string converttoString = Convert.ToString(date.ToString("dd-MMM-yyyy"));
            string getYear = converttoString.Substring(9, 2);
            string getMonth = converttoString.Substring(3, 3).ToUpper();
            getMonth = getMonth + "-" + getYear;

            ViewMcrCollect getModel = new ViewMcrCollect();

            //Http Get - get User Information 
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/Collect/List?userCompanycode=" + _loggedCompid +
                               "&usercode=" + _loggedUserid + "&transMonthYear=" + getMonth + "&transno=" + transNo;
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
                                    getModel.Compid = (Int64)get["Compid"];
                                    getModel.Branchid = (Int64)get["Branchid"];
                                    getModel.Fwid = (Int64)get["Fwid"];
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            List<ViewMcrBranch> passData1 = (List<ViewMcrBranch>)brunchController.BranchList();
            foreach (var n in passData1)
            {
                if (n.Branchid == getModel.Branchid)
                {
                    getBranchName = n.Branchnm;
                }
            }


            List<ViewGlAcchart> passData2 = (List<ViewGlAcchart>)accountChartController.AccountChartList();
            foreach (var n in passData2)
            {
                if (n.Accountcd == getModel.Fwid)
                {
                    getFieldWorkersName = n.Accountnm;
                }
            }

            var userResult = new
            {
                branchid = getModel.Branchid,
                fwid = getModel.Fwid,
                branchName = getBranchName,
                fieldWorkersName = getFieldWorkersName,
            };
            return Json(userResult, JsonRequestBehavior.AllowGet);
        }




        public JsonResult InternalIdLoadAfterSchemeSelect(String schemeId)
        {
            List<SelectListItem> getInternalIdList = new List<SelectListItem>();
            List<ViewMcrMScheme> memberSchemeList = (List<ViewMcrMScheme>)memberSchemeController.MemberSchemeList();

            var findInternalId = (from m in memberSchemeList where m.Compid == _loggedCompid && m.Schemeid == schemeId select m).ToList();
            foreach (var getModel in findInternalId)
            {
                getInternalIdList.Add(new SelectListItem { Text = getModel.Internid.ToString(), Value = Convert.ToString(getModel.Internid) });
            }
            return Json(new SelectList(getInternalIdList, "Value", "Text"));
        }




        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetMemberName(Int64 txtInternId, string txtSchemeId)//with Trans No Generation
        {
            String memberName = "";
            Int64 memberid = 0;
            List<ViewMcrMScheme> memberSchemeList = (List<ViewMcrMScheme>)memberSchemeController.MemberSchemeList();
            var findMemberId = (from m in memberSchemeList where m.Compid == _loggedCompid && m.Schemeid == txtSchemeId && m.Internid == txtInternId select m).ToList();
            foreach (var getModel in findMemberId)
            {
                memberid = getModel.Memberid;
            }

            List<ViewMcrMember> memberList = (List<ViewMcrMember>)memberController.MemberList();
            var findMemberName = (from m in memberList where m.Compid == _loggedCompid && m.Memberid == memberid select m).ToList();
            foreach (var getModel in findMemberName)
            {
                memberName = getModel.Membernm;
            }

            var result = new { MemberName = memberName, MemberId = memberid };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}