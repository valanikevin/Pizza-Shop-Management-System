using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PizzaShopFinalProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminArea : Page
    {
        string connString = "Server=" + Environment.MachineName.ToString() + ";Database=PizzaShopFinal;USER ID=PapaDario; PASSWORD=12345";
        List<Order> orders = new List<Order>();
        public AdminArea()
        {
            this.InitializeComponent();
        }

        List<User> users = new List<User>();
        User user = new User();
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            bool isAllowed = true;
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["curUser"];
            try
            {
                if (value != null)
                {

                    if (user.uType!=null)
                    {
                        if (!(user.uType.Equals("Admin")))
                        {
                            isAllowed = false;
                            return;
                        }
                    }
                }
                else
                {
                    isAllowed = false;
                    return;

                }

            }
            catch (Exception ex)
            {
                return;
            }

            if (isAllowed==false)
            {
                panelFeedback.Visibility = Visibility.Collapsed;
                panelUsers.Visibility = Visibility.Collapsed;
                panelOrders.Visibility = Visibility.Collapsed;
            }
            try
            {
                string query = "Select * FROM USERS;";
                using (var connection = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    int i = 0;
                    while (reader.Read())
                    {
                        cbUsers.Items.Add(reader["uFirst"] + " (" + reader["uEmail"] + ")");
                        User user = new User();
                        user.uFirst = reader["uFirst"].ToString();
                        user.uLast = reader["uLast"].ToString();
                        user.uEmail = reader["uEmail"].ToString();
                        users.Add(user);
                    }
                    cbUsers.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                txOrders.Text = ex.ToString();
            }
            loadOrders();
            loadFeedbacks();
        }

        private void loadOrders()
        {

            using (var connection = new SqlConnection(connString))
            {
                string query = "SELECT a.oID, b.uEmail, a.oTotal, a.oDate FROM Orders a JOIN USERS b ON a.uID = b.uID;";
                SqlCommand cmd1 = new SqlCommand(query, connection);
                connection.Open();
                cmd1 = new SqlCommand(query, connection);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                txOrders.Text = "";
                while (reader1.Read())
                {
                    Order order = new Order();
                    order.oID = Int32.Parse(reader1["oID"].ToString());
                    order.oDate = (DateTime)reader1["oDate"];
                    order.pComboNote = reader1["uEmail"].ToString();
                    order.oTotal = Double.Parse(reader1["oTotal"].ToString());
                    orders.Add(order);
                    txOrders.Text += "OrderID: " + order.oID + Environment.NewLine;
                    txOrders.Text += "Email: " + order.pComboNote + Environment.NewLine;
                    txOrders.Text += "Total: $: " + order.oTotal + Environment.NewLine + Environment.NewLine;

                }
            }
        }

        private void loadFeedbacks()
        {

            using (var connection = new SqlConnection(connString))
            {
                string query = "SELECT a.fID, b.uEmail, a.fData, a.fDate FROM FEEDBACK a JOIN USERS b on a.uID = b.uID;";
                SqlCommand cmd1 = new SqlCommand(query, connection);
                connection.Open();
                cmd1 = new SqlCommand(query, connection);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                txFeedback.Text = "";
                while (reader1.Read())
                {
                   
                    txFeedback.Text += "ID: : " + reader1["fID"] + "Email: "+reader1["uEmail"] + Environment.NewLine;
                    txFeedback.Text += "Date: : " + reader1["fDate"] + Environment.NewLine;
                    txFeedback.Text += "Feedback: " + reader1["fData"] + Environment.NewLine + Environment.NewLine;

                }
            }
        }

        private void cbUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cbUsers.SelectedIndex;
            txFirst.Text = users[index].uFirst;
            txLast.Text = users[index].uLast;
            txEmail.Text = users[index].uEmail;
        }

        private void btnUpdateInfo_Click(object sender, RoutedEventArgs e)
        {
            int index = cbUsers.SelectedIndex;
            string uEmail = txEmail.Text;
            string uFirst = txFirst.Text;
            string uLast = txLast.Text;

            string query = "UPDATE USERS SET uFirst = @in1, uLast = @in2, uEmail = @in3 WHERE uID = @in4";
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("in1", uFirst);
                cmd.Parameters.AddWithValue("in2", uLast);
                cmd.Parameters.AddWithValue("in3", uEmail);
                cmd.Parameters.AddWithValue("in4", index+1);
                int res = cmd.ExecuteNonQuery();
                if (res==1)
                {
                    txResult.Text = "Account Details Updated.";
                }
                else
                {
                    txResult.Text = "Failed: Something Went Wrong.";
                }
            }
        }
          
    }
}
