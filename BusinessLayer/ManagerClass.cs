using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLayer
{
    public class ManagerClass
    {
        String ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
        public string getBranch(string userId)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sql = "getBranch";

            SqlCommand command = new SqlCommand(sql, con);
            SqlParameter param1 = new SqlParameter("@userId", userId);


            command.Parameters.Add(param1);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);
            string branchId = ds.Tables[0].Rows[0]["branchId"].ToString();
            return branchId;

        }
        public string withdraw(long acc, int amt)
        {
            string comment = "withdraw done";
            string type = "withdraw";
           TransactionClass obj1 = new TransactionClass();
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sql = "checkAcc";

            SqlCommand command = new SqlCommand(sql, con);
            SqlParameter param1 = new SqlParameter("@acc", acc);


            command.Parameters.Add(param1);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int res, res1;
            if (ds.Tables[0].Rows.Count > 0)
            {
                res = (int)ds.Tables[0].Rows[0]["customerId"];
                string sql1 = "checkAmo";

                SqlCommand command1 = new SqlCommand(sql1, con);
                SqlParameter param2 = new SqlParameter("@acc", acc);


                command1.Parameters.Add(param2);
                command1.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da1 = new SqlDataAdapter(command1);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    res1 = (int)ds1.Tables[0].Rows[0]["amount"];
                    if (res1 >= amt)
                    {
                        string sql3 = "withdraw";

                        SqlCommand command2 = new SqlCommand(sql3, con);
                        SqlParameter param3 = new SqlParameter("@acc", acc);


                        command2.Parameters.Add(param3);
                        SqlParameter param4 = new SqlParameter("@amt", amt);


                        command2.Parameters.Add(param4);

                        command2.CommandType = CommandType.StoredProcedure;
                        command2.ExecuteNonQuery();
                    }
                    else
                    {
                        return "amountNotSufficient";
                    }
                }
            }
            else
            {
                return "accountNotFound";
            }

            obj1.insTrans(acc, acc, amt, type, comment);
            return "success";

        }
        public string deposit(long acc, int amt)
        {
            string comment = "deposit done";
            string type = "deposit";
            TransactionClass obj1 = new TransactionClass();
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sql = "checkAcc";

            SqlCommand command = new SqlCommand(sql, con);
            SqlParameter param1 = new SqlParameter("@acc", acc);


            command.Parameters.Add(param1);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);
            //int res, res1;
            if (ds.Tables[0].Rows.Count > 0)
            {

                string sql3 = "deposit";

                SqlCommand command2 = new SqlCommand(sql3, con);
                SqlParameter param3 = new SqlParameter("@acc", acc);


                command2.Parameters.Add(param3);
                SqlParameter param4 = new SqlParameter("@amt", amt);


                command2.Parameters.Add(param4);

                command2.CommandType = CommandType.StoredProcedure;
                command2.ExecuteNonQuery();



            }
            else
            {
                con.Close();
                return "accountNotFound";

            }
            obj1.insTrans(acc, acc, amt, type, comment);
            
            return "success";

        }
        public bool AddAccount(string[] EnteredDetails)
        {
            int res = -100, result = 0;
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sql = "checkMedal";
            SqlCommand command2 = new SqlCommand(sql, con);
            SqlParameter param3 = new SqlParameter("@amt", EnteredDetails[6]);
            command2.Parameters.Add(param3);

            command2.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da2 = new SqlDataAdapter(command2);
            DataSet ds2 = new DataSet();
            da2.Fill(ds2);
            String type=ds2.Tables[0].Rows[0][0].ToString();

            

            SqlCommand cmd1 = new SqlCommand("checkCust", con);
            cmd1.Parameters.AddWithValue("@customerId", EnteredDetails[0]);
            cmd1.CommandType = System.Data.CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {

                SqlCommand cmd = new SqlCommand("addDetails", con);


                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@customerId", EnteredDetails[0]);
                cmd.Parameters.AddWithValue("@accountType", EnteredDetails[1]);
                cmd.Parameters.AddWithValue("@DateOfOpen", EnteredDetails[2]);
                cmd.Parameters.AddWithValue("@status", EnteredDetails[3]);
                cmd.Parameters.AddWithValue("@dateOfEdited", EnteredDetails[4]);
                cmd.Parameters.AddWithValue("@ClosingDate", EnteredDetails[5]);
                
                
                cmd.Parameters.AddWithValue("@amount", EnteredDetails[6]);
                cmd.Parameters.AddWithValue("@type", type);



                 result = cmd.ExecuteNonQuery();
                 SqlCommand cmd3 = new SqlCommand("checkCust", con);
                 




                con.Close();

            }

            else
            {
                return false;
            }
            return true;


        }


        public int addLogin(string userId, string password, string userType)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            string procedure_name = "addLogin";
            SqlCommand command = new SqlCommand(procedure_name, conn);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter id = new SqlParameter("@userId", userId);
            command.Parameters.Add(id);
            SqlParameter pass = new SqlParameter("@password", password);
            command.Parameters.Add(pass);
            SqlParameter role = new SqlParameter("@role", userType);
            command.Parameters.Add(role);
            int rows_affected = command.ExecuteNonQuery();
            conn.Close();
            return rows_affected;
        }

        public int addCustomer(Customer customer)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            string procedure_name = "addCustomer";
            SqlCommand command = new SqlCommand(procedure_name, conn);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter _name = new SqlParameter("@custName", customer.customerName);
            command.Parameters.Add(_name);
            SqlParameter _gender = new SqlParameter("@gender", customer.gender);
            command.Parameters.Add(_gender);
            SqlParameter _dob = new SqlParameter("@dob", customer.dob);
            command.Parameters.Add(_dob);
            SqlParameter _address = new SqlParameter("@state", customer.state);
            command.Parameters.Add(_address);
            SqlParameter _state = new SqlParameter("@address", customer.address);
            command.Parameters.Add(_state);


            SqlParameter _city = new SqlParameter("@city", customer.city);
            command.Parameters.Add(_city);
            SqlParameter _pincode = new SqlParameter("@pincode", customer.pincode);
            command.Parameters.Add(_pincode);
            SqlParameter _phoneNo = new SqlParameter("@phoneNo", customer.phoneNo);
            command.Parameters.Add(_phoneNo);
            SqlParameter _email = new SqlParameter("@email", customer.email);
            command.Parameters.Add(_email);
            SqlParameter _createdDate = new SqlParameter("@createdDate", customer.createdDate);
            command.Parameters.Add(_createdDate);
            SqlParameter _editedDate = new SqlParameter("@editedDate", customer.editedDate);
            command.Parameters.Add(_editedDate);
            SqlParameter _userId = new SqlParameter("@userId", customer.userId);
            command.Parameters.Add(_userId);
            SqlParameter _branchId = new SqlParameter("@branchId", customer.branchId);
            command.Parameters.Add(_branchId);
            SqlParameter _managerId = new SqlParameter("@managerId", customer.managerId);
            command.Parameters.Add(_managerId);
            int rows_affected = command.ExecuteNonQuery();
            conn.Close();
            return rows_affected;
        }

        public int updateCustomer(Customer customer)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            string procedure_name = "updateCustomer";
            SqlCommand command = new SqlCommand(procedure_name, conn);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter id = new SqlParameter("@custId", customer.customerId);
            command.Parameters.Add(id);
            SqlParameter _name = new SqlParameter("@custName", customer.customerName);
            command.Parameters.Add(_name);
            SqlParameter _gender = new SqlParameter("@gender", customer.gender);
            command.Parameters.Add(_gender);
            SqlParameter _dob = new SqlParameter("@dob", customer.dob);
            command.Parameters.Add(_dob);
            SqlParameter _state = new SqlParameter("@address", customer.dob);
            command.Parameters.Add(_state);
            SqlParameter _address = new SqlParameter("@state", customer.state);
            command.Parameters.Add(_address);

            SqlParameter _city = new SqlParameter("@city", customer.city);
            command.Parameters.Add(_city);
            SqlParameter _pincode = new SqlParameter("@pincode", customer.pincode);
            command.Parameters.Add(_pincode);
            SqlParameter _phoneNo = new SqlParameter("@phoneNo", customer.phoneNo);
            command.Parameters.Add(_phoneNo);


            SqlParameter _editedDate = new SqlParameter("@editedDate", customer.editedDate);
            command.Parameters.Add(_editedDate);

            int rows_affected = command.ExecuteNonQuery();
            conn.Close();
            return rows_affected;
        }


        public int deleteCustomer(int custId)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            string procedure_name = "deleteCustomer";
            SqlCommand command = new SqlCommand(procedure_name, conn);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter id = new SqlParameter("@custId", custId);
            command.Parameters.Add(id);
            int rows_affected = command.ExecuteNonQuery();
            conn.Close();
            return rows_affected;
        }




        //public int addCustomer(Customer customer)
        //{
        //    SqlConnection conn = new SqlConnection(ConnectionString);
        //    conn.Open();
        //    string procedure_name = "addCustomer";
        //    SqlCommand command = new SqlCommand(procedure_name, conn);
        //    command.CommandType = System.Data.CommandType.StoredProcedure;
        //    SqlParameter _name = new SqlParameter("@custName", customer.CustomerName);
        //    command.Parameters.Add(_name);
        //    SqlParameter _gender = new SqlParameter("@gender", customer.Gender);
        //    command.Parameters.Add(_gender);
        //    SqlParameter _dob = new SqlParameter("@dob", customer.Dob);
        //    command.Parameters.Add(_dob);
        //    SqlParameter _address = new SqlParameter("@state", customer.State);
        //    command.Parameters.Add(_address);
        //    SqlParameter _state = new SqlParameter("@address", customer.Dob);
        //    command.Parameters.Add(_state);
        //    SqlParameter _city = new SqlParameter("@city", customer.City);
        //    command.Parameters.Add(_city);
        //    SqlParameter _pincode = new SqlParameter("@pincode", customer.Pincode);
        //    command.Parameters.Add(_pincode);
        //    SqlParameter _phoneNo = new SqlParameter("@phoneNo", customer.PhoneNo);
        //    command.Parameters.Add(_phoneNo);
        //    SqlParameter _email = new SqlParameter("@email", customer.Email);
        //    command.Parameters.Add(_email);
        //    SqlParameter _createdDate = new SqlParameter("@createdDate", customer.CreatedDate);
        //    command.Parameters.Add(_createdDate);
        //    SqlParameter _editedDate = new SqlParameter("@editedDate", customer.EditedDate);
        //    command.Parameters.Add(_editedDate);
        //    SqlParameter _userId = new SqlParameter("@userId", customer.UserID);
        //    command.Parameters.Add(_userId);
        //    int rows_affected = command.ExecuteNonQuery();
        //    conn.Close();
        //    return rows_affected;
        //}
        public List<BusinessLayer.Customer> getSpecificCustomer(int custId)
        {

            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            //string procedure_name = "getSpecificCustomer";
            SqlCommand command = new SqlCommand("select * from Customer where customerId = " + custId, conn);

            SqlDataAdapter reader = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            BusinessLayer.Customer cobj = new BusinessLayer.Customer();
            reader.Fill(ds);
            conn.Close();
            List<BusinessLayer.Customer> clist = new List<BusinessLayer.Customer>();
            int noofrows = ds.Tables[0].Rows.Count;

            int i = 0;
            while (noofrows != 0)
            {
                BusinessLayer.Customer obj = new BusinessLayer.Customer();
                noofrows--;

                obj.customerId = int.Parse(ds.Tables[0].Rows[i]["CustomerID"].ToString());
                obj.customerName = ds.Tables[0].Rows[i]["CustomerName"].ToString();
                obj.gender = ds.Tables[0].Rows[i]["Gender"].ToString().ElementAt(0).ToString();
                obj.dob = ds.Tables[0].Rows[i]["Dob"].ToString();
                obj.address = (ds.Tables[0].Rows[i]["Address"].ToString());
                obj.state = ds.Tables[0].Rows[i]["State"].ToString();
                obj.pincode = ds.Tables[0].Rows[i]["Pincode"].ToString();
                obj.phoneNo = ds.Tables[0].Rows[i]["PhoneNo"].ToString();
                obj.email = ds.Tables[0].Rows[i]["Email"].ToString();
                obj.createdDate = ds.Tables[0].Rows[i]["CreatedDate"].ToString();
                obj.editedDate = ds.Tables[0].Rows[i]["EditedDate"].ToString();
                obj.userId = ds.Tables[0].Rows[i]["UserID"].ToString();
                clist.Add(obj);
                i++;
            }
            return clist;



        }

        public bool EditAccount(Account selectedAccount)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sql = "checkMedal";
            SqlCommand command2 = new SqlCommand(sql, con);
            SqlParameter param3 = new SqlParameter("@amt", selectedAccount.amount);
            command2.Parameters.Add(param3);

            command2.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da2 = new SqlDataAdapter(command2);
            DataSet ds2 = new DataSet();
            da2.Fill(ds2);
            String type = ds2.Tables[0].Rows[0][0].ToString();
            
            SqlCommand cmd = new SqlCommand("editedDetails", con);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@accountNo", selectedAccount.accountNo);
            
            cmd.Parameters.AddWithValue("@accountType", selectedAccount.accountType);
            
            cmd.Parameters.AddWithValue("@status", selectedAccount.status);
            cmd.Parameters.AddWithValue("@dateOfEdited", selectedAccount.dateOfEdited);
            cmd.Parameters.AddWithValue("@ClosingDate", selectedAccount.ClosingDate);
            cmd.Parameters.AddWithValue("@amount", selectedAccount.amount);
            cmd.Parameters.AddWithValue("@type", type);


            
            int result = cmd.ExecuteNonQuery();
            con.Close();

            if (result == 0)
            {
                return false;
            }

            else
            {
                return true;
            }
        }
        public bool DeleteAccount(long accountNoToDelete)
        {
            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmd = new SqlCommand("deleteDetails", con);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@accountNo", accountNoToDelete);

            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();

            if (result == 0)
            {
                return false;
            }

            else
            {
                return true;
            }
        }
        //public int deleteCustomer(int custId)
        //{
        //    SqlConnection conn = new SqlConnection(ConnectionString);
        //    conn.Open();
        //    string procedure_name = "deleteCustomer";
        //    SqlCommand command = new SqlCommand(procedure_name, conn);
        //    command.CommandType = System.Data.CommandType.StoredProcedure;
        //    SqlParameter id = new SqlParameter("@custId", custId);
        //    command.Parameters.Add(id);
        //    int rows_affected = command.ExecuteNonQuery();
        //    conn.Close();
        //    return rows_affected;
        //}
        //public int updateCustomer(customer customer)
        //{
        //    SqlConnection conn = new SqlConnection(ConnectionString);
        //    conn.Open();
        //    string procedure_name = "updateCustomer";
        //    SqlCommand command = new SqlCommand(procedure_name, conn);
        //    command.CommandType = System.Data.CommandType.StoredProcedure;
        //    SqlParameter id = new SqlParameter("@custId", customer.CustomerID);
        //    command.Parameters.Add(id);
        //    SqlParameter _name = new SqlParameter("@custName", customer.CustomerName);
        //    command.Parameters.Add(_name);
        //    SqlParameter _gender = new SqlParameter("@gender", customer.Gender);
        //    command.Parameters.Add(_gender);
        //    SqlParameter _dob = new SqlParameter("@dob", customer.Dob);
        //    command.Parameters.Add(_dob);
        //    SqlParameter _address = new SqlParameter("@state", customer.State);
        //    command.Parameters.Add(_address);
        //    SqlParameter _state = new SqlParameter("@address", customer.Dob);
        //    command.Parameters.Add(_state);
        //    SqlParameter _city = new SqlParameter("@city", customer.City);
        //    command.Parameters.Add(_city);
        //    SqlParameter _pincode = new SqlParameter("@pincode", customer.Pincode);
        //    command.Parameters.Add(_pincode);
        //    SqlParameter _phoneNo = new SqlParameter("@phoneNo", customer.PhoneNo);
        //    command.Parameters.Add(_phoneNo);
        //    SqlParameter _email = new SqlParameter("@email", customer.Email);
        //    command.Parameters.Add(_email);
        //    SqlParameter _editedDate = new SqlParameter("@editedDate", customer.EditedDate);
        //    command.Parameters.Add(_editedDate);
        //    SqlParameter _userId = new SqlParameter("@userId", customer.UserID);
        //    command.Parameters.Add(_userId);
        //    int rows_affected = command.ExecuteNonQuery();
        //    conn.Close();
        //    return rows_affected;
        //}
        //public int addLogin(string userId, string password, string userType)
        //{
        //    SqlConnection conn = new SqlConnection(ConnectionString);
        //    conn.Open();
        //    string procedure_name = "addLogin";
        //    SqlCommand command = new SqlCommand(procedure_name, conn);
        //    command.CommandType = System.Data.CommandType.StoredProcedure;
        //    SqlParameter id = new SqlParameter("@userId", userId);
        //    command.Parameters.Add(id);
        //    SqlParameter pass = new SqlParameter("@password", password);
        //    command.Parameters.Add(pass);
        //    SqlParameter role = new SqlParameter("@role", userType);
        //    command.Parameters.Add(role);
        //    int rows_affected = command.ExecuteNonQuery();
        //    conn.Close();
        //    return rows_affected;
        //}
        //public List<customer> showAll()
        //{
        //    SqlConnection conn = new SqlConnection(ConnectionString);
        //    conn.Open();
        //    SqlCommand command = new SqlCommand("select * from Customer", conn);
        //    SqlDataAdapter reader = new SqlDataAdapter(command);
        //    DataSet ds = new DataSet();
        //    reader.Fill(ds);
        //    List<customer> showobj = new List<customer>();
        //    conn.Close();
        //    List<customer> clist = new List<customer>();
        //    int noofrows = ds.Tables[0].Rows.Count;

        //    int i = 0;
        //    while (noofrows != 0)
        //    {
        //        customer obj = new customer();
        //        noofrows--;

        //        obj.CustomerID = int.Parse(ds.Tables[0].Rows[i]["CustomerID"].ToString());
        //        obj.CustomerName = ds.Tables[0].Rows[i]["CustomerName"].ToString();
        //        obj.Gender = ds.Tables[0].Rows[i]["Gender"].ToString().ElementAt(0);
        //        obj.Dob = ds.Tables[0].Rows[i]["Dob"].ToString();
        //        obj.Address = (ds.Tables[0].Rows[i]["Address"].ToString());
        //        obj.State = ds.Tables[0].Rows[i]["State"].ToString();
        //        obj.Pincode = ds.Tables[0].Rows[i]["Pincode"].ToString();
        //        obj.PhoneNo = ds.Tables[0].Rows[i]["PhoneNo"].ToString();
        //        obj.Email = ds.Tables[0].Rows[i]["Email"].ToString();
        //        obj.CreatedDate = ds.Tables[0].Rows[i]["CreatedDate"].ToString();
        //        obj.EditedDate = ds.Tables[0].Rows[i]["EditedDate"].ToString();
        //        obj.UserID = ds.Tables[0].Rows[i]["UserID"].ToString();
        //        clist.Add(obj);
        //        i++;
        //    }
        //    return clist;
        //}
        //public int updateUserId(string newUserID, string oldUserID)
        //{
        //    SqlConnection conn = new SqlConnection(ConnectionString);
        //    conn.Open();
        //    string procedure_name = "updateUserID";
        //    SqlCommand command = new SqlCommand(procedure_name, conn);
        //    command.CommandType = System.Data.CommandType.StoredProcedure;
        //    SqlParameter newId = new SqlParameter("@newuserId", newUserID);
        //    command.Parameters.Add(newId);
        //    SqlParameter oldId = new SqlParameter("@olduserId", oldUserID);
        //    command.Parameters.Add(oldId);
        //    int rows_affected = command.ExecuteNonQuery();
        //    return rows_affected;
        //}
    }
}