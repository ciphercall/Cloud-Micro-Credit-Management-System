using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.ASL_DTO
{
    public class PendingMailSmsDTO
    {
        public Int64? CompId { get; set; }

        [Required(ErrorMessage = "Body required!")]
        public string TransDt { get; set; }
        public string Color { get; set; }

        public Int64 Upduserid { get; set; }
        public string Updltude { get; set; }
    }
}