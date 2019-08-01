﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;

namespace MVCPractice.Controllers
{
    public class ManagerController : Controller
    {
        //
        // GET: /Manager/
        
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult Withdraw()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Withdraw(long acc,int amt)
        {
            try
            {

                ManagerClass obj = new ManagerClass();

                string res = obj.withdraw(acc, amt);
                ViewBag.result = res;

            }

            catch (Exception exp)
            {
                ViewBag.Error = "Exception " + exp;
            }
            return View();
       
        }
        
        public ActionResult Deposit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Deposit(long acc, int amt)
        {
            try
            {

                ManagerClass obj = new ManagerClass();

                string res = obj.withdraw(acc, amt);
                ViewBag.result = res;

            }

            catch (Exception exp)
            {
                ViewBag.Error = "Exception " + exp;
            }
            return View();

        }
        public ActionResult Account()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Account(int customerId)
        {


            BankEntities1 dbContext = new BankEntities1();
            Session["customerId"] = customerId;
            string branchId = Session["Branch"].ToString();
            try
            {
                Customer allcustomer = dbContext.Customers.Single(x => x.customerId == customerId && x.branchId == branchId);
                List<Account> accounts = (dbContext.Accounts.Where(x => x.customerId == allcustomer.customerId)).ToList();
                return View(accounts);
            }
            catch(Exception e)
            {
                ViewBag.msg = "This is not your branch customer";
                return View("Account");
            }
           

        }
        
        public ActionResult addAccount(){

            return View();
        }
        
        public ActionResult deleteAccount()
        {
            ManagerClass obj = new ManagerClass();
            long selectedAccount = long.Parse(Session["EditedAccount"].ToString());
            obj.DeleteAccount(selectedAccount);
            return View("Account");

        }
        [HttpPost]
        public ActionResult addAccount(String AccountType, String Status, int AmountTextBox)
        {
            ManagerClass obj = new ManagerClass();
            String[] EnteredDatails=new String[7];
            EnteredDatails[0] = Session["customerId"].ToString();
            EnteredDatails[1]=AccountType;
            EnteredDatails[2] = Session["Today"].ToString();
            EnteredDatails[3] = Status;
            EnteredDatails[4] = Session["Today"].ToString();
            EnteredDatails[5] = "";
            EnteredDatails[6] = AmountTextBox.ToString();
            obj.AddAccount(EnteredDatails);

            return View();
        }
        [HttpPost]
        public ActionResult editAccount(long selectedAccount)
        {
            BankEntities1 dbContext = new BankEntities1();
           
            Session["EditedAccount"] = selectedAccount;
            ManagerClass obj = new ManagerClass();
            Account acc =dbContext.Accounts.Single(x => x.accountNo == selectedAccount);
            return View(acc);
        }
        [HttpPost]
        public ActionResult editAccounts(String AccountType, String Status, int amount)
        {
            ManagerClass obj = new ManagerClass();
            Account acc= new Account();
            acc.accountNo=long.Parse(Session["EditedAccount"].ToString());
            acc.accountType = AccountType;
            acc.status = Status;
            acc.dateOfEdited = Session["Today"].ToString();
            acc.ClosingDate = "";
            acc.amount = amount;
            obj.EditAccount(acc);

            return View("Account");
        }

        public ActionResult ManageCustomer()
        {
            if (TempData["deleteCustomer"] != null)
                ViewBag.deleteCustomer = TempData["deleteCustomer"].ToString();
            return View();
        }

        [HttpPost]
        public ActionResult ManageCustomer(int customerId)
        {
            BankEntities1 dbContext = new BankEntities1();
            string managerId = Session["userId"].ToString();
            Manager mgrDetails = dbContext.Managers.Single(x => x.userId == managerId);
            Customer objCheckUser = dbContext.Customers.SingleOrDefault(x => (x.customerId == customerId));
            if (objCheckUser == null) // No customer exists
            {
                ViewBag.Error = "No customer Exists";
            }
            else
            {
                if (objCheckUser.managerId != mgrDetails.managerId)
                {
                    ViewBag.Error = "You cannot access this customer";
                }
                else
                {
                    return View(objCheckUser);
                }  
            }
            ViewBag.customerId = customerId;
            return View();            
        }

        public ActionResult ShowAllCustomers()
        {
            BankEntities1 dbContext = new BankEntities1();
            string managerId = Session["userId"].ToString();
            Manager mgrDetails = dbContext.Managers.Single(x => x.userId == managerId);

            List<Customer> list = dbContext.Customers.Where(x => x.managerId == mgrDetails.managerId).ToList();
            return View(list);
        }

        public ActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCustomer(Customer custObj)
        {
            try
            {

                custObj.userId = custObj.email;
                custObj.createdDate = DateTime.Now.ToShortDateString();
                custObj.editedDate = DateTime.Now.ToShortDateString();
                //custObj.type = "Bronze";
                string managerId = Session["userId"].ToString();
                BankEntities1 dbContext = new BankEntities1();
                Manager mgrDetails = dbContext.Managers.Single(x => x.userId == managerId);
                custObj.managerId = mgrDetails.managerId;
                custObj.branchId = mgrDetails.branchId;

                ManagerClass classcustObj = new ManagerClass();
                classcustObj.addLogin(custObj.email, "123456", "Customer");
                int rows_affected = classcustObj.addCustomer(custObj);
                if (rows_affected == 0)
                {
                    ViewBag.Error = "Customer Not added";
                }
                else
                {
                    ModelState.Clear();
                    ViewBag.Success = "Customer Added";

                }


            }
            catch (Exception ex)
            {
                ViewBag.Error = "Exception";
            }
            return View();

        }

        public ActionResult EditCustomer(int id)
        {
            BankEntities1 dbContext = new BankEntities1();
            Customer obj = dbContext.Customers.Single(x => x.customerId == id);
            return View(obj);
        }

        [HttpPost]
        public ActionResult EditCustomer(Customer custObj)
        {
            try
            {
                custObj.customerId = int.Parse(RouteData.Values["id"].ToString());
                custObj.editedDate = DateTime.Now.ToShortDateString();
                BankEntities1 dbContext = new BankEntities1();
                ManagerClass classcustObj = new ManagerClass();


                int rows_affected = classcustObj.updateCustomer(custObj);
                if (rows_affected == 0)
                {
                    ViewBag.Error = "Customer Not updated";
                }
                else
                {
                    ModelState.Clear();
                    ViewBag.Success = "Customer Editted successfully";

                }


            }
            catch (Exception ex)
            {
                ViewBag.Error = "Exception";
            }
            return View();
        }

        public ActionResult DeleteCustomer(int id)
        {
            ManagerClass classCustObj = new ManagerClass();
            int rows_affected = classCustObj.deleteCustomer(id);
            if (rows_affected == 0)
            {
                TempData.Add("deleteCustomer", "Error while deleting customer");
                //ViewBag.deleteCustomer="Error while deleting customer";
            }
            else
            {
                TempData.Add("deleteCustomer", "Customer Deleted successfully");
                ViewBag.deleteCustomer = "Customer Deleted successfully";
            }
            return RedirectToAction("ManageCustomer");
        }
        
    }
}
