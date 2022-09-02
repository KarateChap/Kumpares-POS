using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KumparesFinal
{
    /// <summary>
    /// Interaction logic for CashierInterface.xaml
    /// </summary>
    public partial class CashierInterface : Window
    {
        const string getConnectionString = @"server=localhost; user id=root;password=; database=db_Kumpares";
        int quantity = 1;
        string selectedItem = "";
        string selectedCategory = "";
        string selectedAvailability = "";
        string bestLomi = "";
        string bestBarbeeQ = "";
        string bestGotome = "";
        string bestTapsi = "";
        string bestPares = "";
        string bestDrinks = "";
        string bestDesserts = "";
        int bestLomiID = 0;
        int bestBarbeeQID = 0;
        int bestGotomeID = 0;
        int bestTapsiID = 0;
        int bestParesID = 0;
        int bestDrinksID = 0;
        int bestDessertsID = 0;
        int chosenProductInBestSellerID = 0;
        int cashierID = 1;
        int currentTransactionID = 0;
        bool isDrinksOrDesserts = false;
        bool isBestSeller = false;
        DataRowView selected;
        public CashierInterface(string cashierName)
        {
            InitializeComponent();
            startClock();
            lblCashier.Content = "Hello " + cashierName + "!";
            listProducts();
            cashierImageInitialize(cashierName);
            setUpDate();
            clearOrder();
            initializeNewTransactionID();
            listBestSellers();
        }
        public void cashierImageInitialize(string name)
        {
            string query = "SELECT CashierImage, CashierID FROM tbl_Cashier WHERE username like '" + name + "'";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        byte[] blob = (byte[])reader["CashierImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        imgCashier.Source = bi;
                        cashierID = Convert.ToInt32(reader["CashierID"]);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection.Close();
        }
        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindowInterface = new MainWindow();
            mainWindowInterface.Show();
            this.Hide();
        }
        private void listProducts()
        {
            string query = "SELECT ProductID, ProductName, BasePriceInPeso, CategoryDescription, CASE WHEN IsAvailable = 1 THEN 'Available' ELSE 'Unavailable' END AS IsAvailable FROM tbl_Product, tbl_Category WHERE tbl_Product.CategoryID=tbl_Category.CategoryID";
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
                productListDataGrid.ItemsSource = dtProduct.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection.Close();

        }
        private void BtnLogOut_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do You Really Want to Log Out?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {   
            }
            else
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

                DataGrid gd = (DataGrid)orderListDataGrid;
                DataRowView selected = gd.SelectedItem as DataRowView;
                string query2 = "DELETE FROM `tbl_TransactionProduct` WHERE tbl_TransactionProduct.TransactionID = " + currentTransactionID + "";
                MySqlConnection databaseConnection2 = new MySqlConnection(getConnectionString);
                MySqlCommand commandDatabase2 = new MySqlCommand(query2, databaseConnection2);
                commandDatabase2.CommandTimeout = 60;
                MySqlDataReader Myreader;
                try
                {
                    databaseConnection2.Open();
                    Myreader = commandDatabase2.ExecuteReader();
                    databaseConnection2.Close();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                }

                string query = "DELETE FROM `tbl_Transaction` WHERE TransactionID = @TRANSACTIONID";
                MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                commandDatabase.Parameters.AddWithValue("@TRANSACTIONID", currentTransactionID);
                commandDatabase.CommandTimeout = 60;
                MySqlDataReader myReader;
                try
                {
                    databaseConnection.Open();
                    myReader = commandDatabase.ExecuteReader();
                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                }

                MainWindow mainWindowInterface = new MainWindow();
                mainWindowInterface.Show();
                this.Hide();
            }

        }

        private void BtnLomi_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT ProductID, ProductName, BasePriceInPeso, CategoryDescription, CASE WHEN IsAvailable = 1 THEN 'Yes' ELSE 'No' END AS IsAvailable FROM tbl_Product, tbl_Category WHERE tbl_Product.CategoryID=tbl_Category.CategoryID AND tbl_Product.CategoryID = 1";
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
                productListDataGrid.ItemsSource = dtProduct.DefaultView;
            }
            catch (Exception)
            {
            }
            databaseConnection.Close();
        }

        private void BtnBarbeeQ_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT ProductID, ProductName, BasePriceInPeso, CategoryDescription, CASE WHEN IsAvailable = 1 THEN 'Yes' ELSE 'No' END AS IsAvailable FROM tbl_Product, tbl_Category WHERE tbl_Product.CategoryID=tbl_Category.CategoryID AND tbl_Product.CategoryID = 2";
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
                productListDataGrid.ItemsSource = dtProduct.DefaultView;
            }
            catch (Exception)
            {
            }
            databaseConnection.Close();
        }

        private void BtnGotoMe_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT ProductID, ProductName, BasePriceInPeso, CategoryDescription, CASE WHEN IsAvailable = 1 THEN 'Yes' ELSE 'No' END AS IsAvailable FROM tbl_Product, tbl_Category WHERE tbl_Product.CategoryID=tbl_Category.CategoryID AND tbl_Product.CategoryID = 3";
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
                productListDataGrid.ItemsSource = dtProduct.DefaultView;
            }
            catch (Exception)
            {
            }
            databaseConnection.Close();
        }

        private void BtnTapsi_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT ProductID, ProductName, BasePriceInPeso, CategoryDescription, CASE WHEN IsAvailable = 1 THEN 'Yes' ELSE 'No' END AS IsAvailable FROM tbl_Product, tbl_Category WHERE tbl_Product.CategoryID=tbl_Category.CategoryID AND tbl_Product.CategoryID = 4";
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
                productListDataGrid.ItemsSource = dtProduct.DefaultView;
            }
            catch (Exception)
            {
            }
            databaseConnection.Close();
        }

        private void BtnPares_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT ProductID, ProductName, BasePriceInPeso, CategoryDescription, CASE WHEN IsAvailable = 1 THEN 'Yes' ELSE 'No' END AS IsAvailable FROM tbl_Product, tbl_Category WHERE tbl_Product.CategoryID=tbl_Category.CategoryID AND tbl_Product.CategoryID = 5";
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
                productListDataGrid.ItemsSource = dtProduct.DefaultView;
            }
            catch (Exception)
            {
            }
            databaseConnection.Close();
        }

        private void BtnDrinks_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT ProductID, ProductName, BasePriceInPeso, CategoryDescription, CASE WHEN IsAvailable = 1 THEN 'Yes' ELSE 'No' END AS IsAvailable FROM tbl_Product, tbl_Category WHERE tbl_Product.CategoryID=tbl_Category.CategoryID AND tbl_Product.CategoryID = 6";
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
                productListDataGrid.ItemsSource = dtProduct.DefaultView;
            }
            catch (Exception)
            {
            }
            databaseConnection.Close();
        }

        private void BtnDeserts_Click(object sender, RoutedEventArgs e)
        {

            string query = "SELECT ProductID, ProductName, BasePriceInPeso, CategoryDescription, CASE WHEN IsAvailable = 1 THEN 'Yes' ELSE 'No' END AS IsAvailable FROM tbl_Product, tbl_Category WHERE tbl_Product.CategoryID=tbl_Category.CategoryID AND tbl_Product.CategoryID = 7";
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
                productListDataGrid.ItemsSource = dtProduct.DefaultView;
            }
            catch (Exception)
            {
            }
            databaseConnection.Close();
        }

        private void ProductListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtQuantity.Text = "1";
            DataGrid gd = (DataGrid)sender;
            selected = gd.SelectedItem as DataRowView;
            string query = "SELECT ProductImage FROM tbl_Product WHERE tbl_Product.ProductID=@PRODUCTID";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            commandDatabase.Parameters.AddWithValue("@PRODUCTID", selected["ProductID"].ToString());
            MySqlDataReader reader;
            selectedItem = selected["ProductID"].ToString();
            selectedCategory = selected["CategoryDescription"].ToString();
            selectedAvailability = selected["IsAvailable"].ToString();
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        byte[] blob = (byte[])reader["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        imgProduct.Source = bi;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection.Close();

        }

        private void BtnIncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            quantity = quantity + 1;
            txtQuantity.Text = quantity.ToString();
        }

        private void BtnDecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (quantity >= 2)
            {
                quantity = quantity - 1;
                txtQuantity.Text = quantity.ToString();
            }
            else
            {
                MessageBox.Show("Quantity Cannot Be Lowered than 0");
            }
        }

        private void TxtQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtQuantity.Text.Length > 0)
            {
                try
                {
                    quantity = Convert.ToInt32(txtQuantity.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Inputs should be a whole number");
                    txtQuantity.Text = "1";
                }
            }
        }
        public void setUpDate()
        {
            DateTime date = DateTime.Now;
            lblDate.Content = date.Year + "-" + date.Month + "-" + date.Day;
        }
        public void startClock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tickevent;
            timer.Start();
        }
        private void tickevent(object sender, EventArgs e)
        {
            lblTime.Content = DateTime.Now.ToString(@"hh:mm\:ss tt");
        }
        private void ChkBoxOriginal_Checked(object sender, RoutedEventArgs e)
        {
            if (chkBoxOriginal.IsChecked == true)
            {
                chkBoxSpicy.IsChecked = false;
            }
        }
        private void ChkBoxSpicy_Checked(object sender, RoutedEventArgs e)
        {
            if (chkBoxSpicy.IsChecked == true)
            {
                chkBoxOriginal.IsChecked = false;
            }

        }

        public void displayOrder()
        {
            string query = "SELECT tbl_TransactionProduct.TransactionProductID AS ID, tbl_Product.ProductName AS Product, CASE WHEN tbl_TransactionProduct.Spicy = 1 THEN 'Spicy' ELSE 'Original' END AS Flavor, tbl_Product.BasePriceInPeso AS Price, tbl_TransactionProduct.Quantity AS Qty, (tbl_Product.BasePriceInPeso*tbl_TransactionProduct.Quantity) AS Subtotal FROM tbl_Product, tbl_TransactionProduct WHERE tbl_Product.ProductID = tbl_TransactionProduct.ProductID AND tbl_TransactionProduct.TransactionID = (SELECT MAX(tbl_Transaction.TransactionID) FROM tbl_Transaction)";
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
                orderListDataGrid.ItemsSource = dtProduct.DefaultView;
            }
            catch (Exception)
            {
            }
            databaseConnection.Close();

            string query1 = "SELECT SUM(tbl_TransactionProduct.Quantity*tbl_Product.BasePriceInPeso) AS TOTAL FROM tbl_TransactionProduct, tbl_Product WHERE tbl_Product.ProductID = tbl_TransactionProduct.ProductID AND tbl_TransactionProduct.TransactionID = (SELECT MAX(tbl_Transaction.TransactionID) FROM tbl_Transaction)";
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
                        lblTotal.Content = Convert.ToString(reader1["TOTAL"]);
                    }
                }
            }
            catch (Exception)
            {
            }
            databaseConnection1.Close();
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if(isBestSeller == false)
            {
                if (selectedAvailability == "Unavailable")
                {
                    MessageBox.Show("Cannot Add Product, Product is Unavailable");
                    isBestSeller = false;
                }
                else
                {

                    int flavor = 0;
                    if (chkBoxOriginal.IsChecked == true)
                    {
                        flavor = 0;
                    }
                    else
                    {
                        flavor = 1;
                    }
                    if ((selectedCategory == "Drinks" && flavor == 1) || (selectedCategory == "Desserts" && flavor == 1))
                    {
                        MessageBox.Show("Drinks or Desserts Cannot Be Spicy");
                        isBestSeller = false;
                    }
                    else
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
                                    isBestSeller = false;
                                    
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            isBestSeller = false;
                        }
                        databaseConnection1.Close();

                        if (selectedItem.Length > 0)
                        {
                            string query = "INSERT INTO tbl_TransactionProduct(`Quantity`, `Spicy`, `TransactionID`, `ProductID`) VALUES (@QUANTITY, @SPICY, @TRANSACTIONID, @PRODUCTID)";
                            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
                            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                            try
                            {
                                commandDatabase.Parameters.AddWithValue("@QUANTITY", Convert.ToInt32(txtQuantity.Text));
                                commandDatabase.Parameters.AddWithValue("@SPICY", flavor);
                                commandDatabase.Parameters.AddWithValue("@TRANSACTIONID", currentTransactionID);
                                commandDatabase.Parameters.AddWithValue("@PRODUCTID", Convert.ToInt32(selectedItem));
                                commandDatabase.CommandTimeout = 60;
                                databaseConnection.Open();
                                MySqlDataReader myReader = commandDatabase.ExecuteReader();
                                databaseConnection.Close();
                                isBestSeller = false;
                                isDrinksOrDesserts = false;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                isBestSeller = false;
                            }
                            finally
                            {
                                chkBoxOriginal.IsChecked = true;
                                txtQuantity.Text = "1";
                                displayOrder();
                                isBestSeller = false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Select Item First");
                            isBestSeller = false;
                        }
                    }
                }
            }
            else
            {
                int flavor = 0;
                if (chkBoxOriginal.IsChecked == true)
                {
                    flavor = 0;
                }
                else
                {
                    flavor = 1;
                }
                if ((isDrinksOrDesserts == true) && (flavor == 1))
                {
                    MessageBox.Show("Drinks or Desserts Cannot Be Spicy");
                }
                else
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
                                isBestSeller = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        isBestSeller = false;
                    }
                    databaseConnection1.Close();

                    string queryBestSeller = "INSERT INTO tbl_TransactionProduct(`Quantity`, `Spicy`, `TransactionID`, `ProductID`) VALUES (@QUANTITY, @SPICY, @TRANSACTIONID, @PRODUCTID)";
                    MySqlConnection databaseConnectionBestSeller = new MySqlConnection(getConnectionString);
                    MySqlCommand commandDatabaseBestSeller = new MySqlCommand(queryBestSeller, databaseConnectionBestSeller);
                    try
                    {
                        commandDatabaseBestSeller.Parameters.AddWithValue("@QUANTITY", Convert.ToInt32(txtQuantity.Text));
                        commandDatabaseBestSeller.Parameters.AddWithValue("@SPICY", flavor);
                        commandDatabaseBestSeller.Parameters.AddWithValue("@TRANSACTIONID", currentTransactionID);
                        commandDatabaseBestSeller.Parameters.AddWithValue("@PRODUCTID", Convert.ToInt32(chosenProductInBestSellerID));
                        commandDatabaseBestSeller.CommandTimeout = 60;
                        databaseConnectionBestSeller.Open();
                        MySqlDataReader myReaderBestSeller = commandDatabaseBestSeller.ExecuteReader();
                        databaseConnectionBestSeller.Close();
                        isBestSeller = false;
                        isDrinksOrDesserts = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        isBestSeller = false;
                    }
                    finally
                    {
                        chkBoxOriginal.IsChecked = true;
                        txtQuantity.Text = "1";
                        displayOrder();
                        isBestSeller = false;
                        isDrinksOrDesserts = false;
                    }
                }
            }
        }
        public void initializeNewTransactionID()
        {
            string query = "INSERT INTO tbl_Transaction(`TransactionDate`, `PaymentAmount`, `CashierID`, `DineIn`) VALUES (@TRANSACTIONDATE, @PAYMENTAMOUNT, @CASHIERID, @DINEIN)";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            try
            {
                DateTime myDateTime = DateTime.Now;
                string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                commandDatabase.Parameters.AddWithValue("@TRANSACTIONDATE", Convert.ToDateTime(sqlFormattedDate));
                commandDatabase.Parameters.AddWithValue("@PAYMENTAMOUNT", 0);
                commandDatabase.Parameters.AddWithValue("@CASHIERID", cashierID);
                commandDatabase.Parameters.AddWithValue("@DINEIN", 0);
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
            displayOrder();
            Customer customerInterface = new Customer();
            customerInterface.ShowDialog();
            //customerInterface.Show();
            customerInterface.Topmost = true;
            this.Hide();
        }

        private void BtnRemoveOrder_Click(object sender, RoutedEventArgs e)
        {
            DataGrid gd = (DataGrid)orderListDataGrid;
            DataRowView selected = gd.SelectedItem as DataRowView;
            if (orderListDataGrid.SelectedIndex > -1)
            {
                string query = "DELETE FROM `tbl_TransactionProduct` WHERE TransactionProductID = @TRANSACTIONPRODUCTID";
                MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                commandDatabase.Parameters.AddWithValue("@TRANSACTIONPRODUCTID", selected["ID"].ToString());
                commandDatabase.CommandTimeout = 60;
                MySqlDataReader reader;
                try
                {
                    databaseConnection.Open();
                    reader = commandDatabase.ExecuteReader();
                    databaseConnection.Close();
                }
                catch (Exception)
                {
                }
                finally
                {
                    displayOrder();
                }
            }
            else
            {
                MessageBox.Show("Please Select A Product To Remove");
            }
        }

        private void OrderListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            selected = gd.SelectedItem as DataRowView;
            string query = "SELECT ProductImage FROM tbl_Product WHERE tbl_Product.ProductName like @PRODUCTNAME";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            commandDatabase.Parameters.AddWithValue("@PRODUCTNAME", selected["Product"].ToString());
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        byte[] blob = (byte[])reader["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        imgProduct.Source = bi;
                    }
                }
            }
            catch (Exception )
            {
            }
            databaseConnection.Close();
        }

        private void ChkBoxOriginal_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkBoxOriginal.IsChecked == false)
            {
                chkBoxSpicy.IsChecked = true;
            }
        }

        private void ChkBoxSpicy_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkBoxSpicy.IsChecked == false)
            {
                chkBoxOriginal.IsChecked = true;
            }
        }

        private void BtnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckOut checkOutInterface = new CheckOut(lblTotal.Content.ToString(), lblDate.Content.ToString(), cashierID, currentTransactionID);
                checkOutInterface.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Please Add Order First");
            }
        }
        public void clearOrder()
        {
            DataGrid gd = (DataGrid)orderListDataGrid;
            DataRowView selected = gd.SelectedItem as DataRowView;
            string query = "DELETE FROM `tbl_TransactionProduct` WHERE tbl_TransactionProduct.TransactionID = " + currentTransactionID + "";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                databaseConnection.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            finally
            {
                displayOrder();
            }
        }
        private void BtnCancelTransaction_Click(object sender, RoutedEventArgs e)
        {
            DataGrid gd = (DataGrid)orderListDataGrid;
            DataRowView selected = gd.SelectedItem as DataRowView;
            if (orderListDataGrid.Items.Count > 0) 
            {
                string query = "DELETE FROM `tbl_TransactionProduct` WHERE tbl_TransactionProduct.TransactionID = " + currentTransactionID + "";
                MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                commandDatabase.CommandTimeout = 60;
                MySqlDataReader reader;
                try
                {
                    databaseConnection.Open();
                    reader = commandDatabase.ExecuteReader();
                    MessageBox.Show("Transaction Cancelled");
                    databaseConnection.Close();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    displayOrder();
                    Customer customerInterface = new Customer();
                    customerInterface.ShowDialog();
                    customerInterface.Topmost = true;
                }
            }
            else
            {
                MessageBox.Show("Add Product/s First Before Cancelling the Transaction");
            }
        }
        private void BtnALL_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT ProductID, ProductName, BasePriceInPeso, CategoryDescription, CASE WHEN IsAvailable = 1 THEN 'Available' ELSE 'Unavailable' END AS IsAvailable FROM tbl_Product, tbl_Category WHERE tbl_Product.CategoryID=tbl_Category.CategoryID";
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
                productListDataGrid.ItemsSource = dtProduct.DefaultView;
            }
            catch (Exception ex)
            {
            }
            databaseConnection.Close();
        }
        public void listBestSellers()
        {
            string queryBarbeeQ = "select ProductName, ProductImage, BasePriceInPeso, tbl_TransactionProduct.ProductID, Count(ProductName) AS Occurence from tbl_transactionproduct INNER JOIN tbl_product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_product.CategoryID = tbl_category.CategoryID WHERE tbl_category.CategoryID = 2 Group by ProductName order BY Occurence desc LIMIT 1";
            MySqlConnection databaseConnectionBarbeeQ = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabaseBarbeeQ = new MySqlCommand(queryBarbeeQ, databaseConnectionBarbeeQ);
            commandDatabaseBarbeeQ.CommandTimeout = 60;
            MySqlDataReader readerBarbeeQ;
            try
            {
                databaseConnectionBarbeeQ.Open();
                readerBarbeeQ = commandDatabaseBarbeeQ.ExecuteReader();
                if (readerBarbeeQ.HasRows)
                {
                    while (readerBarbeeQ.Read())
                    {
                        byte[] blob = (byte[])readerBarbeeQ["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();

                        
                        bestBarbeeQ = readerBarbeeQ["ProductName"].ToString();
                        string basePriceInBarbeeQ = readerBarbeeQ["BasePriceInPeso"].ToString();
                        bestBarbeeQID = Convert.ToInt32(readerBarbeeQ["ProductID"]);

                        Image productImageBarbeeQ = new Image();
                        productImageBarbeeQ.Source = bi;
                        TextBlock txtProduct = new TextBlock();
                        txtProduct.HorizontalAlignment = HorizontalAlignment.Center;
                        txtProduct.Inlines.Add(bestBarbeeQ + " ");
                        txtProduct.Inlines.Add(basePriceInBarbeeQ);
                        StackPanel stackPnl = new StackPanel();
                        stackPnl.Orientation = Orientation.Vertical;
                        stackPnl.Margin = new Thickness(5);
                        stackPnl.Children.Add(txtProduct);
                        stackPnl.Children.Add(productImageBarbeeQ);

                        btnBestBarbeeQ.Content = stackPnl;

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnectionBarbeeQ.Close();

            string queryGotome = "select ProductName, ProductImage, BasePriceInPeso, tbl_TransactionProduct.ProductID, Count(ProductName) AS Occurence from tbl_transactionproduct INNER JOIN tbl_product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_product.CategoryID = tbl_category.CategoryID WHERE tbl_category.CategoryID = 3 Group by ProductName order BY Occurence desc LIMIT 1";
            MySqlConnection databaseConnectionGotome = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabaseGotome = new MySqlCommand(queryGotome, databaseConnectionGotome);
            commandDatabaseGotome.CommandTimeout = 60;
            MySqlDataReader readerGotome;
            try
            {
                databaseConnectionGotome.Open();
                readerGotome = commandDatabaseGotome.ExecuteReader();
                if (readerGotome.HasRows)
                {
                    while (readerGotome.Read())
                    {
                        byte[] blob = (byte[])readerGotome["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();

                        bestGotome = readerGotome["ProductName"].ToString();
                        string basePriceInGotome = readerGotome["BasePriceInPeso"].ToString();
                        bestGotomeID = Convert.ToInt32(readerGotome["ProductID"]);

                        Image productImageGotome = new Image();
                        productImageGotome.Source = bi;
                        TextBlock txtProduct = new TextBlock();
                        txtProduct.HorizontalAlignment = HorizontalAlignment.Center;
                        txtProduct.Inlines.Add(bestGotome + " ");
                        txtProduct.Inlines.Add(basePriceInGotome);
                        StackPanel stackPnl = new StackPanel();
                        stackPnl.Orientation = Orientation.Vertical;
                        stackPnl.Margin = new Thickness(5);
                        stackPnl.Children.Add(txtProduct);
                        stackPnl.Children.Add(productImageGotome);

                        btnBestGotome.Content = stackPnl;

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnectionGotome.Close();

            string queryLomi = "select ProductName, ProductImage, BasePriceInPeso, tbl_TransactionProduct.ProductID, Count(ProductName) AS Occurence from tbl_transactionproduct INNER JOIN tbl_product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_product.CategoryID = tbl_category.CategoryID WHERE tbl_category.CategoryID = 1 Group by ProductName order BY Occurence desc LIMIT 1";
            MySqlConnection databaseConnectionLomi = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabaseLomi = new MySqlCommand(queryLomi, databaseConnectionLomi);
            commandDatabaseLomi.CommandTimeout = 60;
            MySqlDataReader readerLomi;
            try
            {
                databaseConnectionLomi.Open();
                readerLomi = commandDatabaseLomi.ExecuteReader();
                if (readerLomi.HasRows)
                {
                    while (readerLomi.Read())
                    {
                        byte[] blob = (byte[])readerLomi["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();

                        bestLomi = readerLomi["ProductName"].ToString();
                        string basePriceLomi = readerLomi["BasePriceInPeso"].ToString();
                        bestLomiID = Convert.ToInt32(readerLomi["ProductID"]);

                        Image productImageLomi = new Image();
                        productImageLomi.Source = bi;
                        TextBlock txtProduct = new TextBlock();
                        txtProduct.HorizontalAlignment = HorizontalAlignment.Center;
                        txtProduct.Inlines.Add(bestLomi + " ");
                        txtProduct.Inlines.Add(basePriceLomi);
                        StackPanel stackPnl = new StackPanel();
                        stackPnl.Orientation = Orientation.Vertical;
                        stackPnl.Margin = new Thickness(5);
                        stackPnl.Children.Add(txtProduct);
                        stackPnl.Children.Add(productImageLomi);

                        btnBestLomi.Content = stackPnl;

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnectionLomi.Close();

            string queryPares = "select ProductName, ProductImage, BasePriceInPeso, tbl_TransactionProduct.ProductID, Count(ProductName) AS Occurence from tbl_transactionproduct INNER JOIN tbl_product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_product.CategoryID = tbl_category.CategoryID WHERE tbl_category.CategoryID = 5 Group by ProductName order BY Occurence desc LIMIT 1";
            MySqlConnection databaseConnectionPares = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabasePares = new MySqlCommand(queryPares, databaseConnectionPares);
            commandDatabasePares.CommandTimeout = 60;
            MySqlDataReader readerPares;
            try
            {
                databaseConnectionPares.Open();
                readerPares = commandDatabasePares.ExecuteReader();
                if (readerPares.HasRows)
                {
                    while (readerPares.Read())
                    {
                        byte[] blob = (byte[])readerPares["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();

                        bestPares = readerPares["ProductName"].ToString();
                        string basePricePares = readerPares["BasePriceInPeso"].ToString();
                        bestParesID = Convert.ToInt32(readerPares["ProductID"]);

                        Image productImagePares = new Image();
                        productImagePares.Source = bi;
                        TextBlock txtProduct = new TextBlock();
                        txtProduct.HorizontalAlignment = HorizontalAlignment.Center;
                        txtProduct.Inlines.Add(bestPares + " ");
                        txtProduct.Inlines.Add(basePricePares);
                        StackPanel stackPnl = new StackPanel();
                        stackPnl.Orientation = Orientation.Vertical;
                        stackPnl.Margin = new Thickness(5);
                        stackPnl.Children.Add(txtProduct);
                        stackPnl.Children.Add(productImagePares);

                        btnBestPares.Content = stackPnl;

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnectionPares.Close();

            string queryTapsi = "select ProductName, ProductImage, BasePriceInPeso, tbl_TransactionProduct.ProductID, Count(ProductName) AS Occurence from tbl_transactionproduct INNER JOIN tbl_product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_product.CategoryID = tbl_category.CategoryID WHERE tbl_category.CategoryID = 4 Group by ProductName order BY Occurence desc LIMIT 1";
            MySqlConnection databaseConnectionTapsi = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabaseTapsi = new MySqlCommand(queryTapsi, databaseConnectionTapsi);
            commandDatabaseTapsi.CommandTimeout = 60;
            MySqlDataReader readerTapsi;
            try
            {
                databaseConnectionTapsi.Open();
                readerTapsi = commandDatabaseTapsi.ExecuteReader();
                if (readerTapsi.HasRows)
                {
                    while (readerTapsi.Read())
                    {
                        byte[] blob = (byte[])readerTapsi["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();

                        bestTapsi = readerTapsi["ProductName"].ToString();
                        string basePriceTapsi = readerTapsi["BasePriceInPeso"].ToString();
                        bestTapsiID = Convert.ToInt32(readerTapsi["ProductID"]);

                        Image productImageTapsi = new Image();
                        productImageTapsi.Source = bi;
                        TextBlock txtProduct = new TextBlock();
                        txtProduct.HorizontalAlignment = HorizontalAlignment.Center;
                        txtProduct.Inlines.Add(bestTapsi + " ");
                        txtProduct.Inlines.Add(basePriceTapsi);
                        StackPanel stackPnl = new StackPanel();
                        stackPnl.Orientation = Orientation.Vertical;
                        stackPnl.Margin = new Thickness(5);
                        stackPnl.Children.Add(txtProduct);
                        stackPnl.Children.Add(productImageTapsi);

                        btnBestTapsi.Content = stackPnl;

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnectionTapsi.Close();

            string queryDessert = "select ProductName, ProductImage, BasePriceInPeso, tbl_TransactionProduct.ProductID, Count(ProductName) AS Occurence from tbl_transactionproduct INNER JOIN tbl_product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_product.CategoryID = tbl_category.CategoryID WHERE tbl_category.CategoryID = 7 Group by ProductName order BY Occurence desc LIMIT 1";
            MySqlConnection databaseConnectionDessert = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabaseDessert = new MySqlCommand(queryDessert, databaseConnectionDessert);
            commandDatabaseDessert.CommandTimeout = 60;
            MySqlDataReader readerDessert;
            try
            {
                databaseConnectionDessert.Open();
                readerDessert = commandDatabaseDessert.ExecuteReader();
                if (readerDessert.HasRows)
                {
                    while (readerDessert.Read())
                    {
                        byte[] blob = (byte[])readerDessert["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();

                        bestDesserts = readerDessert["ProductName"].ToString();
                        string basePriceDessert = readerDessert["BasePriceInPeso"].ToString();
                        bestDessertsID = Convert.ToInt32(readerDessert["ProductID"]);

                        Image productImageDessert = new Image();
                        productImageDessert.Source = bi;
                        TextBlock txtProduct = new TextBlock();
                        txtProduct.HorizontalAlignment = HorizontalAlignment.Center;
                        txtProduct.Inlines.Add(bestDesserts + " ");
                        txtProduct.Inlines.Add(basePriceDessert);
                        StackPanel stackPnl = new StackPanel();
                        stackPnl.Orientation = Orientation.Vertical;
                        stackPnl.Margin = new Thickness(5);
                        stackPnl.Children.Add(txtProduct);
                        stackPnl.Children.Add(productImageDessert);

                        btnBestDessert.Content = stackPnl;

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnectionDessert.Close();

            string queryDrink = "select ProductName, ProductImage, BasePriceInPeso, tbl_TransactionProduct.ProductID, Count(ProductName) AS Occurence from tbl_transactionproduct INNER JOIN tbl_product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_product.CategoryID = tbl_category.CategoryID WHERE tbl_category.CategoryID = 6 Group by ProductName order BY Occurence desc LIMIT 1";
            MySqlConnection databaseConnectionDrink = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabaseDrink = new MySqlCommand(queryDrink, databaseConnectionDrink);
            commandDatabaseDrink.CommandTimeout = 60;
            MySqlDataReader readerDrink;
            try
            {
                databaseConnectionDrink.Open();
                readerDrink = commandDatabaseDrink.ExecuteReader();
                if (readerDrink.HasRows)
                {
                    while (readerDrink.Read())
                    {
                        byte[] blob = (byte[])readerDrink["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();

                        bestDrinks = readerDrink["ProductName"].ToString();
                        string basePriceDrinks = readerDrink["BasePriceInPeso"].ToString();
                        bestDrinksID = Convert.ToInt32(readerDrink["ProductID"]);

                        Image productImageDrink = new Image();
                        productImageDrink.Source = bi;
                        TextBlock txtProduct = new TextBlock();
                        txtProduct.HorizontalAlignment = HorizontalAlignment.Center;
                        txtProduct.Inlines.Add(bestDrinks + " ");
                        txtProduct.Inlines.Add(basePriceDrinks);
                        StackPanel stackPnl = new StackPanel();
                        stackPnl.Orientation = Orientation.Vertical;
                        stackPnl.Margin = new Thickness(5);
                        stackPnl.Children.Add(txtProduct);
                        stackPnl.Children.Add(productImageDrink);

                        btnBestDrink.Content = stackPnl;

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnectionDrink.Close();
        }
        private void BtnBestBarbeeQ_Click(object sender, RoutedEventArgs e)
        {
            chosenProductInBestSellerID = bestBarbeeQID;
            isBestSeller = true;
            isDrinksOrDesserts = false;

            txtQuantity.Text = "1";
            string query = "SELECT ProductImage FROM tbl_Product WHERE tbl_Product.ProductID=@PRODUCTID";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            commandDatabase.Parameters.AddWithValue("@PRODUCTID", chosenProductInBestSellerID);
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        byte[] blob = (byte[])reader["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        imgProduct.Source = bi;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection.Close();
        }

        private void BtnBestGotome_Click(object sender, RoutedEventArgs e)
        {
            chosenProductInBestSellerID = bestGotomeID;
            isBestSeller = true;
            isDrinksOrDesserts = false;
            txtQuantity.Text = "1";
            string query = "SELECT ProductImage FROM tbl_Product WHERE tbl_Product.ProductID=@PRODUCTID";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            commandDatabase.Parameters.AddWithValue("@PRODUCTID", chosenProductInBestSellerID);
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        byte[] blob = (byte[])reader["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        imgProduct.Source = bi;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection.Close();
        }

        private void BtnBestLomi_Click(object sender, RoutedEventArgs e)
        {
            chosenProductInBestSellerID = bestLomiID;
            isBestSeller = true;
            isDrinksOrDesserts = false;
            txtQuantity.Text = "1";
            string query = "SELECT ProductImage FROM tbl_Product WHERE tbl_Product.ProductID=@PRODUCTID";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            commandDatabase.Parameters.AddWithValue("@PRODUCTID", chosenProductInBestSellerID);
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        byte[] blob = (byte[])reader["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        imgProduct.Source = bi;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection.Close();
        }

        private void BtnBestPares_Click(object sender, RoutedEventArgs e)
        {
            chosenProductInBestSellerID = bestParesID;
            isBestSeller = true;
            isDrinksOrDesserts = false;
            txtQuantity.Text = "1";
            string query = "SELECT ProductImage FROM tbl_Product WHERE tbl_Product.ProductID=@PRODUCTID";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            commandDatabase.Parameters.AddWithValue("@PRODUCTID", chosenProductInBestSellerID);
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        byte[] blob = (byte[])reader["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        imgProduct.Source = bi;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection.Close();
        }

        private void BtnBestTapsi_Click(object sender, RoutedEventArgs e)
        {
            chosenProductInBestSellerID = bestTapsiID;
            isBestSeller = true;
            isDrinksOrDesserts = false;
            txtQuantity.Text = "1";
            string query = "SELECT ProductImage FROM tbl_Product WHERE tbl_Product.ProductID=@PRODUCTID";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            commandDatabase.Parameters.AddWithValue("@PRODUCTID", chosenProductInBestSellerID);
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        byte[] blob = (byte[])reader["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        imgProduct.Source = bi;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection.Close();
        }

        private void BtnBestDessert_Click(object sender, RoutedEventArgs e)
        {
            chosenProductInBestSellerID = bestDessertsID;
            isBestSeller = true;
            isDrinksOrDesserts = true;

            txtQuantity.Text = "1";
            string query = "SELECT ProductImage FROM tbl_Product WHERE tbl_Product.ProductID=@PRODUCTID";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            commandDatabase.Parameters.AddWithValue("@PRODUCTID", chosenProductInBestSellerID);
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        byte[] blob = (byte[])reader["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        imgProduct.Source = bi;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection.Close();
        }

        private void BtnBestDrink_Click(object sender, RoutedEventArgs e)
        {
            chosenProductInBestSellerID = bestDrinksID;
            isBestSeller = true;
            isDrinksOrDesserts = true;

            txtQuantity.Text = "1";
            string query = "SELECT ProductImage FROM tbl_Product WHERE tbl_Product.ProductID=@PRODUCTID";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            commandDatabase.Parameters.AddWithValue("@PRODUCTID", chosenProductInBestSellerID);
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        byte[] blob = (byte[])reader["ProductImage"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        imgProduct.Source = bi;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection.Close();
        }
    }
}

