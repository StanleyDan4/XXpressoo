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
    public partial class AuthPage : ContentPage
    {
        public AuthPage()
        {
            InitializeComponent();
        }
        private async void OnAuthClicked(object sender, System.EventArgs e)
        {
            // Здесь логика регистрации
            await DisplayAlert("Успешно", "Вход прошла успешно!", "OK");
            await Navigation.PushAsync(new StartPage());

            // После регистрации можно перейти на главную страницу
            // await Navigation.PushAsync(new MainPage());
        }
    }
}