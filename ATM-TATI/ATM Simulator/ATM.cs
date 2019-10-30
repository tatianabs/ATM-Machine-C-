using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ATM_Simulator
{
    class ATM     
    {
        /******** Fields ********/
        #region Fields
         
        static User u;//******** my static Field User reachable by the property ATM.U ***********
        static List<Account> listOfAllAccounts;//************** my static Field List<Account> reachable by the property ATM.ListOfAllAccounts *********

        

        #endregion

        /******** Properties ********/
        #region Properties

        public static User U
        {
            get
            {
                return u;
            }

            set
            {
                u = value;
            }
        }

        public string GetBankAmount()
        {
            string s = "";
            foreach (Account a in listOfAllAccounts)
            {
                if (a.AccountType=="B")
                {
                    s = a.Amount.ToString();
                    break;
                }
            }
            return s;
        }

        internal static List<Account> ListOfAllAccounts
        {
            get
            {
                return listOfAllAccounts;
            }

            set
            {
                listOfAllAccounts = value;
            }
        }

        #endregion

        /******** Controlers ********/
        #region Controler

        public ATM()// my empty controler
        { }

        public string PrintAllAccount()
        {
            string s = "";
            foreach (Account a in listOfAllAccounts)
            {
               s+= a.Tostring();                                    
                                                  
            }
            return s;
        }

        #endregion

        /******** Methods ********/
        #region Authentication method

        public bool Authentication(string username, string password)
        {
            bool success = false;//creating my bool for the method return
            try
            {
                StreamReader sr = new StreamReader("customers.txt");//Using the streamReader to read my file


                string line_customer;// string to receive each line of my file...
                while ((line_customer = sr.ReadLine()) != null)// while not end of file...
                {
                    string[] infos_User = line_customer.Split('|'); //I'm using | to separate my data in customers.txt

                    if (username == infos_User[0] && password == infos_User[1])//the user is authenticated
                    {
                        success = true; //authentication good...

                        // I have to fill my User u with these infos
                        U = new User(infos_User[0], infos_User[1], infos_User[2], infos_User[3]);//this is my user...
                                                                                                 //so my user have accounts... I have to fill the list of account based on my accountNumber

                        sr.Close();//I'm closing the first streamReader to be able to create another one with the same name on the next line

                        ReadAccountsInFile();// Go read all acounts and fill the listOfAllAccounts

                        break; // I have to stop the while because the authentication is good...
                    }
                    else
                    {
                        success = false; //authentication failed...
                    }
                }
            }
            catch (Exception)
            {

                success = false;
            }
            return success;
        }

        #endregion

        /******** Transactions ********/
        #region Transactions-Operations-ReadingFile-WritingFile

        /***************** Method used to read into accounts.txt file***********************************/
        public void ReadAccountsInFile()
        {
            try
            {
                StreamReader sr = new StreamReader("accounts.txt");//opening my accounts.txt file now...

                //I want to add all accounts to the listOfAllAccounts

                string line_account;
                List<Account> temp_list = new List<Account>();//creating a temporary list to make listOfAllAccount equal to that list at the end of the loop
                while ((line_account = sr.ReadLine()) != null)// while not end of file read the next account...
                {
                    string[] infos_Account = line_account.Split('|');//spliting the line in string array with the same separator ('|') AS IN THE WRITER 
                    Account a = new Account(infos_Account[0], infos_Account[1], double.Parse(infos_Account[2]));//creating account with infos from the txt file
                    temp_list.Add(a);// adding this account to the list
                }
                listOfAllAccounts = temp_list;// make the list equal to the temporary list
                sr.Close();// closing the reader....important could cause exception if the stream is not close because the file stay open for the nex time we use a stream
            }
            catch (Exception)
            {

                throw;
            }
        }

        /***************** Method used to write into accounts.txt file***********************************/
        public void WriteAccountsInFile()
        {
            try
            {
                FileStream fs = new FileStream("accounts.txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);

                foreach (Account a in ListOfAllAccounts)
                {
                    sw.WriteLine(a.AccountNumber + "|" + a.AccountType + "|" + a.Amount.ToString());//*********** Using the right separator to sava accounts in file ('|')
                }
                sw.Close();
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        /***************** Method used to make deposit and save the new amount into accounts.txt file***********************************/
        public bool Deposit(string type, string accountNumber, double amount)
        {
            bool success = false;
            for (int i = 0; i < ListOfAllAccounts.Count; i++)//find the account where to do the deposit into the list
            {
                if (ListOfAllAccounts[i].AccountNumber == accountNumber && ListOfAllAccounts[i].AccountType == type)
                {
                    ListOfAllAccounts[i].Amount += amount;
                    success = true;
                    break;// because we found the account... we won't do all the list for nothing
                }
            }
      
            try
            {
                WriteAccountsInFile();//calling the Writer to write down in accounts.txt the new balance...
                listOfAllAccounts.Clear();//clear the old list because there is a new balance now
                ReadAccountsInFile();//reload the new list from the file we just modified
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }


        /***************** Method used to withdraw and save the new amount into accounts.txt file***********************************/
        public bool Withdrawal(string type, string accountNumber, double amount)
        {
            bool success = false;
            for (int i = 0; i < ListOfAllAccounts.Count; i++)//we have to make two withdraws... Into the ATM Machine (Bank) and into the account
            {
                if(ListOfAllAccounts[i].AccountType == "B")//******* the bank amount *********
                {
                    ListOfAllAccounts[i].Amount -= amount;//minus the bank
                }
                if (ListOfAllAccounts[i].AccountNumber == accountNumber && ListOfAllAccounts[i].AccountType == type)//******* the account amount *******
                {
                    ListOfAllAccounts[i].Amount -= amount;//minus the account
                    success = true;
                }
            }

            //calling the Writer, the refresher and the reader to recreate the list who just got modified...
            try
            {
                WriteAccountsInFile();
                listOfAllAccounts.Clear();
                ReadAccountsInFile();
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }

        #endregion

        public bool Paybill(string type, string accountNumber, double amount)  //same thing that withdraw
        {
            bool success = false;
            Account fromAcc = null;

            for (int i = 0; i < ListOfAllAccounts.Count; i++)
            {

                if (ListOfAllAccounts[i].AccountNumber == accountNumber && ListOfAllAccounts[i].AccountType == type)
                {
                    fromAcc = ListOfAllAccounts[i];
                }
            }

            if(fromAcc.Amount < amount)
            {
                MessageBox.Show("This Account does not have enough funds.");
                return false;
            }

            fromAcc.Amount -= amount;
            success = true;

            try
            {
                WriteAccountsInFile();
                listOfAllAccounts.Clear();
                ReadAccountsInFile();
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }

        public bool TransferFunds(string type, string accountNumber, double amount)
        {
            Account fromAcc = null; // here I declared 2 variables of Account, because I need these 2 accounts to do the transactions
            Account toAcc = null;  // I put null to initialize it because I dont have an initial value for them yet.
            string toType = "S";

            if (type =="S")
            {
                toType = "C";
            }

            for (int i = 0; i < ListOfAllAccounts.Count; i++)
            {
                if (ListOfAllAccounts[i].AccountNumber == accountNumber && ListOfAllAccounts[i].AccountType == type)
                {
                    fromAcc = ListOfAllAccounts[i];
                }

                //get the account to the money will be transfer to
                if (ListOfAllAccounts[i].AccountNumber == accountNumber && ListOfAllAccounts[i].AccountType == toType)
                {
                    toAcc = ListOfAllAccounts[i];
                }
            }

            if (fromAcc.Amount < amount)
            {
                MessageBox.Show("This Account does not have enough funds.");
                return false;
            }
            
            fromAcc.Amount -= amount;
            toAcc.Amount += amount;

            try
            {
                WriteAccountsInFile();
                listOfAllAccounts.Clear();
                ReadAccountsInFile();
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        
        public bool RefillATMMachine()
        {
           

            bool success = false;
            for (int i = 0; i < listOfAllAccounts.Count; i++)   // for LOOP is good to SET the value inside, if you want to CHANGE something
            {


                if (listOfAllAccounts[i].AccountType == "B" )
                {
                    
                    if (listOfAllAccounts[i].Amount + 5000 > 20000)
                    {
                        success= false;
                    }
                    else
                    {
                        listOfAllAccounts[i].Amount += 5000;
                        success= true;
                    }

                }
                
            }
           
            try
            {
                WriteAccountsInFile();
                listOfAllAccounts.Clear();
                ReadAccountsInFile();
            }
            catch (Exception)
            {
                success = false;
            }
            return success;

        }

        public bool PayInterest()
        {
            bool success = false;

            for (int i = 0; i < listOfAllAccounts.Count; i++)
            {
                if (listOfAllAccounts[i].AccountType=="S")
                {
                    listOfAllAccounts[i].Amount += listOfAllAccounts[i].Amount* 0.01;
                }
            }
            try
            {
                WriteAccountsInFile();
                listOfAllAccounts.Clear();
                ReadAccountsInFile();
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            return success;

          
        }
        
    }
}

