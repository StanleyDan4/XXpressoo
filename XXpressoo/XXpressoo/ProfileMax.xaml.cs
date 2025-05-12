using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xpressoo.Models;
using XXpressoo.Models.Dtos;
using XXpressoo.Services;

namespace XXpressoo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileMax : ContentPage
    {
        private readonly ApiService _apiService;

        public ProfileMax()
        {
            InitializeComponent();
            _apiService = App.Api;
            LoadUserData();
        }

        private async void LoadUserData()
        {
            if (App.CartUserId <= 0)
            {
                await DisplayAlert("Ошибка", "Пользователь не авторизован", "ОК");
                return;
            }

            try
            {
                var user = await _apiService.GetUserAsync(App.CartUserId);
                if (user == null)
                {
                    await DisplayAlert("Ошибка", "Данные пользователя не были получены", "ОК");
                    return;
                }

                Namet.Text = user.FirstName;
                Surnamet.Text = user.LastName;
                Mailt.Text = user.Email;
                Passwordt.Text = user.Password;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки данных: {ex.Message}");
                await DisplayAlert("Ошибка", $"Не удалось загрузить данные: {ex.Message}", "ОК");
            }
        }
        private async void OnConfirmClicked(object sender, EventArgs e)
        {
            if (App.CartUserId <= 0)
            {
                await DisplayAlert("Ошибка", "Пользователь не авторизован", "ОК");
                return;
            }

            // Проверка email
            if (string.IsNullOrEmpty(Mailt.Text))
            {
                await DisplayAlert("Ошибка", "Email не указан", "ОК");
                return;
            }

            // Проверка имени
            if (string.IsNullOrEmpty(Namet.Text) || !IsValidName(Namet.Text))
            {
                await DisplayAlert("Ошибка", "Имя должно начинаться с заглавной буквы и содержать только буквы", "ОК");
                return;
            }

            // Проверка фамилии
            if (string.IsNullOrEmpty(Surnamet.Text) || !IsValidName(Surnamet.Text))
            {
                await DisplayAlert("Ошибка", "Фамилия должна начинаться с заглавной буквы и содержать только буквы", "ОК");
                return;
            }

            try
            {
                string passwordToUpdate = string.IsNullOrEmpty(Passwordt.Text) ? null : Passwordt.Text;

                await _apiService.UpdateUserAsync(
                    App.CartUserId,
                    Namet.Text,
                    Surnamet.Text,
                    passwordToUpdate);

                await DisplayAlert("Успех", "Данные успешно сохранены", "ОК");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения данных: {ex.Message}");
                await DisplayAlert("Ошибка", $"Не удалось сохранить данные: {ex.Message}", "ОК");
            }
        }
        private bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            // Проверяем, что строка содержит только буквы
            if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[A-Za-zА-Яа-я]+$"))
                return false;

            // Проверяем, начинается ли имя с заглавной буквы
            return char.IsUpper(name[0]);
        }
    }
}