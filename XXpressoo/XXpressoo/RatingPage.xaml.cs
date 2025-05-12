using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XPressoo.Models.Dtos;
using XXpressoo.Models.Dtos;
using XXpressoo.Services;

namespace XXpressoo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingPage : ContentPage
    {
        private int _rating = 0;
        private readonly ApiService _apiService;

        public RatingPage()
        {
            InitializeComponent();
            _apiService = App.Api;
        }

        private void OnRateClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                SetRatings(int.Parse(button.Text));
            }
        }

        private void SetRatings(int selectedRating)
        {
            _rating = selectedRating;

            Rate1.BackgroundColor = selectedRating >= 1 ? Color.FromHex("#4CAF50") : Color.FromHex("#E53935");
            Rate2.BackgroundColor = selectedRating >= 2 ? Color.FromHex("#4CAF50") : Color.FromHex("#E53935");
            Rate3.BackgroundColor = selectedRating >= 3 ? Color.FromHex("#4CAF50") : Color.FromHex("#E53935");
            Rate4.BackgroundColor = selectedRating >= 4 ? Color.FromHex("#4CAF50") : Color.FromHex("#E53935");
            Rate5.BackgroundColor = selectedRating >= 5 ? Color.FromHex("#4CAF50") : Color.FromHex("#E53935");
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            if (_rating == 0)
            {
                await DisplayAlert("Ошибка", "Выберите оценку", "OK");
                return;
            }

            var dto = new ReviewDto
            {
                UserId = App.CartUserId,
                ProductId = 1, // Можно передать ID товара из контекста
                Rating = _rating,
                ReviewText = ReviewTextEditor.Text
            };

            try
            {
                await _apiService.SubmitReviewAsync(dto);
                await DisplayAlert("Успех", "Спасибо за ваш отзыв!", "OK");
                await Navigation.PopAsync(); // Возвращаемся обратно
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось отправить отзыв: {ex.Message}", "OK");
            }
        }
    }
}