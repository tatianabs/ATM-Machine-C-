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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ATM_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int tries = 0;
        ATM atm;

        public MainWindow()
        {
            InitializeComponent();
            atm = new ATM();
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            ClearMethod();
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {

            //doing validation

            //if textboxes are not empty
            if (tb_username.Text.Length != 0 && tb_username.Text.Length != 0)
            {
                //validate user with a boolean method and open the appropriate window with the status of my user
                if (atm.Authentication(tb_username.Text, Passwrdbox_password.Password))
                {
                    switch (ATM.U.Status)
                    {
                        case "Admin":
                            WindowAdmin wa = new WindowAdmin();
                            wa.Show();
                            Hide();
                            break;
                        case "Customer":
                            WindowUsers wc = new WindowUsers();
                            wc.Show();
                           Hide();
                            break;
                        default:
                            MessageBox.Show("A problem just happened. Please try to log in again...");
                           break;
                    }
                }
                else
                {
                    
                    MessageBox.Show("Your username and your password don't match. Please try again...");
                    ClearMethod();
                    tries++;
                    if (tries == 3)
                    {
                        MessageBox.Show("You have tried 3 times already, please try again later!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Your username and your password cannot be empty. Please try again...");
                if (tb_username.Text.Length == 0)
                {
                    tb_username.Focus();
                }
                else
                {
                    Passwrdbox_password.Focus();
                }
            }
        }




        
        private void ClearMethod()
        {
            tb_username.Clear();
            Passwrdbox_password.Clear();
            tb_username.Focus();
        }

        private void Prevent_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
         
            if ((ATM.U== null) ||   (ATM.U.Status != "Admin"))
            {
                MessageBox.Show(" I'm sorry, but only the supervisor can close the ATM machine.");
                e.Cancel = true;
            }

        }

        private void Prevent_Window_Closing_onbuttton(object sender, RoutedEventArgs e)
        {
            if ((ATM.U == null) || (ATM.U.Status != "Admin"))
            {
                MessageBox.Show(" I'm sorry, but only the supervisor can close the ATM machine.");
                
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
    }

 }

