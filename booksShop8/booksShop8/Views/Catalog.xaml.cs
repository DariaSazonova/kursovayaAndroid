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
        string serchS = "viewParametr";
       

        public Catalog()
        {
            InitializeComponent();
            serchS = "viewParametr";
            
        }
        protected async  override void OnAppearing()
        {
            
            //Write the code of your page here
            base.OnAppearing();
            
            GenreViews.BindingContext = Genres.genres.Values;
            GenreViews.ItemsSource = Genres.genres;
            SatusViews.BindingContext = Genres.status.Values;
            SatusViews.ItemsSource = Genres.status;
            BooksList.ItemsSource = Basket.viewBooks;
            BooksList.BindingContext = Basket.viewBooks;
            var r = await view(serchS);
        }
        public  async Task<List<Books>> view(string viewParametr)
        {
            string resultBook = await service.Get();
            var js = JArray.Parse(resultBook);
            foreach (var b in js)
            {
                var check = Basket.viewBooks.Where(book => book.bookId == (int)b["bookId"]).FirstOrDefault();
                Books addBook;
                if (check == null)
                {
                    addBook = new Books();
                }
                else
                {
                    addBook = Basket.viewBooks.Where(boo => boo.bookId == (int)b["bookId"]).FirstOrDefault();
                }

                    addBook.bookId = (int)b["bookId"];
                    addBook.bookName = (string)b["bookName"].ToString().Trim();
                    addBook.bookDescription = (string)b["bookDescription"];
                    addBook.bookCost = (decimal)b["bookCost"];
                    addBook.bookImg = (byte[])b["bookImg"];
                    addBook.bookMark = (double)b["bookMark"];
                    addBook.bookMarkCount = (double)b["bookMarkCount"];
                    addBook.Quantities = (int)b["quantities"];

                    string resulAuthor = await service.GetBooksAuthor((int)b["bookId"]);
                    var jsAuthor = JArray.Parse(resulAuthor);
                    string resauthor = "";
                    foreach(var a in jsAuthor)
                    {
                        string result = await service.GetAuthor((int)a["autorId"]);
                        var jsA = JArray.Parse(result);
                        
                        resauthor += (jsA[0]["surname"].ToString().Trim() + " " + jsA[0]["name"].ToString().Trim() + " " + jsA[0]["patronymic"].ToString().Trim()).ToString() + "  ";
                        if (!Basket.authorDictionary.ContainsKey((int)jsA[0]["authorId"]))
                            Basket.authorDictionary.Add((int)jsA[0]["authorId"], resauthor);
                    }

                    addBook.author = resauthor;//(jsAuthor[0]["surname"].ToString().Trim() + " " + jsAuthor[0]["name"].ToString().Trim() + " " + jsAuthor[0]["patronymic"].ToString().Trim()).ToString();
                    
                    
                    string resulGenre = await service.GetGenre((int)b["genre"]);
                    var jsGenre = JArray.Parse(resulGenre);
                    addBook.genre = ((string)jsGenre[0]["genre1"]).Trim();
                    


                    if ((string)b["statusId"] != null)
                    {
                        string resulStatus = await service.GetBookStatus((int)b["statusId"]);
                        var jsStatus = JArray.Parse(resulStatus);
                        addBook.status = ((string)jsStatus[0]["statusName"]).Trim();
                        if (!Genres.status.ContainsKey((int)b["statusId"]))
                            Genres.status.Add((int)b["statusId"], addBook.status);

                    }
                    else addBook.status = null;

                   
                    if (!Genres.genres.ContainsKey((int)jsGenre[0]["genreId"]))
                        Genres.genres.Add((int)jsGenre[0]["genreId"], addBook.genre);
                    if (viewParametr == "viewParametr" && check == null)
                        Basket.viewBooks.Add(addBook);
                    //else
                    //{
                    //    if (addBook.bookName.ToUpper().Contains(viewParametr.ToUpper()) || addBook.author.ToUpper().Contains(viewParametr.ToUpper()) || addBook.genre.ToUpper().Contains(viewParametr.ToUpper()) || addBook.bookDescription.ToUpper().Contains(viewParametr.ToUpper()))
                    //    {
                    //        Basket.viewBooks.Add(addBook);
                    //    }
                    //}
                }
            
            GenreViews.BindingContext = Genres.genres.Values;
            GenreViews.ItemsSource = Genres.genres;
            SatusViews.BindingContext = Genres.status.Values;
            SatusViews.ItemsSource = Genres.status;
            BooksList.ItemsSource = Basket.viewBooks;
            BooksList.BindingContext = Basket.viewBooks;
            return Basket.viewBooks;


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
            BooksList.ItemsSource = Basket.viewBooks.Where(b=>b.bookName.ToUpper().Contains(searchStr.ToUpper())|| b.author.ToUpper().Contains(searchStr.ToUpper())|| b.bookDescription.ToUpper().Contains(searchStr.ToUpper()));
            BooksList.BindingContext = Basket.viewBooks.Where(b => b.bookName.ToUpper().Contains(searchStr.ToUpper()) || b.author.ToUpper().Contains(searchStr.ToUpper()) || b.bookDescription.ToUpper().Contains(searchStr.ToUpper()));


        }

        private void ButtonGenre_Clicked(object sender, EventArgs e)
        {
            var genre = (sender as Button).Text;
            if (genre == "Все")
            {
                BooksList.ItemsSource = Basket.viewBooks;
                BooksList.BindingContext = Basket.viewBooks;
            }
            else
            {
                BooksList.ItemsSource = Basket.viewBooks.Where(b => b.genre.ToUpper().Contains(genre.ToUpper()));
                BooksList.BindingContext = Basket.viewBooks.Where(b => b.genre.ToUpper().Contains(genre.ToUpper()));
            }
            

        }
    

        private void ButtonStatus_Clicked(object sender, EventArgs e)
        {
            var status = (sender as Button).Text;
            BooksList.ItemsSource = Basket.viewBooks.Where(b => b.status==status);
            BooksList.BindingContext = Basket.viewBooks.Where(b => b.status == status);
        }
    }
}