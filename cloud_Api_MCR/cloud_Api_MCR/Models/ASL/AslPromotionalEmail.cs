using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.ASL
{
    [Table("ASL_PEMAIL")]
    public class AslPromotionalEmail
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }

        [Key, Column(Order = 1)]
        public Int64 Compid { get; set; }

        [Key, Column(Order = 2)]
        public Int64 Transyy { get; set; }//--2016

        [Key, Column(Order = 3)]
        public Int64 TransNo { get; set; }//--101160001



        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? TransDt { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string EmailId { get; set; }
        public string EmailSubject { get; set; }
        public string BodyMsg { get; set; }

        [StringLength(7, MinimumLength = 0)]
        public string Status { get; set; } //--PENDING/SENT

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SentTm { get; set; }




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