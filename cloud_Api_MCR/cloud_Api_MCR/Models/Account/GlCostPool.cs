using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.Account
{
    [Table("GL_COSTPOOL")]
    public class GlCostPool
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 Compid { get; set; }            //--101

        [Key, Column(Order = 1)]
        public Int64 Costcid { get; set; }           //--101001

        [Key, Column(Order = 2)]
        public Int64 Costpid { get; set; }           //--1010010001
        public string Costpnm { get; set; }
        public string Costpsid { get; set; }
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