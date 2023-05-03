﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.Account
{

    [Table("GL_MASTER")]
    public class GlMaster
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 Compid { get; set; }    //--101

        [Key, Column(Order = 1)]
        public string Transtp { get; set; }
     
        [Key, Column(Order = 2)]
        public string Transmy { get; set; }

        [Key, Column(Order = 3)]
        public Int64 Transno { get; set; }   //--2015010001(YYYYMM0001)

        [Key, Column(Order = 4)]
        public Int64 Transsl { get; set; }  //--10001,20001



        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Transdt { get; set; }
        public string Transdrcr { get; set; }
        public string Transfor { get; set; }
        public string Transmode { get; set; }
        public Int64? Costpid { get; set; }     //--1010010001
        public Int64? Creditcd { get; set; }    //--1011010001,1011020001
        public Int64? Debitcd { get; set; }    //--1011010001,1011020001
        public string Chequeno { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Chequedt { get; set; }
        public string Remarks { get; set; }
        public decimal? Debitamt { get; set; }
        public decimal? Creditamt { get; set; }
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