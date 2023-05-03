using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR
{
    [Table("MCR_AREA")]
    public class McrArea
    {
        //Compid NUMBER(3),
        //Branchid NUMBER(5),
        //Areaid NUMBER(7),
        //Areanm VARCHAR(50),	
        //Authpnm VARCHAR(100),		
        //Fwid NUMBER(10),    -- from GL Account Chart table
        //Remarks VARCHAR(100),
        //..
        //PRIMARY KEY(Compid, Branchid, Areaid)

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 Compid { get; set; }

        [Key, Column(Order = 1)]
        public Int64 Branchid { get; set; }

        [Key, Column(Order = 2)]
        public Int64 Areaid { get; set; }



        public String Areanm { get; set; }
        public String Authpnm { get; set; }
        public Int64 Fwid { get; set; }
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