using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace cloud_Api_MCR.Models.ASL_DTO
{
    public class ViewLoginModel
    {
        [Required(ErrorMessage = "User Name can not be empty!")]
        public string Loginid { get; set; }

        [Required(ErrorMessage = "Password can not be empty!")]
        public string Loginpw { get; set; }

        public string Deptnm { get; set; }
        public string Optp { get; set; }
        public Int64? Compid { get; set; }
        public Int64? Userid { get; set; }


        public string Logtime { get; set; }
        public string Logipno { get; set; }
        public string Logltude { get; set; }
        public string Logdata { get; set; }
        public string Userpc { get; set; }

    }
}
