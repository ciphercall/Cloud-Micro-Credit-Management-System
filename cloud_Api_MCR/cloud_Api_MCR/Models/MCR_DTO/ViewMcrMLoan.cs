using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR_DTO
{
    public class ViewMcrMLoan
    {
        public Int64 Compid { get; set; }

        [Required(ErrorMessage = "This field required!")]
        public string Transmy { get; set; }
        public Int64 Transno { get; set; }


        [Required(ErrorMessage = "This field required!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Transdt { get; set; }

        [Required(ErrorMessage = "This field required!")]
        public string Schemeid { get; set; }

        [Required(ErrorMessage = "This field required!")]
        public Int64 Memberid { get; set; }

        [Required(ErrorMessage = "This field required!")]
        public Int64 Internid { get; set; }
        public decimal Loanamt { get; set; }
        public decimal Pchargert { get; set; }
        public decimal Pchargeamt { get; set; }
        public decimal Schargert { get; set; }
        public decimal Schargeamt { get; set; }

        [Required(ErrorMessage = "This field required!")]
        public decimal Disburseamt { get; set; }
        public decimal Riskfundrt { get; set; }
        public decimal Riskfundamt { get; set; }

        [Required(ErrorMessage = "This field required!")]
        public decimal Collectamt { get; set; }
        public decimal Interestrt { get; set; }
        public decimal Schemeiqty { get; set; }

        [Required(ErrorMessage = "This field required!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public String Schemeefdt { get; set; }

        [Required(ErrorMessage = "This field required!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public String Schemeetdt { get; set; }
        public string Remarks { get; set; }



        public string Userpc { get; set; }
        public Int64? Insuserid { get; set; }
        public DateTime? Instime { get; set; }
        public string Insipno { get; set; }
        public string Insltude { get; set; }
        public Int64? Upduserid { get; set; }
        public DateTime? Updtime { get; set; }
        public string Updipno { get; set; }
        public string Updltude { get; set; }


        //For Used Update purpose
        public String TransDate { get; set; }


        //Create, Update view model purpose
        public string MemberName { get; set; }
    }
}