using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR_DTO
{
    public class ViewMcrArea
    {
        public Int64 Compid { get; set; }
        public Int64 Branchid { get; set; }
        public Int64 Areaid { get; set; }

        [StringLength(50, MinimumLength = 0)]
        public String Areanm { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public String Authpnm { get; set; }

        public Int64 Fwid { get; set; }

        [StringLength(100, MinimumLength = 0)]
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
        public String FieldWorkersName { get; set; }
    }
}