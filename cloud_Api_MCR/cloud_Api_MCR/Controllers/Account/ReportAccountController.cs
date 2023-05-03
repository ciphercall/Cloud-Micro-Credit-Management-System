using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using cloud_Api_MCR.Controllers.MCR;
using cloud_Api_MCR.DataAccess.OTHERS;
using cloud_Api_MCR.Models.Account_DTO;
using cloud_Api_MCR.Models.MCR_DTO;
using Newtonsoft.Json.Linq;

namespace cloud_Api_MCR.Controllers.Account
{
    public class ReportAccountController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp, _loggedCompanyName, _loggedCompanyAddress, _loggedCompanyAddress2, _loggedCompanyContactno;
        private AccountChartController accountChartController;
        private SingleTransactionController singleTransactionController;
        private CostPoolController costPoolController;
        private AccountMasterController accountMasterController;
       

        public ReportAccountController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                accountChartController = new AccountChartController();
                singleTransactionController = new SingleTransactionController();
                costPoolController = new CostPoolController();
                accountMasterController = new AccountMasterController();

                _loggedCompid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
                _loggedUserid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
                _loggedToken = Convert.ToString(System.Web.HttpContext.Current.Session["loggedToken"].ToString());
                _loggedUserTp = Convert.ToString(System.Web.HttpContext.Current.Session["loggedUserType"].ToString());
                _loggedCompanyName = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyName"].ToString());
                _loggedCompanyAddress = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyAddress"].ToString());
                _loggedCompanyAddress2 = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyAddress2"].ToString());
                _loggedCompanyContactno = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyContactno"].ToString());
                ViewData["HighLight_Menu_AccountReports"] = "highlight menu";
            }
            else
            {
                RedirectToAction("Index", "Logout");
            }
        }



        //Get Trial Balance Report
        public List<ViewAccountReportModel> TrialBalanceReport(String date, Int64 headCd)
        {
            List<ViewAccountReportModel> list = new List<ViewAccountReportModel>();
            //Http Get - get Mcr Master List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountReports/TrialBalance?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&date=" + date+ "&headCd=" + headCd;

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
                                    ViewAccountReportModel getModel = new ViewAccountReportModel();
                                    if (get["Debitcd"].ToString() != "")
                                    {
                                        getModel.Debitcd = (Int64)get["Debitcd"];
                                    }
                                    getModel.Accountnm = (String)get["Accountnm"];
                                    getModel.Debitamt = (decimal)get["Debitamt"];
                                    getModel.Creditamt = (decimal)get["Creditamt"];
                                    getModel.Headtp = (int)get["Headtp"];
                                    getModel.Headnm = (String)get["Headnm"];
                                    if (get["Headcd"].ToString() != "")
                                    {
                                        getModel.Headcd = (Int64)get["Headcd"];
                                    }
                                    list.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }







        //Get Trial Balance Report
        public List<ViewAccountReportModel> BalanceSheetReport(String date)
        {
            List<ViewAccountReportModel> list = new List<ViewAccountReportModel>();
            //Http Get - get Mcr Master List
            String infoUrl = HttpClientBaseAddress.BaseAddress() + "api/AccountReports/BalanceSheet?userCompanycode=" + _loggedCompid +
                                 "&usercode=" + _loggedUserid + "&date=" + date ;

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
                                    ViewAccountReportModel getModel = new ViewAccountReportModel();
                                    if (get["Debitcd"].ToString() != "")
                                    {
                                        getModel.Debitcd = (Int64)get["Debitcd"];
                                    }
                                    getModel.Accountnm = (String)get["Accountnm"];
                                    getModel.Amount = (decimal)get["Amount"];
                                    list.Add(getModel);
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }









        //..............................REPORT PAGE............................................................................

        // Cash Book
        public ActionResult CashBook()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "CashBook");
            if (check == true)
            {
                TempData["passData_SelectAccountChartList"] = accountChartController.AccountChartList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }

        [HttpPost]
        public ActionResult CashBook(ViewAccountReportModel model)
        {
            TempData["ViewAccountReportModel"] = model;
            return RedirectToAction("_GetCashBook");
        }

        public ActionResult _GetCashBook()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "CashBook");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();

                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
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

                reportModel.AccountChartList = accountChartController.AccountChartList();
                reportModel.AccountMasterList = accountMasterController.DebitcdWiseAccountMasterList(passModel.Accountcd);
                ViewBag.Accountcd = passModel.Accountcd;
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }



        

        // Bank Book
        public ActionResult BankBook()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "BankBook");
            if (check == true)
            {
                TempData["passData_SelectAccountChartList"] = accountChartController.AccountChartList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }

        [HttpPost]
        public ActionResult BankBook(ViewAccountReportModel model)
        {
            TempData["ViewAccountReportModel"] = model;
            return RedirectToAction("_GetBankBook");
        }

        public ActionResult _GetBankBook()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "BankBook");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();

                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
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

                reportModel.AccountChartList = accountChartController.AccountChartList();
                reportModel.AccountMasterList = accountMasterController.DebitcdWiseAccountMasterList(passModel.Accountcd);
                ViewBag.Accountcd = passModel.Accountcd;
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }





        // General Ledger
        public ActionResult GeneralLedger()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "GeneralLedger");
            if (check == true)
            {
                TempData["passData_SelectAccountChartList"] = accountChartController.AccountChartList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }

        [HttpPost]
        public ActionResult GeneralLedger(ViewAccountReportModel model)
        {
            TempData["ViewAccountReportModel"] = model;
            return RedirectToAction("_GetGeneralLedger");
        }

        public ActionResult _GetGeneralLedger()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "GeneralLedger");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();

                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
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

                reportModel.AccountChartList = accountChartController.AccountChartList();
                reportModel.AccountMasterList = accountMasterController.DebitcdWiseAccountMasterList(passModel.Accountcd);
                ViewBag.Accountcd = passModel.Accountcd;
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }






        // Transaction Listing
        public ActionResult TransactionListing()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "TransactionListing");
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
        public ActionResult TransactionListing(ViewAccountReportModel model)
        {
            TempData["ViewAccountReportModel"] = model;
            return RedirectToAction("_GetTransactionListing");
        }

        public ActionResult _GetTransactionListing()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "TransactionListing");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();

                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
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

                reportModel.AccountChartList = accountChartController.AccountChartList();
                reportModel.DateWiseAccountStransList = singleTransactionController.DateWiseList(fromDate, toDate);
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }






        // Cheque Register
        public ActionResult ChequeRegister()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "ChequeRegister");
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
        public ActionResult ChequeRegister(ViewAccountReportModel model)
        {
            TempData["ViewAccountReportModel"] = model;
            return RedirectToAction("_GetChequeRegister");
        }

        public ActionResult _GetChequeRegister()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "ChequeRegister");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();

                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
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

                reportModel.AccountChartList = accountChartController.AccountChartList();
                reportModel.DateWiseAccountMasterList = accountMasterController.DateWiseAccountMasterList(fromDate,toDate);
                ViewBag.Type = passModel.Type;
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }





        // Receipts/Payment Statement Details
        public ActionResult ReceiptsPaymentStatementDetails()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "ReceiptsPaymentStatementDetails");
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
        public ActionResult ReceiptsPaymentStatementDetails(ViewAccountReportModel model)
        {
            TempData["ViewAccountReportModel"] = model;
            return RedirectToAction("_GetReceiptsPaymentStatementDetails");
        }

        public ActionResult _GetReceiptsPaymentStatementDetails()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "ReceiptsPaymentStatementDetails");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();

                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
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

                reportModel.AccountChartList = accountChartController.AccountChartList();
                reportModel.LessThanFromDateWiseAccountMasterList = accountMasterController.LessThanFromDateWiseAccountMasterList(fromDate);
                reportModel.LessThanEqualtoToDateWiseAccountMasterList = accountMasterController.LessThanEqualtoToDateWiseAccountMasterList(toDate);
                reportModel.DateWiseAccountMasterList = accountMasterController.DateWiseAccountMasterList(fromDate, toDate);
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }






        // Receipts/Payment Statement-Summary
        public ActionResult ReceiptsPaymentStatementSummary()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "ReceiptsPaymentStatementSummary");
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
        public ActionResult ReceiptsPaymentStatementSummary(ViewAccountReportModel model)
        {
            TempData["ViewAccountReportModel"] = model;
            return RedirectToAction("_GetReceiptsPaymentStatementSummary");
        }

        public ActionResult _GetReceiptsPaymentStatementSummary()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "ReceiptsPaymentStatementSummary");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();
                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
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

                reportModel.AccountChartList = accountChartController.AccountChartList();
                reportModel.LessThanFromDateWiseAccountMasterList = accountMasterController.LessThanFromDateWiseAccountMasterList(fromDate);
                reportModel.LessThanEqualtoToDateWiseAccountMasterList = accountMasterController.LessThanEqualtoToDateWiseAccountMasterList(toDate);
                reportModel.DateWiseAccountMasterList = accountMasterController.DateWiseAccountMasterList(fromDate, toDate);
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;

                return View(reportModel);
            }
            else
            {
                return View();
            }
        }








        // Trial Balance
        public ActionResult TrialBalance()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "TrialBalance");
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
        public ActionResult TrialBalance(ViewAccountReportModel model, string command)
        {
            if (command == "Show")
            {
                string date = Convert.ToString(model.FromDate);
                DateTime myDateTime = DateTime.Parse(date);
                string converttoString = myDateTime.ToString("dd-MMM-yyyy");

                TempData["Trial_Balance_Date"] = converttoString;
                TempData["ViewAccountReportModel"] = model;
                return RedirectToAction("TrialBalance");
            }
            else //if (command == "Print")
            {
                TempData["ViewAccountReportModel"] = model;
                return RedirectToAction("_GetTrialBalance");
            }
        }

        public ActionResult _GetTrialBalance()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "TrialBalance");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();
                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
                String date ="";
                if (passModel.FromDate != null)
                {
                    DateTime fdate = Convert.ToDateTime(passModel.FromDate);
                    date = fdate.ToString("dd-MMM-yyyy");
                }

                reportModel.AccountChartList = accountChartController.AccountChartList();
                reportModel.LessThanEqualtoToDateWiseAccountMasterList = accountMasterController.LessThanEqualtoToDateWiseAccountMasterList(date);
                ViewBag.Date = date;
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }







        // Income Statement
        public ActionResult IncomeStatement()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "IncomeStatement");
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
        public ActionResult IncomeStatement(ViewAccountReportModel model)
        {
            TempData["ViewAccountReportModel"] = model;
            return RedirectToAction("_GetIncomeStatement");
        }

        public ActionResult _GetIncomeStatement()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "IncomeStatement");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();
                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
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

                reportModel.AccountChartMasterList = accountChartController.AccountChartMasterList();
                reportModel.DateWiseAccountMasterList = accountMasterController.DateWiseAccountMasterList(fromDate, toDate);
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;

                return View(reportModel);
            }
            else
            {
                return View();
            }
        }








        // Balance Sheet
        public ActionResult BalanceSheet()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "BalanceSheet");
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
        public ActionResult BalanceSheet(ViewAccountReportModel model, String command)
        {
            if (command == "Show")
            {
                string date = Convert.ToString(model.FromDate);
                DateTime myDateTime = DateTime.Parse(date);
                string converttoString = myDateTime.ToString("dd-MMM-yyyy");

                TempData["BalanceSheet_Date"] = converttoString;
                TempData["ViewAccountReportModel"] = model;
                return RedirectToAction("BalanceSheet");
            }
            else //if (command == "Print")
            {
                TempData["ViewAccountReportModel"] = model;
                return RedirectToAction("_GetBalanceSheet");
            }
        }

        public ActionResult _GetBalanceSheet()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "BalanceSheet");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();
                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
                String date = "";
                if (passModel.FromDate != null)
                {
                    DateTime fdate = Convert.ToDateTime(passModel.FromDate);
                    date = fdate.ToString("dd-MMM-yyyy");
                }

                reportModel.AccountChartList = accountChartController.AccountChartList();
                reportModel.AccountChartMasterList = accountChartController.AccountChartMasterList();
                reportModel.LessThanEqualtoToDateWiseAccountMasterList = accountMasterController.LessThanEqualtoToDateWiseAccountMasterList(date);
                reportModel.BalanceSheetReportsList = BalanceSheetReport(date);
                ViewBag.Date = date;
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }







        // Schedule/Notes (I)
        public ActionResult ScheduleNotes1()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "ScheduleNotes1");
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
        public ActionResult ScheduleNotes1(ViewAccountReportModel model)
        {
            TempData["ViewAccountReportModel"] = model;
            return RedirectToAction("_GetScheduleNotes1");
        }

        public ActionResult _GetScheduleNotes1()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "ScheduleNotes1");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();
                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
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

                reportModel.AccountChartList = accountChartController.AccountChartList();
                reportModel.AccountChartMasterList = accountChartController.AccountChartMasterList();
                reportModel.LessThanEqualtoToDateWiseAccountMasterList = accountMasterController.LessThanEqualtoToDateWiseAccountMasterList(toDate);
                reportModel.DateWiseAccountMasterList = accountMasterController.DateWiseAccountMasterList(fromDate, toDate);
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.Type = passModel.Type;
                return View(reportModel);
            }
            else
            {
                return View();
            }
        }







        // Schedule/Notes (II)
        public ActionResult ScheduleNotes2()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "ScheduleNotes2");
            if (check == true)
            {
                TempData["passData_SelectAccountChartMasterList"] = accountChartController.AccountChartMasterList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }

        [HttpPost]
        public ActionResult ScheduleNotes2(ViewAccountReportModel model)
        {
            TempData["ViewAccountReportModel"] = model;
            return RedirectToAction("_GetScheduleNotes2");
        }

        public ActionResult _GetScheduleNotes2()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "ScheduleNotes2");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();
                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
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

                reportModel.AccountChartList = accountChartController.AccountChartList();
                reportModel.AccountChartMasterList = accountChartController.AccountChartMasterList();
                reportModel.LessThanEqualtoToDateWiseAccountMasterList = accountMasterController.LessThanEqualtoToDateWiseAccountMasterList(toDate);
                reportModel.DateWiseAccountMasterList = accountMasterController.DateWiseAccountMasterList(fromDate, toDate);
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.HeadCd = passModel.Headcd;

                return View(reportModel);
            }
            else
            {
                return View();
            }
        }






        // Cost Pool Wise Transaction-Details
        public ActionResult CostPoolTranDetails()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "CostPoolTranDetails");
            if (check == true)
            {
                TempData["passData_SelectAccountCostPoolList"] = costPoolController.AccountCostPoolList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }

        [HttpPost]
        public ActionResult CostPoolTranDetails(ViewAccountReportModel model)
        {
            TempData["ViewAccountReportModel"] = model;
            return RedirectToAction("_GetCostPoolTranDetails");
        }

        public ActionResult _GetCostPoolTranDetails()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "CostPoolTranDetails");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();
                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
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

                reportModel.AccountChartList = accountChartController.AccountChartList();
                reportModel.AccountCostPoolList = costPoolController.AccountCostPoolList();
                reportModel.DateWiseAccountMasterList = accountMasterController.DateWiseAccountMasterList(fromDate, toDate);
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.Type = passModel.Type;
                ViewBag.Costpid = passModel.Costpid;

                return View(reportModel);
            }
            else
            {
                return View();
            }
        }







        // Cost Pool Wise Transaction-Summary
        public ActionResult CostPoolTranSummary()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "CostPoolTranSummary");
            if (check == true)
            {
                TempData["passData_SelectAccountCostPoolList"] = costPoolController.AccountCostPoolList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }

        [HttpPost]
        public ActionResult CostPoolTranSummary(ViewAccountReportModel model)
        {
            TempData["ViewAccountReportModel"] = model;
            return RedirectToAction("_GetCostPoolTranSummary");
        }

        public ActionResult _GetCostPoolTranSummary()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "CostPoolTranSummary");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();
                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
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

                reportModel.AccountChartList = accountChartController.AccountChartList();
                reportModel.AccountCostPoolList = costPoolController.AccountCostPoolList();
                reportModel.DateWiseAccountMasterList = accountMasterController.DateWiseAccountMasterList(fromDate, toDate);
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.Type = passModel.Type;
                ViewBag.Costpid = passModel.Costpid;

                return View(reportModel);
            }
            else
            {
                return View();
            }
        }





        // Cost Pool Op. P/L Single
        public ActionResult CostPoolSingle()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "CostPoolSingle");
            if (check == true)
            {
                TempData["passData_SelectAccountCostPoolList"] = costPoolController.AccountCostPoolList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }

        [HttpPost]
        public ActionResult CostPoolSingle(ViewAccountReportModel model)
        {
            TempData["ViewAccountReportModel"] = model;
            return RedirectToAction("_GetCostPoolSingle");
        }

        public ActionResult _GetCostPoolSingle()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "CostPoolSingle");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();
                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
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

                reportModel.AccountChartList = accountChartController.AccountChartList();
                reportModel.AccountCostPoolList = costPoolController.AccountCostPoolList();
                reportModel.DateWiseAccountStransList = singleTransactionController.DateWiseList(fromDate, toDate);
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.Costpid = passModel.Costpid;

                return View(reportModel);
            }
            else
            {
                return View();
            }
        }









        // Cost Pool Operating P/L Group
        public ActionResult CostPoolGroup()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "CostPoolGroup");
            if (check == true)
            {
                TempData["passData_SelectAccountCostPoolMasterList"] = costPoolController.AccountCostPoolMasterList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Logout");
            }
        }

        [HttpPost]
        public ActionResult CostPoolGroup(ViewAccountReportModel model)
        {
            TempData["ViewAccountReportModel"] = model;
            return RedirectToAction("_GetCostPoolGroup");
        }

        public ActionResult _GetCostPoolGroup()
        {
            var check = PermissionCheck.ReportPermissionCheck(_loggedCompid, _loggedUserid, _loggedToken, "ReportAccount", "CostPoolGroup");
            if (check == true)
            {
                var reportModel = new ViewAccountListModel();
                ViewAccountReportModel passModel = (ViewAccountReportModel)TempData["ViewAccountReportModel"];
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

                reportModel.AccountChartList = accountChartController.AccountChartList();
                reportModel.AccountCostPoolMasterList = costPoolController.AccountCostPoolMasterList();
                reportModel.DateWiseAccountStransList = singleTransactionController.DateWiseList(fromDate, toDate);
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.Costcid = passModel.Costcid;

                return View(reportModel);
            }
            else
            {
                return View();
            }
        }






        //List of Cost Pool
        public ActionResult _GetListOfCostPool()
        {
            var reportModel = new ViewAccountListModel();
            reportModel.AccountCostPoolMasterList = costPoolController.AccountCostPoolMasterList();
            reportModel.AccountCostPoolList = costPoolController.AccountCostPoolList();
            return View(reportModel);
        }




        //List of Accounts Head
        public ActionResult _GetListOfAccountsHead()
        {
            var reportModel = new ViewAccountListModel();
            reportModel.AccountChartList = accountChartController.AccountChartList();
            reportModel.AccountChartMasterList = accountChartController.AccountChartMasterList();
            return View(reportModel);
        }
    }
}