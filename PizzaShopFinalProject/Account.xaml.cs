using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
    public sealed partial class Account : Page
    {

        User user = new User();
        string connString = "Server=" + Environment.MachineName.ToString() + "; Database=PizzaShopFinal; User ID=PapaDario; Password=12345";
        public Account()
        {
            this.InitializeComponent();
        }

        private void btnAcLogin_Click(object sender, RoutedEventArgs e)
        {
            string uEmail = txAcID.Text;
            string uPass = txAcPass.Password;
            bool status = false;
            string connString = "Server=" + Environment.MachineName.ToString() + ";Initial Catalog=PizzaShopFinal;Integrated Security=true; Persist Security Info=false";
            try
            {
                string query = "SELECT * FROM users;";
                using (var connection = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["uEmail"].Equals(uEmail))
                        {
                            if (reader["uPass"].Equals(uPass))
                            {
                                user = new User();
                                int uID = Int32.Parse(reader["uID"].ToString());
                                string uFirst = reader["uFirst"].ToString();
                                string uLast = reader["uLast"].ToString();
                                string uAdd = reader["uAdd"].ToString();
                                string uType = reader["uType"].ToString();
                                string usEmail = reader["uEmail"].ToString();
                                string usPass = reader["uPass"].ToString();
                                DateTime uDate = (DateTime)reader["uDate"];

                                user.uAdd = uAdd;
                                user.uDate = uDate;
                                user.uID = uID;
                                user.uEmail = uEmail;
                                user.uPass = usPass;
                                user.uFirst = uFirst;
                                user.uLast = uLast;
                                user.uType = uType;

                                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                                string json = JsonConvert.SerializeObject(user);
                                localSettings.Values["curUser"] = json;
                                txResult.Text = "Login Successful";
                                setPageStatus(1);
                                status = true;
                                break;
                            }
                            else
                            {
                                txResult.Text = "Incorrect Login Details";
                                setPageStatus(2);
                            }
                        }
                        else
                        {
                            txResult.Text = "Incorrect Login Details";
                            setPageStatus(2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (status)
            {
                txResult.Text = "Login Successful";
                btnAcLogOut.IsEnabled = true;
                btnAcLogin.IsEnabled = false;
                txAcID.IsEnabled = false;
                txAcPass.IsEnabled = false;
                updateUserInfo(1);
            }

        }

        void updateUserInfo(int type)
        {
            if (type==1)
            {
                txUsFirst.Text = user.uFirst;
                txUsLast.Text = user.uLast;
                txUsPass.Password = user.uPass;
            }
            else if (type==2)
            {
                txUsFirst.Text = "";
                txUsLast.Text = "";
                txUsPass.Password = "";
            }
            
        }

        private void btnAcLogOut_Click(object sender, RoutedEventArgs e)
        {

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["curUser"];
            User user = new User();
            try
            {
                if (value != null)
                {
                    user = JsonConvert.DeserializeObject<User>(value.ToString());
                }

            }
            catch (Exception ex)
            {

            }
            if (user.uID > 0)
            {
                localSettings.Values["curUser"] = null;
                txResult.Text = "Logout Successful";
                panelUpdate.Visibility = Visibility.Collapsed;
                btnAcLogOut.IsEnabled = false;
                btnAcLogin.IsEnabled = true;
                txAcID.IsEnabled = true;
                txAcPass.IsEnabled = true;
                updateUserInfo(2);
                setPageStatus(2);
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["curUser"];
            try
            {
                if (value != null)
                {
                    user = JsonConvert.DeserializeObject<User>(value.ToString());
                }
                else
                {
                    user = null;
                }

            }
            catch (Exception ex)
            {

            }
            if (user != null)
            {
                setPageStatus(1);
            }
            else
            {
                setPageStatus(2);
            }
        }

        private void btnAcReg_Click(object sender, RoutedEventArgs e)
        {

            string uEmail = txRegEmail.Text;
            string uFirst = txRegFirst.Text;
            string uLast = txRegLast.Text;
            string uPass1 = txRegPass1.Password;
            string uPass2 = txRegPass2.Password;

            if (!(uPass1.Equals(uPass2)))
            {
                txResult.Text = "Password Does Not Match.";
            }
            else
            {
                string query = "INSERT INTO USERS (uFirst, uLast, uEmail, uPass) VALUES(@in1, @in2, @in3, @in4)";
                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("in1", uFirst);
                    cmd.Parameters.AddWithValue("in2", uLast);
                    cmd.Parameters.AddWithValue("in3", uEmail);
                    cmd.Parameters.AddWithValue("in4", uPass1);
                    int res = cmd.ExecuteNonQuery();
                    if (res == 1)
                    {
                        txResult.Text = "Account Registration Successful.";
                    }
                    else
                    {
                        txResult.Text = "Failed: Something Went Wrong.";
                    }

                }
            }

        }

        private void setPageStatus(int type)
        {
            if (type==1)
            {
                //When Logged In
                btnAcLogOut.IsEnabled = true;
                btnAcLogin.IsEnabled = false;
                txAcID.IsEnabled = false;
                txAcPass.IsEnabled = false;
                panelRegister.Visibility = Visibility.Collapsed;
                panelUpdate.Visibility = Visibility.Visible;

                if (user.uFirst!=null)
                {
                    txUsFirst.Text = user.uFirst;
                    txUsLast.Text = user.uLast;
                    txUsPass.Password = user.uPass;
                    txAcID.Text = user.uEmail;
                    txAcPass.Password = user.uPass;
                }
               

            }
            else if (type==2)
            {
                //When Logged Out.
                btnAcLogOut.IsEnabled = false;
                btnAcLogin.IsEnabled = true;
                txAcID.IsEnabled = true;
                txAcPass.IsEnabled = true;

                panelRegister.Visibility = Visibility.Visible;
                panelUpdate.Visibility = Visibility.Collapsed;

            }
          
        }

        private void btnUsUpdate_Click(object sender, RoutedEventArgs e)
        {
            string uFirst = txUsFirst.Text;
            string uLast = txUsLast.Text;
            string uPass = txUsPass.Password;
            int uID = user.uID;
            try
            {
                string query = "UPDATE users SET uFirst = @in1, uLast = @in2, uPass = @in3 WHERE uID = @in4";
                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("in1", uFirst);
                    cmd.Parameters.AddWithValue("in2", uLast);
                    cmd.Parameters.AddWithValue("in3", uPass);
                    cmd.Parameters.AddWithValue("in4", uID);
                    int res = cmd.ExecuteNonQuery();
                    if (res == 1)
                    {
                        txResult.Text = "Account Updated Successfully.";
                    }
                    else
                    {
                        txResult.Text = "Failed: Something Went Wrong.";
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
