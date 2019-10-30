using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Simulator
{
    class User    //mymodel
    {

      
       string username;
       string pin;
       string status;
       string accountNumber;

        List<Account> accountslist = new List<Account>();
      

        public string Username
        {
            get
            {
                return username;
            }

            set
            {
                username = value;
            }
        }

        public string Pin
        {
            get
            {
                return pin;
            }

            set
            {
                pin = value;
            }
        }

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

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

       internal List<Account> AccountsList
        {
            get
            {
                return accountslist;
            }

            set
            {
                accountslist = value;
            }
        }
        public User()
        {

        }

        public User( string _username, string _pin, string _status,string _accountnumber)
        {
            Username = _username;
            Pin = _pin;
            Status = _status;
            AccountNumber = _accountnumber;
        }
    }
}
        
    

