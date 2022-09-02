using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;

namespace KumparesFinal
{
    
    /// <summary>
    /// Interaction logic for AdminInterface.xaml
    /// </summary>
    public partial class AdminInterface : Window
    {
        const string getConnectionString = @"server=localhost; user id=root;password=; database=db_Kumpares";
        string fromDate = "";
        string toDate = "";

        public AdminInterface()
        {
            fromDate = "2000-01-01";
            toDate = "3000-01-01";
            InitializeComponent();
            initializeBarbeeQ();
            initializeGotome();
            initializeLomi();
            initializePares();
            initializeTapsi();
            initializeDessert();
            initializeDrink();
            initializeAll();
            initializeIncome();
            initilizePriceOfEachCategory();
        }
        Func<ChartPoint, string> allLabelPoint = chartpoint => string.Format("{0} ({1:P})", chartpoint.Y, chartpoint.Participation);
        Func<ChartPoint, string> barbeeQLabelPoint = chartpoint => string.Format("{0} ({1:P})", chartpoint.Y, chartpoint.Participation);
        Func<ChartPoint, string> gotomeLabelPoint = chartpoint => string.Format("{0} ({1:P})", chartpoint.Y, chartpoint.Participation);
        Func<ChartPoint, string> lomiLabelPoint = chartpoint => string.Format("{0} ({1:P})", chartpoint.Y, chartpoint.Participation);
        Func<ChartPoint, string> paresLabelPoint = chartpoint => string.Format("{0} ({1:P})", chartpoint.Y, chartpoint.Participation);
        Func<ChartPoint, string> tapsiLabelPoint = chartpoint => string.Format("{0} ({1:P})", chartpoint.Y, chartpoint.Participation);
        Func<ChartPoint, string> dessertLabelPoint = chartpoint => string.Format("{0} ({1:P})", chartpoint.Y, chartpoint.Participation);
        Func<ChartPoint, string> drinkLabelPoint = chartpoint => string.Format("{0} ({1:P})", chartpoint.Y, chartpoint.Participation);
        public void initilizePriceOfEachCategory()
        {
            string queryBarbeeQ = "SELECT SUM(tbl_TransactionProduct.Quantity*tbl_Product.BasePriceInPeso) AS TOTAL FROM tbl_transactionproduct INNER JOIN tbl_Product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_Product.CategoryID = tbl_category.CategoryID INNER JOIN tbl_Transaction  ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID WHERE tbl_category.CategoryID = 2 and tbl_Transaction.TransactionDate Between '" + fromDate + "' and '" + toDate + "'";
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
                        lblBarbeeQPrice.Content = "Total Category Sale: P" + readerBarbeeQ["TOTAL"].ToString();
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnectionBarbeeQ.Close();

            string queryGotome = "SELECT SUM(tbl_TransactionProduct.Quantity*tbl_Product.BasePriceInPeso) AS TOTAL FROM tbl_transactionproduct INNER JOIN tbl_Product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_Product.CategoryID = tbl_category.CategoryID INNER JOIN tbl_Transaction ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID WHERE tbl_category.CategoryID = 3 and tbl_Transaction.TransactionDate Between '" + fromDate + "' and '" + toDate + "'";
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
                        lblGotomePrice.Content = "Total Category Sale: P" + readerGotome["TOTAL"].ToString();
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnectionGotome.Close();

            string queryLomi = "SELECT SUM(tbl_TransactionProduct.Quantity*tbl_Product.BasePriceInPeso) AS TOTAL FROM tbl_transactionproduct INNER JOIN tbl_Product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_Product.CategoryID = tbl_category.CategoryID INNER JOIN tbl_Transaction ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID WHERE tbl_category.CategoryID = 1 and tbl_Transaction.TransactionDate Between '" + fromDate + "' and '" + toDate + "'";
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
                        lblLomiPrice.Content = "Total Category Sale: P" + readerLomi["TOTAL"].ToString();
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnectionLomi.Close();

            string queryPares = "SELECT SUM(tbl_TransactionProduct.Quantity*tbl_Product.BasePriceInPeso) AS TOTAL FROM tbl_transactionproduct INNER JOIN tbl_Product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_Product.CategoryID = tbl_category.CategoryID INNER JOIN tbl_Transaction ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID WHERE tbl_category.CategoryID = 5 and tbl_Transaction.TransactionDate Between '" + fromDate + "' and '" + toDate + "'";
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
                        lblParesPrice.Content = "Total Category Sale: P" + readerPares["TOTAL"].ToString();
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnectionPares.Close();

            string queryTapsi = "SELECT SUM(tbl_TransactionProduct.Quantity*tbl_Product.BasePriceInPeso) AS TOTAL FROM tbl_transactionproduct INNER JOIN tbl_Product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_Product.CategoryID = tbl_category.CategoryID INNER JOIN tbl_Transaction ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID WHERE tbl_category.CategoryID = 4 and tbl_Transaction.TransactionDate Between '" + fromDate + "' and '" + toDate + "'";
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
                        lblTapsiPrice.Content = "Total Category Sale: P" + readerTapsi["TOTAL"].ToString();
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnectionTapsi.Close();

            string queryDessert = "SELECT SUM(tbl_TransactionProduct.Quantity*tbl_Product.BasePriceInPeso) AS TOTAL FROM tbl_transactionproduct INNER JOIN tbl_Product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_Product.CategoryID = tbl_category.CategoryID INNER JOIN tbl_Transaction ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID WHERE tbl_category.CategoryID = 7 and tbl_Transaction.TransactionDate Between '" + fromDate + "' and '" + toDate + "'";
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
                        lblDessertPrice.Content = "Total Category Sale: P" + readerDessert["TOTAL"].ToString();
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnectionDessert.Close();

            string queryDrink = "SELECT SUM(tbl_TransactionProduct.Quantity*tbl_Product.BasePriceInPeso) AS TOTAL FROM tbl_transactionproduct INNER JOIN tbl_Product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_Product.CategoryID = tbl_category.CategoryID INNER JOIN tbl_Transaction ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID WHERE tbl_category.CategoryID = 6 and tbl_Transaction.TransactionDate Between '" + fromDate + "' and '" + toDate + "'";
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
                        lblDrinkPrice.Content = "Total Category Sale: P" + readerDrink["TOTAL"].ToString();
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnectionDrink.Close();

            
        }
        public void initializeIncome()
        {
            string query = "SELECT SUM(tbl_TransactionProduct.Quantity*tbl_Product.BasePriceInPeso) AS TOTAL FROM tbl_transactionproduct INNER JOIN tbl_Product ON tbl_transactionproduct.ProductID = tbl_product.ProductID inner Join tbl_Transaction ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID Where tbl_Transaction.TransactionDate BETWEEN '" + fromDate + "' and '" + toDate + "'";
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
                        lblIncome.Content = "Sales Projection: P" + Convert.ToDouble(reader["TOTAL"]);
                    }
                }
                
                
                
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnection.Close();
        }
        public void initializeAll()
        {
            DataTable dtAll = new DataTable();
            string query = "select ProductName, CategoryDescription, Count(ProductName) AS Customer from tbl_transactionproduct INNER JOIN tbl_product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_product.CategoryID = tbl_category.CategoryID INNER JOIN tbl_transaction ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID Where tbl_Transaction.TransactionDate BETWEEN '" + fromDate+"' and '"+toDate+ "' Group by ProductName order BY Customer desc LIMIT @TOP";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            commandDatabase.Parameters.AddWithValue("@TOP", Convert.ToInt32(txtRank.Text));
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                dtAll.Load(reader);
                
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnection.Close();

            SeriesCollection series = new SeriesCollection();
            if (dtAll.Rows.Count == 0)
            {
                series.Add(new PieSeries() { Title = "No Data", Values = new ChartValues<double> { 0 }, DataLabels = true, LabelPoint = allLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                PieChart allChart1 = allChart;
                allChart1.Series = series;
            }
            else
            {
                foreach (DataRow row in dtAll.Rows)
                {
                    series.Add(new PieSeries() { Title = row["ProductName"].ToString(), Values = new ChartValues<double> { Convert.ToDouble(row["Customer"]) }, DataLabels = true, LabelPoint = allLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                    PieChart allChart1 = allChart;
                    allChart1.Series = series;
                }
                lblTopProducts.Content = "Top " + txtRank.Text + " In All Product Category";
            }
        }
        public void initializeBarbeeQ()
        {
            DataTable dtBarbeeQ = new DataTable();
            string query = "select ProductName, Count(ProductName) AS Customer from tbl_product INNER JOIN tbl_TransactionProduct ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_product.CategoryID = tbl_category.CategoryID INNER JOIN tbl_Transaction ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID WHERE tbl_category.CategoryID = 2 and tbl_Transaction.TransactionDate Between '" + fromDate+"' and '"+toDate+ "' Group by ProductName order BY Customer desc";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                dtBarbeeQ.Load(reader);
                barbeeQDataGrid.ItemsSource = dtBarbeeQ.DefaultView;
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnection.Close();

            SeriesCollection series = new SeriesCollection();
            if (dtBarbeeQ.Rows.Count == 0)
            {
                series.Add(new PieSeries() { Title = "No Data", Values = new ChartValues<int> { 0 }, DataLabels = true, LabelPoint = barbeeQLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                PieChart barbeeQChart1 = barbeeQChart;
                barbeeQChart1.Series = series;
            }
            else
            {
                foreach (DataRow row in dtBarbeeQ.Rows)
                {
                    series.Add(new PieSeries() { Title = row["ProductName"].ToString(), Values = new ChartValues<double> { Convert.ToDouble(row["Customer"]) }, DataLabels = true, LabelPoint = barbeeQLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                    PieChart barbeeQChart1 = barbeeQChart;
                    barbeeQChart1.Series = series;
                }
            }
            
        }
        public void initializeGotome()
        {
            DataTable dtGotome = new DataTable();
            string query = "select ProductName, Count(ProductName) AS Customer from tbl_transactionproduct INNER JOIN tbl_product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_product.CategoryID = tbl_category.CategoryID INNER JOIN tbl_Transaction ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID WHERE tbl_category.CategoryID = 3 and tbl_Transaction.TransactionDate Between '" + fromDate + "' and '" + toDate + "' Group by ProductName order BY Customer desc";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                dtGotome.Load(reader);
                gotomeDataGrid.ItemsSource = dtGotome.DefaultView;
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnection.Close();

            SeriesCollection series = new SeriesCollection();
            if (dtGotome.Rows.Count == 0)
            {
                series.Add(new PieSeries() { Title = "No Data", Values = new ChartValues<int> { 0 }, DataLabels = true, LabelPoint = gotomeLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                PieChart gotomeChart1 = gotomeChart;
                gotomeChart1.Series = series;
            }
            else
            {
                foreach (DataRow row in dtGotome.Rows)
                {
                    series.Add(new PieSeries() { Title = row["ProductName"].ToString(), Values = new ChartValues<double> { Convert.ToDouble(row["Customer"]) }, DataLabels = true, LabelPoint = gotomeLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                    PieChart gotomeChart1 = gotomeChart;
                    gotomeChart1.Series = series;
                }
            }
            
        }
        public void initializeLomi()
        {
            DataTable dtLomi = new DataTable();
            string query = "select ProductName, Count(ProductName) AS Customer from tbl_transactionproduct INNER JOIN tbl_product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_product.CategoryID = tbl_category.CategoryID INNER JOIN tbl_Transaction ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID WHERE tbl_category.CategoryID = 1 and tbl_Transaction.TransactionDate Between '" + fromDate + "' and '" + toDate + "' Group by ProductName order BY Customer desc";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                dtLomi.Load(reader);
                lomiDataGrid.ItemsSource = dtLomi.DefaultView;
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnection.Close();

            SeriesCollection series = new SeriesCollection();
            if (dtLomi.Rows.Count == 0)
            {
                series.Add(new PieSeries() { Title = "No Data", Values = new ChartValues<int> { 0 }, DataLabels = true, LabelPoint = lomiLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                PieChart lomiChart1 = lomiChart;
                lomiChart1.Series = series;
            }
            else
            {
                foreach (DataRow row in dtLomi.Rows)
                {
                    series.Add(new PieSeries() { Title = row["ProductName"].ToString(), Values = new ChartValues<double> { Convert.ToDouble(row["Customer"]) }, DataLabels = true, LabelPoint = lomiLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                    PieChart lomiChart1 = lomiChart;
                    lomiChart1.Series = series;
                }
            }
            
        }
        public void initializePares()
        {
            DataTable dtPares = new DataTable();
            string query = "select ProductName, Count(ProductName) AS Customer from tbl_transactionproduct INNER JOIN tbl_product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_product.CategoryID = tbl_category.CategoryID INNER JOIN tbl_Transaction ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID WHERE tbl_category.CategoryID = 5 and tbl_Transaction.TransactionDate Between '" + fromDate + "' and '" + toDate + "' Group by ProductName order BY Customer desc";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                dtPares.Load(reader);
                paresDataGrid.ItemsSource = dtPares.DefaultView;
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnection.Close();

            SeriesCollection series = new SeriesCollection();
            if (dtPares.Rows.Count == 0)
            {
                series.Add(new PieSeries() { Title = "No Data", Values = new ChartValues<int> { 0 }, DataLabels = true, LabelPoint = paresLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                PieChart paresChart1 = paresChart;
                paresChart1.Series = series;
            }
            else
            {
                foreach (DataRow row in dtPares.Rows)
                {
                    series.Add(new PieSeries() { Title = row["ProductName"].ToString(), Values = new ChartValues<double> { Convert.ToDouble(row["Customer"]) }, DataLabels = true, LabelPoint = paresLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                    PieChart paresChart1 = paresChart;
                    paresChart1.Series = series;
                }
            }
        }
        public void initializeTapsi()
        {
            DataTable dtTapsi = new DataTable();
            string query = "select ProductName, Count(ProductName) AS Customer from tbl_transactionproduct INNER JOIN tbl_product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_product.CategoryID = tbl_category.CategoryID INNER JOIN tbl_Transaction ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID WHERE tbl_category.CategoryID = 4 and tbl_Transaction.TransactionDate Between '" + fromDate + "' and '" + toDate + "' Group by ProductName order BY Customer desc";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                dtTapsi.Load(reader);
                tapsiDataGrid.ItemsSource = dtTapsi.DefaultView;
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnection.Close();

            SeriesCollection series = new SeriesCollection();
            if (dtTapsi.Rows.Count == 0)
            {
                series.Add(new PieSeries() { Title = "No Data", Values = new ChartValues<int> { 0 }, DataLabels = true, LabelPoint = tapsiLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                PieChart tapsiChart1 = tapsiChart;
                tapsiChart1.Series = series;
            }
            else
            {
                foreach (DataRow row in dtTapsi.Rows)
                {
                    series.Add(new PieSeries() { Title = row["ProductName"].ToString(), Values = new ChartValues<double> { Convert.ToDouble(row["Customer"]) }, DataLabels = true, LabelPoint = tapsiLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                    PieChart tapsiChart1 = tapsiChart;
                    tapsiChart1.Series = series;
                }
            }
        }
        public void initializeDessert()
        {
            DataTable dtDessert = new DataTable();
            string query = "select ProductName, Count(ProductName) AS Customer from tbl_transactionproduct INNER JOIN tbl_product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_product.CategoryID = tbl_category.CategoryID INNER JOIN tbl_Transaction ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID WHERE tbl_category.CategoryID = 7 and tbl_Transaction.TransactionDate Between '" + fromDate + "' and '" + toDate + "' Group by ProductName order BY Customer desc";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                dtDessert.Load(reader);
                dessertDataGrid.ItemsSource = dtDessert.DefaultView;
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnection.Close();

            SeriesCollection series = new SeriesCollection();
            if (dtDessert.Rows.Count == 0)
            {
                series.Add(new PieSeries() { Title = "No Data", Values = new ChartValues<int> { 0 }, DataLabels = true, LabelPoint = dessertLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                PieChart dessertChart1 = dessertChart;
                dessertChart1.Series = series;
            }
            else
            {
                foreach (DataRow row in dtDessert.Rows)
                {
                    series.Add(new PieSeries() { Title = row["ProductName"].ToString(), Values = new ChartValues<double> { Convert.ToDouble(row["Customer"]) }, DataLabels = true, LabelPoint = dessertLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                    PieChart dessertChart1 = dessertChart;
                    dessertChart1.Series = series;
                }
            }
            
        }
        public void initializeDrink()
        {
            DataTable dtDrink = new DataTable();
            string query = "select ProductName, Count(ProductName) AS Customer from tbl_transactionproduct INNER JOIN tbl_product ON tbl_transactionproduct.ProductID = tbl_product.ProductID INNER JOIN tbl_category ON tbl_product.CategoryID = tbl_category.CategoryID INNER JOIN tbl_Transaction ON tbl_TransactionProduct.TransactionID=tbl_Transaction.TransactionID WHERE tbl_category.CategoryID = 6 and tbl_Transaction.TransactionDate Between '" + fromDate + "' and '" + toDate + "' Group by ProductName order BY Customer desc";
            MySqlConnection databaseConnection = new MySqlConnection(getConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                dtDrink.Load(reader);
                drinkDataGrid.ItemsSource = dtDrink.DefaultView;
            }

            catch (Exception)
            {
                MessageBox.Show("No Records Found");
            }
            databaseConnection.Close();

            SeriesCollection series = new SeriesCollection();
            if (dtDrink.Rows.Count == 0)
            {
                series.Add(new PieSeries() { Title = "No Data", Values = new ChartValues<double> { 0 }, DataLabels = true, LabelPoint = drinkLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                PieChart drinkChart1 = drinkChart;
                drinkChart1.Series = series;
            }
            else
            {
                foreach (DataRow row in dtDrink.Rows)
                {
                    series.Add(new PieSeries() { Title = row["ProductName"].ToString(), Values = new ChartValues<double> { Convert.ToDouble(row["Customer"]) }, DataLabels = true, LabelPoint = drinkLabelPoint, FontSize = 11, Foreground = Brushes.Black });
                    PieChart drinkChart1 = drinkChart;
                    drinkChart1.Series = series;
                }
            }
        }
        private void MainListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (MainListView.SelectedIndex == 1)
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

        private void BtnRank_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(txtRank.Text) >10 || Convert.ToInt32(txtRank.Text) < 1)
                {
                    MessageBox.Show("Ranking Shows TOP 1 to TOP 10 Products Only");
                }
                else
                {
                    initializeAll();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Enter a Valid Integer");
            }
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

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            initializeAll();
            initializeBarbeeQ();
            initializeGotome();
            initializeLomi();
            initializePares();
            initializeTapsi();
            initializeDessert();
            initializeDrink();
            initializeIncome();
            initilizePriceOfEachCategory();
            lblDate.Content = "From: " + fromDate + " To: " + toDate;
        }
    }
}
