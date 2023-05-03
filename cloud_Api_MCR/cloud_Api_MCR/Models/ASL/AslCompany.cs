using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace cloud_Api_MCR.Models.ASL
{
    [Table("ASL_COMPANY")]
    public class AslCompany
    {
        //[Key, Column(Order = 0)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public Int64 Id { get; set; }

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 Compid { get; set; }

        
        public string Compnm { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Contactno { get; set; }
        public string Emailid { get; set; }
        public string Webid { get; set; }
        public string Status { get; set; }
        


        public string Emailidp { get; set; }
        public string Emailpwp { get; set; }
        public string Smssendernm { get; set; }
        public string Smsidp { get; set; }
        public string Smspwp { get; set; }
        



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