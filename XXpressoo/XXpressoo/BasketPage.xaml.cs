using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XXpressoo.Services;
using Xpressoo.Models;

namespace XXpressoo
{
    public partial class BasketPage : ContentPage
    {
        private List<BasketItem> _cart;

        public BasketPage()
        {
            InitializeComponent();
            LoadCartFromApi();
        }

        private async void LoadCartFromApi()
        {
            try
            {
                if (App.CartUserId <= 0)
                    throw new Exception("Пользователь не авторизован");

                var cartItems = await App.Api.GetCartAsync(App.CartUserId);
                SetCart(cartItems);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось загрузить корзину: {ex.Message}", "ОК");
            }
        }

        public void SetCart(List<BasketItem> cart)
        {
            _cart = cart;

            CartItemsStack.Children.Clear();

            if (_cart == null || !_cart.Any())
            {
                CartItemsStack.Children.Add(new Label
                {
                    Text = "Корзина пустая",
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    FontSize = 18,
                    TextColor = Color.Gray
                });
                UpdateTotalPrice();
                return;
            }

            foreach (var item in _cart)
            {
                var grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(80, GridUnitType.Absolute) },
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Auto }
                    },
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto }
                    },
                    Padding = new Thickness(10),
                    Margin = new Thickness(0, 5)
                };

                // Фото продукта
                var image = new Image
                {
                    Source = string.IsNullOrEmpty(item.ImageUrl)
                        ? ImageSource.FromFile("placeholder.png")
                        : ImageSource.FromUri(new Uri(item.ImageUrl)),
                    WidthRequest = 60,
                    HeightRequest = 60,
                    Aspect = Aspect.AspectFit
                };
                Grid.SetColumn(image, 0);
                Grid.SetRowSpan(image, 2);

                // Название
                var nameLabel = new Label
                {
                    Text = item.Name,
                    Style = (Style)Resources["ProductTitleStyle"]
                };
                Grid.SetColumn(nameLabel, 1);
                Grid.SetRow(nameLabel, 0);

                // Цена
                var priceLabel = new Label
                {
                    Text = $"{item.Price} руб.",
                    Style = (Style)Resources["ProductPriceStyle"]
                };
                Grid.SetColumn(priceLabel, 1);
                Grid.SetRow(priceLabel, 1);

                // Кнопка "Удалить"
                var deleteBtn = new Button
                {
                    Text = "Удалить",
                    CommandParameter = item.ProductId,
                    Style = (Style)Resources["DeleteButtonStyle"]
                };
                deleteBtn.Clicked += async (s, e) =>
                {
                    var productId = Convert.ToInt32(deleteBtn.CommandParameter);
                    var selectedItem = _cart.FirstOrDefault(i => i.ProductId == productId);
                    if (selectedItem != null)
                    {
                        _cart.Remove(selectedItem);
                        await App.Api.RemoveFromCartAsync(App.CartUserId, productId);
                        SetCart(_cart); // Перерисовываем список
                    }
                };

                Grid.SetColumn(deleteBtn, 2);
                Grid.SetRow(deleteBtn, 0);
                Grid.SetRow(deleteBtn, 1); // Занимает две строки

                // Расположение элементов
                grid.Children.Add(image);
                grid.Children.Add(nameLabel);
                grid.Children.Add(priceLabel);
                grid.Children.Add(deleteBtn);

                CartItemsStack.Children.Add(grid);
            }

            UpdateTotalPrice();
        }

        private void UpdateTotalPrice()
        {
            decimal total = _cart?.Sum(i => i.Price * i.Quantity) ?? 0;
            TotalPriceLabel.Text = $"Итого: {total} руб.";
        }

        private async void OnCheckoutClicked(object sender, EventArgs e)
        {
            var total = _cart.Sum(i => i.Price * i.Quantity);
            var result = await DisplayAlert("Заказ",
                $"Сумма заказа: {total} руб.\nПодтвердить оформление?",
                "Да", "Нет");

            string aaa = "Заказ оформлен \n Номер заказа: " + App.CartUserId;
            if (result)
            {
                await App.Api.CheckoutAsync(App.CartUserId, _cart);
                await DisplayAlert("Успех", aaa , "ОК");
                await Navigation.PopAsync();
            }
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StartPage());
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuPage());
        }

        private async void OnOrderClicked(object sender, EventArgs e)
        {
            var total = _cart.Sum(i => i.Price * i.Quantity);
            var result = await DisplayAlert("Заказ",
                $"Сумма заказа: {total} руб.\nПодтвердить оформление?",
                "Да", "Нет");

            if (result)
            {
                await App.Api.CheckoutAsync(App.CartUserId, _cart);
                await DisplayAlert("Успех", "Ваш заказ оформлен!", "ОК");
                await Navigation.PopAsync();
            }
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
                App.CartUserId = 0;
                await Navigation.PopToRootAsync();
            }
        }
    }
}