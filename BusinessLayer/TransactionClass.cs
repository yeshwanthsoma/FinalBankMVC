using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace BusinessLayer
{
    public class TransactionClass
    {


        public void insTrans(long acc1, long acc2, int amt, string type, string comment)
        {

            String ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sql = "insTrans";
            string date = DateTime.Now.ToString();

            SqlCommand command = new SqlCommand(sql, con);
            SqlParameter param1 = new SqlParameter("@acc1", acc1);
            command.Parameters.Add(param1);
            SqlParameter param2 = new SqlParameter("@acc2", acc2);
            command.Parameters.Add(param2);
            SqlParameter param3 = new SqlParameter("@Date", date);
            command.Parameters.Add(param3);
            SqlParameter param4 = new SqlParameter("@amt", amt);
            command.Parameters.Add(param4);
            SqlParameter param5 = new SqlParameter("@type", type);
            command.Parameters.Add(param5);
            SqlParameter param6 = new SqlParameter("@comment", comment);
            command.Parameters.Add(param6);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            con.Close();


        }






    }
}
