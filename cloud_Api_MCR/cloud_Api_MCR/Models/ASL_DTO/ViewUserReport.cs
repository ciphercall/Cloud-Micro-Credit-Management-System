using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.ASL_DTO
{
    public class ViewUserReport
    {
        [Required(ErrorMessage = "From date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessage = "To Date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }





        //..........For Use only (GetUserLogPdfResult) passing List Model, before add this model(create list) from AslLOG table. 
        public Int64? UserId { get; set; }
        public Int64? Compid { get; set; }
        public string Usernm { get; set; }
        public string Optp { get; set; }
        public string LogType { get; set; }
        public string LogDate { get; set; }
        public string LogTime { get; set; }
        public string LogData { get; set; }
    }
}