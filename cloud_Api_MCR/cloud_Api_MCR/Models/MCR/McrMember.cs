using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cloud_Api_MCR.Models.MCR
{
    [Table("MCR_MEMBER")]
    public class McrMember
    {
        //Compid NUMBER(3),
        //MACGID NUMBER(10),	Compid+1030000 (GL_ACCHART) - Fixed
        //Memberid NUMBER(10),	Compid+1030001, Compid+1030002	
        //MEMBERNM VARCHAR(100),	
        //Guardiannm VARCHAR(100),	
        //MOTHERNM VARCHAR(100),	
        //ADDRPRE VARCHAR(100),	
        //ADDRPER VARCHAR(100),	
        //MOBNO1 VARCHAR(11),		
        //MOBNO2 VARCHAR(11),	
        //DOB DATE,
        //Age VARCHAR(10),	
        //NATIONALITY VARCHAR(20),	
        //NID VARCHAR(20),	
        //RELIGION VARCHAR(10),	
        //OCCUPATION VARCHAR(50),	
        //REFNM    VARCHAR(100),	
        //REFMID    NUMBER(10), --Reference Member ID (from this table)
        //REFCNO   VARCHAR(30),	
        //SHARECNO VARCHAR(20),	 --Share Certificate No
        //SHARECDT DATE,     --Share Certificate Date
        //Areaid     NUMBER(7),
        //Branchid NUMBER(5),
        //ACOPDT     DATE,   --A/C Opening Date
        //ACCLDT     DATE,   --A/C Closing Date
        //Status CHAR(1),
        //.. 
        //PRIMARY KEY(Compid, MACGID, Memberid)


        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 Compid { get; set; }

        [Key, Column(Order = 1)]
        public Int64 Macgid { get; set; }

        [Key, Column(Order = 2)]
        public Int64 Memberid { get; set; }



        
        public String Membernm { get; set; }
        public String Guardiannm { get; set; }
        public String Mothernm { get; set; }
        public String Addrpre { get; set; }
        public String Addrper { get; set; }
        public string Mobno1 { get; set; }
        public string Mobno2 { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Dob { get; set; }
        public string Age { get; set; }
        public string Nationality { get; set; }
        public string Nid { get; set; }
        public string Religion { get; set; }
        public string Occupation { get; set; }
        public string Refnm { get; set; }
        public Int64? Refmid { get; set; }
        public string Refcno { get; set; }
        public string Sharecno { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Sharecdt { get; set; }
        public Int64? Areaid { get; set; }
        public Int64? Branchid { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Acopdt { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Accldt { get; set; }
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