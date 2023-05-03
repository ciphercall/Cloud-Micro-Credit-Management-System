using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using cloud_Api_MCR.Controllers.Account;
using cloud_Api_MCR.DataAccess.OTHERS;
using cloud_Api_MCR.Models.MCR_DTO;
using Newtonsoft.Json.Linq;

namespace cloud_Api_MCR.Controllers.MCR
{
    public class ReportMcrController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp,_loggedCompanyName, _loggedCompanyAddress, _loggedCompanyAddress2, _loggedCompanyContactno;
        private BranchController branchController;
        private AreaController areaController;
        private MemberController memberController;
        private SchemeController schemeController;
        private MemberSchemeController memberSchemeController;
        private MemberNomineeController memberNomineeController;
        private MemberLoanController memberLoanController;
        private MemberTransactionController memberTransactionController;
        private McrMasterController mcrMasterController;

        private AccountChartController accountChartController;

        public ReportMcrController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                branchController = new BranchController();
                areaController = new AreaController();
                memberController = new MemberController();
                schemeController = new SchemeController();
                memberSchemeController = new MemberSchemeController();
                memberNomineeController = new MemberNomineeController();
                memberLoanController = new MemberLoanController();
                memberTransactionController = new MemberTransactionController();
                mcrMasterController = new McrMasterController();

