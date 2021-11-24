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
        Service service = new Service();
        string serchS = "";
        public static List<Books> viewBooks = new List<Books>();

        public Catalog()
        {
            InitializeComponent();
            serchS = "";
        }
        protected override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            view(serchS);
        }
        private async void view(string viewParametr)
        {
            string resultBook = await service.Get();
            
            var js = JArray.Parse(resultBook);

           
            foreach (var b in js)
            {
                var check = viewBooks.Where(book => book.bookId == (int)b["bookId"]).FirstOrDefault();
                if (check == null)
                {


                    Books addBook = new Books();
                    addBook.bookId = (int)b["bookId"];
                    addBook.bookName = (string)b["bookName"];
                    addBook.bookDescription = (string)b["bookDescription"];
                    addBook.bookCost = (decimal)b["bookCost"];
                    addBook.bookImg = (byte[])b["bookImg"];

                    string resulAuthor = await service.GetAuthor((int)b["authorId"]);
                    var jsAuthor = JArray.Parse(resulAuthor);
                    addBook.author = (jsAuthor[0]["surname"].ToString().Trim() + " " + jsAuthor[0]["name"].ToString().Trim() + " " + jsAuthor[0]["patronymic"].ToString().Trim()).ToString();

                    string resulGenre = await service.GetGenre((int)b["genre"]);
                    var jsGenre = JArray.Parse(resulGenre);
                    addBook.genre = (string)jsGenre[0]["genre1"];

                    if ((string)b["statusId"] != null)
                    {
                        string resulStatus = await service.GetBookStatus((int)b["statusId"]);
                        var jsStatus = JArray.Parse(resulStatus);
                        addBook.status = (string)jsStatus[0]["statusName"];
                    }
                    else addBook.status = null;



                    if (!Genres.genres.ContainsKey((int)jsGenre[0]["genreId"]))
                        Genres.genres.Add((int)jsGenre[0]["genreId"], (string)jsGenre[0]["genre1"]);
                    if (viewParametr == "")
                        viewBooks.Add(addBook);
                    else
                    {
                        if (addBook.bookName.ToUpper().Contains(viewParametr.ToUpper()) || addBook.author.ToUpper().Contains(viewParametr.ToUpper()) || addBook.genre.ToUpper().Contains(viewParametr.ToUpper()) || addBook.bookDescription.ToUpper().Contains(viewParametr.ToUpper()) || addBook.status.ToUpper().Contains(viewParametr.ToUpper()))
                        {
                            viewBooks.Add(addBook);
                        }
                    }
                }
            }
            GenreViews.BindingContext = Genres.genres.Values;
            GenreViews.ItemsSource = Genres.genres;
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

       

        public void ButtonBasket_Clicked(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).AutomationId);
            if (Basket.basketDiction.Keys.Contains(id))
            {
                Basket.basketDiction[id] += 1;
                App.Current.Properties[id.ToString()] = Basket.basketDiction[id].ToString();
            }
            else
            {
                Basket.basketDiction.Add(id, 1);
                App.Current.Properties.Add(id.ToString(), "1");
            }
           
        }

        private void SearchBook_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchStr = (sender as SearchBar).Text.Trim();
            //if(String.IsNullOrWhiteSpace(searchStr))
            //view("");
            //else view(searchStr);
            BooksList.ItemsSource = viewBooks.Where(b=>b.bookName.ToUpper().Contains(searchStr.ToUpper()));
            BooksList.BindingContext = viewBooks.Where(b => b.bookName.ToUpper().Contains(searchStr.ToUpper()));


        }

        private void ButtonGenre_Clicked(object sender, EventArgs e)
        {
            var genre = (sender as Button).Text;
            if (genre == "Все")
            {
                view(serchS);
            }
            else
            {
                BooksList.ItemsSource = viewBooks.Where(b => b.genre.ToUpper().Contains(genre.ToUpper()));
                BooksList.BindingContext = viewBooks.Where(b => b.genre.ToUpper().Contains(genre.ToUpper()));
            }
            

        }
        public  void newBookList()
        {
            BooksList.ItemsSource = viewBooks.Where(b => b.status=="новинка");
            BooksList.BindingContext = viewBooks.Where(b => b.status == "новинка");
        }
    }
}