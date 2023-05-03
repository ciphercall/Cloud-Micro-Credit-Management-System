using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.ASL_DTO
{
    public class MailDTO
    {
        public Int64? CompId { get; set; }
        public Int64? GroupId { get; set; }

        public string ToEmail { get; set; }
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Subject required!")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Body required!")]
        public string Body { get; set; }

        public string Color { get; set; }
        public string Language { get; set; }
        public string CompanyName { get; set; }


        public String Select { get; set; }
        public String SchemeId { get; set; }
        public Int64? AreaId { get; set; }



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