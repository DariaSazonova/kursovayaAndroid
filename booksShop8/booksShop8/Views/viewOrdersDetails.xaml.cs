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
    public partial class viewOrdersDetails : ContentPage
    {
        Service service = new Service();
        int num;
        public viewOrdersDetails(int orderNum)
        {
            InitializeComponent();
            num = orderNum;
        }
        protected override async void OnAppearing()
        {
            string resultOrderDetails = await service.GetOrderDetails(num.ToString());
            var js = JArray.Parse(resultOrderDetails);
            Decimal itog = 0;
            List<OrderDetails> list = new List<OrderDetails>();
            foreach(var i in js)
            {
                OrderDetails ord = new OrderDetails();
                ord.BookName = Basket.viewBooks.Where(b => b.bookId == (int)i["bookId"]).Select(s => s.bookName).FirstOrDefault();
                ord.OrderId = (int)i["orderId"];
                ord.Quantities = (int)i["quantities"];
                ord.Cost = (decimal)i["cost"];
                ord.sum = ord.Quantities * ord.Cost;
                itog += Convert.ToDecimal(ord.sum);
                list.Add(ord);
            }


            BooksList.ItemsSource = list;
            BooksList.BindingContext = list;
            LabItog.Text = $"Итог: {itog} ₽";
        }

        private  void BooksList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            if (sender is ListView lv) lv.SelectedItem = null;
        }
    }
}