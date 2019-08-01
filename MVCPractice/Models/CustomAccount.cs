using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessLayer;

namespace MVCPractice.Models
{
    public class CustomAccount
    {
        public string selectedAccount { get; set; }

        public List<Account> Accounts
        {
            get
            {
                BankEntities1 db = new BankEntities1();
                return db.Accounts.ToList();
            }
        }
    }
        public enum states
        {
            AndhraPradesh,
            Telangana,
            WestBengal,
            TamilNadu,
            Kerala,
            MadhyaPradesh
        }
    }
