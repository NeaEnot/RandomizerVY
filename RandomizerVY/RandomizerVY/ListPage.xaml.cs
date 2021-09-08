using RandomizerVY.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RandomizerVY
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        private SettingsModel settings;

        private bool[] isNumberUsed;

        public ListPage()
        {
            InitializeComponent();

            settings = new SettingsModel();

            LoadPage();
        }

        private void LoadPage()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < 10; i++)
                list.Add($"Элемент списка номер {i}");

            Stack<View> stack = new Stack<View>();

            while (flexLayout.Children.Count > 0)
            {
                if (!(flexLayout.Children.Last() is Entry) || (flexLayout.Children.Last() as Entry).Keyboard == Keyboard.Numeric)
                    stack.Push(flexLayout.Children.Last());
                flexLayout.Children.Remove(flexLayout.Children.Last());
            }

            foreach (string str in list)
                flexLayout.Children.Add(CreateEntry(str));

            while (stack.Count > 0)
                flexLayout.Children.Add(stack.Pop());

            labelCount.IsVisible = settings.Count;
            entryCount.IsVisible = settings.Count;

            if (!settings.WithoutRepeats)
                isNumberUsed = new bool[1];
        }

        private void SaveSettings()
        {

        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            isNumberUsed = new bool[flexLayout.Children.Count - 3];

            Stack<View> stack = new Stack<View>();

            while (flexLayout.Children.Last() != sender)
            {
                stack.Push(flexLayout.Children.Last());
                flexLayout.Children.Remove(flexLayout.Children.Last());
            }

            Entry entry = CreateEntry("");
            flexLayout.Children.Add(entry);
            entry.Focus();

            while (stack.Count > 0)
                flexLayout.Children.Add(stack.Pop());
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            isNumberUsed = new bool[flexLayout.Children.Count - 3];

            if (e.NewTextValue == "" && (sender as Entry).Keyboard != Keyboard.Numeric)
            {
                if (flexLayout.Children.Count <= 4)
                {
                    (sender as Entry).Text = "\r";
                }
                else
                {
                    Entry entry = sender as Entry;
                    entry.Unfocus();
                    entry.IsVisible = false;

                    Stack<View> stack = new Stack<View>();

                    while (flexLayout.Children.Last() != sender)
                    {
                        stack.Push(flexLayout.Children.Last());
                        flexLayout.Children.Remove(flexLayout.Children.Last());
                    }

                    if (flexLayout.Children.Count > 1)
                    {
                        Entry prev = flexLayout.Children[flexLayout.Children.Count - 2] as Entry;

                        prev.Focus();
                        prev.CursorPosition = prev.Text.Length;
                    }

                    while (stack.Count > 0)
                        flexLayout.Children.Add(stack.Pop());

                    Task.Run(() =>
                    {
                        Thread.Sleep(500);
                        flexLayout.Children.Remove(entry);
                    });
                }
            }
        }

        private Entry CreateEntry(string str)
        {
            Entry entry = new Entry
            {
                Text = "\r" + str,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 300,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            entry.Completed += Entry_Completed;
            entry.TextChanged += Entry_TextChanged;
            return entry;
        }

        private string GetEntryText(Entry entry)
        {
            return entry.Text.Replace("\r", "");
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                Random rnd = new Random();

                int n = settings.Count ? int.Parse(entryCount.Text) : 1;
                string answer = "";

                bool allNumbersUsed = false;

                if (isNumberUsed == null)
                    isNumberUsed = new bool[flexLayout.Children.Count - 3];

                for (int i = 0; i < n; i++)
                {
                    List<string> list = new List<string>();

                    foreach (View child in flexLayout.Children)
                        if (child is Entry && (child as Entry).Keyboard != Keyboard.Numeric)
                            list.Add(GetEntryText(child as Entry));

                    int number = 0;

                    while (true)
                    {
                        number = rnd.Next(0, list.Count);

                        if (!settings.WithoutRepeats || !isNumberUsed[number])
                        {
                            allNumbersUsed = SetUsedNumber(number);
                            break;
                        }
                    }

                    answer += (answer.Length > 0 ? ", " : "") + list[number];
                }

                DisplayAlert("Результат", answer, "Ок");

                if (allNumbersUsed)
                    DisplayAlert("Предупреждение", "Все возможные числа были использованы.", "Ок");
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }

        private async void settingsButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new SettingsPage(settings, () => { SaveSettings(); LoadPage(); }));
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
                isNumberUsed = new bool[flexLayout.Children.Count - 3];
                return true;
            }

            return false;
        }
    }
}