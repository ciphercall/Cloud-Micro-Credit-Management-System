using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.Account;
using cloud_Api_MCR.Models.Account_DTO;

namespace cloud_Api_MCR.DataAccess.API.AccountProcess
{
    public static class GlStransProcess
    {
        // Only For Account Process
        public static string Process(Int64 compid, Int64 userid, ViewGlMaster passModel)
        {
            DatabaseDbContext db = new DatabaseDbContext();
            try
            {
                var forremovedata = (from l in db.GlMasterDbSet
                                     where l.Compid == compid && l.Transdt == passModel.Transdt
                                     select l).ToList();
                foreach (var get in forremovedata)
                {
                    if (get.Tableid == "GL_STRANS")
                    {
                        db.GlMasterDbSet.Remove(get);
                    }
                }
                db.SaveChanges();


                //GL Process for GL_STRANS
                var checkDateGlstrans = (from n in db.GlStransDbSet where n.Transdt == passModel.Transdt && n.Compid == compid select n).OrderBy(x => x.Transno).ToList();

                if (checkDateGlstrans.Count != 0)
                {
                    foreach (var x in checkDateGlstrans)
                    {
                        GlMaster model = new GlMaster();
                        GlMaster model2 = new GlMaster();

                        if (x.Transtp == "MREC")
                        {
                            Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                where a.Compid == compid && a.Transtp == x.Transtp
                                                                select a.Transsl).DefaultIfEmpty().Max());

                            if (maxSlCheck == 0)
                            {
                                model.Transsl = 10001;

                                model.Transdt = x.Transdt;
                                model.Compid = x.Compid;
                                model.Transtp = x.Transtp;
                                model.Transmy = x.Transmy;
                                model.Transno = x.Transno;
                                model.Transfor = x.Transfor;
                                model.Transmode = x.Transmode;
                                model.Costpid = x.Costpid;
                                model.Debitcd = x.Debitcd;
                                model.Creditcd = x.Creditcd;
                                model.Chequeno = x.Chequeno;
                                model.Chequedt = x.Chequedt;
                                model.Remarks = x.Remarks;

                                model.Transdrcr = "DEBIT";
                                model.Tableid = "GL_STRANS";

                                model.Debitamt = x.Amount;
                                model.Creditamt = 0;

                                model.Userpc = UserlogAddress.UserPc();
                                model.Insuserid = userid;
                                model.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model.Insipno = UserlogAddress.IpAddress();
                                model.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model);
                                db.SaveChanges();

                                model2.Transsl = 10002;

                                model2.Transdt = x.Transdt;
                                model2.Compid = x.Compid;
                                model2.Transtp = x.Transtp;
                                model2.Transmy = x.Transmy;
                                model2.Transno = x.Transno;
                                model2.Transfor = x.Transfor;
                                model2.Transmode = x.Transmode;
                                model2.Costpid = x.Costpid;
                                model2.Debitcd = x.Creditcd;
                                model2.Creditcd = x.Debitcd;
                                model2.Chequeno = x.Chequeno;
                                model2.Chequedt = x.Chequedt;
                                model2.Remarks = x.Remarks;

                                model2.Transdrcr = "CREDIT";
                                model2.Tableid = "GL_STRANS";

                                model2.Debitamt = 0;
                                model2.Creditamt = x.Amount;

                                model2.Userpc = UserlogAddress.UserPc();
                                model2.Insuserid = userid;
                                model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model2.Insipno = UserlogAddress.IpAddress();
                                model2.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model2);
                                db.SaveChanges();

                            }
                            else
                            {
                                model.Transsl = maxSlCheck + 1;

                                model.Transdt = x.Transdt;
                                model.Compid = x.Compid;
                                model.Transtp = x.Transtp;
                                model.Transmy = x.Transmy;
                                model.Transno = x.Transno;
                                model.Transfor = x.Transfor;
                                model.Transmode = x.Transmode;
                                model.Costpid = x.Costpid;
                                model.Debitcd = x.Debitcd;
                                model.Creditcd = x.Creditcd;
                                model.Chequeno = x.Chequeno;
                                model.Chequedt = x.Chequedt;
                                model.Remarks = x.Remarks;

                                model.Transdrcr = "DEBIT";
                                model.Tableid = "GL_STRANS";

                                model.Debitamt = x.Amount;
                                model.Creditamt = 0;

                                model.Userpc = UserlogAddress.UserPc();
                                model.Insuserid = userid;
                                model.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model.Insipno = UserlogAddress.IpAddress();
                                model.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model);
                                db.SaveChanges();

                                model2.Transsl = maxSlCheck + 2;

                                model2.Transdt = x.Transdt;
                                model2.Compid = x.Compid;
                                model2.Transtp = x.Transtp;
                                model2.Transmy = x.Transmy;
                                model2.Transno = x.Transno;
                                model2.Transfor = x.Transfor;
                                model2.Transmode = x.Transmode;
                                model2.Costpid = x.Costpid;
                                model2.Debitcd = x.Creditcd;
                                model2.Creditcd = x.Debitcd;
                                model2.Chequeno = x.Chequeno;
                                model2.Chequedt = x.Chequedt;
                                model2.Remarks = x.Remarks;

                                model2.Transdrcr = "CREDIT";
                                model2.Tableid = "GL_STRANS";

                                model2.Debitamt = 0;
                                model2.Creditamt = x.Amount;

                                model2.Userpc = UserlogAddress.UserPc();
                                model2.Insuserid = userid;
                                model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model2.Insipno = UserlogAddress.IpAddress();
                                model2.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model2);
                                db.SaveChanges();
                            }

                        }

                        else if (x.Transtp == "MPAY")
                        {
                            Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                where a.Compid == compid && a.Transtp == x.Transtp
                                                                select a.Transsl).DefaultIfEmpty().Max());

                            if (maxSlCheck == 0)
                            {
                                model.Transsl = 30001;

                                model.Transdt = x.Transdt;
                                model.Compid = x.Compid;
                                model.Transtp = x.Transtp;
                                model.Transmy = x.Transmy;
                                model.Transno = x.Transno;
                                model.Transfor = x.Transfor;
                                model.Transmode = x.Transmode;
                                model.Costpid = x.Costpid;
                                model.Debitcd = x.Debitcd;
                                model.Creditcd = x.Creditcd;
                                model.Chequeno = x.Chequeno;
                                model.Chequedt = x.Chequedt;
                                model.Remarks = x.Remarks;

                                model.Transdrcr = "DEBIT";
                                model.Tableid = "GL_STRANS";

                                model.Debitamt = x.Amount;
                                model.Creditamt = 0;

                                model.Userpc = UserlogAddress.UserPc();
                                model.Insuserid = userid;
                                model.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model.Insipno = UserlogAddress.IpAddress();
                                model.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model);
                                db.SaveChanges();


                                model2.Transsl = 30002;

                                model2.Transdt = x.Transdt;
                                model2.Compid = x.Compid;
                                model2.Transtp = x.Transtp;
                                model2.Transmy = x.Transmy;
                                model2.Transno = x.Transno;
                                model2.Transfor = x.Transfor;
                                model2.Transmode = x.Transmode;
                                model2.Costpid = x.Costpid;
                                model2.Debitcd = x.Creditcd;
                                model2.Creditcd = x.Debitcd;
                                model2.Chequeno = x.Chequeno;
                                model2.Chequedt = x.Chequedt;
                                model2.Remarks = x.Remarks;

                                model2.Transdrcr = "CREDIT";
                                model2.Tableid = "GL_STRANS";

                                model2.Debitamt = 0;
                                model2.Creditamt = x.Amount;

                                model2.Userpc = UserlogAddress.UserPc();
                                model2.Insuserid = userid;
                                model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model2.Insipno = UserlogAddress.IpAddress();
                                model2.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model2);
                                db.SaveChanges();
                            }
                            else
                            {
                                model.Transsl = maxSlCheck + 1;

                                model.Transdt = x.Transdt;
                                model.Compid = x.Compid;
                                model.Transtp = x.Transtp;
                                model.Transmy = x.Transmy;
                                model.Transno = x.Transno;
                                model.Transfor = x.Transfor;
                                model.Transmode = x.Transmode;
                                model.Costpid = x.Costpid;
                                model.Debitcd = x.Debitcd;
                                model.Creditcd = x.Creditcd;
                                model.Chequeno = x.Chequeno;
                                model.Chequedt = x.Chequedt;
                                model.Remarks = x.Remarks;

                                model.Transdrcr = "DEBIT";
                                model.Tableid = "GL_STRANS";

                                model.Debitamt = x.Amount;
                                model.Creditamt = 0;

                                model.Userpc = UserlogAddress.UserPc();
                                model.Insuserid = userid;
                                model.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model.Insipno = UserlogAddress.IpAddress();
                                model.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model);
                                db.SaveChanges();


                                model2.Transsl = maxSlCheck + 2;

                                model2.Transdt = x.Transdt;
                                model2.Compid = x.Compid;
                                model2.Transtp = x.Transtp;
                                model2.Transmy = x.Transmy;
                                model2.Transno = x.Transno;
                                model2.Transfor = x.Transfor;
                                model2.Transmode = x.Transmode;
                                model2.Costpid = x.Costpid;
                                model2.Debitcd = x.Creditcd;
                                model2.Creditcd = x.Debitcd;
                                model2.Chequeno = x.Chequeno;
                                model2.Chequedt = x.Chequedt;
                                model2.Remarks = x.Remarks;

                                model2.Transdrcr = "CREDIT";
                                model2.Tableid = "GL_STRANS";

                                model2.Debitamt = 0;
                                model2.Creditamt = x.Amount;

                                model2.Userpc = UserlogAddress.UserPc();
                                model2.Insuserid = userid;
                                model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model2.Insipno = UserlogAddress.IpAddress();
                                model2.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model2);
                                db.SaveChanges();
                            }
                        }
                        else if (x.Transtp == "JOUR")
                        {
                            Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                where a.Compid == compid && a.Transtp == x.Transtp
                                                                select a.Transsl).DefaultIfEmpty().Max());

                            if (maxSlCheck == 0)
                            {
                                model.Transsl = 50001;

                                model.Transdt = x.Transdt;
                                model.Compid = x.Compid;
                                model.Transtp = x.Transtp;
                                model.Transmy = x.Transmy;
                                model.Transno = x.Transno;
                                model.Transfor = x.Transfor;
                                model.Transmode = x.Transmode;
                                model.Costpid = x.Costpid;
                                model.Debitcd = x.Debitcd;
                                model.Creditcd = x.Creditcd;
                                model.Chequeno = x.Chequeno;
                                model.Chequedt = x.Chequedt;
                                model.Remarks = x.Remarks;

                                model.Transdrcr = "DEBIT";
                                model.Tableid = "GL_STRANS";

                                model.Debitamt = x.Amount;
                                model.Creditamt = 0;

                                model.Userpc = UserlogAddress.UserPc();
                                model.Insuserid = userid;
                                model.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model.Insipno = UserlogAddress.IpAddress();
                                model.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model);
                                db.SaveChanges();


                                model2.Transsl = 50002;

                                model2.Transdt = x.Transdt;
                                model2.Compid = x.Compid;
                                model2.Transtp = x.Transtp;
                                model2.Transmy = x.Transmy;
                                model2.Transno = x.Transno;
                                model2.Transfor = x.Transfor;
                                model2.Transmode = x.Transmode;
                                model2.Costpid = x.Costpid;
                                model2.Debitcd = x.Creditcd;
                                model2.Creditcd = x.Debitcd;
                                model2.Chequeno = x.Chequeno;
                                model2.Chequedt = x.Chequedt;
                                model2.Remarks = x.Remarks;

                                model2.Transdrcr = "CREDIT";
                                model2.Tableid = "GL_STRANS";

                                model2.Debitamt = 0;
                                model2.Creditamt = x.Amount;

                                model2.Userpc = UserlogAddress.UserPc();
                                model2.Insuserid = userid;
                                model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model2.Insipno = UserlogAddress.IpAddress();
                                model2.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model2);
                                db.SaveChanges();
                            }
                            else
                            {
                                model.Transsl = maxSlCheck + 1;

                                model.Transdt = x.Transdt;
                                model.Compid = x.Compid;
                                model.Transtp = x.Transtp;
                                model.Transmy = x.Transmy;
                                model.Transno = x.Transno;
                                model.Transfor = x.Transfor;
                                model.Transmode = x.Transmode;
                                model.Costpid = x.Costpid;
                                model.Debitcd = x.Debitcd;
                                model.Creditcd = x.Creditcd;
                                model.Chequeno = x.Chequeno;
                                model.Chequedt = x.Chequedt;
                                model.Remarks = x.Remarks;

                                model.Transdrcr = "DEBIT";
                                model.Tableid = "GL_STRANS";

                                model.Debitamt = x.Amount;
                                model.Creditamt = 0;

                                model.Userpc = UserlogAddress.UserPc();
                                model.Insuserid = userid;
                                model.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model.Insipno = UserlogAddress.IpAddress();
                                model.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model);
                                db.SaveChanges();

                                model2.Transsl = maxSlCheck + 2;

                                model2.Transdt = x.Transdt;
                                model2.Compid = x.Compid;
                                model2.Transtp = x.Transtp;
                                model2.Transmy = x.Transmy;
                                model2.Transno = x.Transno;
                                model2.Transfor = x.Transfor;
                                model2.Transmode = x.Transmode;
                                model2.Costpid = x.Costpid;
                                model2.Debitcd = x.Creditcd;
                                model2.Creditcd = x.Debitcd;
                                model2.Chequeno = x.Chequeno;
                                model2.Chequedt = x.Chequedt;
                                model2.Remarks = x.Remarks;

                                model2.Transdrcr = "CREDIT";
                                model2.Tableid = "GL_STRANS";

                                model2.Debitamt = 0;
                                model2.Creditamt = x.Amount;

                                model2.Userpc = UserlogAddress.UserPc();
                                model2.Insuserid = userid;
                                model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model2.Insipno = UserlogAddress.IpAddress();
                                model2.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model2);
                                db.SaveChanges();
                            }
                        }
                        else if (x.Transtp == "CONT")
                        {
                            Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                where a.Compid == compid && a.Transtp == x.Transtp
                                                                select a.Transsl).DefaultIfEmpty().Max());

                            if (maxSlCheck == 0)
                            {
                                model.Transsl = 70001;

                                model.Transdt = x.Transdt;
                                model.Compid = x.Compid;
                                model.Transtp = x.Transtp;
                                model.Transmy = x.Transmy;
                                model.Transno = x.Transno;
                                model.Transfor = x.Transfor;
                                model.Transmode = x.Transmode;
                                model.Costpid = x.Costpid;
                                model.Debitcd = x.Debitcd;
                                model.Creditcd = x.Creditcd;
                                model.Chequeno = x.Chequeno;
                                model.Chequedt = x.Chequedt;
                                model.Remarks = x.Remarks;

                                model.Transdrcr = "DEBIT";
                                model.Tableid = "GL_STRANS";

                                model.Debitamt = x.Amount;
                                model.Creditamt = 0;

                                model.Userpc = UserlogAddress.UserPc();
                                model.Insuserid = userid;
                                model.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model.Insipno = UserlogAddress.IpAddress();
                                model.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model);
                                db.SaveChanges();

                                model2.Transsl = 70002;

                                model2.Transdt = x.Transdt;
                                model2.Compid = x.Compid;
                                model2.Transtp = x.Transtp;
                                model2.Transmy = x.Transmy;
                                model2.Transno = x.Transno;
                                model2.Transfor = x.Transfor;
                                model2.Transmode = x.Transmode;
                                model2.Costpid = x.Costpid;
                                model2.Debitcd = x.Creditcd;
                                model2.Creditcd = x.Debitcd;
                                model2.Chequeno = x.Chequeno;
                                model2.Chequedt = x.Chequedt;
                                model2.Remarks = x.Remarks;

                                model2.Transdrcr = "CREDIT";
                                model2.Tableid = "GL_STRANS";

                                model2.Debitamt = 0;
                                model2.Creditamt = x.Amount;

                                model2.Userpc = UserlogAddress.UserPc();
                                model2.Insuserid = userid;
                                model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model2.Insipno = UserlogAddress.IpAddress();
                                model2.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model2);
                                db.SaveChanges();


                            }
                            else
                            {
                                model.Transsl = maxSlCheck + 1;

                                model.Transdt = x.Transdt;
                                model.Compid = x.Compid;
                                model.Transtp = x.Transtp;
                                model.Transmy = x.Transmy;
                                model.Transno = x.Transno;
                                model.Transfor = x.Transfor;
                                model.Transmode = x.Transmode;
                                model.Costpid = x.Costpid;
                                model.Debitcd = x.Debitcd;
                                model.Creditcd = x.Creditcd;
                                model.Chequeno = x.Chequeno;
                                model.Chequedt = x.Chequedt;
                                model.Remarks = x.Remarks;

                                model.Transdrcr = "DEBIT";
                                model.Tableid = "GL_STRANS";

                                model.Debitamt = x.Amount;
                                model.Creditamt = 0;

                                model.Userpc = UserlogAddress.UserPc();
                                model.Insuserid = userid;
                                model.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model.Insipno = UserlogAddress.IpAddress();
                                model.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model);
                                db.SaveChanges();

                                model2.Transsl = maxSlCheck + 2;

                                model2.Transdt = x.Transdt;
                                model2.Compid = x.Compid;
                                model2.Transtp = x.Transtp;
                                model2.Transmy = x.Transmy;
                                model2.Transno = x.Transno;
                                model2.Transfor = x.Transfor;
                                model2.Transmode = x.Transmode;
                                model2.Costpid = x.Costpid;
                                model2.Debitcd = x.Creditcd;
                                model2.Creditcd = x.Debitcd;
                                model2.Chequeno = x.Chequeno;
                                model2.Chequedt = x.Chequedt;
                                model2.Remarks = x.Remarks;

                                model2.Transdrcr = "CREDIT";
                                model2.Tableid = "GL_STRANS";

                                model2.Debitamt = 0;
                                model2.Creditamt = x.Amount;

                                model2.Userpc = UserlogAddress.UserPc();
                                model2.Insuserid = userid;
                                model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model2.Insipno = UserlogAddress.IpAddress();
                                model2.Insltude = passModel.Insltude;

                                db.GlMasterDbSet.Add(model2);
                                db.SaveChanges();
                            }
                        }
                    }
                    return "True";
                }
                else
                {
                    return "False";
                }
            }
            catch (Exception ex)
            {
                var forremovedata = (from l in db.GlMasterDbSet
                                     where l.Compid == compid && l.Transdt == passModel.Transdt
                                     select l).ToList();
                foreach (var get in forremovedata)
                {
                    if (get.Tableid == "GL_STRANS")
                    {
                        db.GlMasterDbSet.Remove(get);
                    }
                }
                db.SaveChanges();
                return "False";
            }
            
        }
    }
}