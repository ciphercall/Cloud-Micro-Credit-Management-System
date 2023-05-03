using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.ASL
{
    [Table("ASL_LOG")]
    public class AslLog
    {
        [Key]
        public Int64 Id { get; set; }
        public Int64 Compid { get; set; }
        public Int64 Userid { get; set; }
        public string Logtype { get; set; } //CREATE,UPDATE,DELETE,LOGIN,LOGOUT
        public Int64 Logslno { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Logdate { get; set; }
        public string Logtime { get; set; }
        public string Logipno { get; set; }
        public string Logltude { get; set; }
        public string Tableid { get; set; }
        public string Logdata { get; set; }
        public string Userpc { get; set; }

    }
}