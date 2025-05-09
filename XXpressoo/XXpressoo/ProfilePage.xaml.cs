using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XXpressoo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }
        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StartPage());
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuPage());
        }

        private async void OnOrderClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Заказ", "Оформление заказа...", "ОК");
        }

        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        private async void OnCartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BasketPage());
        }
    }
}