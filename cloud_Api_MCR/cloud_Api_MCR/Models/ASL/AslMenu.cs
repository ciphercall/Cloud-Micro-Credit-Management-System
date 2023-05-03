using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.ASL
{
    [Table("ASL_MENU")]
    public class AslMenu
    {
        //[Key, Column(Order = 0)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public Int64 Id { get; set; }

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Moduleid { get; set; }

        [Key, Column(Order = 1)]
        public string Menutp { get; set; }

        [Key, Column(Order = 2)]
        public string Menuid { get; set; }


        public Int64 Serial { get; set; }
        public string Menunm { get; set; }
        public string Actionname { get; set; }
        public string Controllername { get; set; }
     


        

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