using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XXpressoo.Popups;

namespace XXpressoo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private int _userId;
        public ProfilePage()
        {
            InitializeComponent();
            
            
        }
        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StartPage());
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            var userId = App.CartUserId;
            await Navigation.PushAsync(new MenuPage());
        }
        private void OnNotificationClicked(object sender, EventArgs e)
        {
            foreach (var item in ((StackLayout)Content).Children)
            {
                if (item is Frame frame && frame.ClassId == "notifications-frame")
                {
                    if (frame.BackgroundColor == Color.FromHex("#E53935"))
                    {
                        frame.BackgroundColor = Color.FromHex("#4CAF50");
                    }
                    else
                    {
                        frame.BackgroundColor = Color.FromHex("#E53935");
                    }
                    break;
                }
            }
        }
        private async void OnRateAppClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RatingPage());
        }
        private async void OnOrderClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new BasketPage());
        }

        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        private async void OnCartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BasketPage());
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfileMax());

        }

        private void Imaggg_Clicked(object sender, EventArgs e)
        {
            
               
                    if (NotificationsFrame.BackgroundColor == Color.FromHex("#E53935"))
                    {
                NotificationsFrame.BackgroundColor = Color.FromHex("#4CAF50");
                    }
                    else
                    {
                        NotificationsFrame.BackgroundColor = Color.FromHex("#E53935");
                    }
            
                
            
        }
    }
}