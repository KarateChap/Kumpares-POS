using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for CheckOut.xaml
    /// </summary>
    public partial class CheckOut : Window
    {
        const string getConnectionString = @"server=localhost; user id=root;password=; database=db_Kumpares";
        double totalAmount = 0;
        double paymentAmount = 0;
        double storeAmount = 0;
        int operationSwitch = 0;
        string dateToday = "";
        int cashierID = 0;
        int currentTransactionID;
        public CheckOut(string total, string date, int cashierID, int currentTransactionID)
        {
            InitializeComponent();
            displayOrder();
            lblTotal.Content = total;
            totalAmount = Convert.ToDouble(lblTotal.Content);
            dateToday = date;
            this.currentTransactionID = currentTransactionID;
            this.cashierID = cashierID;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void TxtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtAmount.Text.Length > 0 && txtAmount.Text.Length < 9)
                {
                    paymentAmount = Convert.ToDouble(txtAmount.Text);
                    lblChange.Content = (paymentAmount - totalAmount).ToString();
                    if (txtAmount.Text.Length >= 8)
                    {
                        MessageBox.Show("Invalid Payment Amount");
                        txtAmount.Text = "";
                    }
                }
                else
                {

                    txtAmount.Text = "";
                }
            }
            catch (Exception )
            {
                MessageBox.Show("Invalid Input");
                txtAmount.Text = "";
            }
            if (txtAmount.Text.Length == 0)
            {
                lblChange.Content = "";
                paymentAmount = 0;
            }
            else
            {
                paymentAmount = Convert.ToDouble(txtAmount.Text);
            }
        }

        private void BtnTwenty_Click(object sender, RoutedEventArgs e)
        {
            paymentAmount = paymentAmount + 20;
            txtAmount.Text = paymentAmount.ToString();
        }

        private void BtnFifty_Click(object sender, RoutedEventArgs e)
        {
            paymentAmount = paymentAmount + 50;
            txtAmount.Text = paymentAmount.ToString();
        }

        private void BtnOneHundred_Click(object sender, RoutedEventArgs e)
        {
            paymentAmount = paymentAmount + 100;
            txtAmount.Text = paymentAmount.ToString();
        }

        private void BtnTwoHundred_Click(object sender, RoutedEventArgs e)
        {
            paymentAmount = paymentAmount + 200;
            txtAmount.Text = paymentAmount.ToString();
        }

        private void BtnFiveHundred_Click(object sender, RoutedEventArgs e)
        {
            paymentAmount = paymentAmount + 500;
            txtAmount.Text = paymentAmount.ToString();
        }

        private void BtnOneThousand_Click(object sender, RoutedEventArgs e)
        {
            paymentAmount = paymentAmount + 1000;
            txtAmount.Text = paymentAmount.ToString();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = "";
            lblChange.Content = "";
            lblOperation.Content = "";
        }

        private void BtnZero_Click(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = txtAmount.Text + "0";
        }

        private void BtnOne_Click(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = txtAmount.Text + "1";
        }

        private void BtnTwo_Click(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = txtAmount.Text + "2";
        }

        private void BtnPoint_Click(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = txtAmount.Text + ".";
        }

        private void BtnThree_Click(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = txtAmount.Text + "3";
        }

        private void BtnFour_Click(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = txtAmount.Text + "4";
        }

        private void BtnFive_Click(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = txtAmount.Text + "5";
        }

        private void BtnSix_Click(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = txtAmount.Text + "6";
        }

        private void BtnSeven_Click(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = txtAmount.Text + "7";
        }

        private void BtnEight_Click(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = txtAmount.Text + "8";
        }

        private void BtnNine_Click(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = txtAmount.Text + "9";
        }

        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                storeAmount = Convert.ToDouble(txtAmount.Text);
                lblOperation.Content = txtAmount.Text + " " + btnPlus.Content;
                operationSwitch = 1;
                txtAmount.Text = "";
            }
            catch (Exception)
            {
            }
           
        }

        private void BtnEquals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblOperation.Content = "";
                switch (operationSwitch)
                {
                    case 1:
                        txtAmount.Text = (Convert.ToDouble(txtAmount.Text) + storeAmount).ToString();
                        break;
                    case 2:
                        txtAmount.Text = (storeAmount - Convert.ToDouble(txtAmount.Text)).ToString();
                        break;
                    case 3:
                        txtAmount.Text = (Convert.ToDouble(txtAmount.Text) * storeAmount).ToString();
                        break;
                    case 4:
                        txtAmount.Text = (storeAmount / Convert.ToDouble(txtAmount.Text)).ToString();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
            
            
        }
        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                storeAmount = Convert.ToDouble(txtAmount.Text);
                lblOperation.Content = txtAmount.Text + " " + btnMinus.Content;
                operationSwitch = 2;
                txtAmount.Text = "";
            }
            catch (Exception)
            {
            }
        }

        private void BtnMultiply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                storeAmount = Convert.ToDouble(txtAmount.Text);
                lblOperation.Content = txtAmount.Text + " " + btnMultiply.Content;
                operationSwitch = 3;
                txtAmount.Text = "";
            }
            catch (Exception)
            {
            }
        }

        private void BtnDivide_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                storeAmount = Convert.ToDouble(txtAmount.Text);
                lblOperation.Content = txtAmount.Text + " " + btnDivide.Content;
                operationSwitch = 4;
                txtAmount.Text = "";
            }
            catch (Exception)
            {
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            int length = txtAmount.Text.Length;
            int number = txtAmount.Text.Length - 1;
            String store;
            if (length > 0)
            {
                
                StringBuilder sb = new StringBuilder(txtAmount.Text);
                sb.Remove(number, 1);
                store = sb.ToString();
                txtAmount.Text = store;
            }
        }

        private void BtnPayCash_Click(object sender, RoutedEventArgs e)
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

            if (paymentAmount >= Convert.ToDouble(lblTotal.Content))
            {
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
            else {
                MessageBox.Show("The Payment Amount is Insufficient");
            }
        }

        private void BtnPayInBitcoin_Click(object sender, RoutedEventArgs e)
        {
            PayBitcoin payBitcoinInterface = new PayBitcoin(cashierID, dateToday, Convert.ToDouble(lblTotal.Content.ToString()), currentTransactionID);
            payBitcoinInterface.Show();
            this.Hide();
        }

        private void BtnPayInEthereum_Click(object sender, RoutedEventArgs e)
        {
            PayEthereum payEthereumInterface = new PayEthereum(cashierID, dateToday, Convert.ToDouble(lblTotal.Content.ToString()), currentTransactionID);
            payEthereumInterface.Show();
            this.Hide();
        }
        public void displayOrder()
        {
            string query = "SELECT tbl_Product.ProductName AS Product, CASE WHEN tbl_TransactionProduct.Spicy = 1 THEN 'Spicy' ELSE 'Original' END AS Flavor, tbl_Product.BasePriceInPeso AS Price, tbl_TransactionProduct.Quantity AS Qty, (tbl_Product.BasePriceInPeso*tbl_TransactionProduct.Quantity) AS Subtotal FROM tbl_Product, tbl_TransactionProduct WHERE tbl_Product.ProductID = tbl_TransactionProduct.ProductID AND tbl_TransactionProduct.TransactionID = (SELECT MAX(tbl_Transaction.TransactionID) FROM tbl_Transaction)";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                DataTable dtProduct = new DataTable();
                dtProduct.Load(reader);
                totalOrderDataGrid.ItemsSource = dtProduct.DefaultView;
            }
            catch (Exception)
            {
            }
            databaseConnection.Close();

            string query1 = "SELECT SUM(tbl_TransactionProduct.Quantity*tbl_Product.BasePriceInPeso) AS TOTAL FROM tbl_TransactionProduct, tbl_Product WHERE tbl_Product.ProductID = tbl_TransactionProduct.ProductID AND tbl_TransactionProduct.TransactionID = (SELECT MAX(tbl_Transaction.TransactionID) FROM tbl_Transaction)";
            MySqlConnection databaseConnection1 = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase1 = new MySqlCommand(query1, databaseConnection1);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader1;

            try
            {
                databaseConnection1.Open();
                reader1 = commandDatabase1.ExecuteReader();
                if (reader1.HasRows)
                {
                    while (reader1.Read())
                    {
                        lblTotal.Content = Convert.ToString(reader1["TOTAL"]);
                    }
                }
            }
            catch (Exception)
            {
            }
            databaseConnection.Close();
        }
    }
}