                accountChartController = new AccountChartController();
                _loggedCompid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
                _loggedUserid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
                _loggedToken = Convert.ToString(System.Web.HttpContext.Current.Session["loggedToken"].ToString());
                _loggedUserTp = Convert.ToString(System.Web.HttpContext.Current.Session["loggedUserType"].ToString());
                _loggedCompanyName = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyName"].ToString());
                _loggedCompanyAddress = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyAddress"].ToString());
                _loggedCompanyAddress2 = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyAddress2"].ToString());
                _loggedCompanyContactno = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyContactno"].ToString());
                ViewData["HighLight_Menu_BillingReport"] = "highlight menu";
            }
            else
            {
                RedirectToAction("Index", "Logout");
            }
        }




        //Branch-Area Information
        public ActionResult _GetBranchAreaInformation()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr","_GetBranchAreaInformation");
            if (check == true)
            {
                ViewBag.CompanyName = _loggedCompanyName;
                ViewBag.CompanyAddress = _loggedCompanyAddress;
                ViewBag.CompanyAddress2 = _loggedCompanyAddress2;

                var reportModel = new ViewMcrListModel();
                reportModel.BranchList = branchController.BranchList();
                reportModel.AreaList = areaController.AreaList();
               
                return View(reportModel);
            }
            else
            {
                return RedirectToAction("Index","Logout");
            }
        }




        //Scheme Information
        public ActionResult _GetSchemeInformation()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "_GetSchemeInformation");
            if (check == true)
            {
                ViewBag.CompanyName = _loggedCompanyName;
                ViewBag.CompanyAddress = _loggedCompanyAddress;
                ViewBag.CompanyAddress2 = _loggedCompanyAddress2;

                var reportModel = new ViewMcrListModel();
                reportModel.SchemeList = schemeController.SchemeList();
                reportModel.AccountChartList = accountChartController.AccountChartList();
                return View(reportModel);
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }





        //Member-Nominee Information
        public ActionResult _GetMemberNomineeInformation()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "_GetMemberNomineeInformation");
            if (check == true)
            {
                ViewBag.CompanyName = _loggedCompanyName;
                ViewBag.CompanyAddress = _loggedCompanyAddress;
                ViewBag.CompanyAddress2 = _loggedCompanyAddress2;

                var reportModel = new ViewMcrListModel();
                reportModel.MembersList = memberController.MemberList();
                reportModel.MembersNomineeList = memberNomineeController.MemberNomineeList();
                return View(reportModel);
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }





        // Member Information
        public ActionResult MemberInformation()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "MemberInformation");
            if (check == true)
            {
                TempData["passData_SelectMemberList"] = memberController.MemberList(); 
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }

        [HttpPost]
        public ActionResult MemberInformation(ViewMcrMember model)
        {
            TempData["ViewMcrMemberModel"] = model;
            return RedirectToAction("_GetMemberInformation");
        }

        public ActionResult _GetMemberInformation()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "MemberInformation");
            if (check == true)
            {
                var reportModel = new ViewMcrListModel();
                reportModel.MembersSchemeList = memberSchemeController.MemberSchemeList();
                reportModel.MembersList = memberController.MemberList();

                ViewBag.MemberModel = (ViewMcrMember) TempData["ViewMcrMemberModel"];
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }









        // Scheme-Member Information
        public ActionResult SchemeMemberInformation()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "SchemeMemberInformation");
            if (check == true)
            {
                TempData["passData_SelectSchemeList"] = schemeController.SchemeList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }


        [HttpPost]
        public ActionResult SchemeMemberInformation(ViewMcrScheme model)
        {
            TempData["ViewMcrSchemeModel"] = model;
            return RedirectToAction("_GetSchemeMemberInformation");
        }


        public ActionResult _GetSchemeMemberInformation()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "SchemeMemberInformation");
            if (check == true)
            {
                var reportModel = new ViewMcrListModel();
                reportModel.MembersSchemeList = memberSchemeController.MemberSchemeList();
                reportModel.MembersList = memberController.MemberList();
                reportModel.SchemeList = schemeController.SchemeList();
                ViewBag.SchemeModel = (ViewMcrScheme)TempData["ViewMcrSchemeModel"];
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }





        // Loan Information
        public ActionResult LoanInformation()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "LoanInformation");
            if (check == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }


        [HttpPost]
        public ActionResult LoanInformation(ViewMcrReportModel model)
        {
            TempData["ViewMcrReportModel"] = model;
            return RedirectToAction("_GetLoanInformation");
        }


        public ActionResult _GetLoanInformation()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "LoanInformation");
            if (check == true)
            {
                var reportModel = new ViewMcrListModel();

                ViewMcrReportModel passModel  = (ViewMcrReportModel)TempData["ViewMcrReportModel"];
                String fromDate = "", toDate = "";
                if (passModel.FromDate != null)
                {
                    DateTime fdate = Convert.ToDateTime(passModel.FromDate);
                    fromDate = fdate.ToString("dd-MMM-yyyy");
                }
                if (passModel.ToDate != null)
                {
                    DateTime tdate = Convert.ToDateTime(passModel.ToDate);
                    toDate = tdate.ToString("dd-MMM-yyyy");
                }

                reportModel.MembersLoanList = memberLoanController.DateWiseMemberLoanList(fromDate, toDate);
                reportModel.MembersList = memberController.MemberList();
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }






        // Receipts/Payments Statement
        public ActionResult ReceiptsPaymentsStatement()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "ReceiptsPaymentsStatement");
            if (check == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }


        [HttpPost]
        public ActionResult ReceiptsPaymentsStatement(ViewMcrReportModel model)
        {
            TempData["ViewMcrReportModel"] = model;
            return RedirectToAction("_GetReceiptsPaymentsStatement");
        }


        public ActionResult _GetReceiptsPaymentsStatement()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "ReceiptsPaymentsStatement");
            if (check == true)
            {
                var reportModel = new ViewMcrListModel();

                ViewMcrReportModel passModel = (ViewMcrReportModel)TempData["ViewMcrReportModel"];
                String fromDate = "", toDate = "";
                if (passModel.FromDate != null)
                {
                    DateTime fdate = Convert.ToDateTime(passModel.FromDate);
                    fromDate = fdate.ToString("dd-MMM-yyyy");
                }
                if (passModel.ToDate != null)
                {
                    DateTime tdate = Convert.ToDateTime(passModel.ToDate);
                    toDate = tdate.ToString("dd-MMM-yyyy");
                }

                reportModel.MembersTransactionList = memberTransactionController.TypeNdDateWiseMemberTransactionList(passModel.Type,fromDate, toDate);
                reportModel.MembersList = memberController.MemberList();
                reportModel.AccountChartList = accountChartController.AccountChartList();
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                if (passModel.Type == "MREC")
                {
                    ViewBag.Type = "Receipt";
                }
                else if (passModel.Type == "MPAY")
                {
                    ViewBag.Type = "Payment";
                }
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }






        // Fund Transfer Statement
        public ActionResult FundTransferStatement()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "FundTransferStatement");
            if (check == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }


        [HttpPost]
        public ActionResult FundTransferStatement(ViewMcrReportModel model)
        {
            TempData["ViewMcrReportModel"] = model;
            return RedirectToAction("_GetFundTransferStatement");
        }


        public ActionResult _GetFundTransferStatement()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "FundTransferStatement");
            if (check == true)
            {
                var reportModel = new ViewMcrListModel();

                ViewMcrReportModel passModel = (ViewMcrReportModel)TempData["ViewMcrReportModel"];
                String fromDate = "", toDate = "";
                if (passModel.FromDate != null)
                {
                    DateTime fdate = Convert.ToDateTime(passModel.FromDate);
                    fromDate = fdate.ToString("dd-MMM-yyyy");
                }
                if (passModel.ToDate != null)
                {
                    DateTime tdate = Convert.ToDateTime(passModel.ToDate);
                    toDate = tdate.ToString("dd-MMM-yyyy");
                }
                passModel.Type = "FTRF";

                reportModel.MembersTransactionList = memberTransactionController.TypeNdDateWiseMemberTransactionList(passModel.Type, fromDate, toDate);
                reportModel.MembersList = memberController.MemberList();
                reportModel.AccountChartList = accountChartController.AccountChartList();
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                if (passModel.Type == "FTRF")
                {
                    ViewBag.Type = "Fund Transfer";
                }
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }




        //Scheme with Ledger Statement
        public ActionResult SchemeWithLedgerStatement()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "SchemeWithLedgerStatement");
            if (check == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }


        [HttpPost]
        public ActionResult SchemeWithLedgerStatement(ViewMcrReportModel model)
        {
            TempData["ViewMcrReportModel"] = model;
            return RedirectToAction("_GetSchemeWithLedgerStatement");
        }


        public ActionResult _GetSchemeWithLedgerStatement()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "SchemeWithLedgerStatement");
            if (check == true)
            {
                var reportModel = new ViewMcrListModel();

                ViewMcrReportModel passModel = (ViewMcrReportModel)TempData["ViewMcrReportModel"];
                String fromDate = "", toDate = "";
                if (passModel.FromDate != null)
                {
                    DateTime fdate = Convert.ToDateTime(passModel.FromDate);
                    fromDate = fdate.ToString("dd-MMM-yyyy");
                }
                if (passModel.ToDate != null)
                {
                    DateTime tdate = Convert.ToDateTime(passModel.ToDate);
                    toDate = tdate.ToString("dd-MMM-yyyy");
                }

                reportModel.MembersTransactionList = memberTransactionController.DateWiseMemberTransactionList(fromDate, toDate);
                reportModel.MembersList = memberController.MemberList();
                reportModel.AccountChartList = accountChartController.AccountChartList();
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }






        // Deposit Scheme
        public ActionResult DepositScheme()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "DepositScheme");
            if (check == true)
            {
                TempData["passData_SelectSchemeList"] = schemeController.SchemeList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }


        [HttpPost]
        public ActionResult DepositScheme(ViewMcrReportModel model)
        {
            TempData["ViewMcrReportModel"] = model;
            return RedirectToAction("_GetDepositScheme");
        }

        public ActionResult _GetDepositScheme()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "DepositScheme");
            if (check == true)
            {
                var reportModel = new ViewMcrListModel();

                ViewMcrReportModel passModel = (ViewMcrReportModel)TempData["ViewMcrReportModel"];
                String date = "";
                if (passModel.FromDate != null)
                {
                    DateTime fdate = Convert.ToDateTime(passModel.FromDate);
                    date = fdate.ToString("dd-MMM-yyyy");
                }

                reportModel.McrMasterList = mcrMasterController.AsOnDateAndSchemeWiseMcrMasterList(passModel.Type, date);
                reportModel.MembersList = memberController.MemberList();
                reportModel.MembersSchemeList = memberSchemeController.MemberSchemeList();
                ViewBag.Date = date;
                ViewBag.SchemeName = passModel.Type;
                ViewBag.Status = passModel.Status;
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }







        // Loan Scheme
        public ActionResult LoanScheme()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "LoanScheme");
            if (check == true)
            {
                TempData["passData_SelectSchemeList"] = schemeController.SchemeList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }


        [HttpPost]
        public ActionResult LoanScheme(ViewMcrReportModel model)
        {
            TempData["ViewMcrReportModel"] = model;
            return RedirectToAction("_GetLoanScheme");
        }

        public ActionResult _GetLoanScheme()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "LoanScheme");
            if (check == true)
            {
                var reportModel = new ViewMcrListModel();

                ViewMcrReportModel passModel = (ViewMcrReportModel)TempData["ViewMcrReportModel"];
                String date = "";
                if (passModel.FromDate != null)
                {
                    DateTime fdate = Convert.ToDateTime(passModel.FromDate);
                    date = fdate.ToString("dd-MMM-yyyy");
                }

                reportModel.McrMasterList = mcrMasterController.AsOnDateAndSchemeWiseMcrMasterList(passModel.Type, date);
                reportModel.MembersList = memberController.MemberList();
                reportModel.MembersLoanList = memberLoanController.SchemeWiseMemberLoanList(passModel.Type);
                ViewBag.Date = date;
                ViewBag.SchemeName = passModel.Type;
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }





        // MEMBER WISE BALANCE STATEMENT
        public ActionResult MemberWiseBalanceStatement()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "MemberWiseBalanceStatement");
            if (check == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }


        [HttpPost]
        public ActionResult MemberWiseBalanceStatement(ViewMcrReportModel model)
        {
            TempData["ViewMcrReportModel"] = model;
            return RedirectToAction("_GetMemberWiseBalanceStatement");
        }


        public ActionResult _GetMemberWiseBalanceStatement()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "MemberWiseBalanceStatement");
            if (check == true)
            {
                var reportModelList = new ViewMcrListModel();

                ViewMcrReportModel passModel = (ViewMcrReportModel)TempData["ViewMcrReportModel"];
                String fromDate = "", toDate = "";
                if (passModel.FromDate != null)
                {
                    DateTime fdate = Convert.ToDateTime(passModel.FromDate);
                    fromDate = fdate.ToString("dd-MMM-yyyy");
                }
                if (passModel.ToDate != null)
                {
                    DateTime tdate = Convert.ToDateTime(passModel.ToDate);
                    toDate = tdate.ToString("dd-MMM-yyyy");
                }


                List<ViewMcrReportModel> reportModel = new List<ViewMcrReportModel>();
                //Http Get - get Mcr Master List
                String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/McrReports/MemberWiseBalanceStatement?userCompanycode=" + _loggedCompid +
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
                                        ViewMcrReportModel getModel = new ViewMcrReportModel();
                                        getModel.Compid = Convert.ToInt64(get["Compid"]);
                                        getModel.Memberid = Convert.ToInt64(get["Memberid"]);
                                        getModel.Membernm = Convert.ToString(get["Membernm"]);
                                        getModel.Areaid = Convert.ToInt64(get["Areaid"]);
                                        getModel.Internid = Convert.ToInt64(get["Internid"]);
                                        getModel.Schemeid = Convert.ToString(get["Schemeid"]);
                                        getModel.Schemetp = Convert.ToString(get["Schemetp"]);
                                        getModel.Opening = Convert.ToDecimal(get["Opening"]);
                                        getModel.Debitamt = Convert.ToDecimal(get["Debitamt"]);
                                        getModel.Creditamt = Convert.ToDecimal(get["Creditamt"]);
                                        getModel.Closing = Convert.ToDecimal(get["Closing"]);
                                        reportModel.Add(getModel);
                                    }
                                }
                            }
                        }
                    }
                }

                reportModelList.McrReportsList = reportModel;
                reportModelList.MembersList = memberController.MemberList();
                reportModelList.AccountChartList = accountChartController.AccountChartList();
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                return View(reportModelList);
            }
            else
            {
                return View();
            }
        }




        //MEMBER WISE AREA STATEMENT-AREA
        public ActionResult MemberWiseBalanceStatementWithArea()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "MemberWiseBalanceStatementWithArea");
            if (check == true)
            {
                TempData["passData_SelectAreaList"] = areaController.AreaList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }


        [HttpPost]
        public ActionResult MemberWiseBalanceStatementWithArea(ViewMcrReportModel model)
        {
            TempData["ViewMcrReportModel"] = model;
            return RedirectToAction("_GetMemberWiseBalanceStatementWithArea");
        }


        public ActionResult _GetMemberWiseBalanceStatementWithArea()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "MemberWiseBalanceStatementWithArea");
            if (check == true)
            {
                var reportModelList = new ViewMcrListModel();

                ViewMcrReportModel passModel = (ViewMcrReportModel)TempData["ViewMcrReportModel"];
                String fromDate = "", toDate = "";
                if (passModel.FromDate != null)
                {
                    DateTime fdate = Convert.ToDateTime(passModel.FromDate);
                    fromDate = fdate.ToString("dd-MMM-yyyy");
                }
                if (passModel.ToDate != null)
                {
                    DateTime tdate = Convert.ToDateTime(passModel.ToDate);
                    toDate = tdate.ToString("dd-MMM-yyyy");
                }


                List<ViewMcrReportModel> reportModel = new List<ViewMcrReportModel>();
                //Http Get - get Mcr Master List
                String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/McrReports/MemberWiseBalanceStatementWithArea?userCompanycode=" + _loggedCompid +
                                     "&usercode=" + _loggedUserid + "&fromdate=" + fromDate + "&toDate=" + toDate+ "&areaCode=" + passModel.Areaid;

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
                                        ViewMcrReportModel getModel = new ViewMcrReportModel();
                                        getModel.Compid = Convert.ToInt64(get["Compid"]);
                                        getModel.Memberid = Convert.ToInt64(get["Memberid"]);
                                        getModel.Membernm = Convert.ToString(get["Membernm"]);
                                        getModel.Areaid = Convert.ToInt64(get["Areaid"]);
                                        getModel.Internid = Convert.ToInt64(get["Internid"]);
                                        getModel.Schemeid = Convert.ToString(get["Schemeid"]);
                                        getModel.Schemetp = Convert.ToString(get["Schemetp"]);
                                        getModel.Opening = Convert.ToDecimal(get["Opening"]);
                                        getModel.Debitamt = Convert.ToDecimal(get["Debitamt"]);
                                        getModel.Creditamt = Convert.ToDecimal(get["Creditamt"]);
                                        getModel.Closing = Convert.ToDecimal(get["Closing"]);
                                        reportModel.Add(getModel);
                                    }
                                }
                            }
                        }
                    }
                }

                String areaName = "";
                var findAreaName = (from m in areaController.AreaList() where m.Areaid == passModel.Areaid select new {m.Areanm})
                        .Distinct().ToList();
                foreach (var getName in findAreaName)
                {
                    areaName = getName.Areanm.ToString();
                }
                reportModelList.McrReportsList = reportModel;
                reportModelList.MembersList = memberController.MemberList();

                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.AreaName = areaName;
                return View(reportModelList);
            }
            else
            {
                return View();
            }
        }



        
        //FIELD WORKER WISE DAILY COLLECTION STATEMENT
        public ActionResult FieldWorkerWiseDailyCollectionStatement()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "FieldWorkerWiseDailyCollectionStatement");
            if (check == true)
            {
                TempData["passData_SelectFieldWorkersList"] = accountChartController.AccountChartList();
                TempData["passData_SelectSchemeList"] = schemeController.SchemeList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }

        [HttpPost]
        public ActionResult FieldWorkerWiseDailyCollectionStatement(ViewMcrReportModel model)
        {
            TempData["ViewMcrReportModel"] = model;
            return RedirectToAction("_GetFieldWorkerWiseDailyCollectionStatement");
        }


        public ActionResult _GetFieldWorkerWiseDailyCollectionStatement()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "FieldWorkerWiseDailyCollectionStatement");
            if (check == true)
            {
                var reportModelList = new ViewMcrListModel();

                ViewMcrReportModel passModel = (ViewMcrReportModel)TempData["ViewMcrReportModel"];
                String date = "";
                if (passModel.FromDate != null)
                {
                    DateTime fdate = Convert.ToDateTime(passModel.FromDate);
                    date = fdate.ToString("dd-MMM-yyyy");
                }

                List<ViewMcrReportModel> reportModel = new List<ViewMcrReportModel>();
                //Http Get - get Opening Balance List
                String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/McrReports/FieldWorkerWiseDailyCollectionStatement?userCompanycode=" + _loggedCompid +
                                     "&usercode=" + _loggedUserid + "&date=" + date + "&schemeId=" + passModel.Schemeid + "&fieldWorkersId=" + passModel.Fwid;

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
                                        ViewMcrReportModel getModel = new ViewMcrReportModel();
                                        getModel.Compid = Convert.ToInt64(get["Compid"]);
                                        getModel.Schemeid = Convert.ToString(get["Schemeid"]);
                                        getModel.Internid = Convert.ToInt64(get["Internid"]);
                                        getModel.Membernm = Convert.ToString(get["Membernm"]);
                                        getModel.AreaName = Convert.ToString(get["AreaName"]);
                                        getModel.Opening = Convert.ToDecimal(get["Opening"]);
                                        getModel.Collection = Convert.ToDecimal(get["Collection"]);
                                        getModel.Payment = Convert.ToDecimal(get["Payment"]);
                                        reportModel.Add(getModel);
                                    }
                                }
                            }
                        }
                    }
                }
                reportModelList.McrReportsList = reportModel;
                reportModelList.AreaList = areaController.AreaList();

                ViewBag.Date = date;
                ViewBag.SchemeName = passModel.Schemeid.ToString();
                var findFiledWorkerName = (from m in accountChartController.AccountChartList() where m.Accountcd == passModel.Fwid
                        select new {m.Accountnm}).Distinct().ToList();
                foreach (var getName in findFiledWorkerName)
                {
                    ViewBag.FieldWorkerName = getName.Accountnm.ToString();
                }
                return View(reportModelList);
            }
            else
            {
                return View();
            }
        }









        //Member Ledger
        public ActionResult MemberLedger()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "MemberLedger");
            if (check == true)
            {
                TempData["passData_SelectSchemeList"] = schemeController.SchemeList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }


        [HttpPost]
        public ActionResult MemberLedger(ViewMcrReportModel model)
        {
            TempData["ViewMcrReportModel"] = model;
            return RedirectToAction("_GetMemberLedger");
        }


        public ActionResult _GetMemberLedger()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportMcr", "MemberLedger");
            if (check == true)
            {
                var reportModelList = new ViewMcrListModel();

                ViewMcrReportModel passModel = (ViewMcrReportModel)TempData["ViewMcrReportModel"];
                String fromDate = "", toDate = "";
                if (passModel.FromDate != null)
                {
                    DateTime fdate = Convert.ToDateTime(passModel.FromDate);
                    fromDate = fdate.ToString("dd-MMM-yyyy");
                }
                if (passModel.ToDate != null)
                {
                    DateTime tdate = Convert.ToDateTime(passModel.ToDate);
                    toDate = tdate.ToString("dd-MMM-yyyy");
                }


                List<ViewMcrReportModel> reportModel = new List<ViewMcrReportModel>();
                //Http Get - get Opening Balance List
                String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/McrReports/MemberLedgerOpeningBalance?userCompanycode=" + _loggedCompid +
                                     "&usercode=" + _loggedUserid + "&fromdate=" + fromDate + "&schemeId=" + passModel.Schemeid + "&internalId=" + passModel.Internid + "&memberId="+ passModel.Memberid;

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
                                        ViewMcrReportModel getModel = new ViewMcrReportModel();
                                        getModel.Compid = Convert.ToInt64(get["Compid"]);
                                        getModel.Memberid = Convert.ToInt64(get["Memberid"]);
                                        getModel.Internid = Convert.ToInt64(get["Internid"]);
                                        getModel.Schemeid = Convert.ToString(get["Schemeid"]);
                                        getModel.Schemetp = Convert.ToString(get["Schemetp"]);
                                        getModel.Opening = Convert.ToDecimal(get["Opening"]);
                                        reportModel.Add(getModel);
                                    }
                                }
                            }
                        }
                    }
                }

                reportModelList.McrMasterList = mcrMasterController.DateWiseMcrMasterList(fromDate,toDate);
                reportModelList.McrReportsList = reportModel;
                reportModelList.MembersList = memberController.MemberList();
                reportModelList.SchemeList = schemeController.SchemeList();

                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.SchemeName = passModel.Schemeid.ToString();
                ViewBag.InternID = passModel.Internid.ToString();
                ViewBag.MemberId = passModel.Memberid.ToString();
                ViewBag.MemberName = passModel.Membernm;
                return View(reportModelList);
            }
            else
            {
                return View();
            }
        }
    }
}