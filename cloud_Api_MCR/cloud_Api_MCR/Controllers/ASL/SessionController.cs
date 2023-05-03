using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cloud_Api_MCR.Controllers.ASL
{
    public class SessionController : Controller
    {
        // GET: Session
        public void Index()
        {
            try
            {
                Session["loggedToken"] = Convert.ToString(System.Web.HttpContext.Current.Session["loggedToken"]);
                Session["loggedCompID"] = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompID"]);
                Session["loggedUserID"] = Convert.ToString(System.Web.HttpContext.Current.Session["loggedUserID"]); ;
                Session["loggedUserType"] = Convert.ToString(System.Web.HttpContext.Current.Session["loggedUserType"]);
                Session["loggedUserName"] = Convert.ToString(System.Web.HttpContext.Current.Session["loggedUserName"]);
                Session["loggedUserStatus"] = Convert.ToString(System.Web.HttpContext.Current.Session["loggedUserStatus"]);
                Session["loggedCompanyStatus"] = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyStatus"]);
                Session["loggedCompanyName"] = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyName"]);
                Session["loggedCompanyAddress"] = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyAddress"]);
                Session["loggedCompanyAddress2"] = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyAddress2"]);
                Session["loggedCompanyContactno"] = Convert.ToString(System.Web.HttpContext.Current.Session["loggedCompanyContactno"]);
            }
            catch(Exception ex)
            {
                RedirectToAction("Index", "Logout");
            }
           
        }
    }
}