using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.ASL_DTO
{
    public class ViewAslMenumst
    {
        public Int64? Compid { get; set; }
        public Int64? Userid { get; set; }

        public string Moduleid { get; set; }

        [Required(ErrorMessage = "Module Name can not be empty!")]
        public string Modulenm { get; set; }


   
        public string Userpc { get; set; }
        public Int64? Insuserid { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Instime { get; set; }
        
        public string Insipno { get; set; }
        public string Insltude { get; set; }
        public Int64? Upduserid { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Updtime { get; set; }
        public string Updipno { get; set; }
        public string Updltude { get; set; }



        //token field used for Angular Js Grid level data
        public string Token { get; set; }

    }
}