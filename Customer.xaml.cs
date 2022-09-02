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
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer : Window
    {
        const string getConnectionString = @"server=localhost; user id=root;password=; database=db_Kumpares";
        int currentCustomerID = 0;
        int currentTransactionID = 0;
        public Customer()
        {
            InitializeComponent();
        }

        private void TxtCustomerName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtCustomerName.Text = "";
        }

        private void BtnProceed_Click(object sender, RoutedEventArgs e)
        {
            if(rbnDineIn.IsChecked != false || rbntakeOut.IsChecked != false)
            {
                int dineInOrTakeOut = 0;
                if (rbnDineIn.IsChecked == true)
                {
                    dineInOrTakeOut = 1;
                }
                else
                {
                    dineInOrTakeOut = 0;
                }

                string query1 = "SELECT MAX(CustomerID) FROM tbl_Customer";
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
                            currentCustomerID = reader.GetInt32(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                databaseConnection1.Close();

                string query3 = "SELECT MAX(TransactionID) FROM tbl_Transaction";
                MySqlConnection databaseConnection3 = new MySqlConnection(getConnectionString);
                MySqlCommand commandDatabase3 = new MySqlCommand(query3, databaseConnection3);
                commandDatabase3.CommandTimeout = 60;
                MySqlDataReader reader3;
                try
                {
                    databaseConnection3.Open();
                    reader3 = commandDatabase3.ExecuteReader();
                    if (reader3.HasRows)
                    {
                        while (reader3.Read())
                        {
                            currentTransactionID = reader3.GetInt32(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                databaseConnection1.Close();

                string query = "UPDATE `tbl_Customer` SET `CustomerFirstName`=@CUSTOMERFIRSTNAME WHERE CustomerID = @CUSTOMERID";
                MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                try
                {
                    commandDatabase.Parameters.AddWithValue("@CUSTOMERID", currentCustomerID);
                    commandDatabase.Parameters.AddWithValue("@CUSTOMERFIRSTNAME", txtCustomerName.Text);
                    commandDatabase.CommandTimeout = 60;
                    databaseConnection.Open();
                    MySqlDataReader myReader = commandDatabase.ExecuteReader();
                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string query2 = "UPDATE `tbl_Transaction` SET `CustomerID`=@CURRENTCUSTOMERID, `DineIn`=@DINEIN WHERE TransactionID = @TRANSACTIONID";
                MySqlConnection databaseConnection2 = new MySqlConnection(getConnectionString);
                MySqlCommand commandDatabase2 = new MySqlCommand(query2, databaseConnection2);
                try
                {
                    commandDatabase2.Parameters.AddWithValue("@CURRENTCUSTOMERID", currentCustomerID);
                    commandDatabase2.Parameters.AddWithValue("@TRANSACTIONID", Convert.ToInt32(currentTransactionID));
                    commandDatabase2.Parameters.AddWithValue("@DINEIN", Convert.ToInt32(dineInOrTakeOut));
                    commandDatabase2.CommandTimeout = 60;
                    databaseConnection2.Open();
                    MySqlDataReader myReader2 = commandDatabase2.ExecuteReader();
                    databaseConnection2.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.Hide();
            }
            else
            {
                MessageBox.Show("Please Choose if Dine In or Take Out ");
            }
 
        }

        private void RbnDineIn_Checked(object sender, RoutedEventArgs e)
        {
            rbntakeOut.IsChecked = false;
        }

        private void RbnDineIn_Unchecked(object sender, RoutedEventArgs e)
        {
            rbntakeOut.IsChecked = true;
        }

        private void RbntakeOut_Checked(object sender, RoutedEventArgs e)
        {
            rbnDineIn.IsChecked = false;
        }

        private void RbntakeOut_Unchecked(object sender, RoutedEventArgs e)
        {
            rbnDineIn.IsChecked = true;
        }
    }
}
