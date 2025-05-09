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
    public partial class MenuPage : ContentPage
    {
        private List<Product> AllProducts = new();
        private List<Product> DrinksList = new();
        private List<Product> FoodList = new();

        public MenuPage()
        {
            InitializeComponent();
            LoadProductsFromApi();
        }

        private async void LoadProductsFromApi()
        {
            var api = new ApiService();
            //AllProducts = await api.GetProductsAsync();

            DrinksList = AllProducts.Where(p => p.CategoryId == 1 || p.CategoryId == 2).ToList(); // напитки
            FoodList = AllProducts.Where(p => p.CategoryId == 3 || p.CategoryId == 4 || p.CategoryId == 5).ToList(); // еда

            ShowDrinks();
        }

        private void ShowDrinks()
        {
            DrinksContent.Children.Clear();
            foreach (var product in DrinksList)
            {
                DrinksContent.Children.Add(CreateProductGrid(product));
            }

            DrinksButton.BackgroundColor = Color.FromHex("#5D4037");
            DrinksButton.TextColor = Color.White;
            FoodButton.BackgroundColor = Color.Transparent;
            FoodButton.TextColor = Color.FromHex("#5D4037");
        }

        private void ShowFood()
        {
            FoodContent.Children.Clear();
            foreach (var product in FoodList)
            {
                FoodContent.Children.Add(CreateProductGrid(product));
            }

            FoodButton.BackgroundColor = Color.FromHex("#5D4037");
            FoodButton.TextColor = Color.White;
            DrinksButton.BackgroundColor = Color.Transparent;
            DrinksButton.TextColor = Color.FromHex("#5D4037");
        }

        private Grid CreateProductGrid(Product product)
        {
            var grid = new Grid
            {
                Padding = new Thickness(0, 5),
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            var nameLabel = new Label
            {
                Text = product.ProductName,
                Style = (Style)Resources["MenuItemNameLabelStyle"]
            };
            var tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += async (s, e) =>
            {
                await PopupNavigation.Instance.PushAsync(new MyCustomPopup(
                    product.ProductName,
                    product.Description,
                    product.Image ?? "placeholder.png"));
            };
            nameLabel.GestureRecognizers.Add(tapRecognizer);
            nameLabel.GestureRecognizers.Add(tapRecognizer);

            var priceLabel = new Label
            {
                Text = $"{product.Price} руб.",
                Style = (Style)Resources["MenuItemPriceLabelStyle"]
            };

            var stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = { nameLabel, priceLabel }
            };

            var cartButton = new Button
            {
                Style = (Style)Resources["AddToCartButtonStyle"],
                CommandParameter = product.ProductId
            };
            cartButton.Clicked += async (s, e) =>
            {
                var productId = Convert.ToInt32(cartButton.CommandParameter);
                //await App.Database.AddToCartAsync(App.CartUserId, productId);
                await DisplayAlert("Корзина", $"{product.ProductName} добавлен(а)", "ОК");
            };

            grid.Children.Add(stackLayout);
            grid.Children.Add(cartButton, 1, 0);

            return grid;
        }

        private void OnCategoryChanged(object sender, EventArgs e)
        {
            if (sender == DrinksButton)
                ShowDrinks();
            else
                ShowFood();
        }
    }
}