using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR
{
    [Table("MCR_BRANCH")]
    public class McrBranch
    {
        //Compid NUMBER(3),
        //Branchid NUMBER(5),
        //Branchnm VARCHAR(100),
        //Address VARCHAR(100),	
        //Contactno VARCHAR(50),
        //Remarks VARCHAR(100),
        //..
        //PRIMARY KEY(Compid, Branchid)

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 Compid { get; set; }
        
        [Key, Column(Order = 1)]
        public Int64 Branchid { get; set; }


        public String Branchnm { get; set; }
        public String Address { get; set; }
        public String Contactno { get; set; }
        public String Remarks { get; set; }
        

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