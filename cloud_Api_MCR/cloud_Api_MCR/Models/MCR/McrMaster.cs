using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR
{
    [Table("MCR_MASTER")]
    public class McrMaster
    {
        //COMPID NUMBER(3),  	--101
        //TRANSTP VARCHAR2(4),
        //TRANSDT DATE,
        //TRANSMY            VARCHAR2(6),
        //TRANSNO NUMBER(10),		--2015010001(YYYYMM0001)
        //TRANSSL NUMBER(5),		--10001,20001
        //TRANSDRCR VARCHAR2(6), 
        //TRANSFOR VARCHAR2(8),	
        //TRANSMODE VARCHAR2(20),
        //MEMBERID NUMBER(10),  
        //SCHEMEID VARHCHAR(10),	
        //INTERNID NUMBER(5),
        //DEBITAMT NUMBER(15,2) DEFAULT 0, 
        //CREDITAMT NUMBER(15,2) DEFAULT 0, 
        //REMARKS VARCHAR2(200),
        //TABLEID VARCHAR2(15),
        //..
        //..
        //PRIMARY KEY(COMPID, TRANSTP, TRANSMY, TRANSNO, TRANSSL)


        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 Compid { get; set; }

        [Key, Column(Order = 1)]
        public string Transtp { get; set; }

        [Key, Column(Order = 2)]
        public string Transmy { get; set; }

        [Key, Column(Order = 3)]
        public Int64 Transno { get; set; }

        [Key, Column(Order = 4)]
        public Int64 Transsl { get; set; }


        

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Transdt { get; set; }
        public string Transdrcr { get; set; }
        public string Transfor { get; set; }
        public string Transmode { get; set; }
        public Int64? Memberid { get; set; }
        public string Schemeid { get; set; }
        public Int64? Internid { get; set; }
        public decimal Debitamt { get; set; }
        public decimal Creditamt { get; set; }
        public string Remarks { get; set; }
        public string Tableid { get; set; }


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