using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.ASL_DTO
{
    public class UploadGroupDTO
    {
        public Int64 Id { get; set; }
        public Int64 Compid { get; set; }
        public Int64? GroupId { get; set; }
        public string GroupNm { get; set; }



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




        public Int64 Count { get; set; }
        public Int64 GetChildDataForDeleteMasterCategory { get; set; } // its used for Delete Group(category) data before check this child data is hold or not.
  
    }
}