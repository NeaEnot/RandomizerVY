using RandomizerVY.Models;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RandomizerVY
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NumberPage : ContentPage
    {
        private SettingsModel settings;

        private bool[] isNumberUsed;
        private int minUsed;
        private int maxUsed;

        private bool loaded = false;

        public NumberPage()
        {
            InitializeComponent();

            settings = new SettingsModel();
            settings.Count = bool.Parse(Preferences.Get("Numbers.Settings.Count", "false"));
            settings.WithoutRepeats = bool.Parse(Preferences.Get("Numbers.Settings.WithoutRepeats", "false"));

            LoadPage();
        }

        private void LoadPage()
        {
            loaded = true;

            labelCount.IsVisible = settings.Count;
            entryCount.IsVisible = settings.Count;

            if (!settings.WithoutRepeats)
            {
                minUsed = 0;
                maxUsed = 0;
                isNumberUsed = new bool[1];
            }

            entryFrom.Text = Preferences.Get("Numbers.From", "");
            entryTo.Text = Preferences.Get("Numbers.To", "");
            entryCount.Text = Preferences.Get("Numbers.Count", "");

            loaded = false;
        }

        private void SaveSettings()
        {
            Preferences.Set("Numbers.Settings.Count", settings.Count.ToString());
            Preferences.Set("Numbers.Settings.WithoutRepeats", settings.WithoutRepeats.ToString());
        }

        private void SaveNumbers()
        {
            Preferences.Set("Numbers.From", entryFrom.Text);
            Preferences.Set("Numbers.To", entryTo.Text);
            Preferences.Set("Numbers.Count", entryCount.Text);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                int n = settings.Count ? int.Parse(entryCount.Text) : 1;
                string answer = "";
                Random rnd = new Random();

                int from = int.Parse(entryFrom.Text);
                int to = int.Parse(entryTo.Text);

                if (settings.WithoutRepeats && (from != minUsed || to != maxUsed))
                {
                    isNumberUsed = new bool[to - from + 1];
                    minUsed = from;
                    maxUsed = to;
                }

                bool allNumbersUsed = false;

                for (int i = 0; i < n; i++)
                {
                    int number = 0;

                    while (true)
                    {
                        number = rnd.Next(from, to + 1);

                        if (!settings.WithoutRepeats || !isNumberUsed[number - from])
                        {
                            allNumbersUsed = SetUsedNumber(number - from);
                            break;
                        }
                    }

                    answer += (answer.Length > 0 ? ", " : "") + number.ToString();
                }

                DisplayAlert("Результат", answer, "Ок");

                if (allNumbersUsed && settings.WithoutRepeats)
                    DisplayAlert("Предупреждение", "Все возможные числа были использованы.", "Ок");
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }

        private bool IsExistUnusedNumbers()
        {
            for (int i = 0; i < isNumberUsed.Length; i++)
                if (!isNumberUsed[i])
                    return true;

            return false;
        }

        private bool SetUsedNumber(int n)
        {
            if (settings.WithoutRepeats)
                isNumberUsed[n] = true;

            if (!IsExistUnusedNumbers())
            {
                isNumberUsed = new bool[maxUsed - minUsed + 1];
                return true;
            }

            return false;
        }

        private async void settingsButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new SettingsPage(settings, () => { SaveSettings(); LoadPage(); }));
        }

        private void entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!loaded)
                SaveNumbers();
        }
    }
}