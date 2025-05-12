using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XXpressoo.Services;

namespace XXpressoo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : ContentPage
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Ошибка", "Введите email и пароль", "ОК");
                return;
            }

            try
            {
                // 1. Проверяем, существует ли пользователь
                bool exists = await App.Api.CheckEmailExistsAsync(email);

                if (!exists)
                {
                    await DisplayAlert("Ошибка", "Пользователь не найден", "ОК");
                    return;
                }

                // 2. Получаем пользователя от сервера
                var user = await App.Api.LoginUserAsync(email, password);

                if (user == null)
                {
                    await DisplayAlert("Ошибка", "Неверный email или пароль", "ОК");
                    return;
                }

                // 3. Сохраняем ID пользователя
                App.CartUserId = user.UserId;

                // 4. Переходим на главную страницу
                await Navigation.PushAsync(new StartPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось войти: {ex.Message}", "ОК");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Ошибка", "Введите email и пароль", "ОК");
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

                int userId = await App.Api.RegisterUserAsync(email, firstName, lastName, password);
                App.CartUserId = userId;

                await DisplayAlert("Успех", "Регистрация прошла успешно!", "ОК");
                await Navigation.PushAsync(new StartPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось зарегистрироваться: {ex.Message}", "ОК");
            }
        }
    }
}