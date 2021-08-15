using System;
using Xamarin.Forms;

namespace RandomizerVY
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Number_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NumberPage());
        }
    }
}
