using Android.Graphics;
using booksShop8.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xunit;

namespace booksShop8.Views
{
    public class BookforUpdating
    {
        public int bookId { get; set; }
        public string bookName { get; set; }
        public string bookDescription { get; set; }
        public decimal bookCost { get; set; }
        public int genre { get; set; }
        public int? statusId { get; set; }
        public int? bookMark { get; set; }
        public int? bookMarkCount { get; set; }
        public byte[] bookImg { get; set; }
        public int quantities { get; set; }
        //public string genreNavigation { get; set; }
        //public string status { get; set; }
        //public string ordersDetails { get; set; }
        //public string booksAuthors { get; set; }

    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class viewBook : ContentPage
    {
        Books book;
        Service service = new Service();
        public viewBook(Books b)
        {
            InitializeComponent();
            book = b;
            //вывод картинки 
            OnAppearing();



        }
        protected override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            byte[] imageArray = book.bookImg; // is your data
            var stream1 = new MemoryStream(imageArray);
            try
            {
                ImageBook.Source = ImageSource.FromStream(() => stream1);
            }
            catch
            {
                stream1.Dispose();
                throw;
            }
            Labelquantity.Text = $"Остаток: {book.Quantities}";
            LabelBookName.Text = $"Название: {book.bookName}";
            LabelAuthor.Text = $"Автор: {book.author}";
            LabelBookCost.Text = $"Цена: {book.bookCost} ₽";
            LabelBookDescription.Text = $"Описание: {book.bookDescription}";
            LabelGenre.Text = $"Жанр: {book.genre}";
            ContentP.Title = book.bookName;
            var m = book.bookMark / book.bookMarkCount;
            string mark;
            if (book.bookMark == 0) 
                mark = "Нет оценок";
            else 
               mark = Math.Round(Convert.ToDecimal(m), 1).ToString();
            LabelMark.Text= $"Оценка: {mark}";




        }

        private void ButtonBasket_Clicked(object sender, EventArgs e)
        {
            int id = book.bookId;
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

        private async void ButtonMark_Clicked(object sender, EventArgs e)
        {
            string res = await DisplayActionSheet("Оценить", "Выход", null, "5", "4", "3", "2", "1");
            Debug.WriteLine("res1: " + res);
            if (res!= "Выход" && res!= null)
            {
                var resmark = Convert.ToInt32(res);
                book.bookMark += resmark;
                book.bookMarkCount += 1;
                BookforUpdating bookUpdate = new BookforUpdating
                {
                    bookId= book.bookId,
                    bookName=book.bookName,
                    bookDescription=book.bookDescription,
                    bookCost=book.bookCost,
                    bookMark=Convert.ToInt32(book.bookMark),
                    bookMarkCount= Convert.ToInt32(book.bookMarkCount),
                    bookImg = book.bookImg,
                    quantities = Convert.ToInt32(book.Quantities),

                    //AuthorId = Basket.authorDictionary.Where(v=>v.Value==book.author).Select(s=>s.Key).FirstOrDefault(),
                    genre = Genres.genres.Where(v=>v.Value==book.genre).Select(s=>s.Key).FirstOrDefault(),
                    statusId = Genres.status.Where(v => v.Value == book.status).Select(s => s.Key).FirstOrDefault()
                    
                };
                if (bookUpdate.statusId == 0) bookUpdate.statusId = null;
                await service.Update(bookUpdate);
                OnAppearing();
            }

        }
    }
}