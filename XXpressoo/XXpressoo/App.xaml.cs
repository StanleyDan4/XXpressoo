using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using Xpressoo.Models;
using XXpressoo.Services;

namespace XXpressoo
{
    public partial class App : Application
    {
        // Глобальная корзина
        public static List<BasketItem> Cart { get; set; } = new();
        public static ApiService Api { get; } = new ApiService();
        public static int CartUserId = 1;

        public App()
        {
            InitializeComponent();
            
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
