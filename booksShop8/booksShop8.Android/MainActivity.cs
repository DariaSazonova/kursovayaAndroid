using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using booksShop8.ViewModels;
using booksShop8.Views;

namespace booksShop8.Droid
{
    //[Activity(Label = "booksShop8", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    [Activity(Label = "booksShop8", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected   override void OnStart()
        {

            base.OnStart();
            if (App.Current.Properties.Count > 0)
            {
                foreach (var el in App.Current.Properties)
                {
                    int n;
                    bool isNumeric = int.TryParse(el.Key, out n);
                    if (isNumeric == true)
                    {
                        if (!Basket.basketDiction.ContainsKey(Convert.ToInt32(el.Key)))
                            Basket.basketDiction.Add(Convert.ToInt32(el.Key), Convert.ToInt32(el.Value));
                    }
                    else
                    {
                        Basket.profil.login = el.Key;
                        Basket.profil.id = Convert.ToInt32(el.Value);
                    }
                    
                }
            }

        }
        protected override void OnPause()
        {
          
            base.OnPause();

        }
       
        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}