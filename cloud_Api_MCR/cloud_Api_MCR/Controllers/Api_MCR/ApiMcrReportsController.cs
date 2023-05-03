using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.MCR;
using cloud_Api_MCR.Models.MCR_DTO;

namespace cloud_Api_MCR.Controllers.Api_MCR
{
    public class ApiMcrReportsController : ApiController
    {
        private DatabaseDbContext db;

        public ApiMcrReportsController()
        {
            db = new DatabaseDbContext();
        }



        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/McrReports/MemberWiseBalanceStatement")]
        public object MemberWiseBalanceStatement(Int64 userCompanycode, Int64 usercode, String fromDate, String toDate)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token) && fromDate != "" && toDate != "")
            {
                List<ViewMcrReportModel> list = new List<ViewMcrReportModel>();
                DateTime fdate = Convert.ToDateTime(fromDate);
                DateTime tdate = Convert.ToDateTime(toDate);

                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseDbContext"].ToString());
                string query = string.Format(
                    @"select A.Compid, A.Memberid,B.Membernm,B.Areaid, A.Internid,A.Schemeid,Schemetp, SUM(OP) Opening, SUM(DR) Debit, SUM(CR) Credit, 
(Case When Schemetp='DEPOSIT' 
then (SUM(OP)-SUM(DR)+SUM(CR))
else SUM(OP)+SUM(DR)-SUM(CR) end) Closing from
(
Select A.Compid, A.Memberid, A.Internid,A.Schemeid,Schemetp, OP, 0 DR, 0 CR from 
(Select A.Compid, Memberid, Internid,A.Schemeid,Schemetp, Sum(Creditamt)- Sum(Debitamt) OP from MCR_MASTER A inner join MCR_SCHEME B 
on A.Compid=B.Compid and A.Schemeid = B.Schemeid and A.Compid='{0}' and Schemetp='DEPOSIT' and Transdt<'{1}'
group by A.Compid, Memberid, Internid,A.Schemeid,Schemetp
UNION
Select A.Compid, Memberid, Internid,A.Schemeid,Schemetp, Sum(Creditamt)- Sum(Debitamt) OP from MCR_MASTER A inner join MCR_SCHEME B 
on A.Compid=B.Compid and A.Schemeid = B.Schemeid and A.Compid='{0}' and Schemetp='LOAN' and Transdt<'{1}'
group by A.Compid, Memberid, Internid,A.Schemeid,Schemetp) A

UNION

Select A.Compid, Memberid, Internid, A.Schemeid, SCHEMETP, 0 OP, Sum (Debitamt) Dr, Sum (Creditamt) Cr from MCR_MASTER A inner join MCR_SCHEME B 
on A.Compid=B.Compid and A.Schemeid = B.Schemeid  where A.Compid='{0}' and Transdt between '{1}' and '{2}'
group by A.Compid, Memberid, Internid,A.Schemeid,Schemetp
) A
inner join MCR_MEMBER B on A.Compid=B.Compid and A.Memberid = B.Memberid Where B.Compid='{0}'  
group by A.Compid,A.Memberid,B.Membernm,B.Areaid, A.Internid,A.Schemeid,Schemetp",
                    userCompanycode, fdate, tdate);
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);

                foreach (DataRow row in ds.Rows)
                {
                    ViewMcrReportModel getModel = new ViewMcrReportModel();
                    getModel.Compid = Convert.ToInt64(row["Compid"]);
                    getModel.Memberid = Convert.ToInt64(row["Memberid"]);
                    getModel.Membernm = Convert.ToString(row["Membernm"]);
                    getModel.Areaid = Convert.ToInt64(row["Areaid"]);
                    getModel.Internid = Convert.ToInt64(row["Internid"]);
                    getModel.Schemeid = Convert.ToString(row["Schemeid"]);
                    getModel.Schemetp = Convert.ToString(row["Schemetp"]);
                    getModel.Opening = Convert.ToDecimal(row["Opening"]);
                    getModel.Debitamt = Convert.ToDecimal(row["Debit"].ToString());
                    getModel.Creditamt = Convert.ToDecimal(row["Credit"].ToString());
                    getModel.Closing = Convert.ToDecimal(row["Closing"]);
                    list.Add(getModel);
                }
                conn.Close();
                if (list.Count != 0)
                    return new
                    {
                        data = list,
                        success = true,
                        message = "Get data Successfully."
                    };
                else return new
                {
                    data = "",
                    success = false,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = "",
                success = false,
                message = "Authorized not permitted."
            };
        }






        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/McrReports/MemberWiseBalanceStatementWithArea")]
        public object MemberWiseBalanceStatementWithArea(Int64 userCompanycode, Int64 usercode, String fromDate, String toDate, Int64 areaCode)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token) && fromDate != "" && toDate != "")
            {
                List<ViewMcrReportModel> list = new List<ViewMcrReportModel>();
                DateTime fdate = Convert.ToDateTime(fromDate);
                DateTime tdate = Convert.ToDateTime(toDate);

                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseDbContext"].ToString());
                string query = string.Format(
                    @"select A.Compid, A.Memberid,B.Membernm,B.Areaid, A.Internid,A.Schemeid,Schemetp, SUM(OP) Opening, SUM(DR) Debit, SUM(CR) Credit, 
(Case When Schemetp='DEPOSIT' 
then (SUM(OP)-SUM(DR)+SUM(CR))
else SUM(OP)+SUM(DR)-SUM(CR) end) Closing from
(
Select A.Compid, A.Memberid, A.Internid,A.Schemeid,Schemetp, OP, 0 DR, 0 CR from 
(Select A.Compid, Memberid, Internid,A.Schemeid,Schemetp, Sum(Creditamt)- Sum(Debitamt) OP from MCR_MASTER A inner join MCR_SCHEME B 
on A.Compid=B.Compid and A.Schemeid = B.Schemeid and A.Compid='{0}' and Schemetp='DEPOSIT' and Transdt<'{1}'
group by A.Compid, Memberid, Internid,A.Schemeid,Schemetp
UNION
Select A.Compid, Memberid, Internid,A.Schemeid,Schemetp, Sum(Creditamt)- Sum(Debitamt) OP from MCR_MASTER A inner join MCR_SCHEME B 
on A.Compid=B.Compid and A.Schemeid = B.Schemeid and A.Compid='{0}' and Schemetp='LOAN' and Transdt<'{1}'
group by A.Compid, Memberid, Internid,A.Schemeid,Schemetp) A

UNION

Select A.Compid, Memberid, Internid, A.Schemeid, SCHEMETP, 0 OP, Sum (Debitamt) Dr, Sum (Creditamt) Cr from MCR_MASTER A inner join MCR_SCHEME B 
on A.Compid=B.Compid and A.Schemeid = B.Schemeid  where A.Compid='{0}' and Transdt between '{1}' and '{2}'
group by A.Compid, Memberid, Internid,A.Schemeid,Schemetp
) A
inner join MCR_MEMBER B on A.Compid=B.Compid and A.Memberid = B.Memberid Where B.Compid='{0}' and B.Areaid='{3}'  
group by A.Compid,A.Memberid,B.Membernm,B.Areaid, A.Internid,A.Schemeid,Schemetp",
                    userCompanycode, fdate, tdate, areaCode);
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);

                foreach (DataRow row in ds.Rows)
                {
                    ViewMcrReportModel getModel = new ViewMcrReportModel();
                    getModel.Compid = Convert.ToInt64(row["Compid"]);
                    getModel.Memberid = Convert.ToInt64(row["Memberid"]);
                    getModel.Membernm = Convert.ToString(row["Membernm"]);
                    getModel.Areaid = Convert.ToInt64(row["Areaid"]);
                    getModel.Internid = Convert.ToInt64(row["Internid"]);
                    getModel.Schemeid = Convert.ToString(row["Schemeid"]);
                    getModel.Schemetp = Convert.ToString(row["Schemetp"]);
                    getModel.Opening = Convert.ToDecimal(row["Opening"]);
                    getModel.Debitamt = Convert.ToDecimal(row["Debit"]);
                    getModel.Creditamt = Convert.ToDecimal(row["Credit"]);
                    getModel.Closing = Convert.ToDecimal(row["Closing"]);
                    list.Add(getModel);
                }
                conn.Close();
                if (list.Count != 0)
                    return new
                    {
                        data = list,
                        success = true,
                        message = "Get data Successfully."
                    };
                else return new
                {
                    data = "",
                    success = false,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = "",
                success = false,
                message = "Authorized not permitted."
            };
        }




        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/McrReports/MemberLedgerOpeningBalance")]
        public object MemberLedgerOpeningBalance(Int64 userCompanycode, Int64 usercode, String fromDate, String schemeId, Int64 internalId, Int64 memberId )
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token) && fromDate != "")
            {
                List<ViewMcrReportModel> list = new List<ViewMcrReportModel>();
                DateTime fdate = Convert.ToDateTime(fromDate);

                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseDbContext"].ToString());
                string query = string.Format(
                    @"Select A.Compid, A.Memberid, A.Internid,A.Schemeid, Schemetp, 
(case when schemetp = 'DEPOSIT' then Sum(Creditamt)- Sum(Debitamt) else Sum(Debitamt)- Sum(Creditamt) end) Opening 
from MCR_MASTER A inner join MCR_MSCHEME B 
ON A.Compid=B.Compid and A.Schemeid = B.Schemeid and A.Memberid=B.Memberid and A.Internid=B.Internid 
and A.Compid='{0}' and B.Schemeid = '{2}' AND B.Internid='{3}' AND B.Memberid='{4}' and Transdt<'{1}'
inner join MCR_SCHEME C 
ON A.Compid=C.Compid and a.schemeid = c.schemeid and A.SCHEMEID = '{2}' and C.Compid='{0}'
group by A.Compid, A.Memberid, A.Internid,A.Schemeid, Schemetp",
                    userCompanycode, fdate, schemeId, internalId, memberId);
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);

                foreach (DataRow row in ds.Rows)
                {
                    ViewMcrReportModel getModel = new ViewMcrReportModel();
                    getModel.Compid = Convert.ToInt64(row["Compid"]);
                    getModel.Memberid = Convert.ToInt64(row["Memberid"]);
                    getModel.Internid = Convert.ToInt64(row["Internid"]);
                    getModel.Schemeid = Convert.ToString(row["Schemeid"]);
                    getModel.Schemetp = Convert.ToString(row["Schemetp"]);
                    getModel.Opening = Convert.ToDecimal(row["Opening"]);
                    list.Add(getModel);
                }
                conn.Close();
                if (list.Count != 0)
                    return new
                    {
                        data = list,
                        success = true,
                        message = "Get data Successfully."
                    };
                else return new
                {
                    data = "",
                    success = false,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = "",
                success = false,
                message = "Authorized not permitted."
            };
        }





        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/McrReports/FieldWorkerWiseDailyCollectionStatement")]
        public object FieldWorkerWiseDailyCollectionStatement(Int64 userCompanycode, Int64 usercode, String date, String schemeId, Int64 fieldWorkersId)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token) && date != "")
            {
                List<ViewMcrReportModel> list = new List<ViewMcrReportModel>();

                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseDbContext"].ToString());
                string query = string.Format(
                    @"SELECT Compid, Schemeid , Internid, Membernm, Areanm, SUM(ISNULL(OPAMT,0)) OPAMT, SUM(ISNULL(AMTCOLLECT,0)) AMTCOLLECT, SUM(ISNULL(AMTPAY,0)) AMTPAY
            FROM(
            SELECT A.Compid,Schemeid, Internid, Membernm, Areanm, SUM(ISNULL(DEBITAMT,0))-SUM(ISNULL(CREDITAMT,0)) OPAMT, 0 AMTCOLLECT, 0 AMTPAY
            FROM   MCR_MASTER A INNER JOIN MCR_MEMBER B ON A.Memberid = B.Memberid and A.Compid = B.Compid
            INNER JOIN MCR_AREA C ON B.AREAID = C.AREAID and A.Compid = C.Compid
            WHERE A.Compid='{0}' AND TRANSDT < '{1}' AND C.Fwid = '{3}' AND A.Schemeid = '{2}'
            GROUP BY A.Compid,Schemeid, Internid, Membernm, Areanm			
			UNION
            SELECT A.Compid,Schemeid, Internid, Membernm, Areanm, 0 OPAMT, SUM(ISNULL(Amount,0)) AMTCOLLECT, 0 AMTPAY
            FROM   MCR_COLLECT A INNER JOIN MCR_MEMBER B ON A.Memberid = B.Memberid and A.Compid = B.Compid
            INNER JOIN MCR_AREA C ON A.Fwid = C.Fwid and A.Compid = C.Compid
            WHERE A.Compid='{0}' AND TRANSDT = '{1}'  AND C.Fwid = '{3}' AND A.Schemeid = '{2}'
            GROUP BY A.Compid,Schemeid, Internid, Membernm, Areanm
            UNION
            SELECT A.Compid, Schemeid, Internid, Membernm, Areanm,  0 OPAMT, 0 AMTCOLLECT, SUM(ISNULL(AMOUNT,0)) AMTPAY
            FROM   MCR_MTRANS A INNER JOIN MCR_MEMBER B ON A.Memberid = B.Memberid and A.Compid = B.Compid
            INNER JOIN MCR_AREA C ON B.Areaid = C.Areaid and A.Compid = C.Compid
            WHERE  A.Compid='{0}' AND TRANSTP = 'MPAY' AND TRANSDT = '{1}' AND Fwid = '{3}' AND A.Schemeid = '{2}'
            GROUP BY A.Compid,Schemeid, Internid, Membernm, Areanm
            ) A GROUP BY Compid, Schemeid, Internid, Membernm, Areanm",
                    userCompanycode, date, schemeId, fieldWorkersId);
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);

                foreach (DataRow row in ds.Rows)
                {
                    ViewMcrReportModel getModel = new ViewMcrReportModel();
                    getModel.Compid = Convert.ToInt64(row["Compid"]);
                    getModel.Schemeid = Convert.ToString(row["Schemeid"]);
                    getModel.Internid = Convert.ToInt64(row["Internid"]);
                    getModel.Membernm = Convert.ToString(row["Membernm"]);
                    getModel.AreaName = Convert.ToString(row["Areanm"]);
                    getModel.Opening = Convert.ToDecimal(row["OPAMT"]);
                    getModel.Collection = Convert.ToDecimal(row["AMTCOLLECT"]);
                    getModel.Payment = Convert.ToDecimal(row["AMTPAY"]);
                    list.Add(getModel);
                }
                conn.Close();
                if (list.Count != 0)
                    return new
                    {
                        data = list,
                        success = true,
                        message = "Get data Successfully."
                    };
                else return new
                {
                    data = "",
                    success = false,
                    message = "No Data Found."
                };
            }
            return new
            {
                data = "",
                success = false,
                message = "Authorized not permitted."
            };
        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
