using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Drawing;
using Microsoft.Win32;

namespace KumparesFinal
{
    /// <summary>
    /// Interaction logic for ProductManagement.xaml
    /// </summary>
    public partial class ProductManagement : Window
    {
        const string getConnectionString = @"server=localhost; user id=root;password=; database=db_Kumpares";
        string strName, imageName = "";
        string productID;
        public ProductManagement()
        {
            InitializeComponent();
            initializeCategoryList();
            listProducts();
            productDataGrid.CanUserAddRows = false;
            productDataGrid.CanUserResizeColumns = false;
            productDataGrid.CanUserResizeRows = false;
            productDataGrid.CanUserDeleteRows = false;
        }
        public void initializeCategoryList()
        {
            string query = "SELECT * FROM `tbl_category` ORDER BY `CategoryDescription`";
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
                        cmbCategory.Items.Add(reader.GetString(1));
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection.Close();
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
                productDataGrid.ItemsSource = dtProduct.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection.Close();

        }
        private void MainListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (MainListView.SelectedIndex == 0)
            {
                AdminInterface adminInterfaceWindow = new AdminInterface();
                adminInterfaceWindow.Show();
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

        private void ProductDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView selected = gd.SelectedItem as DataRowView;
            if (productDataGrid.SelectedIndex > -1)
            {
                btnUpdateProduct.IsEnabled = true;
                btnDeleteProduct.IsEnabled = true;
                txtProductID.Text = selected["ProductID"].ToString();
                txtProductName.Text = selected["ProductName"].ToString();
                txtBasePriceInPeso.Text = selected["BasePriceInPeso"].ToString();
                cmbCategory.SelectedItem = selected["CategoryDescription"].ToString();
                productID = selected["ProductID"].ToString();
                if (selected["IsAvailable"].ToString() == "Available")
                {
                    rbnAvailable.IsChecked = true;
                    rbnUnavailable.IsChecked = false;
                }
                else
                {
                    rbnAvailable.IsChecked = false;
                    rbnUnavailable.IsChecked = true;
                }
            }
            else
            {
                btnUpdateProduct.IsEnabled = false;
                btnDeleteProduct.IsEnabled = false;
                txtProductID.Text = "";
                txtProductName.Text = "";
                txtBasePriceInPeso.Text = "";
                cmbCategory.SelectedItem = "";
                rbnAvailable.IsChecked = null;
                rbnUnavailable.IsChecked = null;
            }
            
            string query = "SELECT ProductImage FROM tbl_Product WHERE tbl_Product.ProductID=@PRODUCTID";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            commandDatabase.Parameters.AddWithValue("@PRODUCTID", txtProductID.Text);
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

        private void BtnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            string query = "DELETE FROM tbl_Product WHERE ProductID = @PRODUCTID";

            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.Parameters.AddWithValue("@PRODUCTID", Convert.ToInt32(txtProductID.Text));
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                MessageBox.Show("Product succesfully deleted");
                rbnAvailable.IsChecked = false;
                rbnUnavailable.IsChecked = false;
                databaseConnection.Close();
                imgProduct.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Photos/Kumpares Icon.jpg"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                listProducts();
            }
        }

        private void checkIfEmpty(object sender, TextChangedEventArgs e)
        {
            if (txtProductName.Text.Length > 0 && txtBasePriceInPeso.Text.Length > 0 && cmbCategory.SelectedItem != null && (rbnAvailable.IsChecked == true || rbnUnavailable.IsChecked == true) && imgProduct.Source != null)
            {
                btnCreateProduct.IsEnabled = true;
            }
            else
            {
                btnCreateProduct.IsEnabled = false;
            }
        }

        private void checkIfEmpty(object sender, SelectionChangedEventArgs e)
        {
            if (txtProductName.Text.Length > 0 && txtBasePriceInPeso.Text.Length > 0 && cmbCategory.SelectedItem != null && (rbnAvailable.IsChecked == true || rbnUnavailable.IsChecked == true) && imgProduct.Source != null)
            {
                btnCreateProduct.IsEnabled = true;
            }
            else
            {
                btnCreateProduct.IsEnabled = false;
            }
        }

        private void checkIfEmpty(object sender, RoutedEventArgs e)
        {
            if (txtProductName.Text.Length > 0 && txtBasePriceInPeso.Text.Length > 0 && cmbCategory.SelectedItem != null && (rbnAvailable.IsChecked == true || rbnUnavailable.IsChecked == true) && imgProduct.Source != null)
            {
                btnCreateProduct.IsEnabled = true;
            }
            else
            {
                btnCreateProduct.IsEnabled = false;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image File (*.jpg;*.bmp;*.gif)|*.jpg;*.bmp;*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                imgProduct.Source = new BitmapImage(fileUri);
                strName = openFileDialog.SafeFileName;
                imageName = openFileDialog.FileName;
            }

        }

