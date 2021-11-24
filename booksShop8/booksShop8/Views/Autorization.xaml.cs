﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using booksShop8.ViewModels;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace booksShop8.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class Autorization : ContentPage
    {
        Service service = new Service();
        
        public static profile profil = new profile();

        public Autorization()
        {
            InitializeComponent();
            
        }
        protected override void OnAppearing()
        {
            
            base.OnAppearing();
            

            foreach (var el in App.Current.Properties.ToList())
            {
                int n = -1;
                bool isNumeric = int.TryParse(el.Key, out n);
                if (isNumeric == false)
                {
                    profil.login = el.Key;
                    profil.id = Convert.ToInt32(el.Value);
                    
                }
            }

            if (String.IsNullOrWhiteSpace(profil.login))
            {
                LabLog.IsVisible = true;
                loginEntry.IsVisible = true;
                LabPass.IsVisible = true;
                passwordEntry.IsVisible = true;
                InButton.IsVisible = true;
                RegButton.IsVisible = true;

                lSurname.IsVisible = false;
                lName.IsVisible = false;
                lPatronymic.IsVisible = false;
                lNumber.IsVisible = false;
                lEmail.IsVisible = false;
                bExit.IsVisible = false;
                ButtonOrders.IsVisible = false;
                LabProfile.IsVisible = false;



            }
            else
            {
                lSurname.IsVisible = true;
                lName.IsVisible = true;
                lPatronymic.IsVisible = true;
                lNumber.IsVisible = true;
                lEmail.IsVisible = true;
                bExit.IsVisible = true;
                ButtonOrders.IsVisible = true;
                LabProfile.IsVisible = true;

                LabLog.IsVisible = false;
                loginEntry.IsVisible = false;
                LabPass.IsVisible = false;
                passwordEntry.IsVisible = false;
                InButton.IsVisible = false;
                RegButton.IsVisible = false;
                

                client();
            }
            Title = "Профиль";
        }
        private async void client()
        {
            string resultBook = await service.Getclient(profil.login);
            var js = JArray.Parse(resultBook);
            profil.name = (string)js[0]["name"];
            profil.surname = (string)js[0]["surname"];
            profil.patronymic = (string)js[0]["patronymic"];
            profil.phone = (string)js[0]["phone"];

            lSurname.Text = $"Фамилия: {profil.surname}";
            lName.Text = $"Имя:{profil.name}";
            lPatronymic.Text = $"Отчество:{profil.patronymic}";
            lNumber.Text = $"Номер телефона:{profil.phone}";
            lEmail.Text = $"Почта:{profil.login}";

        }

            private async void InButton_ClickedAsync(object sender, EventArgs e)
        {

           
            if(!string.IsNullOrWhiteSpace(loginEntry.Text) && !string.IsNullOrWhiteSpace(passwordEntry.Text))
            {
                var login = loginEntry.Text.Trim();
                var password = passwordEntry.Text.Trim();
                passwordEntry.Text = null;
                string resulUser = await service.Getuser(login);
                var js = JArray.Parse(resulUser);
                if(resulUser== "[]")
                {
                    await DisplayAlert("Ошибка", "Пользователь с таким email не зарегистрирован", "ОK");
                }
               
                else
                {
                    var correctPassword = js[0]["password"].ToString().Trim();
                    if (correctPassword != password)
                    {
                        await DisplayAlert("Ошибка", "Неправильный пароль", "ОK");
                    }
                    else
                    {
                        string resultBook = await service.Getclient(login);
                        var js1 = JArray.Parse(resultBook);
                        App.Current.Properties.Add(login, (string)js1[0]["clientId"]);
                        OnAppearing();

                    } 
                        
                }

            }
            else
            {
                await DisplayAlert("Ошибка", "Не все поля заполнены", "ОK");
            }
            
        }

        private void bExit_Clicked(object sender, EventArgs e)
        {
            App.Current.Properties.Remove(profil.login);
            profil.name = "";
            profil.surname = "";
            profil.patronymic = "";
            profil.phone ="";
            profil.login = "";
            OnAppearing();
        }

        private async void ButtonOrders_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrdersHistory());
        }
    }
}