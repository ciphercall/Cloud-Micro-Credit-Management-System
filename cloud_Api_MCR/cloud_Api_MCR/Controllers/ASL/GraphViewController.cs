using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cloud_Api_MCR.Controllers.ASL
{
    public class GraphViewController : AppController
    {
        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken;

        public GraphViewController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                _loggedCompid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
                _loggedUserid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
                _loggedToken = Convert.ToString(System.Web.HttpContext.Current.Session["loggedToken"].ToString());
            }
            else
            {
                RedirectToAction("Index", "Logout");
            }
            ViewData["HighLight_Menu_DashBoard"] = "High Light DashBoard";
        }


        // GET: /GraphView/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LastOneDay()
        {
            return View();
        }
        public ActionResult Last7Day()
        {
            return View();
        }
        public ActionResult LastOneMonth()
        {
            return View();
        }
        public ActionResult LastOneYear()
        {
            return View();
        }

    }
}