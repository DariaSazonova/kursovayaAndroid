using booksShop8.ViewModels;
using Newtonsoft.Json.Linq;
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
    public partial class OrdersHistory : ContentPage
    {
        List<Order> OrdersList = new List<Order>();
        Service service = new Service();
        public OrdersHistory()
        {
            InitializeComponent();
        }
        protected override  void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            view();
            
        }
        private async void view()
        {
            if (OrdersList.Count == 0)
            {
                string resOrders = await service.GetOrderNum(Basket.profil.id);
                var js1 = JArray.Parse(resOrders);
                foreach (var el in js1)
                {
                    Order a = new Order();
                    a.ordernum = (int)el["orderId"];
                    a.DateFirst = (DateTime)el["dateFirst"];
                    a.status = (string)el["status"];
                    OrdersList.Add(a);
                }
                
            }
            OrderListL.BindingContext = OrdersList;
            OrderListL.ItemsSource = OrdersList;
        }

        private async void OrderListL_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            if (sender is ListView lv) lv.SelectedItem = null;
            Order selectedOrder = e.Item as Order;
            if (selectedOrder != null)
            {
                await Navigation.PushAsync(new viewOrdersDetails(selectedOrder.ordernum));
            }

        }
    }
}