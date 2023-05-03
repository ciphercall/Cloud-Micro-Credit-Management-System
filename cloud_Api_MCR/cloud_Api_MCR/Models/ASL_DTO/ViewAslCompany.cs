using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace cloud_Api_MCR.Models.ASL_DTO
{
    public class ViewAslCompany
    {
        public Int64? Compid { get; set; }

        public Int64? Userid { get; set; }

        [Required(ErrorMessage = "Company Name can not be empty!")]
        [Remote("Check_Compnm", "Company", ErrorMessage = "Company Name already exists")]
        public string Compnm { get; set; }

        [Required(ErrorMessage = "Company Address can not be empty!")]
        public string Address { get; set; }
        public string Address2 { get; set; }

    
        [Required(ErrorMessage = "Contact No can not be empty!")]
        [Remote("Check_ContactNo", "Company", ErrorMessage = "Contact No already exists")]
        [RegularExpression(@"^(8{2})([0-9]{11})", ErrorMessage = "Insert a valid phone number like 8801711001100")]
        public string Contactno { get; set; }

        [Required(ErrorMessage = "Email address can not be empty!")]
        [Remote("Check_EmailId", "Company", ErrorMessage = "Email ID already exists")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Emailid { get; set; }
        
        public string Webid { get; set; }

        [Required(ErrorMessage = "Status can not be empty!")]
        public string Status { get; set; }

        

        [Remote("Check_EmailId_Promotional", "Company", ErrorMessage = "Promotional Email ID already exists")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Emailidp { get; set; }

        [DataType(DataType.Password)]
        public string Emailpwp { get; set; }

        //Sms Sender Name
        public string Smssendernm { get; set; }
        public string Smsidp { get; set; }

        [DataType(DataType.Password)]
        public string Smspwp { get; set; }


        
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


    }
}