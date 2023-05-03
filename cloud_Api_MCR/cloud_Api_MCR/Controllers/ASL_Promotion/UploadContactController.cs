using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.ASL;
using cloud_Api_MCR.Models.ASL_DTO;

namespace cloud_Api_MCR.Controllers.ASL_Promotion
{
    public class UploadContactController : AppController
    {
        private DatabaseDbContext db = new DatabaseDbContext();


        Int64 _loggedCompid, _loggedUserid;
        String _loggedToken, _loggedUserTp;


        public UploadContactController()
        {
            if (System.Web.HttpContext.Current.Session["loggedToken"] != null)
            {
                _loggedCompid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
                _loggedUserid = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"].ToString());
                _loggedToken = Convert.ToString(System.Web.HttpContext.Current.Session["loggedToken"].ToString());
                _loggedUserTp = Convert.ToString(System.Web.HttpContext.Current.Session["loggedUserType"].ToString());
                ViewData["HighLight_Menu_PromotionForm"] = "highlight menu";
            }
            else
            {
                RedirectToAction("Index", "Logout");
            }
        }




        public AslLog aslLog = new AslLog();

        // Delete ALL INFORMATION from Grid(Upload Contact data) TO Asl_lOG Database Table.
        public void Delete_Upload_LogData(AslPromotionalContact model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.Compid == model.Compid && n.Userid == model.Insuserid select n.Logslno).Max());
            if (maxSerialNo == 0)
            {
                aslLog.Logslno = Convert.ToInt64("1");
            }
            else
            {
                aslLog.Logslno = maxSerialNo + 1;
            }

            aslLog.Compid = Convert.ToInt64(model.Compid);
            aslLog.Userid = Convert.ToInt64(model.Insuserid);
            aslLog.Logtype = "DELETE";
            aslLog.Logslno = aslLog.Logslno;
            aslLog.Logdate = Convert.ToDateTime(date);
            aslLog.Logtime = Convert.ToString(time);
            aslLog.Logipno = UserlogAddress.IpAddress();
            aslLog.Logltude = model.Insltude;
            aslLog.Tableid = "ASL_PCONTACTS";

            string groupName = "";
            var findGroupName = (from m in db.AslPromotionalGroupsDbSet where m.Compid == model.Compid && m.GroupId == model.GroupId select m).ToList();
            foreach (var x in findGroupName)
            {
                groupName = x.GroupNm.ToString();
            }

