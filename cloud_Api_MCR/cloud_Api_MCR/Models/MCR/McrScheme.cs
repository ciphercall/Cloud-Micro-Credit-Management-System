using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR
{
    [Table("MCR_SCHEME")]
    public class McrScheme
    {
        //Compid     NUMBER(3),
        //Schemeid   VARHCHAR(10),
        // Schemecd NUMBER(5),
        // Schemetp VARCHAR(7),	--DEPOSIT,LOAN
        // Glheadid NUMBER(10),
        // GlIncomehid NUMBER(10),
        // Remarks VARCHAR(100),
        // ..
        // PRIMARY KEY(Compid, Schemeid)


        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 Compid { get; set; }

        [Key, Column(Order = 1)]
        public Int64 Schemecd { get; set; }

        public String Schemeid { get; set; }
        public String Schemetp { get; set; }
        public Int64 Glheadid { get; set; }
        public Int64 GlIncomehid { get; set; }
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