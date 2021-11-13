using booksShop8.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using Newtonsoft.Json.Linq;

using Android.Graphics;

namespace booksShop8.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class Catalog : ContentPage
    {

        public Catalog()
        {
            InitializeComponent();
            
 
        }
        protected override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            view("");
        }
        private async void view(string viewParametr)
        {
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
                addBook.author = (b["Authors"]["surname"].ToString().Trim()+" "+b["Authors"]["name"].ToString().Trim() + " " + b["Authors"]["patronymic"].ToString().Trim()).ToString();
                addBook.bookCost = (decimal)b["bookCost"];
                addBook.genre = (string)b["Genre1"]["genre1"];
                addBook.bookImg = (byte[])b["bookImg"];
                if (viewParametr == "")
                    viewBooks.Add(addBook);
                else
                {
                    if (addBook.bookName.ToUpper().Contains(viewParametr.ToUpper()) || addBook.author.ToUpper().Contains(viewParametr.ToUpper()) || addBook.genre.ToUpper().Contains(viewParametr.ToUpper()) || addBook.bookDescription.ToUpper().Contains(viewParametr.ToUpper()))
                    {
                        viewBooks.Add(addBook);
                    }
                }

            }
            BooksList.ItemsSource = viewBooks;
            BooksList.BindingContext = viewBooks;
        }

        public async void BooksList_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            //// don't do anything if we just de-selected the row.
            if (e.Item == null) return;

            if (sender is ListView lv) lv.SelectedItem = null;

            Books selectedBook = e.Item as Books;
            if (selectedBook != null)
            {
                await Navigation.PushAsync(new viewBook(selectedBook));
            }

        }

       

        private void ButtonBasket_Clicked(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).AutomationId);
            if (Basket.basketDiction.Keys.Contains(id))
            {
                Basket.basketDiction[id] += 1;
            }
            else 
            Basket.basketDiction.Add(id, 1);
        }

        private void SearchBook_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchStr = (sender as SearchBar).Text.Trim();
            if(String.IsNullOrWhiteSpace(searchStr))
             view("");
            else view(searchStr);
        }
    }
}