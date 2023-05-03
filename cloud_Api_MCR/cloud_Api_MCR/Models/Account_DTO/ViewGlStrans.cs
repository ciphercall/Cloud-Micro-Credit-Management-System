using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.Account_DTO
{
    public class ViewGlStrans
    {
        public Int64 Compid { get; set; }    //--101
        [Required(ErrorMessage = "This field is required!")]
        public string Transtp { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public string Transmy { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public Int64 Transno { get; set; }   //--2015010001(YYYYMM0001)


        [Required(ErrorMessage = "This field is required!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Transdt { get; set; }
        public string Transfor { get; set; }
        public string Transmode { get; set; }
        public Int64? Costpid { get; set; }
        public Int64? Creditcd { get; set; }
        public Int64? Debitcd { get; set; }
        public string Chequeno { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Chequedt { get; set; }
        public string Remarks { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public decimal Amount { get; set; }
        public string Chqpayto { get; set; }





        public string Userpc { get; set; }
        public Int64? Insuserid { get; set; }
        public DateTime? Instime { get; set; }
        public string Insipno { get; set; }
        public string Insltude { get; set; }
        public Int64? Upduserid { get; set; }
        public DateTime? Updtime { get; set; }
        public string Updipno { get; set; }
        public string Updltude { get; set; }
    }
}