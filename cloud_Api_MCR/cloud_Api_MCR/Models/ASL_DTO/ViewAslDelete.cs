using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.ASL_DTO
{
    public class ViewAslDelete
    {
        public Int64? Compid { get; set; }
        public Int64? Userid { get; set; }
        public Int64? Delslno { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Deldate { get; set; }
        public string Deltime { get; set; }
        public string Delipno { get; set; }
        public string Delltude { get; set; }
        public string Tableid { get; set; }
        public string Deldata { get; set; }
        public string Userpc { get; set; }
    }
}