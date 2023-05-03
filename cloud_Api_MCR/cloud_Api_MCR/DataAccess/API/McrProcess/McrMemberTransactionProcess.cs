using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.Account;
using cloud_Api_MCR.Models.MCR;
using cloud_Api_MCR.Models.MCR_DTO;

namespace cloud_Api_MCR.DataAccess.API.McrProcess
{
    public class McrMemberTransactionProcess
    {
        // Only For MCR Process
        public static string Process(Int64 compid, Int64 userid, ViewMcrMaster passModel)
        {
            DatabaseDbContext db = new DatabaseDbContext();
            try
            {
                var forremovedata = (from l in db.McrMastersDbSet
                                     where l.Compid == compid && l.Transdt == passModel.Transdt
                                     select l).ToList();
                foreach (var get in forremovedata)
                {
                    if (get.Tableid == "MCR_MTRANS")
                    {
                        db.McrMastersDbSet.Remove(get);
                    }
                }
                db.SaveChanges();


                //MCR Process for MCR_MTRANS
                var checkDateGlstrans = (from n in db.McrMTransesDbSet where n.Transdt == passModel.Transdt && n.Compid == compid select n).OrderBy(x => x.Transno).ToList();

                if (checkDateGlstrans.Count != 0)
                {
                    foreach (var x in checkDateGlstrans)
                    {
                        McrMaster model = new McrMaster();
                        McrMaster model2 = new McrMaster();

                        if (x.Transtp == "MREC")
                        {
                            Int64 maxSlCheck = Convert.ToInt64((from a in db.McrMastersDbSet
                                                                where a.Compid == compid && a.Transtp == x.Transtp
                                                                select a.Transsl).DefaultIfEmpty().Max());

                            if (maxSlCheck == 0)
                            {
                                //For MREC
                                model.Transsl = 90001;
                            }
                            else
                            {
                                model.Transsl = maxSlCheck + 1;
                            }

                            model.Compid = x.Compid;
                            model.Transtp = x.Transtp;
                            model.Transdt = x.Transdt;
                            model.Transmy = x.Transmy;
                            model.Transno = x.Transno;
                            model.Transdrcr = "CREDIT";
                            model.Transfor = x.Transfor;
                            model.Transmode = x.Transmode;
                            model.Memberid = x.Memberid;
                            model.Schemeid = x.Schemeid;
                            model.Internid = x.Internid;

                            model.Debitamt = 0;
                            model.Creditamt = Convert.ToDecimal(x.Amount);

                            model.Remarks = x.Remarks;
                            model.Tableid = "MCR_MTRANS";

                            model.Userpc = UserlogAddress.UserPc();
                            model.Insuserid = userid;
                            model.Instime = UserlogAddress.Timezone(DateTime.Now);
                            model.Insipno = UserlogAddress.IpAddress();
                            model.Insltude = passModel.Insltude;

                            db.McrMastersDbSet.Add(model);
                            db.SaveChanges();

                        }

                        else if (x.Transtp == "MPAY")
                        {
                            Int64 maxSlCheck = Convert.ToInt64((from a in db.McrMastersDbSet
                                                                where a.Compid == compid && a.Transtp == x.Transtp
                                                                select a.Transsl).DefaultIfEmpty().Max());

                            if (maxSlCheck == 0)
                            {
                                //For MPAY
                                model.Transsl = 190001;
                            }
                            else
                            {
                                model.Transsl = maxSlCheck + 1;
                            }

                            model.Compid = x.Compid;
                            model.Transtp = x.Transtp;
                            model.Transdt = x.Transdt;
                            model.Transmy = x.Transmy;
                            model.Transno = x.Transno;
                            model.Transdrcr = "DEBIT";
                            model.Transfor = x.Transfor;
                            model.Transmode = x.Transmode;
                            model.Memberid = x.Memberid;
                            model.Schemeid = x.Schemeid;
                            model.Internid = x.Internid;

                            model.Debitamt = Convert.ToDecimal(x.Amount);
                            model.Creditamt = 0;

                            model.Remarks = x.Remarks;
                            model.Tableid = "MCR_MTRANS";

                            model.Userpc = UserlogAddress.UserPc();
                            model.Insuserid = userid;
                            model.Instime = UserlogAddress.Timezone(DateTime.Now);
                            model.Insipno = UserlogAddress.IpAddress();
                            model.Insltude = passModel.Insltude;

                            db.McrMastersDbSet.Add(model);
                            db.SaveChanges();
                        }
                        else if (x.Transtp == "JOUR")
                        {
                            Int64 maxSlCheck = Convert.ToInt64((from a in db.McrMastersDbSet
                                                                where a.Compid == compid && a.Transtp == x.Transtp
                                                                select a.Transsl).DefaultIfEmpty().Max());

                            if (maxSlCheck == 0)
                            {
                                //For JOUR
                                model.Transsl = 290001;
                            }
                            else
                            {
                                model.Transsl = maxSlCheck + 1;
                            }

                            model.Compid = x.Compid;
                            model.Transtp = x.Transtp;
                            model.Transdt = x.Transdt;
                            model.Transmy = x.Transmy;
                            model.Transno = x.Transno;
                            model.Transfor = x.Transfor;
                            model.Transmode = x.Transmode;
                            model.Memberid = x.Memberid;
                            model.Schemeid = x.Schemeid;
                            model.Internid = x.Internid;

                            if (x.Transfor == "LEDGER TO SCHEME")
                            {
                                model.Transdrcr = "CREDIT";
                                model.Debitamt = 0;
                                model.Creditamt = Convert.ToDecimal(x.Amount);
                            }
                            else if (x.Transfor == "SCHEME TO LEDGER")
                            {
                                model.Transdrcr = "DEBIT";
                                model.Debitamt = Convert.ToDecimal(x.Amount);
                                model.Creditamt = 0;
                            }

                            model.Remarks = x.Remarks;
                            model.Tableid = "MCR_MTRANS";

                            model.Userpc = UserlogAddress.UserPc();
                            model.Insuserid = userid;
                            model.Instime = UserlogAddress.Timezone(DateTime.Now);
                            model.Insipno = UserlogAddress.IpAddress();
                            model.Insltude = passModel.Insltude;

                            db.McrMastersDbSet.Add(model);
                            db.SaveChanges();
                        }
                        else if (x.Transtp == "FTRF")
                        {
                            Int64 maxSlCheck = Convert.ToInt64((from a in db.McrMastersDbSet
                                                                where a.Compid == compid && a.Transtp == x.Transtp
                                                                select a.Transsl).DefaultIfEmpty().Max());

                            if (maxSlCheck == 0)
                            {
                                model.Transsl = 390001;

                                model.Compid = x.Compid;
                                model.Transtp = "JOUR";
                                model.Transdt = x.Transdt;
                                model.Transmy = x.Transmy;
                                model.Transno = x.Transno;
                                model.Transdrcr = "CREDIT";
                                model.Transfor = x.Transfor;
                                model.Transmode = x.Transmode;
                                model.Memberid = x.Memberid;
                                model.Schemeid = x.Schemeid;
                                model.Internid = x.Internid;

                                model.Debitamt = 0;
                                model.Creditamt = Convert.ToDecimal(x.Amount);

                                model.Remarks = x.Remarks;
                                model.Tableid = "MCR_MTRANS";

                                model.Userpc = UserlogAddress.UserPc();
                                model.Insuserid = userid;
                                model.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model.Insipno = UserlogAddress.IpAddress();
                                model.Insltude = passModel.Insltude;

                                db.McrMastersDbSet.Add(model);
                                db.SaveChanges();

                                model2.Transsl = 390002;

                                model2.Compid = x.Compid;
                                model2.Transtp = "JOUR";
                                model2.Transdt = x.Transdt;
                                model2.Transmy = x.Transmy;
                                model2.Transno = x.Transno;
                                model2.Transdrcr = "DEBIT";
                                model2.Transfor = x.Transfor;
                                model2.Transmode = x.Transmode;
                                model2.Memberid = x.Memberid2;
                                model2.Schemeid = x.Schemeid2;
                                model2.Internid = x.Internid2;

                                model2.Debitamt = Convert.ToDecimal(x.Amount);
                                model2.Creditamt = 0;

                                model2.Remarks = x.Remarks;
                                model2.Tableid = "MCR_MTRANS";

                                model2.Userpc = UserlogAddress.UserPc();
                                model2.Insuserid = userid;
                                model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model2.Insipno = UserlogAddress.IpAddress();
                                model2.Insltude = passModel.Insltude;

                                db.McrMastersDbSet.Add(model2);
                                db.SaveChanges();
                            }
                            else
                            {
                                model.Transsl = maxSlCheck + 1;

                                model.Compid = x.Compid;
                                model.Transtp = "JOUR";
                                model.Transdt = x.Transdt;
                                model.Transmy = x.Transmy;
                                model.Transno = x.Transno;
                                model.Transdrcr = "CREDIT";
                                model.Transfor = x.Transfor;
                                model.Transmode = x.Transmode;
                                model.Memberid = x.Memberid;
                                model.Schemeid = x.Schemeid;
                                model.Internid = x.Internid;

                                model.Debitamt = 0;
                                model.Creditamt = Convert.ToDecimal(x.Amount);

                                model.Remarks = x.Remarks;
                                model.Tableid = "MCR_MTRANS";

                                model.Userpc = UserlogAddress.UserPc();
                                model.Insuserid = userid;
                                model.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model.Insipno = UserlogAddress.IpAddress();
                                model.Insltude = passModel.Insltude;

                                db.McrMastersDbSet.Add(model);
                                db.SaveChanges();

                                model2.Transsl = maxSlCheck + 2;

                                model2.Compid = x.Compid;
                                model2.Transtp = "JOUR";
                                model2.Transdt = x.Transdt;
                                model2.Transmy = x.Transmy;
                                model2.Transno = x.Transno;
                                model2.Transdrcr = "DEBIT";
                                model2.Transfor = x.Transfor;
                                model2.Transmode = x.Transmode;
                                model2.Memberid = x.Memberid2;
                                model2.Schemeid = x.Schemeid2;
                                model2.Internid = x.Internid2;

                                model2.Debitamt = Convert.ToDecimal(x.Amount);
                                model2.Creditamt = 0;

                                model2.Remarks = x.Remarks;
                                model2.Tableid = "MCR_MTRANS";

                                model2.Userpc = UserlogAddress.UserPc();
                                model2.Insuserid = userid;
                                model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                                model2.Insipno = UserlogAddress.IpAddress();
                                model2.Insltude = passModel.Insltude;

                                db.McrMastersDbSet.Add(model2);
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
                var forremovedata = (from l in db.McrMastersDbSet
                                     where l.Compid == compid && l.Transdt == passModel.Transdt
                                     select l).ToList();
                foreach (var get in forremovedata)
                {
                    if (get.Tableid == "MCR_MTRANS")
                    {
                        db.McrMastersDbSet.Remove(get);
                    }
                }
                db.SaveChanges();
                return "False";
            }

        }






        // Only For Account Process
        public static string AccountProcess(Int64 compid, Int64 userid, ViewMcrMaster passModel)
        {
            DatabaseDbContext db = new DatabaseDbContext();
            try
            {
                var forremovedata = (from l in db.GlMasterDbSet
                                     where l.Compid == compid && l.Transdt == passModel.Transdt
                                     select l).ToList();
                foreach (var get in forremovedata)
                {
                    if (get.Tableid == "MCR_MTRANS")
                    {
                        db.GlMasterDbSet.Remove(get);
                    }
                }
                db.SaveChanges();


                //Account Process for MCR_MTRANS
                var checkDateGlstrans = (from n in db.McrMTransesDbSet where n.Transdt == passModel.Transdt && n.Compid == compid select n).OrderBy(x => x.Transno).ToList();

                if (checkDateGlstrans.Count != 0)
                {
                    foreach (var x in checkDateGlstrans)
                    {
                        Int64 linkToSchemeId = 0, linkToSchemeId2 = 0;
                        var findlinkToSchemeId = (from m in db.McrSchemesDbSet
                                            where m.Compid == compid && m.Schemeid == x.Schemeid
                                            select new { m.Glheadid }).Distinct().ToList();
                        foreach (var getcreditcd in findlinkToSchemeId)
                        {
                            linkToSchemeId = getcreditcd.Glheadid;
                        }
                        var findlinkToSchemeId2 = (from m in db.McrSchemesDbSet
                                            where m.Compid == compid && m.Schemeid == x.Schemeid2
                                            select new { m.Glheadid }).Distinct().ToList();
                        foreach (var getcreditcd in findlinkToSchemeId2)
                        {
                            linkToSchemeId2 = getcreditcd.Glheadid;
                        }


                        GlMaster model = new GlMaster();
                        GlMaster model2 = new GlMaster();

                        if (x.Transtp == "MREC")
                        {
                            Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                where a.Compid == compid && a.Transtp == x.Transtp
                                                                select a.Transsl).DefaultIfEmpty().Max());

                            if (maxSlCheck == 0)
                            {
                                //For MREC
                                model.Transsl = 90001;
                                model2.Transsl = 90002;
                            }
                            else
                            {
                                model.Transsl = maxSlCheck + 1;
                                model2.Transsl = maxSlCheck + 2;
                            }

                            model.Compid = x.Compid;
                            model.Transtp = x.Transtp;
                            model.Transdt = x.Transdt;
                            model.Transmy = x.Transmy;
                            model.Transno = x.Transno;
                            model.Transdrcr = "DEBIT";
                            model.Transfor = x.Transfor;
                            model.Transmode = x.Transmode;
                            model.Costpid = null;
                            model.Debitcd = x.Glheadid;
                            model.Creditcd = linkToSchemeId;
                            model.Chequeno = null;
                            model.Chequedt = null;
                            model.Remarks = "Scheme-ID: " + x.Schemeid + ". Internal-ID: " + x.Internid + ". Remarks: " + x.Remarks;

                            model.Debitamt = Convert.ToDecimal(x.Amount);
                            model.Creditamt = 0;
                            model.Tableid = "MCR_MTRANS";

                            model.Userpc = UserlogAddress.UserPc();
                            model.Insuserid = userid;
                            model.Instime = UserlogAddress.Timezone(DateTime.Now);
                            model.Insipno = UserlogAddress.IpAddress();
                            model.Insltude = passModel.Insltude;

                            db.GlMasterDbSet.Add(model);
                            db.SaveChanges();

                            model2.Compid = x.Compid;
                            model2.Transtp = x.Transtp;
                            model2.Transdt = x.Transdt;
                            model2.Transmy = x.Transmy;
                            model2.Transno = x.Transno;
                            model2.Transdrcr = "CREDIT";
                            model2.Transfor = x.Transfor;
                            model2.Transmode = x.Transmode;
                            model2.Costpid = null;
                            model2.Debitcd = linkToSchemeId;
                            model2.Creditcd = x.Glheadid;
                            model2.Chequeno = null;
                            model2.Chequedt = null;
                            model2.Remarks = "Scheme-ID: " + x.Schemeid + ". Internal-ID: " + x.Internid + ". Remarks: " + x.Remarks;

                            model2.Debitamt = 0;
                            model2.Creditamt = Convert.ToDecimal(x.Amount);
                            model2.Tableid = "MCR_MTRANS";

                            model2.Userpc = UserlogAddress.UserPc();
                            model2.Insuserid = userid;
                            model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                            model2.Insipno = UserlogAddress.IpAddress();
                            model2.Insltude = passModel.Insltude;

                            db.GlMasterDbSet.Add(model2);
                            db.SaveChanges();
                        }

                        else if (x.Transtp == "MPAY")
                        {
                            Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                where a.Compid == compid && a.Transtp == x.Transtp
                                                                select a.Transsl).DefaultIfEmpty().Max());

                            if (maxSlCheck == 0)
                            {
                                //For MPAY
                                model.Transsl = 110001;
                                model2.Transsl = 110002;
                            }
                            else
                            {
                                model.Transsl = maxSlCheck + 1;
                                model2.Transsl = maxSlCheck + 2;
                            }

                            model.Compid = x.Compid;
                            model.Transtp = x.Transtp;
                            model.Transdt = x.Transdt;
                            model.Transmy = x.Transmy;
                            model.Transno = x.Transno;
                            model.Transdrcr = "DEBIT";
                            model.Transfor = x.Transfor;
                            model.Transmode = x.Transmode;
                            model.Costpid = null;
                            model.Debitcd = linkToSchemeId;
                            model.Creditcd =  x.Glheadid;
                            model.Chequeno = null;
                            model.Chequedt = null;
                            model.Remarks = "Scheme-ID: " + x.Schemeid + ". Internal-ID: " + x.Internid + ". Remarks: " + x.Remarks;

                            model.Debitamt = Convert.ToDecimal(x.Amount);
                            model.Creditamt = 0;
                            model.Tableid = "MCR_MTRANS";

                            model.Userpc = UserlogAddress.UserPc();
                            model.Insuserid = userid;
                            model.Instime = UserlogAddress.Timezone(DateTime.Now);
                            model.Insipno = UserlogAddress.IpAddress();
                            model.Insltude = passModel.Insltude;

                            db.GlMasterDbSet.Add(model);
                            db.SaveChanges();

                            model2.Compid = x.Compid;
                            model2.Transtp = x.Transtp;
                            model2.Transdt = x.Transdt;
                            model2.Transmy = x.Transmy;
                            model2.Transno = x.Transno;
                            model2.Transdrcr = "CREDIT";
                            model2.Transfor = x.Transfor;
                            model2.Transmode = x.Transmode;
                            model2.Costpid = null;
                            model2.Debitcd = x.Glheadid;
                            model2.Creditcd =  linkToSchemeId;
                            model2.Chequeno = null;
                            model2.Chequedt = null;
                            model2.Remarks = "Scheme-ID: " + x.Schemeid + ". Internal-ID: " + x.Internid + ". Remarks: " + x.Remarks;

                            model2.Debitamt = 0;
                            model2.Creditamt = Convert.ToDecimal(x.Amount);
                            model2.Tableid = "MCR_MTRANS";

                            model2.Userpc = UserlogAddress.UserPc();
                            model2.Insuserid = userid;
                            model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                            model2.Insipno = UserlogAddress.IpAddress();
                            model2.Insltude = passModel.Insltude;

                            db.GlMasterDbSet.Add(model2);
                            db.SaveChanges();
                        }
                        else if (x.Transtp == "FTRF")
                        {
                            Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                where a.Compid == compid && a.Transtp == x.Transtp
                                                                select a.Transsl).DefaultIfEmpty().Max());
                            if (maxSlCheck == 0)
                            {
                                model.Transsl = 150001;
                                model2.Transsl = 150002;
                            }
                            else
                            {
                                model.Transsl = maxSlCheck + 1;
                                model2.Transsl = maxSlCheck + 2;
                            }

                            model.Compid = x.Compid;
                            model.Transtp = "JOUR";
                            model.Transdt = x.Transdt;
                            model.Transmy = x.Transmy;
                            model.Transno = x.Transno;
                            model.Transdrcr = "DEBIT";
                            model.Transfor = x.Transfor;
                            model.Transmode = x.Transmode;
                            model.Costpid = null;
                            model.Debitcd = linkToSchemeId;
                            model.Creditcd = linkToSchemeId2;
                            model.Chequeno = null;
                            model.Chequedt = null;
                            model.Remarks = "Scheme-ID: " + x.Schemeid + ". Internal-ID: " + x.Internid + ". Remarks: " + x.Remarks;

                            model.Debitamt = Convert.ToDecimal(x.Amount);
                            model.Creditamt = 0;
                            model.Tableid = "MCR_MTRANS";

                            model.Userpc = UserlogAddress.UserPc();
                            model.Insuserid = userid;
                            model.Instime = UserlogAddress.Timezone(DateTime.Now);
                            model.Insipno = UserlogAddress.IpAddress();
                            model.Insltude = passModel.Insltude;

                            db.GlMasterDbSet.Add(model);
                            db.SaveChanges();

                            model2.Compid = x.Compid;
                            model2.Transtp = "JOUR";
                            model2.Transdt = x.Transdt;
                            model2.Transmy = x.Transmy;
                            model2.Transno = x.Transno;
                            model2.Transdrcr = "CREDIT";
                            model2.Transfor = x.Transfor;
                            model2.Transmode = x.Transmode;
                            model2.Costpid = null;
                            model2.Debitcd = linkToSchemeId2;
                            model2.Creditcd = linkToSchemeId;
                            model2.Chequeno = null;
                            model2.Chequedt = null;
                            model2.Remarks = "Scheme-ID: " + x.Schemeid + ". Internal-ID: " + x.Internid + ". Remarks: " + x.Remarks;

                            model2.Debitamt = 0;
                            model2.Creditamt = Convert.ToDecimal(x.Amount);
                            model2.Tableid = "MCR_MTRANS";

                            model2.Userpc = UserlogAddress.UserPc();
                            model2.Insuserid = userid;
                            model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                            model2.Insipno = UserlogAddress.IpAddress();
                            model2.Insltude = passModel.Insltude;

                            db.GlMasterDbSet.Add(model2);
                            db.SaveChanges();
                        }

                        else if (x.Transtp == "JOUR")
                        {
                            Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                where a.Compid == compid && a.Transtp == x.Transtp
                                                                select a.Transsl).DefaultIfEmpty().Max());

                            if (maxSlCheck == 0)
                            {
                                //For JOUR
                                model.Transsl = 130001;
                                model2.Transsl = 130001;
                            }
                            else
                            {
                                model.Transsl = maxSlCheck + 1;
                                model2.Transsl = maxSlCheck + 2;
                            }

                            model.Compid = x.Compid;
                            model.Transtp = x.Transtp;
                            model.Transdt = x.Transdt;
                            model.Transmy = x.Transmy;
                            model.Transno = x.Transno;
                            model.Transfor = x.Transfor;
                            model.Transmode = x.Transmode;
                            model.Costpid = null;                          
                            model.Chequeno = null;
                            model.Chequedt = null;
                            model.Remarks = "Scheme-ID: " + x.Schemeid + ". Internal-ID: " + x.Internid + ". Remarks: " + x.Remarks;

                            if (x.Transfor == "LEDGER TO SCHEME")
                            {
                                model.Transdrcr = "DEBIT";
                                model.Debitcd = x.Glheadid;
                                model.Creditcd = linkToSchemeId;
                                model.Debitamt = Convert.ToDecimal(x.Amount);
                                model.Creditamt = 0;
                            }
                            else if (x.Transfor == "SCHEME TO LEDGER")
                            {
                                model.Transdrcr = "DEBIT";
                                model.Debitcd = linkToSchemeId;
                                model.Creditcd = x.Glheadid;
                                model.Debitamt = Convert.ToDecimal(x.Amount);
                                model.Creditamt = 0;
                            }
                            model.Tableid = "MCR_MTRANS";

                            model.Userpc = UserlogAddress.UserPc();
                            model.Insuserid = userid;
                            model.Instime = UserlogAddress.Timezone(DateTime.Now);
                            model.Insipno = UserlogAddress.IpAddress();
                            model.Insltude = passModel.Insltude;

                            db.GlMasterDbSet.Add(model);
                            db.SaveChanges();

                            model2.Compid = x.Compid;
                            model2.Transtp = x.Transtp;
                            model2.Transdt = x.Transdt;
                            model2.Transmy = x.Transmy;
                            model2.Transno = x.Transno;
                            model2.Transfor = x.Transfor;
                            model2.Transmode = x.Transmode;
                            model2.Costpid = null;
                            model2.Chequeno = null;
                            model2.Chequedt = null;
                            model2.Remarks = "Scheme-ID: " + x.Schemeid + ". Internal-ID: " + x.Internid + ". Remarks: " + x.Remarks;

                            if (x.Transfor == "LEDGER TO SCHEME")
                            {
                                model2.Transdrcr = "CREDIT";
                                model2.Debitcd = linkToSchemeId;
                                model2.Creditcd = x.Glheadid;
                                model2.Debitamt = 0;
                                model2.Creditamt = Convert.ToDecimal(x.Amount);
                            }
                            else if (x.Transfor == "SCHEME TO LEDGER")
                            {
                                model2.Transdrcr = "CREDIT";
                                model2.Debitcd = x.Glheadid;
                                model2.Creditcd = linkToSchemeId;
                                model2.Debitamt = 0;
                                model2.Creditamt = Convert.ToDecimal(x.Amount);
                            }
                            model2.Tableid = "MCR_MTRANS";

                            model2.Userpc = UserlogAddress.UserPc();
                            model2.Insuserid = userid;
                            model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                            model2.Insipno = UserlogAddress.IpAddress();
                            model2.Insltude = passModel.Insltude;

                            db.GlMasterDbSet.Add(model2);
                            db.SaveChanges();
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
                    if (get.Tableid == "MCR_MTRANS")
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