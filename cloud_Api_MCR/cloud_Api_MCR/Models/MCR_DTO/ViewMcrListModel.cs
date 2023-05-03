using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cloud_Api_MCR.Models.Account_DTO;

namespace cloud_Api_MCR.Models.MCR_DTO
{
    public class ViewMcrListModel
    {

        //MCR
        public IEnumerable<ViewMcrBranch> BranchList { get; set; }
        public IEnumerable<ViewMcrArea> AreaList { get; set; }
        public IEnumerable<ViewMcrScheme> SchemeList { get; set; }
        public IEnumerable<ViewMcrMember> MembersList { get; set; }
        public IEnumerable<ViewMcrMScheme> MembersSchemeList { get; set; }
        public IEnumerable<ViewMcrMNominee> MembersNomineeList { get; set; }
        public IEnumerable<ViewMcrCollect> CollectionList { get; set; }
        public IEnumerable<ViewMcrMTrans> MembersTransactionList { get; set; }
        public IEnumerable<ViewMcrMLoan> MembersLoanList { get; set; }
        public IEnumerable<ViewMcrMaster> McrMasterList { get; set; }
        public IEnumerable<ViewMcrReportModel> McrReportsList { get; set; }


        //Account
        public IEnumerable<ViewGlAcchart> AccountChartList { get; set; }
        
    }
}