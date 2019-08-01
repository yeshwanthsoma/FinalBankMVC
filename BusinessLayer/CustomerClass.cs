using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLayer
{
    public class CustomerClass
    {
        String ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

        public string checkMedal(long acc)
        {

            int amt;
            CustomerClass obj = new CustomerClass();
            amt = obj.getAmount(acc);
            String ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sql = "changeMedal";
            SqlCommand command = new SqlCommand(sql, con);
            SqlParameter param1 = new SqlParameter("@amt", amt);
            SqlParameter param2 = new SqlParameter("@acc", acc);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();


            string sql2 = "getMedal";
            SqlCommand command2 = new SqlCommand(sql2, con);
            SqlParameter param12 = new SqlParameter("@acc", acc);
            command2.Parameters.Add(param12);
            command2.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da2 = new SqlDataAdapter(command2);
            DataSet ds = new DataSet();
            da2.Fill(ds);
            string res = null;

            if (ds.Tables[0].Rows.Count > 0)
            {
                res = ds.Tables[0].Rows[0]["type"].ToString();

            }

            con.Close();
            return res;
        }

        public bool checkAccount(long acc1)
        {

            String ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sql = "checkAcc";
            SqlCommand command = new SqlCommand(sql, con);
            SqlParameter param1 = new SqlParameter("@acc", acc1);
            command.Parameters.Add(param1);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }

            con.Close();
            return false;
        }

        public int getAmount(long acc1)
        {
            int res = 0;
            String ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sql = "checkAmo";
            SqlCommand command = new SqlCommand(sql, con);
            SqlParameter param1 = new SqlParameter("@acc", acc1);
            command.Parameters.Add(param1);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                res = (int)ds.Tables[0].Rows[0]["amount"];
                return res;
            }

            con.Close();
            return -100;
        }

        public void transferAdd(int amount, long acc)
        {
            String ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sql = "transferAdd";
            SqlCommand command = new SqlCommand(sql, con);
            SqlParameter param1 = new SqlParameter("@acc", acc);
            command.Parameters.Add(param1);
            SqlParameter param2 = new SqlParameter("@amt", amount);
            command.Parameters.Add(param2);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            con.Close();
        }

        public void transferSub(int amount, long acc)
        {
            String ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sql = "transferSub";
            SqlCommand command = new SqlCommand(sql, con);
            SqlParameter param1 = new SqlParameter("@acc", acc);
            command.Parameters.Add(param1);
            SqlParameter param2 = new SqlParameter("@amt", amount);
            command.Parameters.Add(param2);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            con.Close();
        }

        public string changePassword(string old, string new1, string new2, string uid, out bool success)
        {
            string res;
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sql = "changePass";//getting password using userId
            SqlCommand command = new SqlCommand(sql, con);
            SqlParameter param1 = new SqlParameter("@uid", uid);
            command.Parameters.Add(param1);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                res = ds.Tables[0].Rows[0]["password"].ToString();
                if (res.Equals(old))
                {
                    if (new1.Equals(new2))
                    {
                        if (new1.Equals(old))
                        {
                            success = false;
                            return "New password is same as old Password";
                        }
                        else
                        {
                            string sql1 = "updatePass";
                            SqlCommand command1 = new SqlCommand(sql1, con);
                            SqlParameter param2 = new SqlParameter("@uid", uid);
                            command1.Parameters.Add(param2);
                            SqlParameter param3 = new SqlParameter("@pass", new1);
                            command1.Parameters.Add(param3);
                            command1.CommandType = CommandType.StoredProcedure;
                            command1.ExecuteNonQuery();
                            con.Close();
                            success = true;
                            return "password Changed";
                        }
                    }
                    else
                    {
                        con.Close();
                        success = false;
                        return "Password Mismatch";
                    }
                }
                else
                {
                    con.Close();
                    success = false;
                    return "Please enter correct old password";
                }


            }
            else
            {
                con.Close();
                success = false;
                return "userId Not Found";
            }
        }

        public List<Transaction> customstatement(int acc, DateTime start, DateTime end)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sql = "datecheck";
            SqlCommand command = new SqlCommand(sql, con);
            SqlParameter param1 = new SqlParameter("@acc", acc);
            command.Parameters.Add(param1);
            DateTime Start = start.Date;
            SqlParameter param2 = new SqlParameter("@start", Start.ToString());
            command.Parameters.Add(param2);
            DateTime End = end.Date;
            SqlParameter param3 = new SqlParameter("@end", End.ToString());
            command.Parameters.Add(param3);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            int noofrows = ds.Tables[0].Rows.Count;
            int i = 0;
            List<Transaction> tlist = new List<Transaction>();
            while (noofrows != 0)
            {
                Transaction obj = new Transaction();
                noofrows--;
                obj.transactionId = long.Parse(ds.Tables[0].Rows[i]["transactionId"].ToString());
                obj.fromAccountNo = long.Parse(ds.Tables[0].Rows[i]["fromAccountNo"].ToString());
                obj.toAccountNo = long.Parse(ds.Tables[0].Rows[i]["toAccountNo"].ToString());
                obj.transactionDate = ds.Tables[0].Rows[i]["transactionDate"].ToString();
                obj.amount = int.Parse(ds.Tables[0].Rows[i]["amount"].ToString());
                obj.transactionType = ds.Tables[0].Rows[i]["transactiontype"].ToString();
                obj.comments = ds.Tables[0].Rows[i]["comments"].ToString();
                tlist.Add(obj);
                i++;
            }
            return tlist;
        }
    }
}
