using RandomizerVY.Models;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;

using Xamarin.Forms.Xaml;

namespace RandomizerVY
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : PopupPage
    {
        private SettingsModel settings;
        private Action action;

        public SettingsPage(SettingsModel settings, Action action)
        {
            InitializeComponent();

            this.settings = settings;
            this.action = action;

            switchRepeats.IsToggled = settings.WithoutRepeats;
            switchCount.IsToggled = settings.Count;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            settings.WithoutRepeats = switchRepeats.IsToggled;
            settings.Count = switchCount.IsToggled;

            action();

            await PopupNavigation.Instance.PopAsync();
        }
    }
}