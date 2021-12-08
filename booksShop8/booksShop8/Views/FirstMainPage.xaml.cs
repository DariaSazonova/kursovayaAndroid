using booksShop8.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
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
        IOrderedEnumerable<popularBooks> sorted;
        //public static List<Books> Bookslist = new List<Books>();
        public FirstMainPage()
        {
            InitializeComponent();
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (popularBooks.Count == 0 || popularBooks.Count != Basket.viewBooks.Count)
            {
                loading.IsVisible = true;
                var wait = await refresh();
                CollectionViewMain.ItemsSource = sorted.Take(10);
                CollectionViewMain.BindingContext = sorted.Take(10);
            }
            loading.IsVisible = false;
            ICommand refreshCommand = new Command(async () =>
            {
                // IsRefreshing is true
                // Refresh data here
                var wait = await refresh();
                CollectionViewMain.ItemsSource = sorted.Take(10);
                CollectionViewMain.BindingContext = sorted.Take(10);
                refreshView.IsRefreshing = false;
            });
            refreshView.Command = refreshCommand;

        }

        async Task<List<Books>> refresh()
        {
            popularBooks.Clear();
            if(sorted!=null)
                sorted.ToList().Clear();
            var r = await c.view("viewParametr");
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
            return r;
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