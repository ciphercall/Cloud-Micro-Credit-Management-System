using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.Account_DTO
{
    public class ViewAccountReportModel
    {
        [Required(ErrorMessage = "From date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessage = "To Date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        public String Type { get; set; }


        public Int64 Accountcd { get; set; }
        public Int64? Creditcd { get; set; }    
        public Int64? Debitcd { get; set; }
        public Int64? Headcd { get; set; }
        public int Headtp { get; set; }        // --1:ASSET, 2:LIABILITY, 3:INCOME, 4:EXPENDITURE
        public string Headnm { get; set; }
        public string Accountnm { get; set; }
        public decimal Amount { get; set; }
        public decimal Debitamt { get; set; }
        public decimal Creditamt { get; set; }

        public Int64 Costpid { get; set; }         
        public string Costpnm { get; set; }
        public Int64 Costcid { get; set; }  
        public string Costcnm { get; set; }
    }
}