using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace BusinessLayer
{
    public class LoginClass
    {
        public string checkCredentials(string userId, string password)
        {
            string role , pwd ;
            String ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sql = "checkCre";
            SqlCommand command = new SqlCommand(sql, con);
            SqlParameter param1 = new SqlParameter("@uid", userId);
            command.Parameters.Add(param1);

            command.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = command.ExecuteReader();

            if (dr.Read())
            {
                role = dr.GetString(1);
                pwd = dr.GetString(0);

                if (password.Equals(pwd))
                {
                    con.Close();
                    return role;


                }
                else
                {
                    con.Close();
                    return "wrongPassword";
                }

            }
            else
            {
                con.Close();
                return "userNotFound";
            }

        }

    }
}
