using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR_DTO
{
    public class ViewMcrMaster
    {
        public Int64 Compid { get; set; }
        public string Transtp { get; set; }
        public string Transmy { get; set; }
        public Int64 Transno { get; set; }
        public Int64 Transsl { get; set; }




        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Transdt { get; set; }
        public string Transdrcr { get; set; }
        public string Transfor { get; set; }
        public string Transmode { get; set; }
        public Int64? Memberid { get; set; }
        public string Schemeid { get; set; }
        public Int64? Internid { get; set; }
        public decimal Debitamt { get; set; }
        public decimal Creditamt { get; set; }
        public string Remarks { get; set; }
        public string Tableid { get; set; }




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