using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PizzaShopFinalProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new HomePage();
            pgTitle.Text = "Home";
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["curUser"];
            try
            {
                if (value != null)
                {
                    User ReList = JsonConvert.DeserializeObject<User>(value.ToString());
                    txUsName.Text = "Hello " + ReList.uFirst;
                    if (ReList.uType.Equals("Admin"))
                    {
                        btnAdmin.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btnAdmin.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    txUsName.Text = "Hello, Friend. Become Member To Get 10% Off.";
                    btnAdmin.Visibility = Visibility.Collapsed;
                }


            }
            catch (Exception ex)
            {
                txUsName.Text = "Hello, Friend. Become Member To Get 10% Off.";
                btnAdmin.Visibility = Visibility.Collapsed;
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new HomePage();
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["curUser"];
            try
            {
                if (value!=null)
                {
                    User ReList = JsonConvert.DeserializeObject<User>(value.ToString());
                    txUsName.Text = "Hello " + ReList.uFirst;
                    if (ReList.uType.Equals("Admin"))
                    {
                        btnAdmin.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btnAdmin.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    txUsName.Text = "Hello, Friend. Become Member To Get 10% Off.";
                }

               
            }
            catch (Exception ex)
            {
                txUsName.Text = "Hello, Friend. Become Member To Get 10% Off.";
            }
        }

        private void btnAccount_Click(object sender, RoutedEventArgs e)
        {
            pgTitle.Text = "Accounts";
            mainFrame.Content = new Account();

        }

        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            pgTitle.Text = "Cart";
            mainFrame.Content = new Cart();
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            pgTitle.Text = "Admin";
            mainFrame.Content = new AdminArea();
        }

        private void btnFeedback_Click(object sender, RoutedEventArgs e)
        {
            pgTitle.Text = "Feedback";
            mainFrame.Content = new Feedback();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            pgTitle.Text = "About";
            mainFrame.Content = new About();
        }
    }
}
