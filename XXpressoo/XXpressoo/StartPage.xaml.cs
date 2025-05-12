using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XXpressoo.Services;
using Rg.Plugins.Popup.Services;
using Xpressoo.Models;
using XXpressoo.Popups;


namespace XXpressoo
{
    public partial class StartPage : ContentPage
    {
        private int _userId;
        public StartPage()
        {
            
            InitializeComponent();
           
          


        }


        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Главная", "Вы на главной странице", "ОК");
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            var userId = App.CartUserId;
            await Navigation.PushAsync(new MenuPage());
        }

        private async void OnOrderClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new BasketPage());
        }

        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        private async void OnCartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BasketPage());
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Выход", "Вы уверены, что хотите выйти?", "Да", "Нет");
            if (result)
            {
                await Navigation.PopToRootAsync(); // или MainPage = new LoginPage();
            }
        }
    }
}