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
        public ListPage()
        {
            InitializeComponent();

            List<string> list = new List<string>();
            for (int i = 0; i < 10; i++)
                list.Add($"Элемент списка номер {i}");

            foreach (string str in list)
            {
                flexLayout.Children.Add(CreateEntry(str));
            }
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            Stack<Entry> stack = new Stack<Entry>();

            while (flexLayout.Children.Last() != sender)
            {
                stack.Push((Entry)flexLayout.Children.Last());
                flexLayout.Children.Remove(flexLayout.Children.Last());
            }

            Entry entry = CreateEntry("");
            flexLayout.Children.Add(entry);
            entry.Focus();

            while (stack.Count > 0)
            {
                flexLayout.Children.Add(stack.Pop());
            }
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
            {
                Entry entry = sender as Entry;
                entry.Unfocus();
                entry.IsVisible = false;

                Stack<Entry> stack = new Stack<Entry>();

                while (flexLayout.Children.Last() != sender)
                {
                    stack.Push((Entry)flexLayout.Children.Last());
                    flexLayout.Children.Remove(flexLayout.Children.Last());
                }

                if (flexLayout.Children.Count > 1)
                {
                    Entry prev = flexLayout.Children[flexLayout.Children.Count - 2] as Entry;

                    prev.Focus();
                    prev.CursorPosition = prev.Text.Length;
                }

                while (stack.Count > 0)
                {
                    flexLayout.Children.Add(stack.Pop());
                }

                Task.Run(() =>
                {
                    Thread.Sleep(500);
                    flexLayout.Children.Remove(entry);
                });
            }
        }

        private Entry CreateEntry(string str)
        {
            Entry entry = new Entry
            {
                Text = "\n" + str,
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
            return entry.Text.Replace("\n", "");
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Random rnd = new Random();
            List<string> list = new List<string>();

            foreach (View child in flexLayout.Children)
                if (child is Entry)
                    list.Add((child as Entry).Text);

            string answer = list[rnd.Next(0, list.Count)];

            DisplayAlert("Результат", answer, "Ок");
        }
    }
}