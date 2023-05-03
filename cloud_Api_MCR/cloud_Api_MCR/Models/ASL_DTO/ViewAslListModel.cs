using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cloud_Api_MCR.Models.ASL;

namespace cloud_Api_MCR.Models.ASL_DTO
{
    public class ViewAslListModel
    {
        //Asl
        public IEnumerable<ViewAslCompany> AslCompanyList { get; set; }
        public IEnumerable<ViewAslUserco> AslUsercoList { get; set; }
        public IEnumerable<ViewAslMenumst> AslMenumstList { get; set; }

    }
}