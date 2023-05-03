using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.Account_DTO
{
    public class ViewAccountListModel
    {

        //Account
        public IEnumerable<ViewGlAcchartMst> AccountChartMasterList { get; set; }
        public IEnumerable<ViewGlAcchart> AccountChartList { get; set; }
        public IEnumerable<ViewGlCostPmst> AccountCostPoolMasterList { get; set; }
        public IEnumerable<ViewGlCostPool> AccountCostPoolList { get; set; }

        public IEnumerable<ViewGlMaster> AccountMasterList { get; set; }
        public IEnumerable<ViewGlMaster> DateWiseAccountMasterList { get; set; }
        public IEnumerable<ViewGlMaster> LessThanFromDateWiseAccountMasterList { get; set; }
        public IEnumerable<ViewGlMaster> LessThanEqualtoToDateWiseAccountMasterList { get; set; }

        public IEnumerable<ViewGlStrans> AccountStransList { get; set; }
        public IEnumerable<ViewGlStrans> DateWiseAccountStransList { get; set; }

        public IEnumerable<ViewAccountReportModel> TrailBalanceReportsList { get; set; }
        public IEnumerable<ViewAccountReportModel> BalanceSheetReportsList { get; set; }
    }
}