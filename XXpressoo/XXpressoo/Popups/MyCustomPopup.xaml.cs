using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace XXpressoo.Popups
{
    public partial class MyCustomPopup : PopupPage
    {
        private int _quantity = 1;

        public MyCustomPopup(string title, string description, string imageUrl)
        {
            InitializeComponent(); // Подгружает XAML

            TitleLabel.Text = title;
            DescriptionLabel.Text = description;
            ItemImage.Source = ImageSource.FromFile(imageUrl);
            QuantityLabel.Text = _quantity.ToString();
        }

        private void OnMinusClicked(object sender, EventArgs e)
        {
            if (_quantity > 1)
            {
                _quantity--;
                QuantityLabel.Text = _quantity.ToString();
            }
        }

        private void OnPlusClicked(object sender, EventArgs e)
        {
            _quantity++;
            QuantityLabel.Text = _quantity.ToString();
        }

        private async void OnAddToCartClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Корзина", $"{TitleLabel.Text} × {_quantity} шт.", "ОК");
            await PopupNavigation.Instance.PopAsync();
        }
    }
}