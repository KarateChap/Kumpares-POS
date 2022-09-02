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
    /// Interaction logic for TransactionHistory.xaml
    /// </summary>
    public partial class TransactionHistory : Window
    {
        const string getConnectionString = @"server=localhost; user id=root;password=; database=db_Kumpares";
        string name = "Kumpares Guiguinto";
        string cashierName = "";
        string date = "";
        string dineIn = "";
        string customerName = "";
        double paymentAmount = 0;
        double totalAmount = 0;
        double change = 0;
        int currentTransactionID = 0;
        int transactionNo = 0;
        string fromDate = "";
        string toDate = "";
        DataGrid productDataGrid = new DataGrid();
        InlineUIContainer products = new InlineUIContainer();
        public TransactionHistory()
        {
            InitializeComponent();
            initializeTransactions();

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
        public void calculateTotalAndChange()
        {
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
        public void intializeProducts()
        {
            string query = "SELECT tbl_Product.ProductName AS Product, CASE WHEN tbl_TransactionProduct.Spicy = 1 THEN 'Spicy' ELSE 'Original' END AS Flavor, tbl_Product.BasePriceInPeso AS Price, tbl_TransactionProduct.Quantity AS Qty, (tbl_Product.BasePriceInPeso*tbl_TransactionProduct.Quantity) AS Subtotal FROM tbl_Product, tbl_TransactionProduct WHERE tbl_Product.ProductID = tbl_TransactionProduct.ProductID AND tbl_TransactionProduct.TransactionID = " + currentTransactionID + "";
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
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
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
        public void initializeTransactions()
        {
            string query = "SELECT tbl_Transaction.TransactionID AS `Transaction ID`, tbl_Transaction.TransactionDate AS Date, tbl_Transaction.PaymentAmount Payment, tbl_Cashier.FirstName AS Cashier FROM tbl_Transaction Inner Join tbl_Cashier ON tbl_Transaction.CashierID = tbl_Cashier.CashierID";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                DataTable dtCashier = new DataTable();
                dtCashier.Load(reader);
                transactionHistoryDataGrid.ItemsSource = dtCashier.DefaultView;
                

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection.Close();
        }

        private void TransactionHistoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView selected = gd.SelectedItem as DataRowView;
            if (transactionHistoryDataGrid.SelectedIndex > -1)
            {
                products.Child = null;
                rtbReceipt.Document.Blocks.Clear();
                currentTransactionID = Convert.ToInt32(selected["Transaction ID"].ToString());
                initializeData();
                intializeProducts();
                calculateTotalAndChange();
                loadDataToReceipt();
            }
        }

        private void MainListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainListView.SelectedIndex == 0)
            {
                AdminInterface adminInterface = new AdminInterface();
                adminInterface.Show();
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
            else if (MainListView.SelectedIndex == 4)
            {
                AboutTheDevelopers aboutTheDevelopersWindow = new AboutTheDevelopers();
                aboutTheDevelopersWindow.Show();
                this.Hide();

            }
            else if (MainListView.SelectedIndex == 5)
            {
                MainWindow mainWindowInterface = new MainWindow();
                mainWindowInterface.Show();
                this.Hide();
            }
        }

        private void TransactionHistoryDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Date")
            {
                DataGridTextColumn column = e.Column as DataGridTextColumn;
                Binding binding = column.Binding as Binding;
                binding.StringFormat = "dd-MM-yyyy";
            }
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT tbl_Transaction.TransactionID AS `Transaction ID`, tbl_Transaction.TransactionDate AS Date, tbl_Transaction.PaymentAmount Payment, tbl_Cashier.FirstName FROM tbl_Transaction Inner Join tbl_Cashier ON tbl_Transaction.CashierID = tbl_Cashier.CashierID Where TransactionDate BETWEEN '"+fromDate+ "'  and '"+toDate+ "'";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                DataTable dtCashier = new DataTable();
                dtCashier.Load(reader);
                transactionHistoryDataGrid.ItemsSource = dtCashier.DefaultView;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection.Close();
        }

        private void DtpTo_CalendarClosed(object sender, RoutedEventArgs e)
        {
            try
            {
                toDate = Convert.ToDateTime(dtpTo.Text).ToString("yyyy-MM-dd");
            }
            catch (Exception)
            {
                MessageBox.Show("Please Choose Date");
            }
        }

        private void DtpFrom_CalendarClosed(object sender, RoutedEventArgs e)
        {
            try
            {
                fromDate = Convert.ToDateTime(dtpFrom.Text).ToString("yyyy-MM-dd");
            }
            catch (Exception)
            {
                MessageBox.Show("Please Choose Date");
            }
        }
    }
}
