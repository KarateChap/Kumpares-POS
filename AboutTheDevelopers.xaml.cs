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

namespace KumparesFinal
{
    /// <summary>
    /// Interaction logic for AboutTheDevelopers.xaml
    /// </summary>
    public partial class AboutTheDevelopers : Window
    {
        public AboutTheDevelopers()
        {
            InitializeComponent();
        }
        private void MainListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainListView.SelectedIndex == 0)
            {
                AdminInterface adminInterfaceWindow = new AdminInterface();
                adminInterfaceWindow.Show();
                this.Hide();
            }
            else if (MainListView.SelectedIndex == 1)
            {
                ProductManagement productManagementWindow = new ProductManagement();
                productManagementWindow.Show();
                this.Hide();
            }
            else if (MainListView.SelectedIndex == 2)
            {
                CashierManagement cashierManagementWindow = new CashierManagement();
                cashierManagementWindow.Show();
                this.Hide();
            }
            else if (MainListView.SelectedIndex == 3)
            {
                TransactionHistory transactionHistoryInterface = new TransactionHistory();
                transactionHistoryInterface.Show();
                this.Hide();
            }
            else if (MainListView.SelectedIndex == 5)
            {
                MainWindow mainWindowInterface = new MainWindow();
                mainWindowInterface.Show();
                this.Hide();
            }
        }
    }
}
