using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.Models.Account_DTO
{
    public class ViewGlCostPmst
    {
        public Int64 Compid { get; set; }  //--101
        public Int64 Costcid { get; set; }  //--101001
        public string Costcnm { get; set; }



        public string Userpc { get; set; }
        public Int64? Insuserid { get; set; }
        public DateTime? Instime { get; set; }
        public string Insipno { get; set; }
        public string Insltude { get; set; }
        public Int64? Upduserid { get; set; }
        public DateTime? Updtime { get; set; }
        public string Updipno { get; set; }
        public string Updltude { get; set; }


        //token field used for Angular Js Grid level data
        public string Token { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }
}