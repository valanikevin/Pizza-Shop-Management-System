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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PizzaShopFinalProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class About : Page
    {
        public About()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txAbout.Text = "Let's talk about Papa Dario's Food Emporium for a little."+Environment.NewLine+ "Para Dario was one of the great developer in the world, but that was his professional life." + Environment.NewLine + " However, he was great chef and food lover." + Environment.NewLine+"One fine day, he was working for his boss and his boss said he is the software he built was useless."+Environment.NewLine+" Since then, he decided to be his own boss and started the Pizza Shop in Down Town Toronto. "+Environment.NewLine+"And that is how slowly and stadily, the Food emporium was born.";
        }
    }
}
