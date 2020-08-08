using Microsoft.Extensions.Configuration;
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
    public sealed partial class HomePage : Page
    {
        List<Order> orders = new List<Order>();
        double billTotal = 0;
        int uID = -1;
        //Adding Combo Products:
        Order comb1 = new Order();
        Order comb2 = new Order();
        Order rec1 = new Order();
        Order rec2 = new Order();

        public HomePage()
        {
            this.InitializeComponent();
        }

        const double prPizSmall = 1.20;
        const double prPizMedium = 2.20;
        const double prPizLarge = 3.20;
        const double prPizXLarge = 4.20;
        const double prPizCheese = 2.80;
        const double prPizTop = 2.80;
        

        private User getCurrentUser()
        {
            User user = null;
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["curUser"];
            try
            {
                if (value!=null)
                {
                    user = JsonConvert.DeserializeObject<User>(value.ToString());
                }
            }
            catch (Exception ex)
            {
                
            }
            return user;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        static string GetConnectionString(string conStrName)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("config.json");
            IConfiguration config = configurationBuilder.Build();

            return config["ConnectionStrings:" + conStrName];
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            double curTotal = 0;
            string pSize = cbPiSize.SelectedValue.ToString();
            string pCheese = cbCheese.SelectedValue.ToString();
            string pTop1 = ckTop1.IsChecked.ToString();
            string pTop2 = ckTop2.IsChecked.ToString();
            string pTop3 = ckTop3.IsChecked.ToString();
            string pTop4 = ckTop4.IsChecked.ToString();
            if (pTop1.Equals("True"))
            {
                pTop1 = "Pepperoni";
                curTotal += prPizTop;
            }
            if (pTop2=="True")
            {
                pTop2 = "Onion";
                curTotal += prPizTop;
            }
            if (pTop3 == "True")
            {
                pTop3 = "Mushrooms";
                curTotal += prPizTop;
            }
            if (pTop4 == "True")
            {
                pTop4 = "Black Olives";
                curTotal += prPizTop;
            }
            

            if (pSize.Contains("Small"))
            {
                curTotal += prPizSmall;
            }
            if (pSize.Contains("Medium"))
            {
                curTotal += prPizMedium;
            }
            if (pSize.Contains("Large"))
            {
                curTotal += prPizLarge;
            }
            if (pSize.Contains("Extra-Large"))
            {
                curTotal += prPizXLarge;
            }
            if (pCheese.Contains("Yes"))
            {
                curTotal += prPizCheese;
                pCheese = "Yes";
            }
            else
            {
                pCheese = "No";
            }
            txResult.Text = "Added To Cart: $" + curTotal;
            Order order = new Order();
            order.pSize = pSize;
            order.pCheese = pCheese;
            order.pTop1 = pTop1;
            order.pTop2 = pTop2;
            order.pTop3 = pTop3;
            order.pTop4 = pTop4;
            billTotal += curTotal;
            order.oTotal = curTotal;
            if (getCurrentUser()!=null)
            {
                order.uID = getCurrentUser().uID;
            }
            else
            {
                order.uID = -1;
            }
            



            addOrder(order);

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cbPiSize.Items.Add("Small ($1.20)");
            cbPiSize.Items.Add("Medium ($2.20)");
            cbPiSize.Items.Add("Large ($3.20)");
            cbPiSize.Items.Add("Extra-Large ($4.20)");
            cbPiSize.SelectedIndex = 1;
            cbCheese.Items.Add("Yes ($2.80)");
            cbCheese.Items.Add("No (Free)");
            cbCheese.SelectedIndex = 0;
            

           
            comb1.oTotal = 21.76;
            comb1.pCheese = "Yes";
            comb1.pSize = "Medium";
            comb1.pTop1 = "Pepporoni";
            comb1.pTop2 = "Black Olives";
            comb1.pTop3 = "No";
            comb1.pTop4 = "No";
            comb1.pComboNote = "2 Cokes Free";


            comb2.oTotal = 17.21;
            comb2.pCheese = "No";
            comb2.pSize = "Large";
            comb2.pTop1 = "Onion";
            comb2.pTop2 = "Mushroom";
            comb2.pTop3 = "No";
            comb2.pTop4 = "No";
            comb2.pComboNote = "4 Cokes Free";

            txComb1.Text = "Combo Order 1: "+Environment.NewLine;
            txComb1.Text += "Pizza Size: "+comb1.pSize+Environment.NewLine;
            txComb1.Text += "Cheese: "+comb1.pCheese+Environment.NewLine;
            txComb1.Text += "Top 1: "+comb1.pTop1+Environment.NewLine;
            txComb1.Text += "Top 2: "+comb1.pTop2+Environment.NewLine;
            txComb1.Text += "Other: "+comb1.pComboNote+Environment.NewLine;
            txComb1.Text += "Price: "+comb1.oTotal+Environment.NewLine;

            txComb2.Text = "Combo Order 2: " + Environment.NewLine;
            txComb2.Text += "Pizza Size: " + comb2.pSize + Environment.NewLine;
            txComb2.Text += "Cheese: " + comb2.pCheese + Environment.NewLine;
            txComb2.Text += "Top 1: " + comb2.pTop1 + Environment.NewLine;
            txComb2.Text += "Top 2: " + comb2.pTop2 + Environment.NewLine;
            txComb2.Text += "Other: " + comb2.pComboNote + Environment.NewLine;
            txComb2.Text += "Price: " + comb2.oTotal + Environment.NewLine;


            
            string connString = "Server="+ Environment.MachineName.ToString() + "; Database=PizzaShopFinal; User ID=PapaDario; Password=12345";
            try
            {
                string query = "SELECT TOP 2 * FROM orders ORDER BY oID DESC;";
                using (var connection = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    int i = 0;
                    while (reader.Read())
                    {
                        if (i==0)
                        {
                            txRec1.Text = "PREVIOUS ORDER: "+Environment.NewLine;
                            txRec1.Text += "Pizza: "+reader["pSize"].ToString()+Environment.NewLine;
                            txRec1.Text += "Cheese: "+reader["pCheese"].ToString() + Environment.NewLine;
                            txRec1.Text += "Topping 1: "+reader["pTop1"].ToString() + Environment.NewLine;
                            txRec1.Text += "Topping 2: "+reader["pTop2"].ToString() + Environment.NewLine;
                            txRec1.Text += "Topping 3: "+reader["pTop3"].ToString() + Environment.NewLine;
                            txRec1.Text += "Topping 4: "+reader["pTop4"].ToString() + Environment.NewLine;
                            rec1.pSize = reader["pSize"].ToString();
                            rec1.pCheese = reader["pCheese"].ToString();
                            rec1.pTop1 = reader["pTop1"].ToString();
                            rec1.pTop2 = reader["pTop2"].ToString();
                            rec1.pTop3 = reader["pTop3"].ToString();
                            rec1.pTop4 = reader["pTop4"].ToString();
                            rec1.oTotal = Double.Parse(reader["oTotal"].ToString());
                            i++;
                        }
                    else if (i==1)
                        {
                            txRec2.Text = "SUGGESTED: " + Environment.NewLine;
                            txRec2.Text += "Pizza: " + reader["pSize"].ToString() + Environment.NewLine;
                            txRec2.Text += "Cheese: " + reader["pCheese"].ToString() + Environment.NewLine;
                            txRec2.Text += "Topping 1: " + reader["pTop1"].ToString() + Environment.NewLine;
                            txRec2.Text += "Topping 2: " + reader["pTop2"].ToString() + Environment.NewLine;
                            txRec2.Text += "Topping 3: " + reader["pTop3"].ToString() + Environment.NewLine;
                            txRec2.Text += "Topping 4: " + reader["pTop4"].ToString() + Environment.NewLine;
                            rec2.pSize = reader["pSize"].ToString();
                            rec2.pCheese = reader["pCheese"].ToString();
                            rec2.pTop1 = reader["pTop1"].ToString();
                            rec2.pTop2 = reader["pTop2"].ToString();
                            rec2.pTop3 = reader["pTop3"].ToString();
                            rec2.pTop4 = reader["pTop4"].ToString();
                            rec2.oTotal = Double.Parse(reader["oTotal"].ToString());
                            i++;
                        }
                    
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void addOrder(Order order)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["cartOrders"];
            if (value!=null)
            {
                orders = JsonConvert.DeserializeObject<List<Order>>(value.ToString());
            }
            else
            {
                orders = new List<Order>();
            }
            orders.Add(order);
            string json = JsonConvert.SerializeObject(orders);
            localSettings.Values["cartORders"] = json;
        }

        private void btnRec1_Click(object sender, RoutedEventArgs e)
        {

            addOrder(rec1);
            txResult.Text = "Order Added To Cart";

        }

        private void btnRec2_Click(object sender, RoutedEventArgs e)
        {
            addOrder(rec2);
            txResult.Text = "Order Added To Cart";
        }

        private void btnComb1_Click(object sender, RoutedEventArgs e)
        {
            addOrder(comb1);
            txResult.Text = "Order Added To Cart";
        }

        private void btnComb2_Click(object sender, RoutedEventArgs e)
        {
            addOrder(comb2);
            txResult.Text = "Order Added To Cart";
        }
    }
}
