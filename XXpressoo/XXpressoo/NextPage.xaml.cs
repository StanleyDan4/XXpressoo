using System;

using System.Threading.Tasks;

using Xamarin.Forms;

using Xamarin.Forms.Xaml;

namespace XXpressoo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NextPage : ContentPage
    {
        
        public NextPage()
        {
            InitializeComponent();
            Appearing += OnPageAppearing;
        }
        private async void OnPageAppearing(object sender, EventArgs e)
        {
            await Task.WhenAll(
                LogoImage.FadeTo(1, 800),
                TitleLabel.FadeTo(1, 800),
                SubtitleLabel.FadeTo(1, 800),
                ButtonsLayout.FadeTo(1, 800)
            );
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage());
            
            await RegisterButton.ScaleTo(0.95, 100, Easing.SinOut);
            await RegisterButton.ScaleTo(1.0, 100, Easing.SpringIn);
        }
        private async void OnAuthClicked(object sender, System.EventArgs e)
        {
            await LoginButton.ScaleTo(0.95, 100, Easing.SinOut);
            await LoginButton.ScaleTo(1.0, 100, Easing.SpringIn);
            await Navigation.PushAsync(new AuthPage());
        }
    }
}