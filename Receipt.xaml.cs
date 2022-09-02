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
    /// Interaction logic for Receipt.xaml
    /// </summary>
    public partial class Receipt : Window
    {
        const string getConnectionString = @"server=localhost; user id=root;password=; database=db_Kumpares";
        string name = "Kumpares Guiguinto";
        string cashierName = "";
        string customerName = "";
        string date = "";
        double paymentAmount = 0;
        double totalAmount = 0;
        double change = 0;
        string dineIn = "";
        int currentTransactionID = 0;
        int transactionNo = 0;
        int cashierID = 0;
        DataGrid productDataGrid = new DataGrid();
        public Receipt(int currentTransactionID, int cashierID)
        {
            this.cashierID = cashierID;
            this.currentTransactionID = currentTransactionID;
            InitializeComponent();
            initializeData();
            intializeProducts();
            calculateTotalAndChange();
            loadDataToReceipt();
        }
        public void loadDataToReceipt()
        {
            change = paymentAmount - totalAmount;
            Paragraph para1 = new Paragraph();
            Paragraph para2 = new Paragraph();
            Paragraph para3 = new Paragraph();
            Paragraph para4 = new Paragraph();
            Paragraph para5 = new Paragraph();
            Paragraph para6 = new Paragraph();
            Paragraph para7 = new Paragraph();
            Paragraph para8 = new Paragraph();
            Paragraph para9 = new Paragraph();
            Paragraph para10 = new Paragraph();

            para1.TextAlignment = TextAlignment.Center;
            para1.FontSize = 20;
            para1.Inlines.Add(new Run(name));
            para1.Margin = new Thickness(5);

            para2.TextAlignment = TextAlignment.Center;
            para2.Inlines.Add(new Run("Date: " + date));
            para2.Margin = new Thickness(5);

            para3.TextAlignment = TextAlignment.Center;
            para3.Inlines.Add(new Run("Transaction No: " + transactionNo));
            para3.Margin = new Thickness(5);

            para4.TextAlignment = TextAlignment.Center;
            para4.Inlines.Add(new Run(dineIn));
            para4.Margin = new Thickness(5);

            para5.TextAlignment = TextAlignment.Center;
            para5.Inlines.Add(new Run("Cashier: " + cashierName));
            para5.Margin = new Thickness(5);

            para6.TextAlignment = TextAlignment.Center;
            para6.Inlines.Add(new Run("Customer: " + customerName));
            para6.Margin = new Thickness(5);

            InlineUIContainer products = new InlineUIContainer();
            products.Child = productDataGrid;
            productDataGrid.AutoGenerateColumns = true;
            productDataGrid.IsReadOnly = true;
            productDataGrid.Height = 270;
            productDataGrid.Width = 283;
            para7.Inlines.Add(products);

            para8.TextAlignment = TextAlignment.Center;
            para8.Inlines.Add(new Run("Total Amount: " + totalAmount));
            para8.Margin = new Thickness(5);

            para9.TextAlignment = TextAlignment.Center;
            para9.Inlines.Add(new Run("Payment: " + paymentAmount));
            para9.Margin = new Thickness(5);

            para10.TextAlignment = TextAlignment.Center;
            para10.Inlines.Add(new Run("Change: " + change));
            para10.Margin = new Thickness(5);

            rtbReceipt.Document.Blocks.Add(para1);
            rtbReceipt.Document.Blocks.Add(para2);
            rtbReceipt.Document.Blocks.Add(para3);
            rtbReceipt.Document.Blocks.Add(para4);
            rtbReceipt.Document.Blocks.Add(para5);
            rtbReceipt.Document.Blocks.Add(para6);
            rtbReceipt.Document.Blocks.Add(para7);
            rtbReceipt.Document.Blocks.Add(para8);
            rtbReceipt.Document.Blocks.Add(para9);
            rtbReceipt.Document.Blocks.Add(para10);
        }
        public void intializeProducts()
        {
            string query = "SELECT tbl_Product.ProductName AS Product, CASE WHEN tbl_TransactionProduct.Spicy = 1 THEN 'Spicy' ELSE 'Original' END AS Flavor, tbl_Product.BasePriceInPeso AS Price, tbl_TransactionProduct.Quantity AS Qty, (tbl_Product.BasePriceInPeso*tbl_TransactionProduct.Quantity) AS Subtotal FROM tbl_Product, tbl_TransactionProduct WHERE tbl_Product.ProductID = tbl_TransactionProduct.ProductID AND tbl_TransactionProduct.TransactionID = "+currentTransactionID+"";
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
                productDataGrid.ItemsSource = dtProduct.DefaultView;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            databaseConnection.Close();
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do You Really Want to Exit System?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
            }
            else
            {
                this.Hide();
                System.Windows.Application.Current.Shutdown();
            }
                
        }
        public void calculateTotalAndChange() {
            string query1 = "SELECT SUM(tbl_TransactionProduct.Quantity*tbl_Product.BasePriceInPeso) AS TOTAL FROM tbl_TransactionProduct, tbl_Product WHERE tbl_Product.ProductID = tbl_TransactionProduct.ProductID AND tbl_TransactionProduct.TransactionID = " + currentTransactionID + "";
            MySqlConnection databaseConnection1 = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase1 = new MySqlCommand(query1, databaseConnection1);
            commandDatabase1.CommandTimeout = 60;
            MySqlDataReader reader1;

            try
            {
                databaseConnection1.Open();
                reader1 = commandDatabase1.ExecuteReader();
                if (reader1.HasRows)
                {
                    while (reader1.Read())
                    {
                        totalAmount = Convert.ToDouble(reader1["TOTAL"]);
                    }
                }
            }
            catch (Exception)
            {
            }
            databaseConnection1.Close();
        }
        public void initializeData()
        {
            int dineInOrTakeOut = 0;
            string dateOnly = "";
            string query = "SELECT tbl_Transaction.TransactionID, tbl_Transaction.TransactionDate, tbl_Transaction.PaymentAmount, tbl_Transaction.DineIn, tbl_Cashier.FirstName, tbl_Customer.CustomerFirstName FROM tbl_Transaction INNER JOIN tbl_Cashier ON tbl_Transaction.CashierID = tbl_Cashier.CashierID INNER JOIN tbl_Customer ON tbl_Transaction.CustomerID=tbl_Customer.CustomerID WHERE tbl_Transaction.TransactionID = " + currentTransactionID + "";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                DataTable dtProduct = new DataTable();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        transactionNo = Convert.ToInt32(reader[0]);
                        dateOnly = reader[1].ToString();
                        paymentAmount = Convert.ToDouble(reader[2]);
                        cashierName = reader[4].ToString();
                        customerName = reader[5].ToString();
                        dineInOrTakeOut = Convert.ToInt32(reader[3]);
                    }
                }    
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            databaseConnection.Close();

            for (int i = 0; i < 11; i++)
            {
                int length = dateOnly.Length;
                int number = dateOnly.Length - 1;
                String store;
                if (length > 0)
                {

                    StringBuilder sb = new StringBuilder(dateOnly);
                    sb.Remove(number, 1);
                    store = sb.ToString();
                    dateOnly = store;
                    date = dateOnly;
                }
            }
            
            if (dineInOrTakeOut == 0)
            {
                dineIn = "Take Out";
            }
            else
            {
                dineIn = "Dine In";
            }
        }

        private void BtnNewTransaction_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                if(printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "Receipt");
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
        }

        private void BtnNewTransaction_Click(object sender, RoutedEventArgs e)
        {
            DateTime initializeDate = DateTime.Now;
            string getDate = initializeDate.Year + "-" + initializeDate.Month + "-" + initializeDate.Day;
            string query = "INSERT INTO tbl_Transaction(`TransactionDate`, `PaymentAmount`, `CashierID`) VALUES (@TRANSACTIONDATE, @PAYMENTAMOUNT, @CASHIERID)";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            try
            {
                DateTime myDateTime = DateTime.Now;
                string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                commandDatabase.Parameters.AddWithValue("@TRANSACTIONDATE", Convert.ToDateTime(sqlFormattedDate));
                //DateTime date = DateTime.ParseExact(getDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                //commandDatabase.Parameters.AddWithValue("@TRANSACTIONDATE", Convert.ToDateTime(getDate));
                commandDatabase.Parameters.AddWithValue("@PAYMENTAMOUNT", 0);
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

            string query1 = "INSERT INTO tbl_Customer(`CustomerFirstName`) VALUES (@CUSTOMERFIRSTNAME)";
            MySqlConnection databaseConnection1 = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase1 = new MySqlCommand(query1, databaseConnection1);
            try
            {
                commandDatabase1.Parameters.AddWithValue("@CUSTOMERFIRSTNAME", "");
                commandDatabase1.CommandTimeout = 60;
                databaseConnection1.Open();
                MySqlDataReader myReader = commandDatabase1.ExecuteReader();
                databaseConnection1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Customer customerInterface = new Customer();
            customerInterface.ShowDialog();
            customerInterface.Topmost = true;
            this.Close();
        }
    }
}
