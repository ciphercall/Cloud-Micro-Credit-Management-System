using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR_DTO
{
    public class ViewMcrReportModel
    {
        [Required(ErrorMessage = "From date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessage = "To Date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
        
        public String  Type { get; set; }
        public String Status { get; set; }

        //User for Mcr Report 
        public Int64 Compid { get; set; }
        public Int64 Memberid { get; set; }
        public String Membernm { get; set; }
        public Int64 Areaid { get; set; }
        public String AreaName { get; set; }
        public Int64 Internid { get; set; }
        public String Schemeid { get; set; }
        public Int64 Fwid { get; set; }
        public String Schemetp { get; set; }
        public Decimal Debitamt { get; set; }
        public Decimal Creditamt { get; set; }
        public Decimal Opening { get; set; }
        public Decimal Closing { get; set; }
        public Decimal Collection { get; set; }
        public Decimal Payment { get; set; }
    }
}