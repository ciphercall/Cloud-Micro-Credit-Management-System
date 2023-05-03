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
    public class McrMemberLoanProcess
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
                    if (get.Tableid == "MCR_MLOAN")
                    {
                        db.McrMastersDbSet.Remove(get);
                    }
                }
                db.SaveChanges();


                //MCR Process for MCR_MLOAN
                var checkDateMcrMemberLoan = (from n in db.McrMLoansDbSet where n.Transdt == passModel.Transdt && n.Compid == compid select n).OrderBy(x => x.Transno).ToList();

                if (checkDateMcrMemberLoan.Count != 0)
                {
                    foreach (var x in checkDateMcrMemberLoan)
                    {
                        McrMaster model = new McrMaster();


                        Int64 maxSlCheck = Convert.ToInt64((from a in db.McrMastersDbSet
                                                            where a.Compid == compid && a.Transtp == "MPAY"
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
                        model.Transtp = "MPAY";
                        model.Transdt = x.Transdt;
                        model.Transmy = x.Transmy;
                        model.Transno = x.Transno;
                        model.Transdrcr = "DEBIT";
                        model.Transfor = null;
                        model.Transmode = null;
                        model.Memberid = x.Memberid;
                        model.Schemeid = x.Schemeid;
                        model.Internid = x.Internid;

                        model.Debitamt = Convert.ToDecimal(x.Disburseamt);
                        model.Creditamt = 0;

                        model.Remarks = x.Remarks;
                        model.Tableid = "MCR_MLOAN";

                        model.Userpc = UserlogAddress.UserPc();
                        model.Insuserid = userid;
                        model.Instime = UserlogAddress.Timezone(DateTime.Now);
                        model.Insipno = UserlogAddress.IpAddress();
                        model.Insltude = passModel.Insltude;

                        db.McrMastersDbSet.Add(model);
                        db.SaveChanges();
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
                    if (get.Tableid == "MCR_MLOAN")
                    {
                        db.McrMastersDbSet.Remove(get);
                    }
                }
                db.SaveChanges();
                return "False";
            }
        }




        // Only For MCR Process
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
                    if (get.Tableid == "MCR_MLOAN")
                    {
                        db.GlMasterDbSet.Remove(get);
                    }
                }
                db.SaveChanges();


                //MCR Process for MCR_MLOAN
                var checkDateMcrMemberLoan = (from n in db.McrMLoansDbSet where n.Transdt == passModel.Transdt && n.Compid == compid select n).OrderBy(x => x.Transno).ToList();

                if (checkDateMcrMemberLoan.Count != 0)
                {
                    foreach (var x in checkDateMcrMemberLoan)
                    {
                        Int64 linkToSchemeId = 0, incomeFromProfit = 0;
                        var findCreditcd = (from m in db.McrSchemesDbSet
                                            where m.Compid == compid && m.Schemeid == x.Schemeid
                                            select new { m.Glheadid, m.GlIncomehid }).Distinct().ToList();
                        foreach (var getcreditcd in findCreditcd)
                        {
                            linkToSchemeId = getcreditcd.Glheadid;
                            incomeFromProfit = getcreditcd.GlIncomehid;
                        }

                        GlMaster model = new GlMaster();
                        GlMaster model2 = new GlMaster();

                        Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                            where a.Compid == compid && a.Transtp == "MPAY"
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
                        model.Transtp = "MPAY";
                        model.Transdt = x.Transdt;
                        model.Transmy = x.Transmy;
                        model.Transno = x.Transno;
                        model.Transdrcr = "DEBIT";
                        model.Transfor = null;
                        model.Transmode = null;
                        model.Costpid = null;
                        model.Debitcd = linkToSchemeId;
                        model.Creditcd = Convert.ToInt64(compid + "1010001");
                        model.Chequeno = null;
                        model.Chequedt = null;
                        model.Remarks = "Scheme-ID: " + x.Schemeid + ". Internal-ID: " + x.Internid + ". Remarks: " + x.Remarks;

                        model.Debitamt = Convert.ToDecimal(x.Loanamt);
                        model.Creditamt = 0;
                        model.Tableid = "MCR_MLOAN";

                        model.Userpc = UserlogAddress.UserPc();
                        model.Insuserid = userid;
                        model.Instime = UserlogAddress.Timezone(DateTime.Now);
                        model.Insipno = UserlogAddress.IpAddress();
                        model.Insltude = passModel.Insltude;

                        db.GlMasterDbSet.Add(model);
                        db.SaveChanges();


                        model2.Compid = x.Compid;
                        model2.Transtp = "MPAY";
                        model2.Transdt = x.Transdt;
                        model2.Transmy = x.Transmy;
                        model2.Transno = x.Transno;
                        model2.Transdrcr = "CREDIT";
                        model2.Transfor = null;
                        model2.Transmode = null;
                        model2.Costpid = null;
                        model2.Debitcd = linkToSchemeId;
                        model2.Creditcd = incomeFromProfit;
                        model2.Chequeno = null;
                        model2.Chequedt = null;
                        model2.Remarks = "Scheme-ID: " + x.Schemeid + ". Internal-ID: " + x.Internid + ". Remarks: " + x.Remarks;

                        model2.Debitamt = 0;
                        model2.Creditamt = Convert.ToDecimal(x.Disburseamt - x.Loanamt);
                        model2.Tableid = "MCR_MLOAN";

                        model2.Userpc = UserlogAddress.UserPc();
                        model2.Insuserid = userid;
                        model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                        model2.Insipno = UserlogAddress.IpAddress();
                        model2.Insltude = passModel.Insltude;

                        db.GlMasterDbSet.Add(model2);
                        db.SaveChanges();
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
                    if (get.Tableid == "MCR_MLOAN")
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