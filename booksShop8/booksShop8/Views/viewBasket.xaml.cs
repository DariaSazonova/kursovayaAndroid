using booksShop8.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace booksShop8.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class viewBasket : ContentPage
    {
        public viewBasket()
        {
            InitializeComponent();
            
        }
        protected override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            view();
            
        }
        private async void view()
        {
            if (Basket.basketDiction.Count == 0)
            {
                NullLabelBasket.IsVisible = true;
                BasketList.IsVisible = false;
            }
            else
            {
                BasketList.IsVisible = true;
                const string Url = "http://192.168.1.42/myserver/api/books/";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
               
                string result = await client.GetStringAsync(Url);  // тут json 

                List<Books> viewBooks = new List<Books>();
                var js = JArray.Parse(result);
                foreach (var b in js)
                {
                    Books addBook = new Books();
                    addBook.bookId = (int)b["bookId"];
                    addBook.bookName = (string)b["bookName"];
                    addBook.bookDescription = (string)b["bookDescription"];
                    addBook.author = (b["Authors"]["surname"].ToString().Trim() + " " + b["Authors"]["name"].ToString().Trim() + " " + b["Authors"]["patronymic"].ToString().Trim()).ToString();
                    addBook.bookCost = (decimal)b["bookCost"];
                    addBook.genre = (string)b["Genre1"]["genre1"];
                    addBook.bookImg = (byte[])b["bookImg"];

                    if (Basket.basketDiction.Keys.Contains(addBook.bookId))
                    {
                        addBook.Quantities = Basket.basketDiction[addBook.bookId];
                        viewBooks.Add(addBook);
                    }

                }

                BasketList.ItemsSource = viewBooks;
                BasketList.BindingContext = viewBooks;
                NullLabelBasket.IsVisible = false;
            }
            
        }

        private async void BasketList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            if (sender is ListView lv) lv.SelectedItem = null;
            Books selectedBook = e.Item as Books;
            if (selectedBook != null)
            {
                await Navigation.PushAsync(new viewBook(selectedBook));
            }
        }

        private void ButtonPlus_Clicked(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).AutomationId);
            if (Basket.basketDiction[id] > 0)
            {
                Basket.basketDiction[id] += 1;
            }
            else
                Basket.basketDiction.Remove(id);
            view();
        }

        private void ButtonMin_Clicked(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).AutomationId);
            if (Basket.basketDiction.ContainsKey(id)){
                if (Basket.basketDiction[id] > 1)
                {
                    Basket.basketDiction[id] -= 1;
                }
                else
                    Basket.basketDiction.Remove(id);
            }
            view();
            
        }
    }
}