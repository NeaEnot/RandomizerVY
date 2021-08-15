using RandomizerVY.Models;
using Rg.Plugins.Popup.Services;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RandomizerVY
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NumberPage : ContentPage
    {
        private SettingsModel settings;

        public NumberPage()
        {
            InitializeComponent();
            settings = new SettingsModel();
            LoadPage();
        }

        private void LoadPage()
        {
            labelCount.IsVisible = settings.Count;
            entryCount.IsVisible = settings.Count;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                int n = settings.Count ? int.Parse(entryCount.Text) : 1;
                string answer = "";
                Random rnd = new Random();

                for (int i = 0; i < n; i++)
                {
                    int from = int.Parse(entryFrom.Text);
                    int to = int.Parse(entryTo.Text);

                    int number = rnd.Next(from, to + 1);

                    answer += (answer.Length > 0 ? ", " : "") + number.ToString();
                }

                DisplayAlert("Результат", answer, "Ок");
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }

        private async void settingsButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new SettingsPage(settings, LoadPage));
        }
    }
}