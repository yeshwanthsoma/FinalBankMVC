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
        BankEntities2 dbContext = new BankEntities2();

        public string checkMedal(long acc)
        {
            BankEntities2 dbContext2 = new BankEntities2();
            int amt;
            CustomerClass obj = new CustomerClass();

            Account account = dbContext.Accounts.Where(val => val.accountNo == acc).Single<Account>();
            amt = (int)(account.amount);
            CustomerMedal medal = dbContext2.CustomerMedals.Where(val => val.min < amt && val.max > amt).Single<CustomerMedal>();
            account.type = medal.type;
            dbContext.SaveChanges();

            return account.type;
        }

        public bool checkAccount(long acc1)
        {
            try
            {
                Account acc = dbContext.Accounts.Where(val => val.accountNo == acc1).Single<Account>();
            }
            catch (Exception exe)
            {
                return false;
            }
            return true;
        }
 
        public int getAmount(long acc1)
        {
            try
            {
                Account account = dbContext.Accounts.Where(val => val.accountNo == acc1).Single<Account>();
                int amt = (int)(account.amount);
                return amt;
            }
            catch (Exception e)
            {
                return -100;
            }
        }
       
        public void transferAdd(int amount, long acc)
        {
            try
            {
                Account account = dbContext.Accounts.Where(val => val.accountNo == acc).Single<Account>();
                account.amount += amount;
                dbContext.SaveChanges();
            }
            catch (Exception exe)
            {

            }
        }
        
        public void transferSub(int amount, long acc)
        {
            try
            {
                Account account = dbContext.Accounts.Where(val => val.accountNo == acc).Single<Account>();
                account.amount -= amount;
                dbContext.SaveChanges();
            }
            catch (Exception exe)
            {

            }
        }
        public string Decrypt(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        public string Encrypt(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }

        public string changePassword(string old, string new1, string new2, string uid, out bool success)
        {
            try
            {
            Login login = dbContext.Logins.Where(val => val.userId == uid).Single<Login>();
            if(old ==Decrypt(login.password))
            {
                if (new1 == new2)
                {
                    login.password = Encrypt(new1);
                    dbContext.SaveChanges();
                    success = true;
                    return "password Changed";
                    
                }
                else
                {
                    success = false;
                    return "Password Mismatch";
                    
                }
            }
            else
            {
                success = false;
                return "Please enter correct old password";
                
            }

            }
            catch(Exception exe)
            {
                success = false;
                return "User not found";   
            }
        }

        public List<datecheck_Result> customstatement(int acc, DateTime start, DateTime end)
        {
            try
            {
               
               var transactionList = dbContext.datecheck(acc, start.ToString(), end.ToString());
               List<datecheck_Result> final = transactionList.ToList<datecheck_Result>();
               return final;
            }
            catch (Exception exe)
            {
                return null;
            }
        }
    }
}