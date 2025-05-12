using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xpressoo.Models;

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
            ItemImage.Source = ImageSource.FromUri(new Uri(imageUrl));
            
        }

      

        

      
    }
}