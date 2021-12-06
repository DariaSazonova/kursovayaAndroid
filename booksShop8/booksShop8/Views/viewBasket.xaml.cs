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
        Service service = new Service();
        public viewBasket()
        {
            InitializeComponent();

            
        }
        protected override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            viewB();

       

        }
        private  void viewB()
        {
            foreach (var el in Basket.basketDiction)
            {
                if (!App.Current.Properties.ContainsKey((el.Key).ToString()))
                    App.Current.Properties.Add(el.Key.ToString(), el.Value.ToString());
                else 
                    App.Current.Properties[el.Key.ToString()] = Basket.basketDiction[el.Key].ToString();
            }
            foreach (var el in App.Current.Properties.ToList())
            {
                int n;
                bool isNumeric = int.TryParse(el.Key, out n);

                if (!Basket.basketDiction.ContainsKey(n))
                    {
                        App.Current.Properties.Remove(el);
                    }

            }
            if (Basket.basketDiction.Count == 0)
            {
                NullLabelBasket.IsVisible = true;
                BasketList.IsVisible = false;
            }
            else
            {

                BasketList.IsVisible = true;
                
                //string result = await service.Get();  // тут json 

                List<Books> viewBooksBasket = new List<Books>();
               // var js = JArray.Parse(result);
               
                foreach (var bas in Basket.basketDiction.Keys)
                {
                    var b = Basket.viewBooks.Where(id => id.bookId == bas).FirstOrDefault();
                    if (b!=null)
                    {
                        Books addBook = new Books();
                        addBook.bookId = b.bookId;
                        addBook.bookName = b.bookName;
                        addBook.bookDescription = b.bookDescription;
                        addBook.author = b.author;
                        addBook.bookCost = b.bookCost;
                        addBook.genre = b.genre;
                        addBook.bookImg = b.bookImg;
                        addBook.status = b.status;
                        addBook.Quantities = Basket.basketDiction[bas];

                        viewBooksBasket.Add(addBook);
                    }
                    

                }

                BasketList.ItemsSource = viewBooksBasket;
                BasketList.BindingContext = viewBooksBasket;
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
                App.Current.Properties[id.ToString()] = Basket.basketDiction[id].ToString();
            }
            else
            {
                Basket.basketDiction.Remove(id);
                App.Current.Properties.Remove(id.ToString());
            }

            viewB();
        }

        private void ButtonMin_Clicked(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).AutomationId);
            if (Basket.basketDiction.ContainsKey(id)){
                if (Basket.basketDiction[id] > 1)
                {
                    Basket.basketDiction[id] -= 1;
                    App.Current.Properties[id.ToString()] = Basket.basketDiction[id].ToString();

                }
                else
                {
                    Basket.basketDiction.Remove(id);
                    App.Current.Properties.Remove(id.ToString());

                }
                   
            }
            viewB();
            
        }

        private async void Button_ClickedOrder(object sender, EventArgs e)
        {
            if (Basket.profil.id != 0)
            {
                if (Basket.basketDiction.Count>0)
                {
                    List<OrderDetails> listOr = new List<OrderDetails>();
                    List<BookforUpdating> listB = new List<BookforUpdating>();

                    foreach (var b in Basket.basketDiction)
                    {
                        string resBook = await service.GetBook(b.Key);
                        var js1 = JArray.Parse(resBook);
                        OrderDetails newB = new OrderDetails();
                        var book = Basket.viewBooks.Where(id => id.bookId == b.Key).First();
                        
                        if (b.Value <= book.Quantities)
                        {
                            newB.BookId = b.Key;
                            newB.Quantities = b.Value;
                            //newB.OrderId = newOrder.ordernum;
                            newB.Cost = Convert.ToInt32(js1[0]["bookCost"]);
                            listOr.Add(newB);
                            //await service.AddOrderDetails(newB);

                            BookforUpdating bookUpdate = new BookforUpdating
                            {
                                bookId = book.bookId,
                                bookName = book.bookName,
                                bookDescription = book.bookDescription,
                                bookCost = book.bookCost,
                                bookMark = Convert.ToInt32(book.bookMark),
                                bookMarkCount = Convert.ToInt32(book.bookMarkCount),
                                bookImg = book.bookImg,
                                quantities = Convert.ToInt32(book.Quantities)- b.Value,
                                genre = Genres.genres.Where(v => v.Value == book.genre).Select(s => s.Key).FirstOrDefault(),
                                statusId = Genres.status.Where(v => v.Value == book.status).Select(s => s.Key).FirstOrDefault()
                            };
                            if (bookUpdate.statusId == 0) bookUpdate.statusId = null;
                            listB.Add(bookUpdate);

                        }
                        else if(book.Quantities==0)
                        {
                            string res = await DisplayActionSheet($"{book.bookName.Trim()} нет в наличии", "Отменить заказ", null, "Оформить заказ с изменениями");
                            if(res== "Отменить заказ")
                            {
                                listOr.Clear();
                                listB.Clear();
                                break;
                            }
                        } 
                    }
                    if (listOr.Count > 0)
                    {



                        foreach (var o in listB)
                        {
                            await service.Update(o);
                        }

                        Order newOrder = new Order();
                        newOrder.ClientId = Basket.profil.id;
                        newOrder.DateFirst = DateTime.Today;
                        newOrder.status = "Создан";
                        await service.AddOrder(newOrder);

                        string resOrdNum = await service.GetOrderNum(Convert.ToInt32(newOrder.ClientId));
                        var js = JArray.Parse(resOrdNum);
                        newOrder.ordernum = Convert.ToInt32(js[0]["orderId"]);
                        foreach (var o in listOr)
                        {
                            o.OrderId = newOrder.ordernum;
                            await service.AddOrderDetails(o);
                        }
                        await DisplayAlert("Заказ оформлен", "Спасибо за заказ! Вся подробная информация о заказе находтся в разделе Профиль. История заказов.", "ОK");
                        Basket.basketDiction.Clear();
                       
                        OnAppearing();
                    }
                    else
                    {
                        await DisplayAlert("Сообщение", "Не удалось оформить заказ, так как нет товаров в наличии", "ОK");
                    }
                    listOr.Clear();
                    listB.Clear();
                }
                else
                {
                    await DisplayAlert("Ошибка", "Корзина пуста. Для офрмления заказа добавьте книги в корзину.", "ОK");
                }
               
            }
            else
            {
                await DisplayAlert("Ошибка", "Для оформления заказа нужно выполнить вход", "ОK");
            }


        }
            
    }
}