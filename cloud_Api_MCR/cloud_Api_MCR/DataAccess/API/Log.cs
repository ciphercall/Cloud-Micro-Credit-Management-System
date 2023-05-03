using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.ASL;

namespace cloud_Api_MCR.DataAccess.API
{
    public static class Log
    {
        // CREATE, UPDATE, DELETE LOG Data Save in to ASL_LOG Table
        public static String SaveLog(AslLog model)
        {
            try
            {
                DatabaseDbContext db = new DatabaseDbContext();

                var date = Convert.ToString(UserlogAddress.Timezone(DateTime.Now).ToString("dd-MMM-yyyy"));
                var time = Convert.ToString(UserlogAddress.Timezone(DateTime.Now).ToString("hh:mm:ss tt"));

                var findMaxSerialNo = (from n in db.AslLogDbSet where n.Compid == model.Compid && n.Userid == model.Userid select n.Logslno).DefaultIfEmpty().Max();
                Int64 maxSerialNo = Convert.ToInt64(findMaxSerialNo);
                if (maxSerialNo == 0)
                {
                    model.Logslno = Convert.ToInt64("1");
                }
                else
                {
                    model.Logslno = maxSerialNo + 1;
                }

                model.Compid = Convert.ToInt64(model.Compid);
                model.Userid = Convert.ToInt64(model.Userid);
                model.Logtype = model.Logtype;
                model.Logslno = model.Logslno;
                model.Logdate = Convert.ToDateTime(date);
                model.Logtime = Convert.ToString(time);
                model.Logipno = UserlogAddress.IpAddress();
                model.Logltude = model.Logltude;
                model.Logdata = model.Logdata;
                model.Tableid = model.Tableid;
                model.Userpc = UserlogAddress.UserPc();
                db.AslLogDbSet.Add(model);
                db.SaveChanges();
                return "True";
            }
            catch (Exception ex)
            {
                return "False";
            }
        }



        // Delete LOG Data Save in to ASL_DELETE Table
        public static String DeleteLog(AslDelete model)
        {
            try
            {
                DatabaseDbContext db = new DatabaseDbContext();

                var date = Convert.ToString(UserlogAddress.Timezone(DateTime.Now).ToString("dd-MMM-yyyy"));
                var time = Convert.ToString(UserlogAddress.Timezone(DateTime.Now).ToString("hh:mm:ss tt"));

                Int64 maxSerialNo = Convert.ToInt64((from n in db.AslDeleteDbSet where n.Compid == model.Compid && n.Userid == model.Userid select n.Delslno).DefaultIfEmpty().Max());
                if (maxSerialNo == 0)
                {
                    model.Delslno = Convert.ToInt64("1");
                }
                else
                {
                    model.Delslno = maxSerialNo + 1;
                }

                model.Compid = Convert.ToInt64(model.Compid);
                model.Userid = Convert.ToInt64(model.Userid);
                model.Delslno = model.Delslno;
                model.Deldate = Convert.ToDateTime(date);
                model.Deltime = Convert.ToString(time);
                model.Delipno = UserlogAddress.IpAddress();
                model.Delltude = model.Delltude;
                model.Deldata = model.Deldata;
                model.Tableid = model.Tableid;
                model.Userpc = UserlogAddress.UserPc();
                db.AslDeleteDbSet.Add(model);
                db.SaveChanges();
                db.SaveChanges();
                return "True";
            }
            catch (Exception ex)
            {
                return "False";
            }
        }

    }
}