using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR_DTO
{
    public class ViewMcrScheme
    {
        public Int64 Compid { get; set; }
        public Int64 Schemecd { get; set; }


        public String Schemeid { get; set; }

        [StringLength(7, MinimumLength = 0)]
        public String Schemetp { get; set; }
        public Int64 Glheadid { get; set; }
        public Int64 GlIncomehid { get; set; }
        public String Remarks { get; set; }




        public string Userpc { get; set; }
        public Int64? Insuserid { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Instime { get; set; }
        public string Insipno { get; set; }
        public string Insltude { get; set; }
        public Int64? Upduserid { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Updtime { get; set; }
        public string Updipno { get; set; }
        public string Updltude { get; set; }




        //token field used for Angular Js Grid level data
        public string Token { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }

        //Grid
        public String GlHeadName { get; set; }
        public String GlIncomeName { get; set; }
    }
}