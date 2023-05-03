using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cloud_Api_MCR.Models.ASL
{
    [Table("ASL_USERCO")]
    public class AslUserco
    {
        //[Key, Column(Order = 0)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public Int64 Id { get; set; }

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 Compid { get; set; }

        [Key, Column(Order = 1)]
        public Int64 Userid { get; set; }
        
       
        public string Usernm { get; set; }
        public string Deptnm { get; set; }
        public string Optp { get; set; }
        public string Address { get; set; }
        public string Mobno { get; set; }
        public string Emailid { get; set; }
        public string Loginby { get; set; }
        public string Loginid { get; set; }
        public string Loginpw { get; set; }
        public string Timefr { get; set; }
        public string Timeto { get; set; }
        public string Status { get; set; }





        
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