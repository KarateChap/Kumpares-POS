using MySql.Data.MySqlClient;
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
    /// Interaction logic for PayEthereum.xaml
    /// </summary>
    public partial class PayEthereum : Window
    {
        const string getConnectionString = @"server=localhost; user id=root;password=; database=db_Kumpares";
        int cashierID = 0;
        string dateToday = "";
        double paymentAmount = 0;
        int currentTransactionID = 0;
        public PayEthereum(int cashierID, string date, double paymentAmount, int currentTransactionID)
        {
            InitializeComponent();
            this.cashierID = cashierID;
            this.dateToday = date;
            this.paymentAmount = paymentAmount;
            lblAmount.Content = paymentAmount;
            this.currentTransactionID = currentTransactionID;
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            CheckOut checkOutInterface = new CheckOut(lblAmount.Content.ToString(), dateToday, cashierID, currentTransactionID);
            checkOutInterface.Show();
            this.Hide();
        }

        private void BtnViewReceipt_Click(object sender, RoutedEventArgs e)
        {
            string query1 = "SELECT MAX(TransactionID) FROM tbl_Transaction";
            MySqlConnection databaseConnection1 = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase1 = new MySqlCommand(query1, databaseConnection1);
            commandDatabase1.CommandTimeout = 60;
            MySqlDataReader reader;
            try
            {
                databaseConnection1.Open();
                reader = commandDatabase1.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        currentTransactionID = reader.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection1.Close();

            string query = "UPDATE `tbl_Transaction` SET `TransactionDate`=@TRANSACTIONDATE,`PaymentAmount`=@PAYMENTAMOUNT, `CashierID`=@CASHIERID WHERE TransactionID = @TRANSACTIONID";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            try
            {
                commandDatabase.Parameters.AddWithValue("@TRANSACTIONID", currentTransactionID);
                DateTime myDateTime = DateTime.Now;
                string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                commandDatabase.Parameters.AddWithValue("@TRANSACTIONDATE", Convert.ToDateTime(sqlFormattedDate));
                commandDatabase.Parameters.AddWithValue("@PAYMENTAMOUNT", paymentAmount);
                commandDatabase.Parameters.AddWithValue("@CASHIERID", cashierID);
                commandDatabase.CommandTimeout = 60;
                databaseConnection.Open();
                MySqlDataReader myReader = commandDatabase.ExecuteReader();
                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Hide();
                Receipt receiptInterface = new Receipt(currentTransactionID, cashierID);
                receiptInterface.Show();
            }
        }
    }
}
