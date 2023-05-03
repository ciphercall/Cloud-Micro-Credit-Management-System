using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR_DTO
{
    public class ViewMcrBranch
    {
        public Int64 Compid { get; set; }
        public Int64 Branchid { get; set; }

        [Required(ErrorMessage = "Branch name field required!")]
        [StringLength(100, MinimumLength = 1)]
        public String Branchnm { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public String Address { get; set; }

        [Required(ErrorMessage = "Contact name field required!")]
        [StringLength(50, MinimumLength = 1)]
        public String Contactno { get; set; }

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