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
    public sealed partial class Feedback : Page
    {
        int uID = -1;
        public Feedback()
        {
            this.InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["curUser"];

            try
            {
                if (value != null)
                {
                    User ReList = JsonConvert.DeserializeObject<User>(value.ToString());
                    uID = ReList.uID;
                }

                string fData = txFeedback.Text;

                string connString = "Server=" + Environment.MachineName.ToString() + ";Database=PizzaShopFinal;USER ID=PapaDario; PASSWORD=12345";
                string query = "INSERT INTO FEEDBACK(uID, fData) VALUES(@in1, @in2)";
                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("in1", uID);
                    cmd.Parameters.AddWithValue("in2", fData);

                    int res = cmd.ExecuteNonQuery();
                    if (res == 1)
                    {
                        txResult.Text = "Feedback Submitted.";
                    }
                    else
                    {
                        txResult.Text = "Failed: Something Went Wrong.";
                    }
                }
            }
            catch (Exception ex)
            {
                txFeedback.Text = ex.ToString();
            }
        }
    }
}
