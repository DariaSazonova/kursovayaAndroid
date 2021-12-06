using booksShop8.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace booksShop8.Views
{
    public class popularBooks
    {
        public string bookName { get; set; }
        public int bookId { get; set; }
        public string author { get; set; }
        public Image img { get; set; }
        public double? bookMark { get; set; }
        public double? bookMarkCount { get; set; }
    }
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstMainPage : ContentPage
    {
        Catalog c = new Catalog();
        Service service = new Service();
        List<popularBooks> popularBooks = new List<popularBooks>();
        //public static List<Books> Bookslist = new List<Books>();
        public FirstMainPage()
        {
            InitializeComponent();
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            IOrderedEnumerable<popularBooks> sorted;
            var r = await c.view("viewParametr");
            if (popularBooks == null || popularBooks.Count != Basket.viewBooks.Count)
            {
                foreach (var b in Basket.viewBooks)
                {
                    popularBooks pop = new popularBooks();
                    pop.bookName = b.bookName;
                    pop.bookId = b.bookId;
                    pop.author = b.author;
                    Image img = new Image();
                    var stream1 = new MemoryStream(b.bookImg);
                    img.Source = ImageSource.FromStream(() => stream1);
                    pop.img = img;
                    pop.bookMark = b.bookMark;
                    pop.bookMarkCount = b.bookMarkCount;
                    popularBooks.Add(pop);
                }

               sorted = popularBooks.OrderByDescending(it => it.bookMark / it.bookMarkCount);
                CollectionViewMain.ItemsSource = sorted.Take(10);
                CollectionViewMain.BindingContext = sorted.Take(10);
            }
           
        }


        private async void  CollectionViewMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var selected = Convert.ToInt32((sender as CollectionView).AutomationId);

            
            if (e.CurrentSelection == null) 
                return;
            popularBooks selectedBook = e.CurrentSelection.Last() as popularBooks;
            


            Books book = Basket.viewBooks.Where(b => b.bookId == selectedBook.bookId).FirstOrDefault();
            if (book != null)
            {
               
                    await Navigation.PushAsync(new viewBook(book));
        
            }
            //if (sender is CollectionView lv)
            //    lv.SelectedItem = null;

        }
    }
}