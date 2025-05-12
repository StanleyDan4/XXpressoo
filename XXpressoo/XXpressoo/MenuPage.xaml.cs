using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XXpressoo.Services;
using Rg.Plugins.Popup.Services;
using Xpressoo.Models;
using XXpressoo.Popups;
using XPressoo.Models.Dtos;

namespace XXpressoo
{
    public partial class MenuPage : ContentPage
    {
        private List<Product> AllProducts = new();
        private List<Product> DrinksList = new();
        private List<Product> FoodList = new();
        public int _userId;
         
        public MenuPage()
        {
            InitializeComponent();
            LoadProductsFromApi();
            
          
        }

        private async void LoadProductsFromApi()
        {
            
            try
            {
                // Загружаем все продукты
                AllProducts = await App.Api.GetProductsAsync();

                // Разделяем по категориям
                DrinksList = AllProducts
                    .Where(p => p.CategoryId == 1 || p.CategoryId == 2)
                    .ToList();

                FoodList = AllProducts
                    .Where(p => p.CategoryId == 3 || p.CategoryId == 4 || p.CategoryId == 5)
                    .ToList();

                ShowDrinks(); // По умолчанию показываем напитки
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось загрузить товары: {ex.Message}", "ОК");
            }
        }

        private void ShowDrinks()
        {
            ProductsCollection.ItemsSource = DrinksList;
            CategoryTitle.Text = "Напитки";
            SetActiveButton(DrinksButton);
        }

        private void ShowFood()
        {
            ProductsCollection.ItemsSource = FoodList;
            CategoryTitle.Text = "Еда";
            SetActiveButton(FoodButton);
        }
        private async void OnProductNameTapped(object sender, EventArgs e)
        {
            var label = sender as Label;
            var product = label?.BindingContext as Product;

            if (product != null)
            {
                await PopupNavigation.Instance.PushAsync(new MyCustomPopup(
                    product.ProductName,
                    product.Description,
                    product.Image ?? "placeholder.png"));
            }
        }
        private async void OnAddToCartClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var productId = Convert.ToInt32(button?.CommandParameter);

            try
            {
                await App.Api.AddToCartAsync(App.CartUserId, productId);
                await DisplayAlert("Корзина", $"{(button?.BindingContext as Product)?.ProductName} добавлен(а)", "ОК");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось добавить в корзину: {ex.Message}", "ОК");
            }
        }

        private void SetActiveButton(Button activeButton)
        {
            DrinksButton.BackgroundColor = Color.Transparent;
            DrinksButton.TextColor = Color.FromHex("#5D4037");

            FoodButton.BackgroundColor = Color.Transparent;
            FoodButton.TextColor = Color.FromHex("#5D4037");

            activeButton.BackgroundColor = Color.FromHex("#5D4037");
            activeButton.TextColor = Color.White;
        }

        private void OnCategoryChanged(object sender, EventArgs e)
        {
            if (sender == DrinksButton && DrinksButton.BackgroundColor != Color.FromHex("#5D4037"))
                ShowDrinks();
            else if (sender == FoodButton && FoodButton.BackgroundColor != Color.FromHex("#5D4037"))
                ShowFood();
        }
    }
}