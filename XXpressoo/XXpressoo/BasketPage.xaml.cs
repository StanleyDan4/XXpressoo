using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpressoo.Models;

namespace XXpressoo
{
    public partial class BasketPage : ContentPage
    {
        private List<BasketItem> _cart;

        public BasketPage()
        {
            InitializeComponent();
        }

        public async void SetCart(List<BasketItem> cart)
        {
            _cart = cart;

            if (_cart == null || _cart.Count == 0)
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label { Text = "Корзина пустая", HorizontalOptions = LayoutOptions.CenterAndExpand }
                    }
                };
                return;
            }

            var listView = new ListView
            {
                ItemsSource = _cart,
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(() =>
                {
                    // Создаем элемент UI
                    var grid = new Grid
                    {
                        Padding = new Thickness(10),
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(80, GridUnitType.Absolute) },
                            new ColumnDefinition { Width = GridLength.Star },
                            new ColumnDefinition { Width = GridLength.Auto }
                        }
                    };

                    var image = new Image
                    {
                        Source = "placeholder.png",
                        WidthRequest = 60,
                        HeightRequest = 60,
                        Aspect = Aspect.AspectFit
                    };
                    Grid.SetColumn(image, 0);

                    var nameLabel = new Label();
                    nameLabel.SetBinding(Label.TextProperty, new Binding("Name"));

                    var priceLabel = new Label();
                    priceLabel.SetBinding(Label.TextProperty, new Binding("Price", stringFormat: "{0} руб."));

                    var nameStack = new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        Children = { nameLabel, priceLabel }
                    };
                    Grid.SetColumn(nameStack, 1);

                    var minusBtn = new Button
                    {
                        Text = "-",
                        WidthRequest = 40,
                        BackgroundColor = Color.FromHex("#E0E0E0"),
                        TextColor = Color.Black
                    };
                    minusBtn.Clicked += OnMinusClicked;

                    var quantityLabel = new Label
                    {
                        Text = "1",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        WidthRequest = 30
                    };
                    quantityLabel.SetBinding(Label.TextProperty, new Binding("Quantity"));

                    var plusBtn = new Button
                    {
                        Text = "+",
                        WidthRequest = 40,
                        BackgroundColor = Color.FromHex("#5D4037"),
                        TextColor = Color.White
                    };
                    plusBtn.Clicked += OnPlusClicked;

                    var buttonsStack = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.End,
                        Spacing = 10,
                        Children = { minusBtn, quantityLabel, plusBtn }
                    };
                    Grid.SetColumn(buttonsStack, 2);

                    grid.Children.Add(image);
                    grid.Children.Add(nameStack);
                    grid.Children.Add(buttonsStack);

                    return new ViewCell { View = grid };
                })
            };

            Content = new StackLayout
            {
                Padding = new Thickness(20),
                Children =
                {
                    new Label { Text = "Корзина", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new BoxView { HeightRequest = 1, Color = Color.LightGray },
                    listView
                }
            };
        }
        private async void OnCheckoutClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Заказ оформлен", "Спасибо за покупку!", "ОК");
            await Navigation.PopAsync();
        }

        private void OnMinusClicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var item = btn?.CommandParameter as BasketItem;

            if (item != null && item.Quantity > 1)
            {
                item.Quantity--;
                ((ListView)((StackLayout)Content).Children[2]).ItemsSource = null;
                ((ListView)((StackLayout)Content).Children[2]).ItemsSource = _cart;
            }
        }

        private void OnPlusClicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var item = btn?.CommandParameter as BasketItem;

            if (item != null)
            {
                item.Quantity++;
                ((ListView)((StackLayout)Content).Children[2]).ItemsSource = null;
                ((ListView)((StackLayout)Content).Children[2]).ItemsSource = _cart;
            }
        }

        private  async void ImageButton_Clicked(object sender, EventArgs e)
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
            //await Navigation.PushAsync(new ProfilePage());
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
                await Navigation.PopToRootAsync(); // или MainPage = new LoginPage();
            }
        }
    }
}