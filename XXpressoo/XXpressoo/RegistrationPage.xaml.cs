using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XXpressoo.Services;

namespace XXpressoo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Ошибка", "Заполните все поля", "ОК");
                return;
            }

            try
            {
                bool emailExists = await App.Api.CheckEmailExistsAsync(email);
                if (emailExists)
                {
                    await DisplayAlert("Ошибка", "Пользователь с таким email уже существует", "ОК");
                    return;
                }

                string firstName = "не задано";
                string lastName = "не задано";
                bool success = await App.Api.RegisterUserAsync(email, firstName, lastName, password);

                if (success)
                {
                    await DisplayAlert("Успех", "Вы успешно зарегистрировались!", "ОК");
                    await Navigation.PushAsync(new MenuPage());
                }
                else
                {
                    await DisplayAlert("Ошибка", "Не удалось зарегистрироваться. Попробуйте позже.", "ОК");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Произошла ошибка: {ex.Message}", "ОК");
            }
        }
    }
}