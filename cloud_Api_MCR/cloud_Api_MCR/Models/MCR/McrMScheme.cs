using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR
{

    [Table("MCR_MSCHEME")]
    public class McrMScheme
    {
        //Compid NUMBER(3),
        //Memberid NUMBER(10),  --Compid+1030001, Compid+1030002	
        //Schemesl NUMBER(12),   --Memberid +01 ,Memberid +02
        //Schemeid VARHCHAR(10),  --Scheme
        //Internid NUMBER(5),   -- Internal ID
        //SCHEMEEFDT DATE,          -Effect From
        //SCHEMEETDT DATE,          -Effect To
        //Remarks VARHCHAR(100),
        //Status CHAR(1),
        //.. 
        //PRIMARY KEY(Compid, Memberid, Schemesl)

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 Compid { get; set; }

        [Key, Column(Order = 1)]
        public Int64 Memberid { get; set; }

        [Key, Column(Order = 2)]
        public Int64 Schemesl { get; set; }
        

        public String Schemeid { get; set; }
        public Int64 Internid { get; set; }
        public DateTime? Schemeefdt { get; set; }
        public DateTime? Schemeetdt { get; set; }
        public String Remarks { get; set; }
        public String Status { get; set; }



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