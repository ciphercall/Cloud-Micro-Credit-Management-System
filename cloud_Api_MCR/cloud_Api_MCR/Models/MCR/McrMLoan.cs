using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.MCR
{
    [Table("MCR_MLOAN")]
    public class McrMLoan
    {
        //COMPID NUMBER(3),  	
        //TRANSDT DATE,
        //TRANSMY        VARCHAR2(6),
        //TRANSNO NUMBER(10),		--2015010001(YYYYMM0001)
        //SCHEMEID VARHCHAR(10),	
        //MEMBERID NUMBER(10),  
        //INTERNID NUMBER(5),
        //LOANAMT NUMBER(15,2),    
        //PCHARGERT NUMBER(15,2),  -Profit Charge(%)
        //PCHARGEAMT NUMBER(15,2),  -Profit Charge
        //SCHARGERT NUMBER(15,2),  -Service Charge(%)
        //SCHARGEAMT NUMBER(15,2),  -Service Charge
        //DISBURSEAMT NUMBER(15,2),  -Disburse Amount
        //RISKFUNDRT NUMBER(15,2),  -Risk Fund(%)
        //RISKFUNDAMT NUMBER(15,2),  -Risk Fund
        //COLLECTAMT NUMBER(15,2),  -Collect Amount
        //INTERESTRT NUMBER(15,2),  -Interest(%)
        //SCHEMEIQTY NUMBER(15,2),  -Scheme Qty
        //SCHEMEEFDT DATE,          -Effect From
        //SCHEMEETDT DATE,          -Effect To
        //REMARKS VARCHAR2(100),
        //..
        //PRIMARY KEY(COMPID, TRANSMY, TRANSNO)


        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 Compid { get; set; }

        [Key, Column(Order = 1)]
        public string Transmy { get; set; }

        [Key, Column(Order = 2)]
        public Int64 Transno { get; set; }



        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Transdt { get; set; }
        public string Schemeid { get; set; }
        public Int64 Memberid { get; set; }
        public Int64 Internid { get; set; }
        public decimal Loanamt { get; set; }
        public decimal Pchargert { get; set; }
        public decimal Pchargeamt { get; set; }
        public decimal Schargert { get; set; }
        public decimal Schargeamt { get; set; }
        public decimal Disburseamt { get; set; }
        public decimal Riskfundrt { get; set; }
        public decimal Riskfundamt { get; set; }
        public decimal Collectamt { get; set; }
        public decimal Interestrt { get; set; }
        public decimal Schemeiqty { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Schemeefdt { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Schemeetdt { get; set; }
        public string Remarks { get; set; }



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