        private void BtnUpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            int categorySelection = 0;
            int availability = 0;
            if (cmbCategory.SelectedIndex == 0)
            {
                categorySelection = 2;
            }
            else if (cmbCategory.SelectedIndex == 1)
            {
                categorySelection = 7;
            }
            else if (cmbCategory.SelectedIndex == 2)
            {
                categorySelection = 6;
            }
            else if (cmbCategory.SelectedIndex == 3)
            {
                categorySelection = 3;
            }
            else if (cmbCategory.SelectedIndex == 4)
            {
                categorySelection = 1;
            }
            else if (cmbCategory.SelectedIndex == 5)
            {
                categorySelection = 5;
            }
            else if (cmbCategory.SelectedIndex == 6)
            {
                categorySelection = 4;
            }
            if (rbnAvailable.IsChecked == true)
            {
                availability = 1;
            }
            else
            {
                availability = 0;
            }
            if (imageName.Length > 0)
            {
                string query = "UPDATE `tbl_Product` SET `ProductName`=@PRODUCTNAME,`BasePriceInPeso`=@BASEPRICEINPESO, `CategoryID`=@CATEGORYID, `ProductImage`=@PRODUCTIMAGE, `IsAvailable`=@ISAVAILABLE WHERE ProductID = @PRODUCTID";
                MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                try
                {
                    commandDatabase.Parameters.AddWithValue("@PRODUCTID", Convert.ToInt32(txtProductID.Text));
                    commandDatabase.Parameters.AddWithValue("@PRODUCTNAME", txtProductName.Text);
                    commandDatabase.Parameters.AddWithValue("@BASEPRICEINPESO", Convert.ToDouble(txtBasePriceInPeso.Text));
                    commandDatabase.Parameters.AddWithValue("@CATEGORYID", categorySelection);
                    FileStream fileStream = new FileStream(imageName, FileMode.Open, FileAccess.Read);
                    byte[] imgByteArr = new byte[fileStream.Length];
                    fileStream.Read(imgByteArr, 0, Convert.ToInt32(fileStream.Length));
                    fileStream.Close();
                    commandDatabase.Parameters.Add(new MySqlParameter("@PRODUCTIMAGE", imgByteArr));
                    commandDatabase.Parameters.AddWithValue("@ISAVAILABLE", availability);
                    commandDatabase.CommandTimeout = 60;
                    databaseConnection.Open();
                    MySqlDataReader myReader = commandDatabase.ExecuteReader();
                    MessageBox.Show("Product Successfully Updated");
                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    listProducts();
                }
            }
            else 
            {
                byte[] blob;
                string query2 = "SELECT ProductImage FROM tbl_Product WHERE tbl_Product.ProductID=@PRODUCTID";
                MySqlConnection databaseConnection2 = new MySqlConnection(getConnectionString);
                MySqlCommand commandDatabase2 = new MySqlCommand(query2, databaseConnection2);
                commandDatabase2.CommandTimeout = 60;
                commandDatabase2.Parameters.AddWithValue("@PRODUCTID", txtProductID.Text);
                MySqlDataReader reader;
                try
                {
                    databaseConnection2.Open();
                    reader = commandDatabase2.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                         blob = (byte[])reader["ProductImage"];
                            string query = "UPDATE `tbl_Product` SET `ProductName`=@PRODUCTNAME,`BasePriceInPeso`=@BASEPRICEINPESO, `CategoryID`=@CATEGORYID, `ProductImage`=@PRODUCTIMAGE, `IsAvailable`=@ISAVAILABLE WHERE ProductID = @PRODUCTID";
                            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
                            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                            try
                            {
                                commandDatabase.Parameters.AddWithValue("@PRODUCTID", Convert.ToInt32(txtProductID.Text));
                                commandDatabase.Parameters.AddWithValue("@PRODUCTNAME", txtProductName.Text);
                                commandDatabase.Parameters.AddWithValue("@BASEPRICEINPESO", Convert.ToDouble(txtBasePriceInPeso.Text));
                                commandDatabase.Parameters.AddWithValue("@CATEGORYID", categorySelection);
                                commandDatabase.Parameters.Add(new MySqlParameter("@PRODUCTIMAGE", blob));
                                commandDatabase.Parameters.AddWithValue("@ISAVAILABLE", availability);
                                commandDatabase.CommandTimeout = 60;
                                databaseConnection.Open();
                                MySqlDataReader myReader = commandDatabase.ExecuteReader();
                                MessageBox.Show("Product Successfully Updated");
                                databaseConnection.Close();
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("No Picture Selected Or Base Price Format Is Incorrect");
                            }
                            finally
                            {
                                listProducts();
                            }

                        }
                        databaseConnection2.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            imgProduct.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Photos/Kumpares Icon.jpg"));
            imageName = "";
        }
        private void BtnCreateProduct_Click_1(object sender, RoutedEventArgs e)
        {
            int categorySelection = 0;
            int availability = 0;
            if (cmbCategory.SelectedIndex == 0)
            {
                categorySelection = 2;
            }
            else if (cmbCategory.SelectedIndex == 1)
            {
                categorySelection = 7;
            }
            else if (cmbCategory.SelectedIndex == 2)
            {
                categorySelection = 6;
            }
            else if (cmbCategory.SelectedIndex == 3)
            {
                categorySelection = 3;
            }
            else if (cmbCategory.SelectedIndex == 4)
            {
                categorySelection = 1;
            }
            else if (cmbCategory.SelectedIndex == 5)
            {
                categorySelection = 5;
            }
            else if (cmbCategory.SelectedIndex == 6)
            {
                categorySelection = 4;
            }
            if (rbnAvailable.IsChecked == true)
            {
                availability = 1;
            }
            else
            {
                availability = 0;
            }
            if (imageName.Length > 0)
            {
                string query = "INSERT INTO tbl_Product(`ProductName`, `BasePriceInPeso`, `CategoryID`, `ProductImage`, `IsAvailable`) VALUES (@PRODUCTNAME, @BASEPRICEINPESO, @CATEGORYID, @PRODUCTIMAGE, @ISAVAILABLE)";
                MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                try
                {
                    commandDatabase.Parameters.AddWithValue("@PRODUCTNAME", txtProductName.Text);
                    commandDatabase.Parameters.AddWithValue("@BASEPRICEINPESO", Convert.ToDouble(txtBasePriceInPeso.Text));
                    commandDatabase.Parameters.AddWithValue("@CATEGORYID", categorySelection);
                    FileStream fileStream = new FileStream(imageName, FileMode.Open, FileAccess.Read);
                    byte[] imgByteArr = new byte[fileStream.Length];
                    fileStream.Read(imgByteArr, 0, Convert.ToInt32(fileStream.Length));
                    fileStream.Close();
                    commandDatabase.Parameters.Add(new MySqlParameter("@PRODUCTIMAGE", imgByteArr));
                    commandDatabase.Parameters.AddWithValue("@ISAVAILABLE", availability);
                    commandDatabase.CommandTimeout = 60;
                    databaseConnection.Open();
                    MySqlDataReader myReader = commandDatabase.ExecuteReader();
                    MessageBox.Show("Product Successfully Created");
                    databaseConnection.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("No Picture Selected Or Base Price Format Is Incorrect");
                }
                finally
                {
                    listProducts();
                }
            }
            else
            {
                byte[] blob;
                string query2 = "SELECT ProductImage FROM tbl_Product WHERE tbl_Product.ProductID=@PRODUCTID";
                MySqlConnection databaseConnection2 = new MySqlConnection(getConnectionString);
                MySqlCommand commandDatabase2 = new MySqlCommand(query2, databaseConnection2);
                commandDatabase2.CommandTimeout = 60;
                commandDatabase2.Parameters.AddWithValue("@PRODUCTID", productID);
                MySqlDataReader reader;
                try
                {
                    databaseConnection2.Open();
                    reader = commandDatabase2.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            blob = (byte[])reader["ProductImage"];
                            string query = "INSERT INTO tbl_Product(`ProductName`, `BasePriceInPeso`, `CategoryID`, `ProductImage`, `IsAvailable`) VALUES (@PRODUCTNAME, @BASEPRICEINPESO, @CATEGORYID, @PRODUCTIMAGE, @ISAVAILABLE)";
                            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
                            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                            try
                            {
                                commandDatabase.Parameters.AddWithValue("@PRODUCTNAME", txtProductName.Text);
                                commandDatabase.Parameters.AddWithValue("@BASEPRICEINPESO", Convert.ToDouble(txtBasePriceInPeso.Text));
                                commandDatabase.Parameters.AddWithValue("@CATEGORYID", categorySelection);
                                commandDatabase.Parameters.Add(new MySqlParameter("@PRODUCTIMAGE", blob));
                                commandDatabase.Parameters.AddWithValue("@ISAVAILABLE", availability);
                                commandDatabase.CommandTimeout = 60;
                                databaseConnection.Open();
                                MySqlDataReader myReader = commandDatabase.ExecuteReader();
                                MessageBox.Show("Product Successfully Created");
                                databaseConnection.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                listProducts();
                            }

                        }
                        databaseConnection2.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            imgProduct.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Photos/Kumpares Icon.jpg"));
            imageName = "";
        }    
    }
}
