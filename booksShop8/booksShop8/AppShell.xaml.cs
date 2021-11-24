
using booksShop8.Views;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;



namespace booksShop8
{
    public partial class AppShell : TabbedPage//Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            //
            

        }

        private void TabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {

        }

        private void TabbedPage_CurrentPageChanged_1(object sender, EventArgs e)
        {

        }



        //protected void TabBar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    int index=-1;
        //    if (TabbarM.IsChecked == true)
        //        if (TabbarM.TabIndex == 2)
        //            index = 0;

        //}
    }

}
