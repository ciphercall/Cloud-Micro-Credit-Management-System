using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR_DTO
{
    public class ViewMcrMember
    {
        public Int64 Compid { get; set; }
        public Int64 Macgid { get; set; }
        public Int64 Memberid { get; set; }


        [Required(ErrorMessage = "This field is required!")]
        [StringLength(100, MinimumLength = 0)]
        public String Membernm { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public String Guardiannm { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public String Mothernm { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(100, MinimumLength = 0)]
        public String Addrpre { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(100, MinimumLength = 0)]
        public String Addrper { get; set; }

        [Required(ErrorMessage = "Mobile Number field can not be empty!")]
        [Display(Name = "Mobile")]
        [RegularExpression(@"^(8{2})([0-9]{11})", ErrorMessage = "Insert a valid phone Number like 8801900000000")]
        public string Mobno1 { get; set; }

        [Display(Name = "Mobile")]
        [RegularExpression(@"^(8{2})([0-9]{11})", ErrorMessage = "Insert a valid phone Number like 8801900000000")]
        public string Mobno2 { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Dob { get; set; }

        [StringLength(10, MinimumLength = 0)]
        public string Age { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(20, MinimumLength = 0)]
        public string Nationality { get; set; }

        [StringLength(20, MinimumLength = 0)]
        public string Nid { get; set; }

        [StringLength(10, MinimumLength = 0)]
        public string Religion { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(50, MinimumLength = 0)]
        public string Occupation { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string Refnm { get; set; }

        public Int64? Refmid { get; set; }

        [StringLength(30, MinimumLength = 0)]
        public string Refcno { get; set; }

        [StringLength(20, MinimumLength = 0)]
        public string Sharecno { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Sharecdt { get; set; }

        public Int64? Areaid { get; set; }
        public Int64? Branchid { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Acopdt { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Accldt { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(1, MinimumLength = 0)]
        public String Status { get; set; }



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