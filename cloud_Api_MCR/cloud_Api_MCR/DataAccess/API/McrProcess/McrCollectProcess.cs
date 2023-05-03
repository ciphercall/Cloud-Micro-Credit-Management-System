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
    public static class McrCollectProcess
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
                    if (get.Tableid == "MCR_COLLECT")
                    {
                        db.McrMastersDbSet.Remove(get);
                    }
                }
                db.SaveChanges();


                //MCR Process for MCR_COLLECT
                var checkDateMcrCollect = (from n in db.McrCollectsDbSet where n.Transdt == passModel.Transdt && n.Compid == compid select n).OrderBy(x => x.Transno).ToList();

                if (checkDateMcrCollect.Count != 0)
                {
                    foreach (var x in checkDateMcrCollect)
                    {
                        McrMaster model = new McrMaster();


                        Int64 maxSlCheck = Convert.ToInt64((from a in db.McrMastersDbSet
                                                            where a.Compid == compid && a.Transtp == "MREC"
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
                        model.Transtp = "MREC";
                        model.Transdt = x.Transdt;
                        model.Transmy = x.Transmy;
                        model.Transno = x.Transno;
                        model.Transdrcr = "CREDIT"; 
                        model.Transfor = null;
                        model.Transmode = null;
                        model.Memberid = x.Memberid;
                        model.Schemeid = x.Schemeid;
                        model.Internid = x.Internid;

                        model.Debitamt = 0;
                        model.Creditamt = Convert.ToDecimal(x.Amount);

                        model.Remarks = x.Remarks;
                        model.Tableid = "MCR_COLLECT";

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
                    if (get.Tableid == "MCR_COLLECT")
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
                    if (get.Tableid == "MCR_COLLECT")
                    {
                        db.GlMasterDbSet.Remove(get);
                    }
                }
                db.SaveChanges();


                //GL Process for MCR_COLLECT
                var getTransNoWiseSchemeIdFromMcrCollect = (from n in db.McrCollectsDbSet where n.Transdt == passModel.Transdt && n.Compid == compid select new {n.Transno, n.Schemeid }).OrderBy(x => x.Transno).Distinct().ToList();

                if (getTransNoWiseSchemeIdFromMcrCollect.Count != 0)
                {
                    foreach (var x in getTransNoWiseSchemeIdFromMcrCollect)
                    {
                        Decimal amount = 0;
                        var checkDateMcrCollect = (from n in db.McrCollectsDbSet where n.Transdt == passModel.Transdt && n.Compid == compid && n.Schemeid== x.Schemeid && n.Transno==x.Transno select n).ToList();
                        foreach (var getData in checkDateMcrCollect)
                        {
                            amount = Convert.ToDecimal(amount + getData.Amount);
                        }


                        foreach (var getData in checkDateMcrCollect)
                        {
                            Int64 linkToSchemeId = 0;
                            var findCreditcd = (from m in db.McrSchemesDbSet where m.Compid == compid && m.Schemeid == getData.Schemeid
                                select new { m.Glheadid }).Distinct().ToList();
                            foreach (var getcreditcd in findCreditcd)
                            {
                                linkToSchemeId = getcreditcd.Glheadid;
                            }


                            GlMaster model = new GlMaster();
                            GlMaster model2 = new GlMaster();

                            Int64 maxSlCheck = Convert.ToInt64((from a in db.GlMasterDbSet
                                                                where a.Compid == compid && a.Transtp == "MREC"
                                                                select a.Transsl).DefaultIfEmpty().Max());

                            if (maxSlCheck == 0)
                            {
                                //For MREC
                                model.Transsl = 170001;
                                model2.Transsl = 170002;
                            }
                            else
                            {
                                model.Transsl = maxSlCheck + 1;
                                model2.Transsl = maxSlCheck + 2;
                            }

                            model.Transdt = getData.Transdt;
                            model.Compid = getData.Compid;
                            model.Transtp = "MREC";
                            model.Transmy = getData.Transmy;
                            model.Transno = getData.Transno;
                            model.Transfor = null;
                            model.Transmode = null;
                            model.Costpid = null;
                            model.Debitcd = Convert.ToInt64(compid+"1010001");
                            model.Creditcd = linkToSchemeId;
                            model.Chequeno = null;
                            model.Chequedt = null;
                            model.Remarks = "Scheme-ID: "+ getData.Schemeid +". Internal-ID: "+getData.Internid+". Remarks: "+getData.Remarks;

                            model.Transdrcr = "DEBIT";
                            model.Tableid = "MCR_COLLECT";

                            model.Debitamt = getData.Amount;
                            model.Creditamt = 0;

                            model.Userpc = UserlogAddress.UserPc();
                            model.Insuserid = userid;
                            model.Instime = UserlogAddress.Timezone(DateTime.Now);
                            model.Insipno = UserlogAddress.IpAddress();
                            model.Insltude = passModel.Insltude;

                            db.GlMasterDbSet.Add(model);
                            db.SaveChanges();


                            model2.Transdt = getData.Transdt;
                            model2.Compid = getData.Compid;
                            model2.Transtp = "MREC";
                            model2.Transmy = getData.Transmy;
                            model2.Transno = x.Transno;
                            model2.Transfor = null;
                            model2.Transmode = null;
                            model2.Costpid = null;
                            model2.Debitcd = linkToSchemeId;
                            model2.Creditcd = Convert.ToInt64(compid + "1010001");
                            model2.Chequeno = null;
                            model2.Chequedt = null;
                            model2.Remarks = "Scheme-ID: " + getData.Schemeid + ". Internal-ID: " + getData.Internid + ". Remarks: " + getData.Remarks;

                            model2.Transdrcr = "CREDIT";
                            model2.Tableid = "MCR_COLLECT";

                            model2.Debitamt = 0;
                            model2.Creditamt = getData.Amount;

                            model2.Userpc = UserlogAddress.UserPc();
                            model2.Insuserid = userid;
                            model2.Instime = UserlogAddress.Timezone(DateTime.Now);
                            model2.Insipno = UserlogAddress.IpAddress();
                            model2.Insltude = passModel.Insltude;

                            db.GlMasterDbSet.Add(model2);
                            db.SaveChanges();

                            break;
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
                    if (get.Tableid == "MCR_COLLECT")
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