using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR_DTO
{
    public class ViewMcrMNominee
    {
        public Int64 Compid { get; set; }
        public Int64 Memberid { get; set; }
        public Int64 Nomineeid { get; set; }



        [StringLength(100, MinimumLength = 0)]
        public String Nomineenm { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public String Guardiannm { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public String Mothernm { get; set; }

        [StringLength(10, MinimumLength = 0)]
        public String Age { get; set; }

        [StringLength(20, MinimumLength = 0)]
        public String Relation { get; set; }

        public Decimal? Npcnt { get; set; }

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
    }
}