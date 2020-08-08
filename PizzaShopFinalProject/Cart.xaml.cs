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
using System.Net;
using System.Net.Mail;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PizzaShopFinalProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
   
    public sealed partial class Cart : Page
    {
        List<Order> orders = new List<Order>();
        User user = new User();
        int uID = -1;
        SmtpClient client;
        MailMessage msg;
        NetworkCredential login;

        public Cart()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["cartOrders"];
            try
            {
                if (value != null)
                {
                    orders = JsonConvert.DeserializeObject<List<Order>>(value.ToString());
                    txOrderPreview.Text += Environment.NewLine;
                    int count = 0;
                    double total = 0;
                    txOrderPreview.Text = "";
                    foreach (Order order in orders)
                    {
                        txOrderPreview.Text += "Order: " + count + Environment.NewLine;
                        txOrderPreview.Text += "Pizza: " + order.pSize + Environment.NewLine + "Cheese: " + order.pCheese + Environment.NewLine + "Topping: 1 - " + order.pTop1 + Environment.NewLine + "Topping: 2 - " + order.pTop2 + Environment.NewLine + "Topping: 3 - " + order.pTop3 + Environment.NewLine + "Topping: 4 - " + order.pTop4 + Environment.NewLine + Environment.NewLine;
                        count++;
                        total += order.oTotal;
                    }
                    txCount.Text = count.ToString();
                    txTotal.Text = total.ToString();
                    txCount.Text = "Total Items: " + count.ToString();
                    txSub.Text = "Sub Total: $" + total.ToString();

                    double tDisc = 0;
                    double fTotal = total;

                    Object value1 = localSettings.Values["curUser"];
                    bool isUser = false;
                    try
                    {
                        if (value1!=null)
                        {
                            user = JsonConvert.DeserializeObject<User>(value1.ToString());
                            isUser = true;
                            uID = user.uID;
                            txEmail.Text = user.uEmail;
                        }
                        else
                        {
                            user = null;
                            txEmail.Text = "demo@demo.com";
                        }
                    }
                    catch (Exception ex)
                    {
                        isUser = false;
                    }

                    if (user!=null)
                    {
                        tDisc = total * 0.10;
                        fTotal -= tDisc;
                        uID = user.uID;
                    }
                    txDisc.Text = "Total Discount: " + tDisc;
                    txTotal.Text = "Payable: $" + fTotal.ToString();
                }
                else
                {
                    emptyCart();
                }
            }
            catch (Exception ex)
            {
                txOrderPreview.Text += "No Orders In Cart." + Environment.NewLine;
                emptyCart();
            }
        }



        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            string content = "";

            bool status = false;
            string connString = "Server=" + Environment.MachineName.ToString() + "; Database=PizzaShopFinal; User ID=PapaDario; Password=12345";
            
            try
            {
                string query = "INSERT INTO orders(uID, oTotal, pSize, pCheese, pTop1, pTop2, pTop3, pTop4) VALUES(@in1, @in2, @in3, @in4, @in5, @in6, @in7, @in8)";
                using (var connection = new SqlConnection(connString))
                {
                    foreach (Order order in orders)
                    {
                        connection.Open();
                        
                        SqlCommand cmd = new SqlCommand(query, connection);
                        
                        cmd.Parameters.AddWithValue("in1", uID);
                        cmd.Parameters.AddWithValue("in3", order.pSize);
                        cmd.Parameters.AddWithValue("in4", order.pCheese);
                        cmd.Parameters.AddWithValue("in5", order.pTop1);
                        cmd.Parameters.AddWithValue("in6", order.pTop2);
                        cmd.Parameters.AddWithValue("in7", order.pTop3);
                        cmd.Parameters.AddWithValue("in8", order.pTop4);
                        if (user!=null)
                        {
                            double tDisc = order.oTotal*0.10;
                            cmd.Parameters.AddWithValue("in2", (order.oTotal-tDisc));
                            order.oTotal = order.oTotal - tDisc;
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("in2", order.oTotal);
                        }

                        cmd.ExecuteNonQuery();
                        connection.Close();
                        status = true;
                        content += "<h3>Order<h3>";
                        content += "Pizza Size: "+order.pSize+Environment.NewLine;
                        content += "Pizza Cheese: "+order.pCheese+Environment.NewLine;
                        content += "Pizza Top 1: "+order.pTop1+Environment.NewLine;
                        content += "Pizza Top 2: "+order.pTop2+Environment.NewLine;
                        content += "Pizza Top 3: "+order.pTop3+Environment.NewLine;
                        content += "Pizza Top 4: "+order.pTop4+Environment.NewLine;
                        content += "Pizza Total: "+order.oTotal+Environment.NewLine + Environment.NewLine;
                    }

                }
                printReceipt(txEmail.Text, content);
            }
            catch (Exception ex)
            {
                status = false;
                txOrderPreview.Text = ex.ToString();
            }
            if (status)
            {
                emptyCart();
                txResult.Text = "Your Order Has Been Placed.";
                txOrderPreview.Text = "You can place new orders by Adding pizzas to cart from Home Page.";
            }
        }

        private void emptyCart()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["cartOrders"] = null;
            orders = new List<Order>();
            txResult.Text = "No Orders In Cart.";
            txOrderPreview.Text = "Please Add Orders To Cart From Home Page.";
            txTotal.Text = "Total: $0.00";
            txCount.Text = "Total Items: 0";
            txSub.Text = "Sub Total: $0.00";
            txDisc.Text = "Total Discount: $0.00";
            btnEmptyCart.IsEnabled = false;
            btnPay.IsEnabled = false;
        }

        private void btnEmptyCart_Click(object sender, RoutedEventArgs e)
        {
            emptyCart();
        }

        private void printReceipt(string emailAdd, string msgBody)
        {
            if (!(emailAdd.Equals("")))
            {
                login = new NetworkCredential("valanikevin@gmail.com", "ZbcEvnyXM1k5zmtq");
                client = new SmtpClient("smtp-relay.sendinblue.com");
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = login;
                msg = new MailMessage(new MailAddress("valanikevin@gmail.com"), new MailAddress(emailAdd));
                msg.Body = "<h2>Receipts: </h2><br>" + msgBody;
                msg.Subject = "Receipts";
                msg.Priority = MailPriority.High;
                msg.IsBodyHtml = true;
                client.SendAsync(msg, "Demo");
            }

        }
    }
}
