using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XXpressoo.Services;
using XXpressoo.Models.Dtos;
using XXpressoo.Models;
using XXpressoo.Controls;

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

            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Ошибка", "Заполните все поля", "ОК");
                return;
            }

            // Проверка формата email
            if (!IsValidEmail(email))
            {
                await DisplayAlert("Ошибка", "Некорректный формат email. Пример: user@example.com", "ОК");
                return;
            }

            // Проверка пароля
            if (!IsValidPassword(password))
            {
                await DisplayAlert("Ошибка",
                    "Пароль должен содержать не менее 6 символов,\nвключая буквы и цифры.",
                    "ОК");
                return;
            }

            try
            {
                // Проверка существования email на сервере
                bool emailExists = await App.Api.CheckEmailExistsAsync(email);
                if (emailExists)
                {
                    await DisplayAlert("Ошибка", "Пользователь с таким email уже существует", "ОК");
                    return;
                }

                string firstName = "не задано";
                string lastName = "не задано";

                int userId = await App.Api.RegisterUserAsync(email, firstName, lastName, password);

                await DisplayAlert("Успех", "Регистрация прошла успешно!", "ОК");
                await Navigation.PushAsync(new StartPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Произошла ошибка: {ex.Message}", "ОК");
            }
        }

        // Проверка формата email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Проверка пароля: минимум 6 символов, содержит буквы и цифры
        private bool IsValidPassword(string password)
        {
            return password.Length >= 6 &&
                   Regex.IsMatch(password, @"[A-Za-z]") &&
                   Regex.IsMatch(password, @"[0-9]");
        }
    }
}