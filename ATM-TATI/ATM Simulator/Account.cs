using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Simulator
{
    class Account
    {
      
        private string accountNumber;
        private string accountType;
        private double amount;
       
            //properties
        public string AccountNumber
        {
            get
            {
                return accountNumber;
            }

            set
            {
                accountNumber = value;
            }
        }

        
        public string AccountType
        {
            get
            {
                return accountType;
            }

            set
            {
                accountType = value;
            }
        }

        public double Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
            }
        }


        public  string Tostring()    
        {
            string accountsInformation = "\nAccountNumber: " + accountNumber + "\nAccountType: " + accountType + "\nAmount: " + amount + "\n\n";
            return accountsInformation;
        }

        //constructors

        public Account()
        {

        }
        public  Account( string _accountnumber,string _accounttype,double _amount)
        {
           
            accountNumber = _accountnumber;
            accountType = _accounttype;
            amount = _amount;
        }
    }
}
