using booksShop8.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace booksShop8.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    
    public partial class Registaration : ContentPage
    {
        Service service = new Service();

        string login;
        string password;
        string password2;
        string surname;
        string name;
        string patronymic;
        string phone;
        public Registaration()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //var login = EntryLogin.Text.Trim();
            //var password = EntryPassword.Text.Trim();
            //var password2 = EntryPasswordAgain.Text.Trim();
            surname = EntrySurname.Text.Trim();
            name = EntryName.Text.Trim();
            patronymic = EntryPatronymic.Text.Trim();
            //var phone = EntryPhone.Text.Trim();

            if(!String.IsNullOrEmpty(login)
                &&!String.IsNullOrEmpty(password)
                && !String.IsNullOrEmpty(password2)
                && !String.IsNullOrEmpty(surname)
                && !String.IsNullOrEmpty(name)
                && !String.IsNullOrEmpty(patronymic)
                && !String.IsNullOrEmpty(phone))
            {
                string resultBook = await service.GetUser(login);
                //var js = JArray.Parse(resultBook);
                if (resultBook == "[]")
                {
                    if (password2 == password)
                    {
                        var newUser = new User
                        {
                            Login = login,
                            Password = password,
                            Role = "2"
                        };
                        var client = new Client
                        {
                            ClientEmail = login,
                            Surname = surname,
                            Name = name,
                            Patronymic = patronymic,
                            Phone = phone
                        };
                        var u = await service.AddUser(newUser);
                        var c = await service.AddClient(client);
                        if(u!=null && c!=null)
                        {
                            await DisplayAlert("Сообщение", "Вы успешно зарегистрировались", "ОK");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Ошибка", "Извините. Произошла какая-то оишбка на сервере", "ОK");
                        }
                            

                    }
                    else 
                        await DisplayAlert("Ошибка", "Пароли не совпадают", "ОK");
                }
                else
                    await DisplayAlert("Ошибка", "Пользователь с таким email уже зарегистрирован", "ОK");
            }
            else await DisplayAlert("Ошибка", "Не все поля заполнены", "ОK");

        }


      
        private void EntryLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var login = EntryLogin.Text.Trim();
            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
            if (!Regex.IsMatch((sender as Entry).Text, pattern, RegexOptions.IgnoreCase))
                (sender as Entry).TextColor = Color.Red;
            else 
            {
                (sender as Entry).TextColor = Color.Black;
                login = (sender as Entry).Text;
            }
        }

        private void EntryPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            var rule = @"^(?=.{8,16}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9]).*$";
            if (!Regex.IsMatch((sender as Entry).Text, rule, RegexOptions.IgnoreCase))
                (sender as Entry).TextColor = Color.Red;
            else
            {
                (sender as Entry).TextColor = Color.Black;
                if ((sender as Entry).AutomationId == "p1") password = (sender as Entry).Text;
                else if ((sender as Entry).AutomationId == "p2") password2 = (sender as Entry).Text;
            }
        }

        private void EntryPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            var pattern = "[+7|8]{1}[0-9]{10}";
            if (!Regex.IsMatch((sender as Entry).Text, pattern, RegexOptions.IgnoreCase))
                (sender as Entry).TextColor = Color.Red;
            else
            {
                (sender as Entry).TextColor = Color.Black;
                phone = (sender as Entry).Text;
            }
        }
    }
}