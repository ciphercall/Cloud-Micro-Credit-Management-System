using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace cloud_Api_MCR.DataAccess.API
{
    public class Login
    {
        static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseDbContext"].ToString());



        internal static bool UserIsValid(string username, string password)
        {
            bool authenticated = false;

            string query = string.Format("SELECT * FROM [ASL_USERCO] WHERE Loginid = '{0}' AND Loginpw = '{1}'", username, password);

            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
            authenticated = sdr.HasRows;
            conn.Close();
            return (authenticated);
        }



       
    }
}