using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR_DTO
{
    public class ViewMcrMTrans
    {
        public Int64 Compid { get; set; }

        [Required(ErrorMessage = "This field required!")]
        public string Transtp { get; set; }

        [Required(ErrorMessage = "This field required!")]
        public string Transmy { get; set; }
        public Int64 Transno { get; set; }
        public Int64 Transsl { get; set; }




        [Required(ErrorMessage = "This field required!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Transdt { get; set; }

        [Required(ErrorMessage = "This field required!")]
        public string Transfor { get; set; }

        [Required(ErrorMessage = "This field required!")]
        public string Transmode { get; set; }
        public Int64? Glheadid { get; set; }
        public string Schemeid { get; set; }
        public Int64? Memberid { get; set; }
        public Int64? Internid { get; set; }
        public string Schemeid2 { get; set; }
        public Int64? Memberid2 { get; set; }
        public Int64? Internid2 { get; set; }
        public decimal Amount { get; set; }
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



        //Create, Update view model purpose
        public string GlheadName { get; set; }
        public string MemberName { get; set; }
        public string MemberName2 { get; set; }



        //token field used for Angular Js Grid level data
        public string Token { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }

    }
}