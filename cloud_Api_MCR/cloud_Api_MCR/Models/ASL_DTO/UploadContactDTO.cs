using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.ASL_DTO
{
    public class UploadContactDTO
    {
        public Int64 Id { get; set; }
        public Int64 CompId { get; set; }

        public Int64? FromGroupId { get; set; }
        public Int64? ToGroupId { get; set; }
        public string ToGroupNm { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string MobNo1 { get; set; }
        public string MobNo2{ get; set; }
        public string Address { get; set; }







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

        //public string Insert { get; set; }
        //public string Update { get; set; }
        //public string Delete { get; set; }


        public Int64 Count { get; set; }
        public Int64 GetChildDataForDeleteMasterCategory { get; set; } // its used for Delete Group(category) data before check this child data is hold or not.

        //Update group wise data
        public Int64 CheckPreviousData { get; set; }
        public Int64 CheckValidation { get; set; }
    }
}