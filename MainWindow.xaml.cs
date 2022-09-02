using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace KumparesFinal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string defaultAdminUsername = "admin";
        string defaultAdminPassword = "admin";
        string getConnectionString = @"server=localhost; user id=root;password=; database=db_Kumpares";
        MySqlCommand cm = new MySqlCommand();
        MySqlConnection cn = new MySqlConnection();
        MySqlDataReader dr;
        public MainWindow()
        {
            InitializeComponent();
            cn = new MySqlConnection(getConnectionString);
            cn.Open();

        }
        private void TxtUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtUserName.Text = "";
            txtUserName.Foreground = Brushes.Black;
        }

        private void PwdPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            pwdPassword.Password = "";
            pwdPassword.Foreground = Brushes.Black;
        }
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            AdminInterface adminInterfaceWindow = new AdminInterface();
            String sql = "select * from tbl_Cashier where username like '" + txtUserName.Text + "'";
            cm = new MySqlCommand(sql, cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if (txtUserName.Text == defaultAdminUsername && pwdPassword.Password == defaultAdminPassword)
            {
                adminInterfaceWindow.Show();
                this.Hide();
                dr.Close();
            }
            else if (dr.HasRows)
            {
                try
                {
                    if (txtUserName.Text == dr[1].ToString() && CalculateMD5Hash(pwdPassword.Password) == dr[2].ToString())
                    {
                        CashierInterface cashierInterfaceWindow = new CashierInterface(txtUserName.Text);
                        cashierInterfaceWindow.Show();
                        this.Hide();
                        dr.Close();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect Username or Password");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Incorrect Username or Password");
                    dr.Close();
                }
                dr.Close();
            }
            else
            {
                MessageBox.Show("Incorrect Username or Password");
                dr.Close();
            }
        }

        private void PwdPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AdminInterface adminInterfaceWindow = new AdminInterface();
                String sql = "select * from tbl_Cashier where username like '" + txtUserName.Text + "'";
                cm = new MySqlCommand(sql, cn);
                dr = cm.ExecuteReader();
                dr.Read();
                if (txtUserName.Text == defaultAdminUsername && pwdPassword.Password == defaultAdminPassword)
                {
                    adminInterfaceWindow.Show();
                    this.Hide();
                    dr.Close();

                }

                else if (dr.HasRows)
                {
                    try
                    {
                        if (txtUserName.Text == dr[1].ToString() && CalculateMD5Hash(pwdPassword.Password) == dr[2].ToString())
                        {
                            CashierInterface cashierInterfaceWindow = new CashierInterface(txtUserName.Text);
                            cashierInterfaceWindow.Show();
                            this.Hide();
                            dr.Close();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Username or Password");
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Incorrect Username or Password");
                        dr.Close();
                    }
                    dr.Close();
                }
                else
                {
                    MessageBox.Show("Incorrect Username or Password");
                    dr.Close();
                }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
