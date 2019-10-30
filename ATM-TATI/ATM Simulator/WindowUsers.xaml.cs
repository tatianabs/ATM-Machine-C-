using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace ATM_Simulator
{
    /// <summary>
    /// Interaction logic for WindowUsers.xaml
    /// </summary>
    public partial class WindowUsers : Window
    {
       
        ATM atm;
        
        public WindowUsers()
        {
            InitializeComponent();
             atm = new ATM();
        }

        //************** WHEN THE WINDOWS IS LOADED THE INFOS FOR THE TEXTBOX AND LABEL ARE CREATED***************************/
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //setting the labels with the infos of my customer...the user
            lbl_welcome.Content += " " + ATM.U.Username;
            tb_check_display.Text = MyCheckingBalance();//here... call to a method to set the checking balance amount with dollar sign $
            tb_sav_display.Text = MySavingBalance();//here... call to a method to set the saving balance amount with dollar sign $
            tb_amount_keypad.Text = "0.00"; //Initializing the amount in the textbox
        }


        #region Methods

        //*************** This method is used to populate the textbox tb_check_display with the amount of the account checking of the user************/
        private string MyCheckingBalance()// each time I want to refresh my checking amount I can call this method at the end of a transaction
        {
            string s = "";
            foreach (Account a in ATM.ListOfAllAccounts)
            {
                if (a.AccountNumber == ATM.U.AccountNumber && a.AccountType == "C")
                {
                    s = a.Amount.ToString() + " $";
                    break;
                }
            }
            return s;
        }

        //*************** This method is used to populate the textbox tb_sav_display with the amount of the account saving of the user************/
        private string MySavingBalance()// each time I want to refresh my saving amount I can call this method at the end of a transaction
        {
            string s = "";
            foreach (Account a in ATM.ListOfAllAccounts)
            {
                if (a.AccountNumber == ATM.U.AccountNumber && a.AccountType == "S")
                {
                    s = a.Amount.ToString() + " $";
                    break;
                }
            }
            return s;
        }

        /********************** This method refresh both my textbox showing the saving and the checking amount using 2 methods *******************/
        private void RefreshBalances()
        {
            tb_check_display.Text = MyCheckingBalance();
            tb_sav_display.Text = MySavingBalance();
        }

      
        
        private bool ValidateAmount(string amountInText)
        {
            double amount = 0;
            if (amountInText.Length <= 1) // here... if the amount is a number of at least one in lenght and it's different than 0 = it's valid
            {
                MessageBox.Show("Wrong Amount");
                return false;
            }

            if  (! double.TryParse(amountInText, out amount))
            {
                MessageBox.Show("Wrong Amount");
                return false;
            }

            if (amount == 0)
            {
                MessageBox.Show("Wrong Amount");
                return false;
            }

            //withdrawal
            if (rdbtn_withdrawal.IsChecked.Value)
            {
                if (amount % 10 != 0)
                {
                    MessageBox.Show("Must be 10s");
                    return false;
                }

                if (amount > 1000)
                {
                    MessageBox.Show("You can not withdraw more than $1000");
                    return false;
                }
            }

            if (rdbtn_pay_bill.IsChecked.Value)
            {
                if (amount > 10000)
                {
                    MessageBox.Show("You can not pay a bill of over $10000");
                    return false;
                }
            }

            return true;
        }
        #endregion


        #region Buttons events

        //********here each number in the keypad call the same method****************
        private void NumberClick(object sender, RoutedEventArgs e)//the Object sender in the parameters is in fact, the button who trigger this event click... ex: btn_4 or btn_8
        {
            if(tb_amount_keypad.Text == "0.00")
            {
                tb_amount_keypad.Text = ((Button)sender).Content.ToString();// here the textbox contains the initial value 0.00 so I have to change it for the number using just the = sign not the +=
            }
            else
            {
                tb_amount_keypad.Text += ((Button)sender).Content.ToString();//here the tb_amount_keypad.Text adding the content of the Button named sender to use it globally... the button who triggered the action

            }
        }
        //*********** here the bnt_dot click********************/
        private void btn_dot_Click(object sender, RoutedEventArgs e)
        {
            //checking first if the tb_amount_keypas.Text already contains a dot... allow only one dot by amount
            if(tb_amount_keypad.Text.IndexOf('.') != -1)//this method IndexOf return the index of the char '.' in the string... If there is no dot, the method return the value -1 
            {
                //do nothing...only one dot by amount so we skip the add text instruction
            }
            else
            {
                //adding the dot to the text
                tb_amount_keypad.Text += ".";
            }
            
        }


        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateAmount(tb_amount_keypad.Text))// We have to validate the amount before even thinking to do a transaction...
            {
                double amount = double.Parse(tb_amount_keypad.Text);// now I can parse it into a double because I validated it...

                // First, we have to know in which account we want to do the transaction .... accountType
                string type = "";
                if ((bool)rdbtn_checking.IsChecked)
                {
                    type = "C";
                }
                else
                {
                    type = "S";
                }

                // ****** Now, we have to choose which transaction we have to do ***********

                if ((bool)rdbtn_deposit.IsChecked)
                {
                    if (atm.Deposit(type, ATM.U.AccountNumber, amount))
                    {
                        MessageBox.Show("Your deposit has been saved... Please choose another transaction or quit. Thank you!");
                        RefreshBalances();
                        tb_amount_keypad.Text = "0.00";
                    }
                    else
                    {
                        MessageBox.Show("Something goes wrong with your deposit, it hasn't been saved... Please try again or quit. Thank you!");
                    }
                }
                else if ((bool)rdbtn_withdrawal.IsChecked)
                {
                    /************** IMPORTANT WE HAVE TO CHECK THE AMOUNT BEFORE... CANNOT WITHDRAWN MONEY IF THE BANK DOESN'T HAVE IT ************/
                    foreach (Account b in ATM.ListOfAllAccounts)
                    {
                        if (b.AccountType == "B")//having the BANK account
                        {
                            if (b.Amount < amount)//checking the availability of money in the BANK
                            {
                                MessageBox.Show("There is not enough money in the ATM Machine. Please try again later. Thank you.");
                            }
                            else// we can proceed to the transaction
                            {
                                /************** IMPORTANT WE HAVE TO CHECK THE AMOUNT BEFORE... CANNOT WITHDRAWN MONEY IF THE ACCOUNT DOESN'T HAVE IT ************/
                                foreach (Account a in ATM.ListOfAllAccounts)
                                {
                                    if (a.AccountNumber == ATM.U.AccountNumber && a.AccountType == type)//having the right account
                                    {
                                        if (a.Amount < amount)//checking the availability of money in the account
                                        {
                                            MessageBox.Show("There is not enough money in your account to withdraw this amount. Please try again with a smaller amount. Thank you.");
                                        }
                                        else// we can proceed to the transaction
                                        {
                                            if (atm.Withdrawal(type, ATM.U.AccountNumber, amount))
                                            {
                                                MessageBox.Show("Your withdrawal has been saved... Please choose another transaction or quit. Thank you!");
                                                RefreshBalances();

                                            }
                                            else
                                            {
                                                MessageBox.Show("Something goes wrong with your withdrawal... Please try again or quit. Thank you!");
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                            tb_amount_keypad.Text = "0.00";
                            break;
                        }
                    }
                }
                else if ((bool)rdbtn_transfer_funds.IsChecked)
                {
                    if (atm.TransferFunds(type, ATM.U.AccountNumber, amount))
                    {
                        MessageBox.Show("Your transfer has been saved... Please choose another transaction or quit. Thank you!");
                        RefreshBalances();
                        tb_amount_keypad.Text = "0.00";
                    }
                    else
                    {
                        MessageBox.Show("Something goes wrong with your transfer, it hasn't been saved... Please try again or quit. Thank you!");
                    }
                }
                else if ((bool)rdbtn_pay_bill.IsChecked && type=="C")
                {
                    MessageBox.Show("The fee for this transaction is going to be $1.25");

                    amount+=1.25;   // I need to add the $1.25 fee  in the amount //
                    foreach (Account b in ATM.ListOfAllAccounts)
                    {
                        if (b.AccountType == "B")//having the BANK account
                        {
                            if (b.Amount < amount)//checking the availability of money in the BANK
                            {
                                MessageBox.Show("There is not enough money in the ATM Machine. Please try again later. Thank you.");
                            }
                            else// we can proceed to the transaction
                            {
                                /************** IMPORTANT WE HAVE TO CHECK THE AMOUNT BEFORE... CANNOT WITHDRAWN MONEY IF THE ACCOUNT DOESN'T HAVE IT ************/
                                foreach (Account a in ATM.ListOfAllAccounts)
                                {
                                    if (a.AccountNumber == ATM.U.AccountNumber && a.AccountType == type)//having the right account
                                    {
                                        if (a.Amount < amount)//checking the availability of money in the account
                                        {
                                            MessageBox.Show("There is not enough money in your account to withdraw this amount. Please try again with a smaller amount. Thank you.");
                                        }
                                        else// we can proceed to the transaction
                                        {
                                            if (atm.Paybill(type, ATM.U.AccountNumber, amount))
                                            {
                                                MessageBox.Show("Your transaction has been saved... Please choose another transaction or quit. Thank you!");
                                                RefreshBalances();

                                            }
                                            else
                                            {
                                                MessageBox.Show("Something goes wrong with your transaction... Please try again or quit. Thank you!");
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                            tb_amount_keypad.Text = "0.00";
                            break;
                        }
                    }
                   
                }
                else
                {
                    MessageBox.Show("You can not pay a bill from your savings account");
                }
            }
            else
            {
                MessageBox.Show("Your amount is not valid or you are trying to withdraw an amount not divisible by 10$... Please correct it.");
                tb_amount_keypad.Text = "0.00";
            }
        }


        #endregion

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            tb_amount_keypad.Text = "0.00";
        }

        private void Win_Users_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
        }
    }
}