            aslLog.Logdata = Convert.ToString("When upload File then duplicate data deleted. Group Name: " + groupName + ",\nName: " + model.Name + ",\nEmail: " + model.Email + ",\nMobile No 1: " + model.MobNo1 + ",\nMobile No 2: " + model.MobNo2 + ",\nAddress: " + model.Address + ".");
            aslLog.Userpc = UserlogAddress.UserPc(); 

            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }





        public AslDelete AslDelete = new AslDelete();

        // Delete ALL INFORMATION from TO ASL_DELETE Database Table.
        public void Delete_Upload_LogDelete(AslPromotionalContact model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslDeleteDbSet where n.Compid == model.Compid && n.Userid == model.Insuserid select n.Delslno).Max());
            if (maxSerialNo == 0)
            {
                AslDelete.Delslno = Convert.ToInt64("1");
            }
            else
            {
                AslDelete.Delslno = maxSerialNo + 1;
            }

            AslDelete.Compid = Convert.ToInt64(model.Compid);
            AslDelete.Userid = Convert.ToInt64(model.Insuserid);
            AslDelete.Delslno = AslDelete.Delslno;
            AslDelete.Deldate = Convert.ToDateTime(date);
            AslDelete.Deltime = Convert.ToString(time);
            AslDelete.Delipno = UserlogAddress.IpAddress();
            AslDelete.Delltude = model.Insltude;
            AslDelete.Tableid = "ASL_PCONTACTS";

            string groupName = "";
            var findGroupName = (from m in db.AslPromotionalGroupsDbSet where m.Compid == model.Compid && m.GroupId == model.GroupId select m).ToList();
            foreach (var x in findGroupName)
            {
                groupName = x.GroupNm.ToString();
            }
            AslDelete.Deldata = Convert.ToString("When upload File then duplicate data deleted. Group Name: " + groupName + ",\nName: " + model.Name + ",\nEmail: " + model.Email + ",\nMobile No 1: " + model.MobNo1 + ",\nMobile No 2: " + model.MobNo2 + ",\nAddress: " + model.Address + ".");
            AslDelete.Userpc = UserlogAddress.UserPc(); 

            db.AslDeleteDbSet.Add(AslDelete);
            db.SaveChanges();
        }




        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }



        private bool BdNumberValidate(string number)
        {
            try
            {
                if (number.Length > 13 || number.Length < 13)
                {
                    return false;
                }
                else
                {
                    string operatorCode = number.Substring(0, 5);
                    switch (operatorCode)
                    {
                        case "88018":
                        case "88017":
                        case "88019":
                        case "88016":
                        case "88011":
                        case "88015":
                            return true; //all of the operator in case is return true
                            break;
                        default:
                            return false; //other operator code return false
                    }
                }
            }
            catch
            {
                return false;
            }
        }









        // GET: /UploadDocument/
        public ActionResult UploadDocument()
        {
            return View();
        }

        // POST: /UploadDocument/
        [HttpPost]
        public ActionResult UploadDocument(HttpPostedFileBase file, AslPromotionalContact model)
        {
            DataSet ds = new DataSet();
            String fileLocation = "";
            Int64 count = 0;
            try
            {
                if (Request.Files["file"].ContentLength > 0)
                {

                    string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        fileLocation = string.Concat(Server.MapPath("~/Content/UploadFile/") + model.Compid + Request.Files["file"].FileName);
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                        Request.Files["file"].SaveAs(fileLocation);
                        string excelConnectionString = string.Empty;

                        //connection String for xls file format.
                        if (fileExtension == ".xls")
                        {
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        //connection String for xlsx file format.
                        else if (fileExtension == ".xlsx")
                        {
                            //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                        }
                        //Create Connection to Excel work book and add oledb namespace
                        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                        excelConnection.Open();
                        DataTable dt = new DataTable();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }

                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                        try
                        {
                            string query = string.Format("Select * from [{0}]", excelSheets[0]);
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                            {
                                dataAdapter.Fill(ds);
                            }

                            List<UploadContactDTO> errorUploadList = new List<UploadContactDTO>();

                            AslPromotionalContact pContacts = new AslPromotionalContact();
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                //string conn = ConfigurationManager.ConnectionStrings["Store_GL_DbContext"].ConnectionString;
                                //SqlConnection con = new SqlConnection(conn);


                                string email = ds.Tables[0].Rows[i][1].ToString();
                                string mobileNo1 = ds.Tables[0].Rows[i][2].ToString();
                                string mobileNo2 = ds.Tables[0].Rows[i][3].ToString();
                                //string insertQuery = "";
                                if (IsValidEmail(email) || BdNumberValidate(mobileNo1) || BdNumberValidate(mobileNo2))
                                {
                                    if (check_ExcelData(ds.Tables[0].Rows[i], model) == "True")
                                    {
                                    }
                                    else // check_ExcelData Exists in database (return Flase)
                                    {
                                        delete_ExcelData(ds.Tables[0].Rows[i], model);
                                    }

                                    pContacts.Compid = model.Compid;
                                    pContacts.GroupId = model.GroupId;
                                    pContacts.Name = ds.Tables[0].Rows[i][0].ToString();
                                    if (IsValidEmail(email))
                                    {
                                        pContacts.Email = ds.Tables[0].Rows[i][1].ToString();
                                    }
                                    if (BdNumberValidate(mobileNo1))
                                    {
                                        pContacts.MobNo1 = ds.Tables[0].Rows[i][2].ToString();
                                    }
                                    if (BdNumberValidate(mobileNo2))
                                    {
                                        pContacts.MobNo2 = ds.Tables[0].Rows[i][3].ToString();
                                    }
                                    pContacts.Address = ds.Tables[0].Rows[i][4].ToString();

                                    pContacts.Userpc = UserlogAddress.UserPc();
                                    pContacts.Insipno = UserlogAddress.IpAddress();
                                    pContacts.Instime = Convert.ToDateTime(UserlogAddress.Timezone(DateTime.Now));
                                    pContacts.Insuserid = model.Insuserid;
                                    pContacts.Insltude = Convert.ToString(model.Insltude);

                                    db.AslPromotionalContactDbSet.Add(pContacts);
                                    db.SaveChanges();
                                    count++;


                                    //insertQuery = "Insert into ASL_PCONTACTS(COMPID,GROUPID,NAME,EMAIL,MOBNO1,ADDRESS) Values('" +
                                    //    model.COMPID + "','" + model.GROUPID + "','" + ds.Tables[0].Rows[i][0].ToString() + "','" + ds.Tables[0].Rows[i][1].ToString() + "','" + ds.Tables[0].Rows[i][2].ToString() +
                                    //    "','" + ds.Tables[0].Rows[i][3].ToString() + "')";
                                    //con.Open();
                                    //SqlCommand cmd = new SqlCommand(insertQuery, con);
                                    //cmd.ExecuteNonQuery();
                                    //con.Close();
                                }
                                else
                                {
                                    errorUploadList.Add(new UploadContactDTO { Name = ds.Tables[0].Rows[i][0].ToString(), Email = ds.Tables[0].Rows[i][1].ToString(), MobNo1 = ds.Tables[0].Rows[i][2].ToString(), MobNo2 = ds.Tables[0].Rows[i][3].ToString(), Address = ds.Tables[0].Rows[i][4].ToString() });
                                    TempData["ErrorUploadList"] = errorUploadList;
                                }
                            }


                            excelConnection1.Close();
                            excelConnection.Close();
                            System.IO.FileInfo currentFile = new System.IO.FileInfo(fileLocation);
                            if (currentFile.Exists)
                            {
                                currentFile.Delete();
                            }

                            if (count != 0)
                            {
                                ViewBag.UploadMessage = "Upload successfully done! ";
                            }
                            else
                            {
                                ViewBag.UploadMessage = "Your upload file contains invalid data!!!";
                            }
                        }
                        catch
                        {
                            excelConnection1.Close();
                            excelConnection.Close();
                            System.IO.FileInfo currentFile = new System.IO.FileInfo(fileLocation);
                            if (currentFile.Exists)
                            {
                                currentFile.Delete();
                            }
                        }
                    }

                    //if (fileExtension.ToString().ToLower().Equals(".xml"))
                    //{
                    //    fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                    //    if (System.IO.File.Exists(fileLocation))
                    //    {
                    //        System.IO.File.Delete(fileLocation);
                    //    }

                    //    Request.Files["FileUpload"].SaveAs(fileLocation);
                    //    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    //    // DataSet ds = new DataSet();
                    //    ds.ReadXml(xmlreader);
                    //    xmlreader.Close();
                    //}                                       
                }

            }
            catch //(Exception ex)
            {
                //Response.Write(ex.Message);
                //System.IO.FileInfo currentFile = new System.IO.FileInfo(fileLocation);
                //if (currentFile.Exists)
                //{
                //    currentFile.Delete();
                //}
                ViewBag.UploadMessage = "Excel file is not in correct Format.";
            }
            return View();
        }


















        protected string check_ExcelData(DataRow d, AslPromotionalContact model)
        {
            string Result = "";

            var name = d.ItemArray[0].ToString();// d["NAME"].ToString();
            var email = d.ItemArray[1].ToString();//d["EMAIL"].ToString();
            var mobileNo1 = d.ItemArray[2].ToString();//d["MOBILENO1"].ToString();

            var get_asl_Contact =
                (from m in db.AslPromotionalContactDbSet
                 where m.Compid == model.Compid && m.GroupId == model.GroupId && m.Name == name && (m.Email == email || m.MobNo1 == mobileNo1)
                 select m).ToList();
            if (get_asl_Contact.Count != 0)
            {
                Result = "false";
            }
            else //count == 0
            {
                Result = "True";
            }

            return Result;
        }





        protected void delete_ExcelData(DataRow d, AslPromotionalContact model)
        {
            try
            {
                var name = d.ItemArray[0].ToString();// d["NAME"].ToString();
                var email = d.ItemArray[1].ToString();//d["EMAIL"].ToString();
                var mobileNo1 = d.ItemArray[2].ToString();//d["MOBILENO1"].ToString();
                var mobileNo2 = d.ItemArray[3].ToString();//d["MOBILENO2"].ToString();
                var address = d.ItemArray[4].ToString();//d["ADDRESS"].ToString();

                var remove = (from m in db.AslPromotionalContactDbSet
                              where m.Compid == model.Compid && m.GroupId == model.GroupId && m.Name == name && (m.Email == email || m.MobNo1 == mobileNo1)
                              select m).FirstOrDefault();
                db.AslPromotionalContactDbSet.Remove(remove);
                db.SaveChanges();

                model.Name = name;
                model.Email = email;
                model.MobNo1 = mobileNo1;
                model.MobNo2 = mobileNo2;
                model.Address = address;
                Delete_Upload_LogData(model);
                Delete_Upload_LogDelete(model);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }









        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}