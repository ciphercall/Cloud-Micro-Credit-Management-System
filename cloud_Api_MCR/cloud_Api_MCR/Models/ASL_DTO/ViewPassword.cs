using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cloud_Api_MCR.Models.ASL_DTO
{
    public class ViewPassword
    {

        [Required(ErrorMessage = "Enter Old Password!")]
        [Remote("Check_Password", "Password", ErrorMessage = "Your old password does not match!")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Enter New Password!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Enter Confirmed Password!")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The New password and confirmation password do not match!")]
        [DataType(DataType.Password)]
        public string ConfirmedPassword { get; set; }


        public Int64? Compid { get; set; }
        public Int64? Userid { get; set; }
    }
}