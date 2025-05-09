using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XXpressoo.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuItemView : ContentView
    {
        public static readonly BindableProperty NameProperty =
            BindableProperty.Create(nameof(Name), typeof(string), typeof(MenuItemView), string.Empty);

        public static readonly BindableProperty PriceProperty =
            BindableProperty.Create(nameof(Price), typeof(string), typeof(MenuItemView), string.Empty);

        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public string Price
        {
            get => (string)GetValue(PriceProperty);
            set => SetValue(PriceProperty, value);
        }

        public MenuItemView()
        {
            InitializeComponent();
        }
    }
}