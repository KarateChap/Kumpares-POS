using System.Windows;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Security.Cryptography;

namespace KumparesFinal
{
    /// <summary>
    /// Interaction logic for CashierManagement.xaml
    /// </summary>
    public partial class CashierManagement : Window
    {
        const string getConnectionString = @"server=localhost; user id=root;password=; database=db_Kumpares";
        string strName, imageName = "";
        string cashierID;
        public CashierManagement()
        {
            InitializeComponent();
            listUsers();
            cashierDataGrid.CanUserAddRows = false;
            cashierDataGrid.CanUserResizeColumns = false;
            cashierDataGrid.CanUserResizeRows = false;
            cashierDataGrid.CanUserDeleteRows = false;
        }
        private void listUsers()
        {
            string query = "SELECT CashierID, FirstName, LastName, ContactNumber, EmailAddress FROM tbl_Cashier";
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
                cashierDataGrid.ItemsSource = dtCashier.DefaultView;

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
            if (MainListView.SelectedIndex == 1)
            {
                ProductManagement productManagementWindow = new ProductManagement();
                productManagementWindow.Show();
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

        private void CashierDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView selected = gd.SelectedItem as DataRowView;
            if (cashierDataGrid.SelectedIndex > -1)
            {
                btnUpdateAccount.IsEnabled = true;
                btnDeleteAccount.IsEnabled = true;
                txtCashierID.Text = selected["CashierID"].ToString();
                txtUsername.Text = "[Change Username]";
                txtPassword.Text = "[Change Password]";
                txtFirstName.Text = selected["FirstName"].ToString();
                txtLastName.Text = selected["LastName"].ToString();
                txtContactNumber.Text = selected["ContactNumber"].ToString();
                txtEmailAddress.Text = selected["EmailAddress"].ToString();
                cashierID = selected["CashierID"].ToString();
            }
            else
            {
                btnUpdateAccount.IsEnabled = false;
                btnDeleteAccount.IsEnabled = false;
                txtCashierID.Text = "";
                txtUsername.Text = "";
                txtPassword.Text = "";
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtContactNumber.Text = "";
                txtEmailAddress.Text = "";
            }
            string query = "SELECT CashierImage FROM tbl_Cashier WHERE tbl_Cashier.CashierID=@CASHIERID";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            commandDatabase.Parameters.AddWithValue("@CASHIERID", txtCashierID.Text);
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
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            databaseConnection.Close();
        }
        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image File (*.jpg;*.bmp;*.gif)|*.jpg;*.bmp;*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                imgCashier.Source = new BitmapImage(fileUri);
                strName = openFileDialog.SafeFileName;
                imageName = openFileDialog.FileName;
            }
        }
        private void BtnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            bool noAccount1 = true;
            bool noAccount2 = true;
            if (imageName.Length > 0)
            {
                string query1 = "select * from tbl_Cashier where username like '" + txtUsername.Text + "'";
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
                            if (txtUsername.Text == reader[1].ToString())
                            {
                                MessageBox.Show("Username is Already Existing. Please Choose Other Username");
                                noAccount1 = false;
                            }
                            else
                            {

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                databaseConnection1.Close();

                if(noAccount1 == true)
                {
                    string query = "INSERT INTO tbl_Cashier(`Username`, `Password`, `FirstName`, `LastName`, `ContactNumber`, `EmailAddress`, `CashierImage`) VALUES (@USERNAME, @PASSWORD, @FIRSTNAME, @LASTNAME, @CONTACTNUMBER, @EMAILADDRESS, @CASHIERIMAGE)";
                    MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
                    MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                    try
                    {
                        commandDatabase.Parameters.AddWithValue("@USERNAME", txtUsername.Text);
                        commandDatabase.Parameters.AddWithValue("@PASSWORD", CalculateMD5Hash(txtPassword.Text));
                        commandDatabase.Parameters.AddWithValue("@FIRSTNAME", txtFirstName.Text);
                        commandDatabase.Parameters.AddWithValue("@LASTNAME", txtLastName.Text);
                        commandDatabase.Parameters.AddWithValue("@CONTACTNUMBER", txtContactNumber.Text);
                        commandDatabase.Parameters.AddWithValue("@EMAILADDRESS", txtEmailAddress.Text);
                        FileStream fileStream = new FileStream(imageName, FileMode.Open, FileAccess.Read);
                        byte[] imgByteArr = new byte[fileStream.Length];
                        fileStream.Read(imgByteArr, 0, Convert.ToInt32(fileStream.Length));
                        fileStream.Close();
                        commandDatabase.Parameters.Add(new MySqlParameter("@CASHIERIMAGE", imgByteArr));
                        commandDatabase.CommandTimeout = 60;
                        databaseConnection.Open();
                        MySqlDataReader myReader = commandDatabase.ExecuteReader();
                        MessageBox.Show("Account Succesfully Created");
                        databaseConnection.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("No Picture Selected");
                    }
                    finally
                    {
                        listUsers();
                    }
                }
                
            }
            else
            {
                string query1 = "select * from tbl_Cashier where username like '" + txtUsername.Text + "'";
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
                            if (txtUsername.Text == reader1[1].ToString())
                            {
                                MessageBox.Show("Username is Already Existing. Please Choose Other Username");
                                noAccount2 = false;
                            }
                            else
                            {

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                databaseConnection1.Close();
                if(noAccount2 == true)
                {
                    byte[] blob;
                    string query2 = "SELECT CashierImage FROM tbl_Cashier WHERE tbl_Cashier.CashierID=@CASHIERID";
                    MySqlConnection databaseConnection2 = new MySqlConnection(getConnectionString);
                    MySqlCommand commandDatabase2 = new MySqlCommand(query2, databaseConnection2);
                    commandDatabase2.CommandTimeout = 60;
                    commandDatabase2.Parameters.AddWithValue("@CASHIERID", cashierID);
                    MySqlDataReader reader3;
                    try
                    {
                        databaseConnection2.Open();
                        reader3 = commandDatabase2.ExecuteReader();
                        if (reader3.HasRows)
                        {
                            while (reader3.Read())
                            {
                                blob = (byte[])reader3["CashierImage"];
                                string query = "INSERT INTO tbl_Cashier(`Username`, `Password`, `FirstName`, `LastName`, `ContactNumber`, `EmailAddress`, `CashierImage`) VALUES (@USERNAME, @PASSWORD, @FIRSTNAME, @LASTNAME, @CONTACTNUMBER, @EMAILADDRESS, @CASHIERIMAGE)";
                                MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
                                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                                try
                                {
                                    commandDatabase.Parameters.AddWithValue("@USERNAME", txtUsername.Text);
                                    commandDatabase.Parameters.AddWithValue("@PASSWORD", CalculateMD5Hash(txtPassword.Text));
                                    commandDatabase.Parameters.AddWithValue("@FIRSTNAME", txtFirstName.Text);
                                    commandDatabase.Parameters.AddWithValue("@LASTNAME", txtLastName.Text);
                                    commandDatabase.Parameters.AddWithValue("@CONTACTNUMBER", txtContactNumber.Text);
                                    commandDatabase.Parameters.AddWithValue("@EMAILADDRESS", txtEmailAddress.Text);
                                    commandDatabase.Parameters.Add(new MySqlParameter("@CASHIERIMAGE", blob));
                                    commandDatabase.CommandTimeout = 60;
                                    databaseConnection.Open();
                                    MySqlDataReader myReader4 = commandDatabase.ExecuteReader();
                                    MessageBox.Show("Account succesfully updated");
                                    databaseConnection.Close();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                finally
                                {
                                    listUsers();
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
            }
            imgCashier.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Photos/Woman Icon.jpg"));
            imageName = "";
        }
        private void checkTextBoxIfEmpty(object sender, TextChangedEventArgs e)
        {
            if (txtUsername.Text.Length > 0 && txtPassword.Text.Length > 0 && txtFirstName.Text.Length > 0 && txtLastName.Text.Length > 0 && txtContactNumber.Text.Length > 0 && txtEmailAddress.Text.Length > 0 && imgCashier.Source != null)
            {
                btnCreateAccount.IsEnabled = true;
            }
            else
            {
                btnCreateAccount.IsEnabled = false;
            }
        }
        private void BtnUpdateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (imageName.Length > 0)
            {
                string query = "UPDATE `tbl_Cashier` SET `Username`=@USERNAME,`Password`=@PASSWORD, `FirstName`=@FIRSTNAME, `LastName`=@LASTNAME, `ContactNumber`=@CONTACTNUMBER, `EmailAddress`=@EMAILADDRESS, `CashierImage`=@CASHIERIMAGE WHERE CashierID = @CASHIERID";
                MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                try
                {
                    commandDatabase.Parameters.AddWithValue("@CASHIERID", Convert.ToInt32(txtCashierID.Text));
                    commandDatabase.Parameters.AddWithValue("@USERNAME", txtUsername.Text);
                    commandDatabase.Parameters.AddWithValue("@PASSWORD", CalculateMD5Hash(txtPassword.Text));
                    commandDatabase.Parameters.AddWithValue("@FIRSTNAME", txtFirstName.Text);
                    commandDatabase.Parameters.AddWithValue("@LASTNAME", txtLastName.Text);
                    commandDatabase.Parameters.AddWithValue("@CONTACTNUMBER", txtContactNumber.Text);
                    commandDatabase.Parameters.AddWithValue("@EMAILADDRESS", txtEmailAddress.Text);
                    FileStream fileStream = new FileStream(imageName, FileMode.Open, FileAccess.Read);
                    byte[] imgByteArr = new byte[fileStream.Length];
                    fileStream.Read(imgByteArr, 0, Convert.ToInt32(fileStream.Length));
                    fileStream.Close();
                    commandDatabase.Parameters.Add(new MySqlParameter("@CASHIERIMAGE", imgByteArr));
                    commandDatabase.CommandTimeout = 60;
                    databaseConnection.Open();
                    MySqlDataReader reader = commandDatabase.ExecuteReader();
                    MessageBox.Show("Account succesfully updated");
                    databaseConnection.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("No Picture Selected");
                }
                finally
                {
                    listUsers();
                }
            }
            else
            {
                byte[] blob;
                string query2 = "SELECT CashierImage FROM tbl_Cashier WHERE tbl_Cashier.CashierID=@CASHIERID";
                MySqlConnection databaseConnection2 = new MySqlConnection(getConnectionString);
                MySqlCommand commandDatabase2 = new MySqlCommand(query2, databaseConnection2);
                commandDatabase2.CommandTimeout = 60;
                commandDatabase2.Parameters.AddWithValue("@CASHIERID", txtCashierID.Text);
                MySqlDataReader myReader;
                try
                {
                    databaseConnection2.Open();
                    myReader = commandDatabase2.ExecuteReader();
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            blob = (byte[])myReader["CashierImage"];
                            string query = "UPDATE `tbl_Cashier` SET `Username`=@USERNAME,`Password`=@PASSWORD, `FirstName`=@FIRSTNAME, `LastName`=@LASTNAME, `ContactNumber`=@CONTACTNUMBER, `EmailAddress`=@EMAILADDRESS, `CashierImage`=@CASHIERIMAGE WHERE CashierID = @CASHIERID";
                            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
                            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                            try
                            {
                                commandDatabase.Parameters.AddWithValue("@CASHIERID", Convert.ToInt32(txtCashierID.Text));
                                commandDatabase.Parameters.AddWithValue("@USERNAME", txtUsername.Text);
                                commandDatabase.Parameters.AddWithValue("@PASSWORD", CalculateMD5Hash(txtPassword.Text));
                                commandDatabase.Parameters.AddWithValue("@FIRSTNAME", txtFirstName.Text);
                                commandDatabase.Parameters.AddWithValue("@LASTNAME", txtLastName.Text);
                                commandDatabase.Parameters.AddWithValue("@CONTACTNUMBER", txtContactNumber.Text);
                                commandDatabase.Parameters.AddWithValue("@EMAILADDRESS", txtEmailAddress.Text);
                                commandDatabase.Parameters.Add(new MySqlParameter("@CASHIERIMAGE", blob));
                                commandDatabase.CommandTimeout = 60;
                                databaseConnection.Open();
                                MySqlDataReader reader = commandDatabase.ExecuteReader();
                                MessageBox.Show("Account succesfully updated");
                                databaseConnection.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                listUsers();
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
            imgCashier.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Photos/Woman Icon.jpg"));
            imageName = "";
            
        }
        
        private void BtnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            string query = "DELETE FROM `tbl_Cashier` WHERE CashierID = @CASHIERID";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.Parameters.AddWithValue("@CASHIERID", Convert.ToInt32(txtCashierID.Text));
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                MessageBox.Show("Account succesfully deleted");
                databaseConnection.Close();
                imgCashier.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Photos/Woman Icon.jpg"));
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot Delete User Account, Account is Listed in Some Transactions");
            }
            finally
            {
                listUsers();
            }
        }

        public string CalculateMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        
    }

}