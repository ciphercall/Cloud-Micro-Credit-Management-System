using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cloud_Api_MCR.Models.ASL_DTO
{
    public class ViewAslUserco
    {
        public Int64? Compid { get; set; }
        public Int64? Userid { get; set; }
        



        [Required(ErrorMessage = "User Name can not be empty!")]
        public string Usernm { get; set; }
        
        public string Deptnm { get; set; }

        [Required(ErrorMessage = "Operation Type field can not be empty!")]
        public string Optp { get; set; }
        
        [Required(ErrorMessage = "User Address can not be empty!")]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "Mobile Number field can not be empty!")]
        [Remote("Check_MobileNo", "User", ErrorMessage = "User Mobile number already exists!")]
        [RegularExpression(@"^(8{2})([0-9]{11})", ErrorMessage = "Insert a valid phone Number like 8801900000000")]
        public string Mobno { get; set; }

        [Remote("Check_UserEmail", "User", ErrorMessage = "User Email address already exists!")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Emailid { get; set; }

        [Required(ErrorMessage = "Login By Email/MobileNO field can not be empty!")]
        public string Loginby { get; set; }
        
        [Required(ErrorMessage = "Login ID can not be empty!")]
        [Remote("Check_UserLogIn", "User", ErrorMessage = "User Login ID already exists!")]
        public string Loginid { get; set; }
        
        [Required(ErrorMessage = "Login Password Field can not be empty!")]
        [DataType(DataType.Password)]
        public string Loginpw { get; set; }
        
        [Required(ErrorMessage = "Starting Time can not be empty!")]
        public string Timefr { get; set; }
        
        [Required(ErrorMessage = "Ending Time can not be empty!")]
        public string Timeto { get; set; }
        
        [Required(ErrorMessage = "Status can not be empty!")]
        public string Status { get; set; }
        


        
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




        //User for view purpose
        public string Compnm { get; set; }
        public string StatusPermission { get; set; }
        public string InsertPermission { get; set; }
        public string UpdatePermission { get; set; }
        public string DeletePermission { get; set; }

    }
}