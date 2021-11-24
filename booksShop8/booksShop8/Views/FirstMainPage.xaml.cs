using booksShop8.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace booksShop8.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstMainPage : ContentPage
    {
        Service service = new Service();
        //public static List<Books> Bookslist = new List<Books>();
        public FirstMainPage()
        {
            InitializeComponent();
        }

        private void Button_ClickedPopular(object sender, EventArgs e)
        {

        }

        private  void Button_ClickedNew(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new Catalog("новинка"));
        }

        private void Button_ClickedSoon(object sender, EventArgs e)
        {

        }
    }
}