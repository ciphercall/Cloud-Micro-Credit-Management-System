using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR
{
    [Table("MCR_COLLECT")]
    public class McrCollect
    {
        //Compid NUMBER(3),
        //Transdt DATE,
        //Transmy    VARCHAR(7),
        //Transno NUMBER(10),		--2015010001(YYYYMM0001)	
        //Transsl NUMBER(5),
        //Branchid NUMBER(5),
        //Fwid NUMBER(10),
        //Schemeid VARCHAR(10),							--Grid Start
        //Memberid NUMBER(10),  
        //Internid NUMBER(5),
        //Amount NUMBER(15,2),
        //Remarks VARCHAR(100),
        //..
        //PRIMARY KEY(Compid, Transmy, Transno, Transsl)


        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 Compid { get; set; }

        [Key, Column(Order = 1)]
        public String Transmy { get; set; }

        [Key, Column(Order = 2)]
        public Int64 Transno { get; set; }

        [Key, Column(Order = 3)]
        public Int64 Transsl { get; set; }



        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Transdt { get; set; }
        public Int64 Branchid { get; set; }       
        public Int64 Fwid { get; set; }
        public String Schemeid { get; set; }
        public Int64? Memberid { get; set; }
        public Int64? Internid { get; set; }
        public Decimal? Amount { get; set; }
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