using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.Account;
using cloud_Api_MCR.Models.ASL;
using cloud_Api_MCR.Models.ASL_DTO;

namespace cloud_Api_MCR.Controllers.Api_ASL
{
    public class ApiCompanyController : ApiController
    {
        private DatabaseDbContext db;

        public ApiCompanyController()
        {
            db = new DatabaseDbContext();
        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Company/Create")]
        public object Create(Int64 userCompanycode, Int64 usercode, [FromBody]ViewAslCompany passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            //Int64 userCompanycode = Convert.ToInt64(passModel.Compid);
            //Int64 usercode = Convert.ToInt64(passModel.Userid);
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    AslCompany model = new AslCompany();
                    Int64 maxCompanyId = Convert.ToInt64((from m in db.AslCompanyDbSet select m.Compid).DefaultIfEmpty().Max());
                    Int64 companyid = 0;
                    if (maxCompanyId == 0)
                    {
                        companyid = 101;
                    }
                    else //if (maxCompanyId!=0)
                    {
                        companyid = maxCompanyId + 1;
                    }

                    model.Compid = companyid;
                    model.Compnm = passModel.Compnm;
                    model.Address = passModel.Address;
                    model.Address2 = passModel.Address2;
                    model.Contactno = passModel.Contactno;
                    model.Emailid = passModel.Emailid;
                    model.Webid = passModel.Webid;
                    model.Status = passModel.Status;

                    model.Emailidp = passModel.Emailidp;
                    model.Emailpwp = passModel.Emailpwp;
                    model.Smssendernm = passModel.Smssendernm;
                    model.Smsidp = passModel.Smsidp;
                    model.Smspwp = passModel.Smspwp;

                    model.Userpc = UserlogAddress.UserPc();
                    model.Insuserid = usercode;
                    model.Instime = UserlogAddress.Timezone(DateTime.Now);
                    model.Insipno = UserlogAddress.IpAddress();
                    model.Insltude = passModel.Insltude;
                    db.AslCompanyDbSet.Add(model);
                    db.SaveChanges();

                    // Auto Insert in GlAccharMst table
                    for (int count = 1; count < 4; count++)
                    {
                        GlAcchartMst aGlAccharMst = new GlAcchartMst();
                        aGlAccharMst.Compid = companyid;
                        aGlAccharMst.Headtp = 1;
                        if (count == 1)
                        {
                            aGlAccharMst.Headcd = Convert.ToInt64(companyid + "1" + "01");
                            aGlAccharMst.Headnm = "CASH";
                        }
                        else if (count == 2)
                        {
                            aGlAccharMst.Headcd = Convert.ToInt64(companyid + "1" + "02");
                            aGlAccharMst.Headnm = "BANK";
                        }
                        else if (count == 3)
                        {
                            aGlAccharMst.Headcd = Convert.ToInt64(companyid + "1" + "03");
                            aGlAccharMst.Headnm = "PARTY";
                        }
                        aGlAccharMst.Userpc = UserlogAddress.UserPc();
                        aGlAccharMst.Insuserid = usercode;
                        aGlAccharMst.Instime = UserlogAddress.Timezone(DateTime.Now);
                        aGlAccharMst.Insipno = UserlogAddress.IpAddress();
                        aGlAccharMst.Insltude = passModel.Insltude;
                        db.GlAccharMstDbSet.Add(aGlAccharMst);
                        db.SaveChanges();
                    }
                    return new
                    {
                        data = model,
                        success = true,
                        message = "Save data Successfully."
                    };
                }
                catch (Exception ex)
                {
                    return new
                    {
                        data = ex,
                        success = true,
                        message = "Save failed!"
                    };
                }
            }
            return new
            {
                success = false,
                message = "Authorized not permitted."
            };
        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Company/Update")]
        public object Update(Int64 userCompanycode, Int64 usercode, [FromBody]ViewAslCompany passModel)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            //Int64 companyCode = Convert.ToInt64(passModel.Compid);
            //Int64 usercode = Convert.ToInt64(passModel.Userid);
            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                try
                {
                    var findCompanyInfo = (from m in db.AslCompanyDbSet where m.Compid == passModel.Compid select m).ToList();
                    if (findCompanyInfo.Count == 1)
                    {
                        foreach (var get in findCompanyInfo)
                        {
                            get.Compnm = passModel.Compnm;
                            get.Address = passModel.Address;
                            get.Address2 = passModel.Address2;
                            get.Contactno = passModel.Contactno;
                            get.Emailid = passModel.Emailid;
                            get.Webid = passModel.Webid;
                            get.Status = passModel.Status;

                            get.Emailidp = passModel.Emailidp;
                            get.Emailpwp = passModel.Emailpwp;
                            get.Smssendernm = passModel.Smssendernm;
                            get.Smsidp = passModel.Smsidp;
                            get.Smspwp = passModel.Smspwp;

                            get.Userpc = UserlogAddress.UserPc();
                            get.Upduserid = usercode;
                            get.Updtime = UserlogAddress.Timezone(DateTime.Now);
                            get.Updipno = UserlogAddress.IpAddress();
                            get.Updltude = passModel.Updltude;
                        }
                        db.SaveChanges();
                        return new
                        {
                            data = passModel,
                            success = true,
                            message = "Update data Successfully."
                        };
                    }
                    else
                    {
                        return new
                        {
                            success = true,
                            message = "Update failed!"
                        };
                    }

                }
                catch (Exception ex)
                {
                    return new
                    {
                        data = ex,
                        success = true,
                        message = "Update failed!"
                    };
                }
            }
            return new
            {
                success = false,
                message = "Authorized not permitted."
            };
        }





        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Company/CompanyList")]
        public object CompanyList(Int64 userCompanycode, Int64 usercode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            List<ViewAslUserco> nulllist = new List<ViewAslUserco>();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<AslCompany> list = new List<AslCompany>();
                list = (from m in db.AslCompanyDbSet select m).ToList();
                if (list.Count != 0)
                    return new
                    {
                        data = list,
                        success = true,
                        message = "Get data Successfully."
                    };
                else return new
                {
                    data = nulllist,
                    success = true,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = nulllist,
                success = false,
                message = "Authorized not permitted."
            };
        }





        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Company/CompanyInformation")]
        public object CompanyInformation(Int64 userCompanycode, Int64 usercode, Int64 selectCompanyCode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            List<ViewAslUserco> nulllist = new List<ViewAslUserco>();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token))
            {
                List<AslCompany> list = new List<AslCompany>();
                list = (from m in db.AslCompanyDbSet where m.Compid == selectCompanyCode select m).ToList();
                if (list.Count == 1)
                    return new
                    {
                        data = list,
                        success = true,
                        message = "Get data Successfully."
                    };
                else return new
                {
                    data = nulllist,
                    success = true,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = nulllist,
                success = false,
                message = "Authorized not permitted."
            };
        }





        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Company/Check_CompanyName")]
        public object Check_CompanyName(Int64 userCompanycode, Int64 usercode, String companyName)
        {
            var findCompanyName = (from m in db.AslCompanyDbSet where m.Compnm == companyName select m).ToList();
            if (findCompanyName.Count == 1)
            {
                return new
                {
                    data = "1",
                    success = true,
                    message = "Company Name already exists!"
                };
            }
            else
            {
                return new
                {
                    data = "0",
                    success = false,
                    message = "Company Name does not match!"
                };
            }
        }





        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Company/Check_ContactNo")]
        public object Check_ContactNo(Int64 userCompanycode, Int64 usercode, String contactNo)
        {
            var findContactNo = (from m in db.AslCompanyDbSet where m.Contactno == contactNo select m).ToList();
            if (findContactNo.Count == 1)
            {
                return new
                {
                    data = "1",
                    success = true,
                    message = "Contact No already exists!"
                };
            }
            else
            {
                return new
                {
                    data = "0",
                    success = false,
                    message = "Contact No does not match!"
                };
            }
        }




        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Company/Check_EmailId")]
        public object Check_EmailId(Int64 userCompanycode, Int64 usercode, String emailId)
        {
            var findEmailId = (from m in db.AslCompanyDbSet where m.Emailid == emailId select m).ToList();
            if (findEmailId.Count == 1)
            {
                return new
                {
                    data = "1",
                    success = true,
                    message = "Email ID already exists!"
                };
            }
            else
            {
                return new
                {
                    data = "0",
                    success = false,
                    message = "Email ID does not match!"
                };
            }
        }





        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Company/Check_Promotional_EmailId")]
        public object Check_Promotional_EmailId(Int64 userCompanycode, Int64 usercode, String promotionalEmailId)
        {
            var findPromotionalEmailId = (from m in db.AslCompanyDbSet where m.Emailidp == promotionalEmailId select m).ToList();
            if (findPromotionalEmailId.Count == 1)
            {
                return new
                {
                    data = "1",
                    success = true,
                    message = "Promotional Email ID already exists!"
                };
            }
            else
            {
                return new
                {
                    data = "0",
                    success = false,
                    message = "Promotional Email ID does not match!"
                };
            }
        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
