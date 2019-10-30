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
    /// Interaction logic for WindowAdmin.xaml
    /// </summary>
    public partial class WindowAdmin : Window
    {
        ATM atm = new ATM();
        public WindowAdmin()
        {
            InitializeComponent();
            lbl_bank_amount.Content =GetBankAmount();
            
        }

        private string GetBankAmount()
        {
            return "Bank Amount: " + atm.GetBankAmount() + " $";
        }

        private void btn_pay_interest_Click(object sender, RoutedEventArgs e)
        {
            if (atm.PayInterest())
            {
                MessageBox.Show(" The interest has been saved to all savings accounts");
            }
            
        }

        private void btn_Refill_atm_Click(object sender, RoutedEventArgs e)
        {
            if (atm.RefillATMMachine())
            {
                MessageBox.Show("The ATM machine has been  filled with $5000");
            }
            else
            {
                MessageBox.Show("You can not refiil the ATM machine");
            }
            lbl_bank_amount.Content = GetBankAmount();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_print_acc_report_Click(object sender, RoutedEventArgs e)
        {
            tb_all_accounts.Text = atm.PrintAllAccount();
        }

        private void btn_out_of_service_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
