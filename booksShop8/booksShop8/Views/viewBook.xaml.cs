using Android.Graphics;
using booksShop8.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace booksShop8.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class viewBook : ContentPage
    {
        Books book;
        public viewBook(Books b)
        {
            InitializeComponent();
            book = b;
            //вывод картинки 
            

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

            LabelBookName.Text = $"Название: {book.bookName}";
            LabelAuthor.Text = $"Автор: {book.author}";
            LabelBookCost.Text = $"Цена: {book.bookCost} ₽";
            LabelBookDescription.Text = $"Описание: {book.bookDescription}";
            LabelGenre.Text = $"Жанр: {book.genre}";
            ContentP.Title = book.bookName;
       

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
    }
}