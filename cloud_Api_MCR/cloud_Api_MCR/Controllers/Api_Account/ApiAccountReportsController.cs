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
using cloud_Api_MCR.Models.Account_DTO;
using cloud_Api_MCR.Models.MCR_DTO;

namespace cloud_Api_MCR.Controllers.Api_Account
{
    public class ApiAccountReportsController : ApiController
    {
        private DatabaseDbContext db;

        public ApiAccountReportsController()
        {
            db = new DatabaseDbContext();
        }



        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/AccountReports/TrialBalance")]
        public object TrialBalance(Int64 userCompanycode, Int64 usercode, String date, Int64 headCd)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token) && date != "" && headCd != 0)
            {
                List<ViewAccountReportModel> list = new List<ViewAccountReportModel>();

                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseDbContext"].ToString());
                string query = string.Format(
                     @"SELECT C.DEBITCD, GL_ACCHART.ACCOUNTNM, C.DEBIT, C.CREDIT ,GL_ACCHART.HEADTP, GL_ACCHARTMST.HEADNM, GL_ACCHARTMST.HEADCD
                                                FROM (SELECT DEBITCD, (CASE WHEN a.BAMT > 0 THEN a.BAMT ELSE 0 END) AS DEBIT,
                                                (CASE WHEN a.BAMT < 0 THEN a.BAMT * - 1 ELSE 0 END) AS CREDIT
                                                FROM (SELECT DEBITCD, SUM(ISNULL(DEBITAMT, 0)) - SUM(ISNULL(CREDITAMT, 0)) AS BAMT
                                                FROM  GL_MASTER
                                                WHERE (SUBSTRING(cast(DEBITCD as nvarchar(20)), 1, 1) IN ('1', '4')) AND (TRANSDT <= '{1}') and COMPID='{0}'
                                                GROUP BY DEBITCD) AS a
                                                UNION
                                                SELECT DEBITCD, (CASE WHEN b.BAMT < 0 THEN b.BAMT * - 1 ELSE 0 END) AS DEBIT,
                                                (CASE WHEN b.BAMT > 0 THEN B.BAMT ELSE 0 END) AS CREDIT
                                                FROM (SELECT DEBITCD, SUM(ISNULL(CREDITAMT, 0)) - SUM(ISNULL(DEBITAMT, 0)) AS BAMT
                                                FROM GL_MASTER AS GL_MASTER_1
                                                WHERE (SUBSTRING(cast(DEBITCD as nvarchar(20)), 1, 1) IN ('2', '3')) AND (TRANSDT <= '{1}')  and COMPID='{0}'
                                                GROUP BY DEBITCD) AS b) AS C INNER JOIN
                                                GL_ACCHART ON C.DEBITCD = GL_ACCHART.ACCOUNTCD and GL_ACCHART.COMPID = '{0}'
                                                Inner join GL_ACCHARTMST on GL_ACCHARTMST.HEADCD='{2}' and GL_ACCHART.HEADCD=GL_ACCHARTMST.HEADCD and GL_ACCHART.COMPID = GL_ACCHARTMST.COMPID
                                                and GL_ACCHARTMST.COMPID = '{0}'
                                                order by GL_ACCHART.ACCOUNTNM",
                                                            userCompanycode, date, headCd);
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);

                foreach (DataRow row in ds.Rows)
                {
                    ViewAccountReportModel getModel = new ViewAccountReportModel();
                    getModel.Debitcd = Convert.ToInt64(row["DEBITCD"]);
                    getModel.Accountnm = Convert.ToString(row["ACCOUNTNM"]);
                    getModel.Debitamt = Convert.ToDecimal(row["DEBIT"]);
                    getModel.Creditamt = Convert.ToDecimal(row["CREDIT"]);
                    getModel.Headtp = Convert.ToInt16(row["HEADTP"]);
                    getModel.Headnm = Convert.ToString(row["HEADNM"]);
                    getModel.Headcd = Convert.ToInt64(row["HEADCD"]);
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
        [System.Web.Http.Route("api/AccountReports/BalanceSheet")]
        public object BalanceSheet(Int64 userCompanycode, Int64 usercode, String date)
        {
            var re = Request;
            var headers = re.Headers.Authorization.Parameter;
            string token = headers.ToString();

            if (TokenInfo.TokenCheck(userCompanycode, usercode, token) && date != "")
            {
                List<ViewAccountReportModel> list = new List<ViewAccountReportModel>();

                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseDbContext"].ToString());
                string query = string.Format(
                     @"Select DEBITCD, AMOUNT ,B.ACCOUNTNM from
(SELECT DEBITCD, (SUM(CREDITAMT) - SUM(DEBITAMT)) AMOUNT FROM GL_MASTER
WHERE SUBSTRING(CONVERT(CHAR,DEBITCD),4,1) = '2' and COMPID='{0}' and TRANSDT<='{1}'
GROUP BY DEBITCD
UNION
SELECT cast(COMPID as nvarchar)+ '2020001' DEBITCD, (SUM(CREDITAMT) - SUM(DEBITAMT)) AMOUNT FROM GL_MASTER
WHERE SUBSTRING(CONVERT(CHAR,DEBITCD),4,1) IN ('3','4') and COMPID='{0}' and TRANSDT<='{1}'
GROUP BY COMPID)A left outer join GL_ACCHART B on A.DEBITCD=B.ACCOUNTCD and B.COMPID = '{0}'
order by A.DEBITCD",
                                                            userCompanycode, date);
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);

                foreach (DataRow row in ds.Rows)
                {
                    ViewAccountReportModel getModel = new ViewAccountReportModel();
                    getModel.Debitcd = Convert.ToInt64(row["DEBITCD"]);
                    getModel.Amount = Convert.ToDecimal(row["AMOUNT"]);
                    getModel.Accountnm = Convert.ToString(row["ACCOUNTNM"]);
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

    }
}
