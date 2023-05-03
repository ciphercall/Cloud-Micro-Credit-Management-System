using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR
{
    [Table("MCR_MNOMINEE")]
    public class McrMNominee
    {
        //Compid NUMBER(3),
        //Memberid NUMBER(10),	--1011030001
        //Nomineeid NUMBER(11), 	--10110300011, 10110300012
        //Nomineenm VARCHAR(100),
        //Guardiannm VARCHAR(100),
        //MOTHERNM VARCHAR(100),
        //Age VARCHAR(10),	
        //RELATION VARCHAR(20),	
        //NPCNT NUMBER(5,2),
        //Remarks VARCHAR(100),	
        //.. 
        //PRIMARY KEY(Compid, Memberid, Nomineeid)


        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 Compid { get; set; }

        [Key, Column(Order = 1)]
        public Int64 Memberid { get; set; }

        [Key, Column(Order = 2)]
        public Int64 Nomineeid { get; set; }


        
        public String Nomineenm { get; set; }
        public String Guardiannm { get; set; }
        public String Mothernm { get; set; }
        public String Age { get; set; }
        public String Relation { get; set; }
        public Decimal? Npcnt { get; set; }
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