
using booksShop8.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace booksShop8
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            //DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new AppShell())
            {
                BarBackgroundColor = Color.FromHex("#7c7b76"),
                BarTextColor = Color.White

            };
        }
        
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
