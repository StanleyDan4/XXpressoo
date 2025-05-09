using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XXpressoo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async Task Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NextPage());
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NextPage());
        }
    }
}
